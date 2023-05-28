using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.animator.SetBool("IsDefeated", true);
    }

    public override void UpdateState(PlayerStateManager player)
    {

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        
    }

}
