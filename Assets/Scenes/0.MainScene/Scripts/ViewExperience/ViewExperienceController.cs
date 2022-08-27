using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ViewExperienceController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private MouseCursorBehaviour mouseCursorBehaviour;
    [SerializeField]
    private AppController appController;
    [SerializeField]
    private ViewMenuController viewMenuController;
    [SerializeField]
    private ViewProductController viewProductController;

    [Header("Behaviours")]
    [SerializeField]
    private ViewMessageBehaviour viewMessageBehaviour;

    [Header("ViewMenu Components")]
    [SerializeField]
    private Button startActivity;

    [Header("ViewNavigation Components")]
    [SerializeField]
    private Button quitActivity;

    [Header("ViewMessage Components")]
    [SerializeField]
    private Button checkProductButton;
    [SerializeField]
    private Button resumeActivityButton;


    [Header("Raycast UI Blockers")]
    [SerializeField]
    private Image[] uiElements;

    private ViewExperienceBehaviour behaviour;

    void Start()
    {
        behaviour = GetComponent<ViewExperienceBehaviour>();
        SearchChallengeController.OnProductFounded += SearchChallengeController_OnSearchFinished;
        SearchChallengeController.OnProductFailed += SearchChallengeController_OnProductFailed;

        // Start Activity
        startActivity.onClick.AddListener(() => StartActivityButtonClicked());

        // Finish Activity
        quitActivity.onClick.AddListener(() => QuitActivityButtonClicked());


        checkProductButton.onClick.AddListener(() => CheckProductAfterSearch());
        resumeActivityButton.onClick.AddListener(() => ResumeSearching());

        viewMenuController.SetQuestType(appController.activityType);

        mouseCursorBehaviour.EnableMouseAction(false);
    }

    public void StopUIBlocking()
    {
        StopCoroutine("UIBlockMouse");
    }

    public void StartUIBlocking()
    {
        StopCoroutine("UIBlockMouse");
        StartCoroutine("UIBlockMouse");
    }

    private IEnumerator UIBlockMouse()
    {
        while (true)
        {
            // Check if mouse is on top of the UI Navigation Panels
            bool isTouchingUI = false;

            foreach (Image im in uiElements)
            {
                if (im.IsActive())
                {
                    Vector3[] corners = new Vector3[4];
                    im.rectTransform.GetWorldCorners(corners);
                    Rect r = new Rect(corners[0], corners[2] - corners[0]);
                    isTouchingUI |= r.Contains(Input.mousePosition);
                }
            }

            mouseCursorBehaviour.EnableMouseAction(!isTouchingUI);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ResumeSearching()
    {
        //Debug.Log("Resuming Search");

        viewProductController.DismissProductViewer();

        StartCoroutine("UIBlockMouse");
    }

    public void CheckProductAfterSearch()
    {
        //Debug.Log("Check Product After Search");

        StopCoroutine("UIBlockMouse");
        mouseCursorBehaviour.EnableMouseAction(false);

        behaviour.SetViewBehaviour(ViewExperienceBehaviour.ViewExperienceLayouts.None);
        viewProductController.SetProductViewerState(true);
    }

    private void SearchChallengeController_OnProductFailed()
    {
        //Debug.Log("OnProduct Failed");

        StopCoroutine("UIBlockMouse");

        if (appController.activityType == AppController.ActivityTypes.TimedSearch)
            viewProductController.SetProductViewerState(false);

        mouseCursorBehaviour.EnableMouseAction(false);

        playerMovement.EnablePlayerMovement(false);
        behaviour.SetViewBehaviour(ViewExperienceBehaviour.ViewExperienceLayouts.Message);
        viewMessageBehaviour.SetResultMessage(false);
    }

    private void SearchChallengeController_OnSearchFinished()
    {
        StopCoroutine("UIBlockMouse");

        if (appController.activityType == AppController.ActivityTypes.TimedSearch)
            viewProductController.SetProductViewerState(false);

        playerMovement.EnablePlayerMovement(false);
        behaviour.SetViewBehaviour(ViewExperienceBehaviour.ViewExperienceLayouts.Message);
        viewMessageBehaviour.SetResultMessage(true);
    }

    private void QuitActivityButtonClicked()
    {
        playerMovement.EnablePlayerMovement(false);
        appController.StopActivity();

        behaviour.SetViewBehaviour(ViewExperienceBehaviour.ViewExperienceLayouts.Finish);

        appController.FinishApplication();
    }

    private void StartActivityButtonClicked()
    {
        playerMovement.EnablePlayerMovement(true);
        appController.StartActivity();

        behaviour.SetViewBehaviour(ViewExperienceBehaviour.ViewExperienceLayouts.Activity);

        mouseCursorBehaviour.EnableMouseAction(true);

        StartCoroutine("UIBlockMouse");
    }

}
