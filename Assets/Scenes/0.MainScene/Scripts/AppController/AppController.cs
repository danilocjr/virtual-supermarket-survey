using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
    [Header("External References")]
    [SerializeField]
    private AttentionController attentionController;
    [SerializeField]
    private SearchChallengeController searchChallengeController;
   
    [Header("External URL Params")]
    public string userId;
    public string studyId;
    public int activityId;
    public string productName;
    public string callbackUrl;

    [Header("Editor Debug")]
    public ActivityTypes activityType;

    public Text text;

    private bool _isRunning = false;
    public enum ActivityTypes
    {
        Exploration,
        TimedSearch
    }

    void Start()
    {
        AttentionController.OnRecordFinished += AttentionController_OnRecordFinished;
        ApplicationSetupByURL();
    }

    public void ApplicationSetupByURL()
    {
        string url = Application.absoluteURL;

        if (string.IsNullOrWhiteSpace(url))
        {
            activityId = (int)activityType;
            return;
        }

        string args_url = url.Split('?')[1];
        string[] args = args_url.Split('&');

        foreach (string arg in args)
        {
            string[] kv = arg.Split('=');

            switch (kv[0])
            {
                case "uid":
                    userId = kv[1];
                    break;
                case "sid":
                    studyId = kv[1];
                    break;
                case "cbk":
                    callbackUrl = kv[1];
                    break;
                case "aid":
                    activityId = int.Parse(kv[1]);
                    activityType = (ActivityTypes)activityId;
                    break;
            }
        }

        text.text = activityType.ToString();
    }

    private void AttentionController_OnRecordFinished()
    {
        Debug.Log("Exploration Finished");
    }

    public void StartActivity()
    {
        if (_isRunning)
            return;

        _isRunning = true;

        attentionController.BeginRecording(userId, studyId);

        if (activityType == ActivityTypes.TimedSearch)
            searchChallengeController.BeginChallenge(userId, studyId, productName);
    }

    public void StopActivity()
    {
        if (!_isRunning)
            return;

        _isRunning = false;

        attentionController.FinishRecording();

        if (activityType == ActivityTypes.TimedSearch)
            searchChallengeController.FinishChallenge();
  
    }

    public void FinishApplication()
    {
        //Todo: Call the URL sent in the App Open...
        //Application.Quit();
        Application.OpenURL(callbackUrl);
    }



}
