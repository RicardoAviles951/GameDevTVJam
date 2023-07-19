using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction moveAction, jumpAction, restartAction, attackAction, debugAction;
    public InputSystem playerControls;
    public bool Weapon = false;

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
        attackAction = playerControls.Player.Fire;
        jumpAction.Enable();
        moveAction.Enable();
        restartAction.Enable();
        attackAction.Enable();

        debugAction = playerControls.Debug.Toggle;
        debugAction.Enable();

    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        restartAction.Disable();
        attackAction.Disable();

        debugAction.Disable();
    }
    

    private void Update()
    {
        
    }

    public float HorizontalInput()
    {
        float moveInputX = moveAction.ReadValue<Vector2>().x;
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

    public bool AttackPressed()
    {
        return attackAction.triggered;
    }

    public bool ToggleWeapon()
    {

        return debugAction.IsPressed();
    }

    
}
