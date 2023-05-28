using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputAction moveAction, jumpAction;
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
        jumpAction.Enable();
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }
    

    private void Update()
    {
        
    }

    public float HorizontalInput()
    {
        float moveInputX = moveAction.ReadValue<Vector2>().x;
        print(moveInputX);
        return moveInputX;
    }
    
     public bool JumpInput()
    {
        return jumpAction.triggered;
    }

}
