using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    [Header("Player Camera/Head")]
    [SerializeField] private Transform playerHead;

    //[Header("Player Setup")]
    //[SerializeField] private float PlayerHeight = 1.6f;
    //[SerializeField] private Vector3 startPosition = Vector3.zero;

    [Header("Look Behaviour")]
    [SerializeField] private float lookSensitivity = 100f;
    [SerializeField] private float anglesPerClick = 90f;
    [SerializeField] private bool enableSideLook = false;
    [SerializeField] private bool enableUpDownLook = false;
    [SerializeField] private bool keysControlSideLook = false;


    [Header("Movement Behaviour")]
    [SerializeField] private bool enableMovesInX = false;
    [SerializeField] private bool enableMovesInY = false;
    [SerializeField] private bool moveGuidedByLook = false;
    [SerializeField] private float movementSpeed = 12f;

    [Header("Crouch Behaviour")]
    [SerializeField] private float smoothFactor = 1f;
    [SerializeField] private float crouchHeight = 0f;
    [SerializeField] private float normalHeight = 1f;
    [SerializeField] private float standHeight = 1.3f;
    private bool isCrouchAnimating = false;
    private NavCommand lastPosturePosition = NavCommand.normal;

    [Header("Controllers")]
    [SerializeField] private bool mouseControlLooks = false;
    [SerializeField] private bool isMovingEnabled = false;

    // Internal variables for Movements
    private float moveX = 0f;
    private float moveZ = 0f;
    private Vector3 move;

    // Internal variables for Looks
    private float lookX;
    private float lookY;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Nav Controller Event Listenner
        NavController.OnNavCommandClicked += NavController_OnNavCommandClicked;

        // Player Setup
        //transform.position = new Vector3(startPosition.x, startPosition.y + PlayerHeight, startPosition.z);
        //controller.height = PlayerHeight;
        //controller.center = new Vector3(0, -PlayerHeight / 2f, 0);

        // Start States
        if (mouseControlLooks)
            Cursor.lockState = CursorLockMode.Locked;

        if (isMovingEnabled)
        {
            isMovingEnabled = false; //Reverse at the start because toggle will reverse it again...
            TogglePlayerMovement();
        }

    }

    private IEnumerator PlayerMovementUpdate()
    {
        while (isMovingEnabled)
        {
            BodyMovements();

            HeadMovements();

            CrouchMovements();

            yield return new WaitForEndOfFrame();
        }
    }

    #region Movement

    private void BodyMovements()
    {
        float _px = 0f;
        float _pz = 0f;

        if (enableMovesInY)
            _px = moveX + Input.GetAxis("Horizontal");

        if (enableMovesInX)
            _pz = moveZ + Input.GetAxis("Vertical");


        if (moveGuidedByLook)
        {
            move = transform.right * _px + transform.forward * _pz;
        }
        else
        {
            int direction = 1;
            if (transform.rotation.eulerAngles.y > 90f && transform.rotation.eulerAngles.y < 270f)
                direction = -1;

            move = new Vector3(_px, 0f, _pz * direction);
        }


        controller.Move(move * movementSpeed * Time.deltaTime);
    }

    private void HeadMovements()
    {
        if (enableSideLook)
        {
            float _lx = 0f;

            if (mouseControlLooks)
                _lx = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
            else if (keysControlSideLook)
                _lx = (lookX + Input.GetAxis("Horizontal")) * anglesPerClick;

            transform.Rotate(Vector3.up * _lx);
        }

        if (enableUpDownLook)
        {
            if (mouseControlLooks)
                lookY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

            xRotation -= lookY;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);
            playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    private void CrouchMovements()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //    SetCrouch(NavCommand.stand);

        //if (Input.GetKeyDown(KeyCode.G))
        //    SetCrouch(NavCommand.normal);

        //if (Input.GetKeyDown(KeyCode.B))
        //    SetCrouch(NavCommand.crouch);
    }

    #endregion

    #region Crouch

    private void SetCrouch(NavCommand posturePosition)
    {
        if (!isMovingEnabled || isCrouchAnimating)
            return;

        if (lastPosturePosition == posturePosition)
            return;

        lastPosturePosition = posturePosition;

        Vector3 init = playerHead.transform.localPosition;
        Vector3 target = Vector3.zero;

        float height = 0f;

        switch (posturePosition)
        {
            case NavCommand.stand:
                height = standHeight;
                break;
            case NavCommand.normal:
                height = normalHeight;
                break;
            case NavCommand.crouch:
                height = crouchHeight;
                break;
        }

        target = new Vector3(playerHead.transform.localPosition.x, height, playerHead.transform.localPosition.z);
        StartCoroutine(CrouchAnimation(init, target));
    }

    private IEnumerator CrouchAnimation(Vector3 init, Vector3 target)
    {
        isCrouchAnimating = true;

        float i = 0.0f;
        float rate = 1f / smoothFactor;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            playerHead.transform.localPosition = Vector3.Lerp(init, target, i);
            yield return new WaitForEndOfFrame();
        }

        isCrouchAnimating = false;
    }

    #endregion

    #region Public Methods

    public void TogglePlayerMovement()
    {
        isMovingEnabled = !isMovingEnabled;
        EnablePlayerMovement(isMovingEnabled);
    }

    public void EnablePlayerMovement(bool enable)
    {
        isMovingEnabled = enable;

        StopCoroutine("PlayerMovementUpdate");
        if (isMovingEnabled)
            StartCoroutine("PlayerMovementUpdate");
    }

    #endregion

    #region NavController Integration

    private void ClearLastNavControllerCommand()
    {
        moveX = 0;
        moveZ = 0;
        lookX = 0;
        lookY = 0;
    }

    private void NavController_OnNavCommandClicked(NavCommand navCommand)
    {
        switch (navCommand)
        {
            case NavCommand.forward:
                moveZ = 0.4f;
                break;
            case NavCommand.center:
                break;
            case NavCommand.backward:
                moveZ = -0.4f;
                break;
            case NavCommand.left:
                moveX = -0.4f;
                break;
            case NavCommand.right:
                moveX = 0.4f;
                break;
            case NavCommand.lookRight:
                lookX = 1f;
                break;
            case NavCommand.lookLeft:
                lookX = -1f;
                break;
            case NavCommand.crouch:
            case NavCommand.normal:
            case NavCommand.stand:
                SetCrouch(navCommand);
                break;
            default:
                ClearLastNavControllerCommand();
                break;
        }
    }

    #endregion

}
