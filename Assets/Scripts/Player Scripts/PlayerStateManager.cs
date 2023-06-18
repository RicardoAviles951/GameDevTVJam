using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{

    PlayerBaseState currentState;
    public PlayerIdleState idleState           = new PlayerIdleState();
    public PlayerMoveState moveState           = new PlayerMoveState();
    public PlayerAttackState attackState       = new PlayerAttackState();
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
    public float fallModifier = 8.0f;
    public float maxJumpTime = 0.2f;
    public float slipModifier = 5.0f;
    [HideInInspector]
    public bool isSlipping = false;

    public float coyoteTime = 1.0f;
    public float goombaJumpForce = 20.0f;
    public Transform groundCheck;
    [HideInInspector]
    public LayerMask groundLayer;
    [HideInInspector]
    public float direction;
    [HideInInspector]
    public bool goombaJump = false;
    [HideInInspector]
    public Collider2D playerCollider;

    [Header("Knockback State Variables")]
    public float horizontalKnockbackForce = 10f;
    public float verticalKnockbackForce = 5.0f;
    public float stunDuration = 0.5f;

    [Header("Attack Variables")]
    public int attackPower = 5;
    public float attackTime = 0.3f;
    public Transform attackPoint;
    public float attackRangeLength = 1;
    public float attackRangeHeight = 1;
    public ParticleSystem attackHitParticles;
    [HideInInspector]
    public HitStopController hitStopController;


    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerInput  = GetComponent<PlayerInputHandler>();
        rb           = GetComponent<Rigidbody2D>();
        animator     = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        hitStopController = GetComponent<HitStopController>();
        groundLayer = LayerMask.GetMask("Ground");
        attackHitParticles = Instantiate(attackHitParticles,attackPoint);


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

    // void OnDrawGizmos()
    // {
    //     Vector3 posR = new Vector3(transform.position.x,transform.position.y - playerCollider.bounds.extents.y+.50f,0);
    //     Gizmos.DrawCube(posR,new Vector3(.2f,.2f,0));
        
    // }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(attackPoint.position,attackRange);
        Vector2 pos = new Vector2(attackRangeLength,attackRangeHeight);
        Gizmos.DrawWireCube(attackPoint.position,pos);
    }

}

