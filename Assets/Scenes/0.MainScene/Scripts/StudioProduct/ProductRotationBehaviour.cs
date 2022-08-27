using UnityEngine;
using UnityEngine.UI;

public class ProductRotationBehaviour : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 500f;
    [SerializeField] private float buttonRotationStep = 1f;

    private Quaternion initialRotation;
    private float mouseX, mouseY;

    private float buttonX = 0f;
    private float buttonY = 0f;

    private bool _hasMouse = false;

    private void Start()
    {
        ModelViewerConnector.OnModelViewerCommandClicked += ModelViewerConnector_OnModelViewerCommandClicked;
        initialRotation = transform.rotation;
    }

    private void ModelViewerConnector_OnModelViewerCommandClicked(ModelViewerCommand viewerCommand, bool hasMouse)
    {
        switch (viewerCommand)
        {
            case ModelViewerCommand.RotateLeft:
                buttonX = buttonRotationStep;
                break;
            case ModelViewerCommand.RotateRight:
                buttonX = -buttonRotationStep;
                break;
            case ModelViewerCommand.RotateUp:
                buttonY = buttonRotationStep;
                break;
            case ModelViewerCommand.RotateDown:
                buttonY = -buttonRotationStep;
                break;
            case ModelViewerCommand.None:
                buttonX = 0;
                buttonY = 0;
                break;
            case ModelViewerCommand.MouseAction:
                buttonX = 0;
                buttonY = 0;
                _hasMouse = hasMouse;
                break;
        }
    }

    public void ResetRotation()
    {
        transform.rotation = initialRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && _hasMouse)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Mathf.Deg2Rad;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Mathf.Deg2Rad;

            SetRotation(mouseX, mouseY);
        }
        else
        {
            SetRotation(buttonX, buttonY);
        }
    }

    private void SetRotation(float x, float y)
    {
        transform.Rotate(Vector3.down, x, Space.World);
        transform.Rotate(Vector3.right, y, Space.World);
    }

    

}
