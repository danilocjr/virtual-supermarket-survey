using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ModelViewerCommand
{
    ZoomUp,
    ZoomDown,
    RotateLeft,
    RotateRight,
    RotateUp,
    RotateDown,
    MouseAction,
    None
}

public class ModelViewerConnector : MonoBehaviour
{
    [SerializeField] private RawImage viewer;
    [SerializeField] private GameObject controlParent;
    private MDXButton[] controlButtons;

    private Vector3[] corners = new Vector3[4];
    private Rect newRect;
    private bool _hasMouse = false;

    public delegate void ModelViewerAction(ModelViewerCommand viewerCommand, bool hasMouse);
    public static event ModelViewerAction OnModelViewerCommandClicked;

    void Start()
    {
        // Getting Viewer Boudaries to determine the mouse pointer presence
        viewer.rectTransform.GetWorldCorners(corners);
        newRect = new Rect(corners[0], corners[2] - corners[0]);

        controlButtons = controlParent.GetComponentsInChildren<MDXButton>();

        foreach (MDXButton nb in controlButtons)
        {
            ModelViewerCommand _viewCmd = (ModelViewerCommand)Enum.Parse(typeof(ModelViewerCommand), nb.name, true);
            nb.OnPressed(() => ClickCommand(_viewCmd));
            nb.OnReleased(() => ClickCommand(ModelViewerCommand.None));
        }
    }

    private void ClickCommand(ModelViewerCommand viewCmd)
    {
        OnModelViewerCommandClicked?.Invoke(viewCmd,false);
    }

    void Update()
    {
        HasMouse();
    }

    private void HasMouse()
    {
        bool state = newRect.Contains(Input.mousePosition);
        if (state == _hasMouse)
            return;

        _hasMouse = state;
        OnModelViewerCommandClicked?.Invoke(ModelViewerCommand.MouseAction, _hasMouse);
    }
}
