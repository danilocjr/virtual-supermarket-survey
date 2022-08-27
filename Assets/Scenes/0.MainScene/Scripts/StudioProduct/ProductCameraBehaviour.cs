using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductCameraBehaviour : MonoBehaviour
{
    private Transform camTransform;

    [Header("Zoom Setup")]
    [SerializeField] private float startZoom = 3.0f;
    [SerializeField] private float minZoom = 1.0f;
    [SerializeField] private float maxZoom = 6.0f;

    [Header("Zoom Control")]
    [SerializeField] private float buttonSencitivity = 0.2f;
    [SerializeField] private float mouseSensitivity = 1f;

    [Header("Camera Position")]
    [SerializeField] private Vector2 position = Vector2.zero;

    private float _currentZoom;
    private float z = 0f;
    private bool _hasMouse = false;

    void Start()
    {
        ModelViewerConnector.OnModelViewerCommandClicked += ModelViewerConnector_OnModelViewerCommandClicked;
        camTransform = GetComponent<Camera>().transform;

        _currentZoom = startZoom;
        SetCameraZoom(_currentZoom);
    }

    private void Update()
    {
        if (_hasMouse)
            SetCameraZoom(Input.mouseScrollDelta.y * mouseSensitivity);
        else
            SetCameraZoom(z);

    }

    public void ResetZoom()
    {
        SetCameraZoom(startZoom);
    }

    private void ModelViewerConnector_OnModelViewerCommandClicked(ModelViewerCommand viewerCommand, bool hasMouse)
    {
        switch (viewerCommand)
        {
            case ModelViewerCommand.ZoomUp:
                z = - buttonSencitivity;
                break;
            case ModelViewerCommand.ZoomDown:
                z = buttonSencitivity;
                break;
            case ModelViewerCommand.None:
                z = 0f;
                break;
            case ModelViewerCommand.MouseAction:
                _hasMouse = hasMouse;
                z = 0f;
                break;
        }
    }

    private void SetCameraZoom(float zoom)
    {
        _currentZoom += zoom;

        if (_currentZoom > maxZoom)
            _currentZoom = maxZoom;

        if (_currentZoom < minZoom)
            _currentZoom = minZoom;

        camTransform.localPosition = new Vector3(position.x, position.y, -_currentZoom);
    }


}
