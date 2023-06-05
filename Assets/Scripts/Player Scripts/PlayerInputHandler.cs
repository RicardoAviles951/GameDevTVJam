using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction moveAction, jumpAction, restartAction;
    public InputSystem playerControls;

    private void Awake()
    {
        playerControls = new InputSystem();
    }

    void OnEnable()
    {
        //Enables player controls as part of Unity input system.
        moveAction = playerControls.Player.Move;
        jumpAction = playerControls.Player.Jump;
        restartAction = playerControls.Player.Reset;
        jumpAction.Enable();
        moveAction.Enable();
        restartAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        restartAction.Disable();
    }
    

    private void Update()
    {
        
    }

    public float HorizontalInput()
    {
        float moveInputX = moveAction.ReadValue<Vector2>().x;
        //print(moveInputX);
        return moveInputX;
    }
    
     public bool JumpInput()
    {
        return jumpAction.triggered;
    }

    public bool JumpInputHeld()
    {
        return jumpAction.IsPressed();
    }

    public bool JumpButtonUp()
    {
        return jumpAction.WasReleasedThisFrame();
    }

    public void RestartScene(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //Gets current level index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Loads current scene again. 
        SceneManager.LoadScene(currentSceneIndex);
        }
    }

    
}
