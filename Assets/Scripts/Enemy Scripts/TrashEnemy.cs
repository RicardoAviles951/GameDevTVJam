using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEnemy : EnemyBaseClass, IDamageable
{
    //reference to player health
    [SerializeField] private PlayerHealth playerHealth;
    private IDamageable player;
    private PlayerDetector detector;
    private EnemyPatroller patroller;
    private ChasePlayer chaser;
    private GroundChecker groundChecker;
    private float timer;
    private float time = 2.0f;
    private Animator anim;
    private Collider2D _collider;
    [Header("Knockback Parameters")]
    [SerializeField] float stunTime = 0.5f;
    private float stunTimer = 0;
    [SerializeField] float knockbackX = 10.0f;
    [SerializeField] float knockbackY = 10.0f;
    private bool isStunned = false;
    public ParticleSystem DeathparticleReference;
    private ParticleSystem DeathParticles;
    [SerializeField] AudioClip deathSound;


    #region INTERFACE METHODS

    public void TakeDamage(int damage)
    {
        var healthRemaining = currentHP - damage;

        if(healthRemaining > 0)
        {
            currentHP -= damage;
            timer = 0;
            currentState = EnemyState.Knockback;
        }
        else
        {
            Kill(gameObject);
        }

        //if(currentHP > 0)
        //{
        //    currentHP -= damage;
        //    timer = 0;
        //    currentState = EnemyState.Knockback;
        //} 
        //else if(currentHP <= 0)
        //{
        //    Kill(gameObject);
        //}
        
    }

    public void Kill(GameObject self)
    {
        //Play Particle Effect
        DeathParticles.transform.position = gameObject.transform.position;
        DeathParticles.Play();
        //Destroy Enemy Object
        SoundManager.Instance.PlaySound(deathSound);
        Destroy(self);

    }
    #endregion 
    
    #region MONOBEHAVIORS
    private void Awake()
    {
        currentHP     = maxHP;
        detector      = GetComponent<PlayerDetector>();
        patroller     = GetComponent<EnemyPatroller>();
        chaser        = GetComponent<ChasePlayer>();
        anim          = GetComponent<Animator>();
        _collider     = GetComponent<Collider2D>();
        groundChecker = GetComponent<GroundChecker>();

        if (detector == null)
        {
            Debug.LogError("PlayerDetector component not found. Make sure the PlayerDetector script is attached to the same GameObject.");
        }
    }
    protected override void Start()
    {
        base.Start();
        // Additional initialization specific to GruntEnemy
        currentState = EnemyState.Idle;
        timer = 0;
        GameObject particles = GameObject.Find("EnemyDeath");
        if (particles == null)
        {
            Debug.Log("Enemy death particles created...");
            DeathParticles = Instantiate(DeathparticleReference); //Create the particles if they don't exist yet
            DeathParticles.transform.position = gameObject.transform.position;// Set to position of enemy
        }
        else
        {
            Debug.Log("Enemy death particles already exist!");
        }
        
    }

    protected override void Update()
    {
        //Runs state machine
        base.Update();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


    }
    #endregion

    #region STATE MACHINE METHODS
    protected override void IdleBehavior()
    {
//        print("Currently Idle");
        //Checking if player nearby
        if(detector.IsPlayerDetected())
        {
            //Go to Attack mode
            //currentState = EnemyState.Attack;
            currentState = EnemyState.Chase;
        }

        //Randomly Decides to enter patrol state
        StateTimer(EnemyState.Patrol);
        
    }

    protected override void PatrolBehavior()
    {
       //print("Currently patrolling");
        //Patrolling
        patroller.Patrol();
        //Checking if player in range
        if(detector.IsPlayerDetected())
        {
            //Go to Attack mode
            currentState = EnemyState.Chase;
        }
    }

    protected override void ChaseBehavior()
    {
//        print("Currently Chasing");
        if(chaser.isPlayerAttackable()) // Checks if player is in attack range
        {
            currentState = EnemyState.Attack;
        }
        else if(chaser.isPlayerChaseable() == false) //Check if player left detection range
        {
            currentState = EnemyState.Idle;
        }
    }

    protected override void AttackBehavior()
    {
        print("Currently attacking");
        //Play attack animations 
        
        //Check Hitboxes

        //Call Attack();



        if(detector.IsPlayerDetected() == false) //Checking if player has left detection range
        {
            //Go to idle
            currentState = EnemyState.Idle;
        }
    }

    protected override void KnockbackBehavior()
    {
        Vector2 knockforce = new Vector2(knockbackX*detector.hitdirection(),knockbackY);
        if(isStunned == false)
        {
            rb.velocity = new Vector2(0,0);
            rb.AddForce(knockforce,ForceMode2D.Impulse);
            isStunned = true;
        }
        
        if(stunTimer < stunTime)
        {
            stunTimer += Time.deltaTime;
        }
        else
        {
            stunTimer = 0;
            isStunned = false;
            currentState = EnemyState.Idle;
        }
        
    }
    
    #endregion

    //BEHAVIOR METHODS
    protected override void Die()
    {
        // Implement GruntEnemy death logic here
        Debug.Log("GruntEnemy dies!");
        Destroy(gameObject); // Optional: Destroy the GameObject when the enemy dies
    }

    protected override void Move()
    {
        // Implement GruntEnemy movement logic here
        //Debug.Log("GruntEnemy moves!");

        if(currentState == EnemyState.Patrol)
        {
            rb.velocity = patroller.PatrolMovement(rb);
        }
        else if(currentState == EnemyState.Chase)
        {
            rb.velocity = chaser.ChaseMovement(rb);
        }
    }

    protected override void Attack(int damageAmount)
    {
        // Implement GruntEnemy attack logic here
        Debug.Log("GruntEnemy attacks!");
        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
    // -------------------------------------------------------------------------

    //Timer for leaving states at random intervals
    void StateTimer(EnemyState stateToEnter)
    {
        if(timer >= time)
        {
            int rand = Random.Range(1,5);
            if(rand == 4)
            {
                currentState = stateToEnter;
            }
            timer = 0;
        }


        timer += Time.deltaTime;
    }

    void UpdateAnimation()
    {
        bool isgrounded = groundChecker.CheckGround();
        //Stores if character is moving horizontally and sets the animator parameter.
        bool isRunning = rb.velocity.x != 0;
        anim.SetBool("isRunning", isRunning);

        //This code flips the 2D sprite when running right and left.
        if (isRunning)
        {
            //selfScale.x = Mathf.Sign(rb.velocity.x);
            transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x),transform.localScale.y, transform.localScale.z);
        }

        if(isgrounded)
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }
    }

    
}