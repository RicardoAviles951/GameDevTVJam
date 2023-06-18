using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.rb.velocity = Vector2.zero;
        player.animator.SetBool("isAttacking", true);
        Debug.Log("Attack State");

        Vector2 size = new Vector2(player.attackRangeLength,player.attackRangeHeight);
        Collider2D[] enemiesToAttack = Physics2D.OverlapBoxAll(player.attackPoint.position,size,0);
        foreach (var enemyColliders in enemiesToAttack)
        {
            //Deal damage to enemies
            IDamageable enemy = enemyColliders.GetComponent<IDamageable>();
            if(enemy != null)
            {
                //player.animator.SetBool("")
                player.attackHitParticles.Play();
                enemy.TakeDamage(player.attackPower);
                player.StartCoroutine(player.hitStopController.HitStopCoroutine());
            }
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        bool animfinished = IsAnimationFinished(player);
        if(animfinished)
        {
            player.animator.SetBool("isAttacking", false);
            player.SwitchState(player.moveState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {
        
    }

    private bool IsAnimationFinished(PlayerStateManager player)
{
    AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
    return stateInfo.normalizedTime >= 1f; // Animation is considered finished if the normalized time is equal to or greater than 1
}

}
