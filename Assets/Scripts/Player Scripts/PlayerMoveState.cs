using System.Collections;
using System.Collections.Generic;
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
    private int pos = 0;

    public override void EnterState(PlayerStateManager player)
    {
        //Debug.Log("In Move State");
    }

   
    public override void UpdateState(PlayerStateManager player)
    {



        //Cache inputs
        bool isJumpInput    = player.playerInput.JumpInput();
        bool isJumpHeld     = player.playerInput.JumpInputHeld();
        bool isJumpButtonUp = player.playerInput.JumpButtonUp();
        bool isAttackInput  = player.playerInput.AttackPressed();
        //bool toggleDebug = player.playerInput.ToggleWeapon();

        

        moveX = player.playerInput.HorizontalInput();

        
        //Checks if player is on the ground
        isGrounded = CheckGround(player);

        //if(isAttackInput && isGrounded)
        //{
        //    if (toggleDebug)
        //    {
        //        player.SwitchState(player.attackState);
        //    }
        //    else if(toggleDebug == false)
        //    {
        //        player.SwitchState(player.whipState);
        //    }
            
        //}

        

        if(isGrounded)
        {
            if(buffered)
            {
                jumpInputBufferTime = jumpBufferTimer;
                jumpBufferTimer = 0;
                //bufferjump = true;
                if(jumpInputBufferTime < .2f)
                {
                    bufferjump = true;
                    
                }
                buffered = false;
            }
            coyoteTimer = player.coyoteTime;
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
       



        //Single Frame Check
        if (isJumpInput)
        {
            if(coyoteTimer > 0)
            {
                isJumping = true;
                jumpTimer = 0;
                player.rb.velocity = new Vector2(player.rb.velocity.x,player.jumpForce);
                SoundManager.Instance.PlaySound(player.jumpClip,1);
                isGrounded = false;
            }
            if(isGrounded == false)
            {
                buffered = true;
                float rnd = Random.Range(1.2f, 1.5f);
                SoundManager.Instance.PlaySound(player.jumpClip, rnd);

            }
             
        }

        if (isJumpHeld && isJumping)
        {
            if (jumpTimer < player.maxJumpTime)
            {
                player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if(isJumpHeld == false)
        {
            isJumping = false;
            
        }

        if(isJumpButtonUp)
        {
            coyoteTimer = 0;
        }

        

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
            else //Take damage and get knocked back
            {   
                
                float dir = Mathf.Sign(player.transform.position.x - collision.gameObject.transform.position.x);
                player.direction = dir;
                player.rb.velocity = new Vector2(0,0);
                player.playerHealth.TakeDamage(damage);
                player.SwitchState(player.knockbackState);
            }

        }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {

        
        //Takes in move direction from player input handler
        player.rb.velocity = new Vector2((moveX*player.speed) + slipSpeed,player.rb.velocity.y);


        if(bufferjump)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x,player.jumpForce*2);
            
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
    Debug.Log("is going up? " + isGoingUp);
    player.animator.SetBool("IsGoingUp", isGoingUp);

    player.animator.SetBool("onGround", isGrounded);
}

    

    
}
