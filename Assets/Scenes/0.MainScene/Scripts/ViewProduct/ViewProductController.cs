using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewProductController : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] ViewExperienceBehaviour viewExperienceBehaviour;
    [SerializeField] ViewExperienceController viewExperienceController;

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] MouseCursorBehaviour mouseCursorBehaviour;
    [SerializeField] ProductRotationBehaviour rotationBehaviour;
    [SerializeField] ProductCameraBehaviour zoomBehaviour;

    [Header("ViewerPanel Components")]
    [SerializeField] Button closeViewerPanelButton;

    private ViewProductBehaviour behaviour;

    [Header("SpaceProduct Components")]
    [SerializeField] Transform contentParent;
    [SerializeField] GameObject[] contentPrefabs;

    [SerializeField] private GameObject currentContent;

    void Start()
    {
        behaviour = GetComponent<ViewProductBehaviour>();
        MouseCursorBehaviour.OnMousePointedOn += MouseCursorBehaviour_OnMousePointedOn;

        closeViewerPanelButton.onClick.AddListener(()=>OnCloseProductViewerClicked());

        // Initialize Content with the first contentProduct
        currentContent = Instantiate(contentPrefabs[0], contentParent);
        currentContent.name = currentContent.name.Replace("(Clone)", "");
    }

    private void OnCloseProductViewerClicked()
    {
        rotationBehaviour.ResetRotation();
        zoomBehaviour.ResetZoom();

        playerMovement.EnablePlayerMovement(true);
        viewExperienceBehaviour.SetViewBehaviour(ViewExperienceBehaviour.ViewExperienceLayouts.Activity);

        behaviour.ShowProductViewerPanel(false);

        mouseCursorBehaviour.EnableMouseAction(true);

        viewExperienceController.StartUIBlocking();
    }

    public void SetProductViewerState(bool state)
    {
        //Debug.Log("SetProductViewer:"+state.ToString());

        behaviour.ShowProductViewerPanel(state);
    }

    public void DismissProductViewer()
    {
        OnCloseProductViewerClicked();
    }

    private void MouseCursorBehaviour_OnMousePointedOn(string objectName)
    {
        viewExperienceController.StopUIBlocking();

        if (!CreateContentObject(objectName))
            return;

        behaviour.ShowProductViewerPanel(true);
        playerMovement.EnablePlayerMovement(false);

        viewExperienceBehaviour.SetViewBehaviour(ViewExperienceBehaviour.ViewExperienceLayouts.None);

        mouseCursorBehaviour.EnableMouseAction(false);

        zoomBehaviour.ResetZoom();
        rotationBehaviour.ResetRotation();
    }

    private bool CreateContentObject(string contentReferenceName)
    {
        if (currentContent.name == contentReferenceName)
            return true;

        foreach (var contentPrefab in contentPrefabs)
        {
            if (contentPrefab.name == contentReferenceName)
            {
                Destroy(contentParent.GetChild(0).gameObject);
                currentContent = Instantiate(contentPrefab, contentParent);
                currentContent.name = currentContent.name.Replace("(Clone)", "");
                return true;
            }
        }

        return false;
    }

}
