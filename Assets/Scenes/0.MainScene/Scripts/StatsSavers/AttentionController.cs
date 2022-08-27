using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttentionController : MonoBehaviour
{
    private Dictionary<string, DateTime> timers = new Dictionary<string, DateTime>();
    private Dictionary<string, int>[] attentionTime = new Dictionary<string, int>[3];

    private bool isRecording = false;
    [SerializeField] private int updateIntervalInSecs = 5;

    private string jsonPath;

    public delegate void AttetionControllerEvents();
    public static event AttetionControllerEvents OnRecordStarted;
    public static event AttetionControllerEvents OnRecordFinished;

    #region SETUP

    private void Start()
    {
        Dictionary<string, int> initDict = new Dictionary<string, int>();
        attentionTime[0] = initDict;
        attentionTime[1] = initDict;
        attentionTime[2] = initDict;

        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        FocusDetection.OnFocusChanged += FocusDetection_OnFocusChanged;
    }

    private void ClearDicts()
    {
        attentionTime[0].Clear();
        attentionTime[1].Clear();
        attentionTime[2].Clear();
        timers.Clear();
    }

    private void SetJsonPath(string userId, string studyId)
    {
        string filename = string.Format("AttentionReport_{0}_{1}_{2}.json", studyId, userId, DateTime.UtcNow.ToString("yyyyMMddTHHmmss"));
        jsonPath = Path.Combine(Application.streamingAssetsPath, filename);
    }

    #endregion

    private void FocusDetection_OnFocusChanged(int focusIndex, string objectName, bool hasFocus)
    {
        if (!isRecording)
            return;

        SetAttentionTimer(objectName, focusIndex, hasFocus);
    }

    private void StartTimer(string key)
    {
        timers[key] = DateTime.UtcNow;
    }

    private int StopTimer(string key)
    {
        TimeSpan delta = DateTime.UtcNow - timers[key];
        return delta.Seconds;
    }

    private void SetAttentionTimer(string key, int focusIndex, bool enabled)
    {
        if (enabled)
        {
            StartTimer(key);
            if (!attentionTime[focusIndex].ContainsKey(key))
                attentionTime[focusIndex][key] = 0;
        }
        else
        {
            attentionTime[focusIndex][key] += StopTimer(key);
        }
    }

    private IEnumerator Record()
    {
        while (isRecording)
        {
            yield return new WaitForSeconds(updateIntervalInSecs);
            SaveStatsLog();
        }
    }

    private void SaveStatsLog()
    {
        int focusCount = 0;
        string json = "{ \"attentionTimes\": [";
        foreach (var focus in attentionTime)
        {
            if(focus.Count > 0)
            {
                json += "{";
                foreach (var at in focus)
                    json += "\"" + at.Key + "\" : " + at.Value.ToString() + ",";
                json = json.TrimEnd(',');
                json += "},";
            }
            focusCount++;
        }
        json = json.TrimEnd(',');
        json += "]}";

        SaveJsonFile(json);
    }

    private void SaveJsonFile(string fileContent)
    {
        if (string.IsNullOrWhiteSpace(fileContent))
            return;

        //TODO: Replace to API Send
        byte[] data = System.Text.Encoding.UTF8.GetBytes(fileContent);
        File.WriteAllBytes(jsonPath, data);
    }

    #region PUBLIC METHODS

    public void BeginRecording(string userId, string studyId)
    {
        if (isRecording)
            return;

        isRecording = true;

        SetJsonPath(userId, studyId);
        StartCoroutine(Record());

        OnRecordStarted?.Invoke();
    }

    public void FinishRecording()
    {
        if (!isRecording)
            return;

        isRecording = false;

        StopCoroutine(Record());
        ClearDicts();

        OnRecordFinished?.Invoke();
    }

    #endregion

}
