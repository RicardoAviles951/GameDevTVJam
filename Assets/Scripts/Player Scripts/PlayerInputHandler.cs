using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction moveAction, jumpAction, restartAction, attackAction;
    public InputSystem playerControls;
    public bool isJumping = false;

    private void Awake()
    {
        playerControls = new InputSystem();
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
        //Enables player controls as part of Unity input system.
        moveAction    = playerControls.Player.Move;
        jumpAction    = playerControls.Player.Jump;
        restartAction = playerControls.Player.Reset;
        attackAction  = playerControls.Player.Fire;
        
    }

    void OnDisable()
    {
        playerControls.Player.Disable();
    }
    

    private void Update()
    {
        Debug.Log(isJumping);
        if(GameStateController.isPaused == true )
        {
            playerControls.Player.Disable();
        }
        else
        {
            playerControls.Player.Enable();
        }
    }

    public float HorizontalInput()
    {
        float moveInputX = moveAction.ReadValue<Vector2>().x;
        //Debug.Log("moving horz");
        return moveInputX;
    }
    
     public bool JumpInput()
    {
        return jumpAction.WasPerformedThisFrame();
    }

    public bool JumpInputHeld()
    {
        //Debug.Log("jump held");
        return jumpAction.IsPressed();
        
    }
    public bool JumpButtonUp()
    {
        Debug.Log("Jump released");
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

    public bool AttackPressed()
    {
        //Debug.Log("clicked");
        return attackAction.triggered;
    }

    
}
