using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerBaseState
{
    float moveX;
    private float jumpInputBufferTime = 0;
    private float jumpBufferTimer = 0;
    private bool buffered, bufferjump;
    private bool isGrounded;

    private float jumpTimer = 0;
    private bool isJumping;

    private float coyoteTimer;
    private float slipSpeed = 0;


    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In Move State");
    }

   
    public override void UpdateState(PlayerStateManager player)
    {



        //Cache inputs
        bool isJumpInput    = player.playerInput.JumpInput();
        bool isJumpHeld     = player.playerInput.JumpInputHeld();
        bool isJumpButtonUp = player.playerInput.JumpButtonUp();
        bool isAttackInput  = player.playerInput.AttackPressed();
        bool WeaponSwitched = player.playerInput.WeaponToggle();

        

        moveX = player.playerInput.HorizontalInput();

        if(WeaponSwitched)
        {
            SoundManager.Instance.PlaySound(player.SwitchSound);
            if(player.attackType == PlayerStateManager.AttackType.cutter)
            {
                player.attackType = PlayerStateManager.AttackType.whip;
            }
            else if(player.attackType == PlayerStateManager.AttackType.whip)
            {
                player.attackType = PlayerStateManager.AttackType.cutter;
            }
            if(player.hudRef != null)
            {
                player.hudRef.GetComponent<PlayerHudManager>().m_WeaponLabel.text = player.attackType.ToString().ToUpper();
            }
            
            Debug.Log(player.attackType.ToString());
        }
        



        //Checks if player is on the ground
        isGrounded = CheckGround(player);
        if(isAttackInput && isGrounded)
        {
            if(player.attackType == PlayerStateManager.AttackType.cutter)
            {
                player.SwitchState(player.attackState);
            }
            else if(player.attackType == PlayerStateManager.AttackType.whip)
            {
                player.SwitchState(player.whipState);
            }
            
        }

        //Jump input + Coyote 
        if (isJumpInput)
        {
            if (coyoteTimer > 0) //Checks if player has time to jump when not grounded
            {
                isJumping = true;
                jumpTimer = 0; //

                player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);//Applies upward change in velocity 
                SoundManager.Instance.PlaySound(player.jumpClip, 1);//Plays sound once
            }
            if (isGrounded == false)//If we press the jump key after while in the air after jumping, then we buffer the input.
            {
                buffered = true;

            }

        }

        //Variable Jump Height
        if (isJumpHeld && isJumping)//In the air after jumping and holding the jump button
        {
            //There is a certain window of time that the player has to hold onto the jump button to increase the total height of their jump.
            
            if (jumpTimer < player.maxJumpTime) 
            {
                player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce); //Keeps increasing upward velocity until the max jump time is reached. 
                jumpTimer += Time.deltaTime;//Increments jump timer until max is reached.
            }
            else //If the max jump time has been reached, then the player is no longer able to increase their jump height.
            {
                isJumping = false;
            }
        }
        if (isJumpHeld == false) //prevents double jump
        {
           isJumping = false;

        }

        if (isJumpButtonUp)//prevents double jump from coyote time
        {
            coyoteTimer = 0;
        }
        //Input Buffering 
        if (isGrounded)
        {
            if(buffered) //Player has hit the ground and buffered is flagged
            {
                jumpInputBufferTime = jumpBufferTimer;
                jumpBufferTimer = 0;//Reset the timer that increments when you press the jump button before landing.
                if(jumpInputBufferTime < .2f)//Actually checks if the buffered jump will process.
                {
                    bufferjump = true;
                }

                buffered = false;
                
            }
            coyoteTimer = player.coyoteTime;

            TrailReset(player);
        }
        else 
        {
            coyoteTimer -= Time.deltaTime;
        }



        if(buffered)
        {
            jumpBufferTimer += Time.deltaTime;
        }
        FallHandler(player);
       



        

        

        

        if(player.isSlipping)
        {
            player.speed = 5f;
            if(moveX != 0)
            {
                slipSpeed = player.slipModifier;
                slipSpeed *= moveX;
            }
        }
        else
        {
            slipSpeed = 0;
            player.speed = 15f;
        }

        if(player.goombaJump)
        {
            Vector2 gooForce = new Vector2(player.rb.velocity.x,player.goombaJumpForce);
            player.rb.AddForce(gooForce,ForceMode2D.Impulse);
            player.goombaJump = false;
        }
       

        // Animation parameters
        UpdateAnimationParameters(player, isGrounded);

        // Flip sprite when running
        FlipSprite(player);

        // // Slip and speed modifiers
        // HandleSlip(player, moveX);
        //Debug.Log("Player Y vel: " + player.rb.velocity.y);
        
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {

            
            int damage = collision.gameObject.GetComponent<EnemyBaseClass>().damage;
            //Cache Collider bounds
            Bounds enemyBounds  =  collision.collider.bounds;
            Bounds playerBounds =  player.playerCollider.bounds;
            //Cache contact points
            float playerY = player.transform.position.y - 1.5f; 
            float enemyY = collision.gameObject.transform.position.y;
            //Perform checks
            bool isAbove = playerY > enemyY;
            bool isWithinBounds = ((playerBounds.max.x >= enemyBounds.min.x-.25f) || (playerBounds.min.x <= enemyBounds.max.x+.25));

            if(isAbove && isWithinBounds) //If above enemy, goomba stomp
            {
                player.rb.velocity = new Vector2(player.rb.velocity.x,0);
                player.goombaJump = true;
                damageable.Kill(collision.gameObject);
            }
            else if(player.isInvulnerable == true)
            {
                player.attackHitParticles.Play();
                damageable.Kill(collision.gameObject);
            }
            else //Take damage and get knocked back
            {   
                
                float dir = Mathf.Sign(player.transform.position.x - collision.gameObject.transform.position.x);
                player.direction = dir;
                player.rb.velocity = new Vector2(0,0);
                player.playerHealth.TakeDamage(damage);
                player.SwitchState(player.knockbackState);
                //bhhDebug.Log("OOPS");
            }

        }


        var knots = GameObject.Find("GarlicKnots");
        if(collision.gameObject == knots)
        {
           var garlic =  knots.GetComponent<GarlicKnotManager>();
            garlic.Active = false;
            garlic.FireParticles();
            garlic.MoveOutPosition();
            player.isInvulnerable = true;
            SoundManager.Instance.PlaySound(player.crunchSound, 1);
        }
        
        
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {

        
        //Takes in move direction from player input handler
        player.rb.velocity = new Vector2((moveX*player.speed) + slipSpeed,player.rb.velocity.y);


        if(bufferjump)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x,player.jumpForce*2);
            Debug.Log("SUPER JUMP");
            SoundManager.Instance.PlaySound(player.superJumpClip, 1);
            player.StartCoroutine(WaitAFrame(player));
            bufferjump = false;
        }

    }

    private bool CheckGround(PlayerStateManager player)
    {
        //Checking the ground
        RaycastHit2D hit = Physics2D.Raycast(player.groundCheck.position, Vector2.down, 0.1f, player.groundLayer);

        // If the raycast hits a ground object, consider the character grounded
        return hit.collider != null;
    }
   
