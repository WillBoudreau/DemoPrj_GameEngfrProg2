using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

// Sam Robichaud 
// NSCC Truro 2024
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)

public class InputManager : MonoBehaviour
{
    // Script References
    [SerializeField] private PlayerLocomotionHandler playerLocomotionHandler;
    [SerializeField] private CameraManager cameraManager; // Reference to CameraManager
    public Text InputDeviceText;
    public bool IsGamePadActive;

    [Header("Movement Inputs")]
    public float verticalInput;
    public float horizontalInput;
    public bool jumpInput;
    public Vector2 movementInput;
    public float moveAmount;

    public PlayerInputActions playerControls;
    // move
    private InputAction move;
    // look
    private InputAction look;
    //Jump 
    private InputAction jump;
//Control Scheme
private string currentControlScheme;

    [Header("Camera Inputs")]
    public float scrollInput; // Scroll input for camera zoom
    public Vector2 cameraInput; // Mouse input for the camera

    public bool isPauseKeyPressed = false;


    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
        HandleCameraInput();
        HandlePauseKeyInput();
        if (Gamepad.current.aButton.wasPressedThisFrame) { Debug.Log("A button was pressed"); }
    }
    private void Awake()
    {
       
    }
    void CheckInputType()
    {
        foreach(InputDevice device in playerControls.devices)
        {
            if(device is Mouse || device is Keyboard)
            {
                IsGamePadActive = false;
                InputDeviceText.text = "KeyBoard Connected";
            }
            else if(device is Gamepad)
            {
                IsGamePadActive = true;
                InputDeviceText.text = "GamePad Connected";
            }
        }
    }
    private void OnEnable()
    {
        playerControls = new PlayerInputActions();
        //playerControls.Enable();
        move = playerControls.Player.Move;
        move.Enable();

        look = playerControls.Player.Look;
        look.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
    }


    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        jump.Disable();
    }


    private void HandleCameraInput()
    {


        cameraManager.zoomInput = scrollInput;
        cameraManager.cameraInput = cameraInput;


        // Get mouse input for the camera
        cameraInput = look.ReadValue<Vector2>();
        
        //cameraInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Get scroll input for camera zoom
        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Send inputs to CameraManager
        cameraManager.zoomInput = scrollInput;
        cameraManager.cameraInput = cameraInput;
    }

    private void HandleMovementInput()
    {
        
        // FIRST METHOD
        // movementInput = playerControls.ReadValue<Vector2>();
        // NEW METHOD
        movementInput = move.ReadValue<Vector2>();
        
        
        
        //movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }

    private void HandlePauseKeyInput()
    {
        isPauseKeyPressed = Input.GetKeyDown(KeyCode.Escape); // Detect the escape key press
    }

    private void HandleSprintingInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && moveAmount > 0.5f)
        {
            playerLocomotionHandler.isSprinting = true;
        }
        else
        {
            playerLocomotionHandler.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        jumpInput = jump.IsPressed(); // Detect jump input (spacebar)
        if (jumpInput)
        {
            playerLocomotionHandler.HandleJump(); // Trigger jump in locomotion handler
        }
    }
}