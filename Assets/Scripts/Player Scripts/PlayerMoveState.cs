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

    private bool isgoingright = false;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In Move State");
    }

   
    public override void UpdateState(PlayerStateManager player)
    {

        
        moveX = player.playerInput.HorizontalInput();
        

        //Checking the ground
        RaycastHit2D hit = Physics2D.Raycast(player.groundCheck.position, Vector2.down, 0.1f, player.groundLayer);

        // If the raycast hits a ground object, consider the character grounded
        isGrounded = hit.collider != null;

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
            player.animator.SetBool("onGround", true);
        }
        else 
        {
            player.animator.SetBool("onGround", false);
            coyoteTimer -= Time.deltaTime;
        }



        if(buffered)
        {
            jumpBufferTimer += Time.deltaTime;
        }

         //This code checks if the player is on their way down, if yes then fall faster. If not, fall regularly.
        if(player.rb.velocity.y < 0)
        {
            player.rb.gravityScale = 5.0f + player.fallModifier;
        }
        else
        {
            player.rb.gravityScale = 5.0f;
        }



        //Single Frame Check
        if (player.playerInput.JumpInput())
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

        if (player.playerInput.JumpInputHeld() && isJumping)
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
        if(player.playerInput.JumpInputHeld() == false)
        {
            isJumping = false;
            
        }

        if(player.playerInput.JumpButtonUp())
        {
            coyoteTimer = 0;
        }

        //Debug.Log("Y VEL = " + player.rb.velocity.y);
        
        //Stores if character is moving horizontally and sets the animator parameter.
        bool isRunning = player.rb.velocity.x != 0;
        player.animator.SetBool("isRunning", isRunning);

        //bool isgoingup = player.rb.velocity.y > 0;
        bool isgoingup = player.rb.velocity.y > 0 && isJumping;
        player.animator.SetBool("IsGoingUp", isgoingup);
        
        //This code flips the 2D sprite when running right and left.
        if (isRunning)
        {
            Vector3 scale = player.transform.localScale;
            scale.x = Mathf.Sign(player.rb.velocity.x);
            player.transform.localScale = new Vector3(scale.x,player.transform.localScale.y,player.transform.localScale.z);
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
        
        
    Debug.Log("move dir "+ moveX);
        
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
            player.rb.velocity = new Vector2(player.rb.velocity.x,player.jumpForce);
            bufferjump = false;
        }

    }
   

    

    
}
