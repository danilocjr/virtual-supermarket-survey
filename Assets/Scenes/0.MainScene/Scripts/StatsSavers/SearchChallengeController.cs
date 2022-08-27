using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class SearchChallengeController : MonoBehaviour
{
    private Dictionary<string, int> searchTimers = new Dictionary<string, int>();
    private DateTime challengeStartTime;

    private string targetObjectName;
    private bool isChallenging;

    private string jsonPath;

    public delegate void SearchChallengeControllerEvents();
    public static event SearchChallengeControllerEvents OnSearchStarted;
    public static event SearchChallengeControllerEvents OnProductFounded;
    public static event SearchChallengeControllerEvents OnProductFailed;

    void Start()
    {
        MouseCursorBehaviour.OnMousePointedOn += MouseCursorBehaviour_OnMousePointedOn;
    }

    private void SetJsonPath(string userId, string studyId)
    {
        string filename = string.Format("ChallengeReport_{0}_{1}_{2}.json", studyId, userId, DateTime.UtcNow.ToString("yyyyMMddTHHmmss"));
        jsonPath = Path.Combine(Application.streamingAssetsPath, filename);
    }

    private void SaveStatsLog()
    {
        if (searchTimers.Count <= 0)
            return;


        string json = "{ \"ChallengeTimes\": {";
        foreach (var st in searchTimers)
            json += "\"" + st.Key + "\" : " + st.Value.ToString() + ",";
        json = json.TrimEnd(',');
        json += "}";

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

    private void MouseCursorBehaviour_OnMousePointedOn(string objectName)
    {
        if (!isChallenging)
            return;

        //Debug.Log("Touched:" + objectName + " and Expected:" + targetObjectName);

        if (searchTimers.ContainsKey(objectName))
        {
            OnProductFailed?.Invoke();
            return;
        } 

        TimeSpan delta = DateTime.UtcNow - challengeStartTime;
        searchTimers[objectName] = delta.Seconds;

        if (string.Compare(objectName, targetObjectName, true) == 0)
            FinishChallenge();
        else
            OnProductFailed?.Invoke();
    }

    public void BeginChallenge(string userId, string studyId, string targetName)
    {
        if (isChallenging)
            return;

        isChallenging = true;

        SetJsonPath(userId, studyId);
        targetObjectName = targetName;

        challengeStartTime = DateTime.UtcNow;

        OnSearchStarted?.Invoke();
    }

    public void FinishChallenge()
    {
        if (!isChallenging)
            return;

        isChallenging = false;

        SaveStatsLog();

        OnProductFounded?.Invoke();
    }




}
