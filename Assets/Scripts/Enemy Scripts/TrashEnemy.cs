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
    private float timer;
    private float time = 2.0f;
    private Animator anim;

    #region INTERFACE METHODS

    public void TakeDamage(int damage)
    {
        // Implement the behavior for the enemy taking damage
        // ...
    }
    #endregion 
    
    #region MONOBEHAVIORS
    private void Awake()
    {
        detector  = GetComponent<PlayerDetector>();
        patroller = GetComponent<EnemyPatroller>();
        chaser    = GetComponent<ChasePlayer>();
        anim = GetComponent<Animator>();

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
        // Check if collided with an object that implements IDamageable
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Deal damage to the damageable object
            damageable.TakeDamage(damage);
            print("HIT DAMAGE");
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStateManager>().direction = detector.hitdirection();
        }

    }
    #endregion

    #region STATE MACHINE METHODS
    protected override void IdleBehavior()
    {
        print("Currently Idle");
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
        print("Currently patrolling");
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
        print("Currently Chasing");
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
        //Stores if character is moving horizontally and sets the animator parameter.
        bool isRunning = rb.velocity.x != 0;
        anim.SetBool("isRunning", isRunning);

        //This code flips the 2D sprite when running right and left.
        if (isRunning)
        {
            //selfScale.x = Mathf.Sign(rb.velocity.x);
            transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x)*2,transform.localScale.y, transform.localScale.z);
        }
    }

    public float SideofPlayer()
    {
        return detector.hitdirection();
    }

    
}