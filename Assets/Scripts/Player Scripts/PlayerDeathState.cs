using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    //Restart of level is activated through player defeat animation event
    float deathTimer = 0;
    float deathTime = .5f;
    public override void EnterState(PlayerStateManager player)
    {
        //Stop music
        SoundManager.Instance.musicSource.Pause();
        //Player the defeat animation
        //Player is still experiencing knockback force
        player.animator.SetBool("IsDefeated", true);
        GameStateController.PizzasDropped += 1;
        Debug.Log("DEATH STATE");
        
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //Run a timer 
       if(deathTimer < deathTime)
       {
            deathTimer += Time.deltaTime;
       }
       else
       {    
            //Stop the player after knockback
            player.rb.velocity = Vector2.zero;
            deathTimer = 0;
       }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(PlayerStateManager player)
    {   

    }

    public override void OnTriggerEnter(PlayerStateManager player, Collider2D other)
    {
        
    }

   
}
