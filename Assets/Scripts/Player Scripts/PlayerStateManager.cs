using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{

    PlayerBaseState currentState;
    public PlayerIdleState idleState           = new PlayerIdleState();
    public PlayerMoveState moveState           = new PlayerMoveState();
    public PlayerKnockbackState knockbackState = new PlayerKnockbackState();
    public PlayerDeathState deathState         = new PlayerDeathState();

    [HideInInspector]
    public PlayerInputHandler playerInput;

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public PlayerHealth playerHealth;

    [Header("Move State Variables")]
    public float speed = 10f;
    public float jumpForce = 20.0f;
    public float fallModifier = 6.0f;
    public float maxJumpTime = 1.5f;

    public float coyoteTime = 1.0f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    [HideInInspector]
    public float direction;



    // Start is called before the first frame update
    void Start()
    {
        playerInput  = GetComponent<PlayerInputHandler>();
        rb           = GetComponent<Rigidbody2D>();
        animator     = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();


        currentState = moveState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

   public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

}