private void FallHandler(PlayerStateManager player)
{
     //This code checks if the player is on their way down, if yes then fall faster. If not, fall regularly.
    if(player.rb.velocity.y < 0)
    {
        player.rb.gravityScale = 5.0f + player.fallModifier;
    }
    else
    {
        player.rb.gravityScale = 5.0f;
    }
 }
    
private void HandleJumping(PlayerStateManager player, bool isGrounded, bool isJumpInput, bool isJumpHeld, bool isJumpButtonUp)
{
    if (isJumpInput)
    {
        if (isGrounded || coyoteTimer > 0)
        {
            StartJump(player);
        }
        else
        {
            buffered = true;
        }
    }

    if (isJumpHeld && isJumping)
    {
        if (jumpTimer < player.maxJumpTime)
        {
            ContinueJump(player);
        }
        else
        {
            isJumping = false;
        }
    }

    if (!isJumpHeld)
    {
        isJumping = false;
    }

    if (isJumpButtonUp)
    {
        coyoteTimer = 0;
    }

    
}

private void StartJump(PlayerStateManager player)
{
    isJumping = true;
    jumpTimer = 0;
    player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
    isGrounded = false;
}

private void ContinueJump(PlayerStateManager player)
{
    player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
    jumpTimer += Time.deltaTime;
}

private void HandleSlip(PlayerStateManager player, float moveX)
{
    if (player.isSlipping)
    {
        player.speed = 5f;

        if (moveX != 0)
        {
            player.slipModifier = 5.0f * moveX;
        }
        else
        {
            player.slipModifier = 0;
        }
    }
    else
    {
        player.slipModifier = 0;
        player.speed = 15f;
    }
}

private void FlipSprite(PlayerStateManager player)
{
    if (player.rb.velocity.x != 0)
    {
        Vector3 scale = player.transform.localScale;
        scale.x = Mathf.Sign(player.rb.velocity.x);
        player.transform.localScale = scale;
    }
}

private void UpdateAnimationParameters(PlayerStateManager player, bool isGrounded)
{
    bool isRunning = player.rb.velocity.x != 0 && isGrounded;
    
    player.animator.SetBool("isRunning", isRunning);

    bool isGoingUp = player.rb.velocity.y > 0;
    //Debug.Log("is going up? " + isGoingUp);
    player.animator.SetBool("IsGoingUp", isGoingUp);

    player.animator.SetBool("onGround", isGrounded);
}

    public override void OnTriggerEnter(PlayerStateManager player, Collider2D other)
    {
        
        if(other.gameObject.tag == "Coin") //Check if coin object
        {
            if(player.coinCollected == false)//Check to make sure coin has not been collected already
            {
                Debug.Log("Collected a coin!");
                SoundManager.Instance.PlaySound(RandomClip(player));//Play coin pickup sound
                other.gameObject.GetComponent<CoinManager>().KillCoin();//Destroy coin
                GameStateController.CoinsCollected += 1;//Add to coin counter 
                //Send message to UI to update 
                player.UICoinCounter.Invoke();
            }
            
        }
    }

    private AudioClip RandomClip(PlayerStateManager player)//Picks a random clip
    {
       int clip = Random.Range(0, player.coinSounds.Count);
        return player.coinSounds[clip];
    }

    private void TrailColor(PlayerStateManager player)
    {
        player.trail.startColor = Color.green;
        player.trail.startWidth = 1;
    }

    private void TrailReset(PlayerStateManager player)
    {
        player.trail.startColor = Color.white;
        player.trail.startWidth = .25f;
    }
    private IEnumerator WaitAFrame(PlayerStateManager player)
    {
        Debug.Log("Waiting one frame...");
        yield return new WaitForSeconds(.10f);
        TrailColor(player);
    }
}
