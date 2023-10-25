using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction moveAction, jumpAction, restartAction, attackAction,switchAction;
    public InputSystem playerControls;
    public bool isJumping = false;

    private void Awake()
    {
        playerControls = new InputSystem();
    }
    

    void OnEnable()
    {
        //Enables player controls as part of Unity input system.
        playerControls.Player.Enable();
        //Cache individual actions
        moveAction    = playerControls.Player.Move;
        jumpAction    = playerControls.Player.Jump;
        restartAction = playerControls.Player.Reset;
        attackAction  = playerControls.Player.Fire;
        switchAction  = playerControls.Player.Switch; 
        
    }

    void OnDisable()
    {
        playerControls.Player.Disable();
    }
    

    private void Update()
    {
        //Check if game is paused to disable player controls.
        if(GameStateController.isPaused == true )
        {
            playerControls.Player.Disable();
            //Debug.Log("Controls disabled");
        }
        else
        {
            playerControls.Player.Enable();
           // Debug.Log("Controls Enabled");
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
        //Debug.Log("Jump released");
        return jumpAction.WasReleasedThisFrame();
    }

    public void RestartScene(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            //Gets current level index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            GameStateController.CoinsCollected = GameStateController.startCoins;
        //Loads current scene again. 
        SceneManager.LoadScene(currentSceneIndex);
        }
    }

    public bool AttackPressed()
    {
        //Debug.Log("clicked");
        return attackAction.triggered;
    }

    public bool WeaponToggle()
    {
        //Debug.Log("Weapon Switched");
        return switchAction.triggered;
    }

    
}
