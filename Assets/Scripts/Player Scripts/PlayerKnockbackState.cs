using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackState : PlayerBaseState
{
    float stateTimer = 0;
    float knockbackForce = 10.0f;
    public override void EnterState(PlayerStateManager player)
    {
        Vector2 force = new Vector2(knockbackForce*player.direction,4);
        player.rb.AddForce(force,ForceMode2D.Impulse);
        player.animator.SetBool("isDamaged", true);  
    }

    public override void UpdateState(PlayerStateManager player)
    {
        
       

        if(stateTimer >= .50f)
        {   
            stateTimer = 0;
            player.SwitchState(player.moveState);
            player.animator.SetBool("isDamaged", false);
        }

         stateTimer += Time.deltaTime;


        if(player.playerHealth.currentHealth <= 0)
        {
            player.SwitchState(player.deathState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        
    }

}
