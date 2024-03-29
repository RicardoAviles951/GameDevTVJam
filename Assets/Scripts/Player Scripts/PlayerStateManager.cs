using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerStateManager : MonoBehaviour
{
    public enum AttackType
    {
        cutter,
        whip
    }

    PlayerBaseState currentState;
    public PlayerIdleState idleState           = new PlayerIdleState();
    public PlayerMoveState moveState           = new PlayerMoveState();
    public PlayerAttackState attackState       = new PlayerAttackState();
    public PlayerWhipAttackState whipState     = new PlayerWhipAttackState();
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
    public AudioClip jumpClip;
    public AudioClip superJumpClip;
    public AudioClip stepClip;
    public ParticleSystem stepDust;
    private ParticleSystem dustobj;

    [Header("Knockback State Variables")]
    public float horizontalKnockbackForce = 10f;
    public float verticalKnockbackForce = 5.0f;
    public float stunDuration = 0.5f;
    public AudioClip damageSound;
    public AudioClip splatSound;

    [Header("Attack Variables")]
    public int attackPower = 5;
    public float attackTime = 0.3f;
    public Transform attackPoint;
    public float attackRangeLength = 1;
    public float attackRangeHeight = 1;
    public ParticleSystem attackHitParticles;
    [HideInInspector]
    public HitStopController hitStopController;
    public AudioClip cutterSound;
    public AudioClip whipSound;
    public AudioClip SwitchSound;
    public bool Attacking { get; set; } = false;
    public AttackType attackType { get; set; } = AttackType.cutter;
    [HideInInspector]
    public GameObject hudRef;
    //[HideInInspector]
    [Header("Invulnerability")]
    public bool isInvulnerable = false;
    [SerializeField] private float invulnerableTimer = 0;
    [SerializeField] private float defaultTime = 10.0f; //10 Seconds
    [SerializeField] private bool timerEnabled = true;
    public ParticleSystem partRef;
    private ParticleSystem invulnerableParticles;
    private bool particlesPlaying = false;
    public AudioClip crunchSound;
    [HideInInspector]
    public Camera cam;
    public float shakeDur = .25f;
    public float shakeMag = .25f;
    public AudioClip coinSound1, coinSound2, coinSound3;
    public List<AudioClip> coinSounds = new List<AudioClip>();
    [HideInInspector] public bool coinCollected = false;
    public UnityEvent UICoinCounter;
    [HideInInspector] public TrailRenderer trail;


    // Start is called before the first frame update
    void Start()
    {
        GameObject dustPoint = GameObject.Find("Dust");
        dustobj = Instantiate(stepDust,dustPoint.transform);
        invulnerableTimer = defaultTime;
        playerCollider    = GetComponent<Collider2D>();
        playerInput       = GetComponent<PlayerInputHandler>();
        rb                = GetComponent<Rigidbody2D>();
        animator          = GetComponent<Animator>();
        playerHealth      = GetComponent<PlayerHealth>();
        hitStopController = GetComponent<HitStopController>();
        cam = Camera.main;
        coinSounds.Add(coinSound1);
        coinSounds.Add(coinSound2);
        coinSounds.Add(coinSound3);
        trail = GetComponent<TrailRenderer>();

        groundLayer        = LayerMask.GetMask("Ground");
        attackHitParticles = Instantiate(attackHitParticles,attackPoint);
        hudRef             = GameObject.Find("PlayerHUD");

        if(invulnerableParticles == null)
        {
            var playerPos = gameObject.transform;
            invulnerableParticles = Instantiate(partRef, playerPos);
        }

        currentState       = moveState;
        currentState.EnterState(this);
       // Debug.Log("Current State: " +  currentState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        if(isInvulnerable == true)
        {
            //Play Particle Effect
            if(particlesPlaying == false)
            {
                invulnerableParticles.Play();
                particlesPlaying = true;
            }
            
            //Play song maybe
            //Decrement Timer
            if(timerEnabled == true)
            {
                if (invulnerableTimer > 0)
                {
                    invulnerableTimer -= Time.deltaTime;
                }
                else
                {
                    isInvulnerable = false;
                    invulnerableTimer = defaultTime;
                }
                //Debug.Log("Invulnerable Timer enabled!");
                //Debug.Log("Invulnerable timer: " + invulnerableTimer.ToString());
            }
            else
            {
                //Debug.Log("Invulnerable Timer NOT enabled!");
            }
            
        }
        else
        {
            invulnerableParticles.Stop();
            particlesPlaying = false;
        }
        //Debug.Log(attackType.ToString());
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter(this, collision);
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

    public void PlayStepSound()
    {
        //Creates variation by changing the pitch every call
        float rnd = Random.Range(.9f, 1.3f);
        SoundManager.Instance.PlaySound(stepClip,rnd);
        
      
    }

    public void ReleaseDust()
    {
        dustobj.Play();
        //Debug.Log("Dust released");
    }

    public void PizzaSplat()
    {
        SoundManager.Instance.PlaySound(splatSound);
    }
}

