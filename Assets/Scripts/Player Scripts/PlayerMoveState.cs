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

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In Move State");
    }

   
    public override void UpdateState(PlayerStateManager player)
    {
       moveX = player.playerInput.HorizontalInput();
        //Stores if character is moving horizontally and sets the animator parameter.
        bool isRunning = player.rb.velocity.x != 0;
        player.animator.SetBool("isRunning", isRunning);

        bool isgoingup = player.rb.velocity.y > 0;
        player.animator.SetBool("IsGoingUp", isgoingup);
        if(isGrounded)
        {
            player.animator.SetBool("onGround", true);
        }
        else 
        {
            player.animator.SetBool("onGround", false);
        }

        //This code flips the 2D sprite when running right and left.
        if (isRunning)
        {
            Vector3 scale = player.transform.localScale;
            scale.x = Mathf.Sign(player.rb.velocity.x);
            player.transform.localScale = new Vector3(scale.x,player.transform.localScale.y,player.transform.localScale.z);
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


        //Checking the ground
        RaycastHit2D hit = Physics2D.Raycast(player.groundCheck.position, Vector2.down, 0.1f, player.groundLayer);

        // If the raycast hits a ground object, consider the character grounded
        isGrounded = hit.collider != null;

        if(isGrounded && buffered)
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

        if (player.playerInput.JumpInput())
        {
            if(isGrounded)
            {
                player.rb.velocity = new Vector2(player.rb.velocity.x,player.jumpForce);
                isGrounded = false;
            }
            else
            {
                buffered = true;
            }
             
        }

        
        
        
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
        player.rb.velocity = new Vector2(moveX*player.speed,player.rb.velocity.y);

        if(bufferjump)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x,player.jumpForce);
            bufferjump = false;
        }

    }
   

    

    
}
