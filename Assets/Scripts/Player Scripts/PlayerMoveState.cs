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
        
        moveX = player.playerInput.HorizontalInput();

        
        //Checks if player is on the ground
        isGrounded = CheckGround(player);

        

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
                
                isGrounded = false;
            }
            if(isGrounded == false)
            {
                buffered = true;
               
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
                player.slipModifier = 5.0f;
                player.slipModifier *= moveX;
            }
        }
        else{
            player.slipModifier = 0;
            player.speed = 15f;
        }
       

        // Animation parameters
        UpdateAnimationParameters(player, isGrounded);

        // Flip sprite when running
        FlipSprite(player);

        // // Slip and speed modifiers
        // HandleSlip(player, moveX);
        
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            player.SwitchState(player.knockbackState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {

        
        //Takes in move direction from player input handler
        player.rb.velocity = new Vector2((moveX*player.speed) + player.slipModifier,player.rb.velocity.y);


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
    bool isRunning = player.rb.velocity.x != 0;
    player.animator.SetBool("isRunning", isRunning);

    bool isGoingUp = player.rb.velocity.y > 0 && isJumping;
    player.animator.SetBool("IsGoingUp", isGoingUp);

    player.animator.SetBool("onGround", isGrounded);
}

    
}
