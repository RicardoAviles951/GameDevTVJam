using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWhipAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        //Play attack sound
        SoundManager.Instance.PlaySound(player.whipSound);
        //Stop player movement
        player.rb.velocity = Vector2.zero;

        //Set hitbox size
        player.attackRangeLength = 2;
        player.attackRangeHeight = 2;

        //Set animator bool
        player.animator.SetBool("isAttackWhip", true);

        //Debug Information
        Debug.Log("Whip Attack State");
        Debug.Log("Whip Attacking ?" + player.Attacking);
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        //Nothing here
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {
        //Nothing here
    }

    public override void OnTriggerEnter(PlayerStateManager player, Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        Vector2 size = new Vector2(player.attackRangeLength, player.attackRangeHeight);
        Collider2D[] enemiesToAttack = Physics2D.OverlapBoxAll(player.attackPoint.position, size, 0);
        foreach (Collider2D enemyColliders in enemiesToAttack)
        {
            //Deal damage to enemies
            IDamageable enemy = enemyColliders.GetComponent<IDamageable>();
            if (enemy != null)
            {
                //player.animator.SetBool("")
                if (player.Attacking == false)
                {
                    player.attackHitParticles.Play();
                    player.cam.GetComponent<CameraManager>().CameraShake(player.shakeDur, player.shakeMag);
                    enemy.TakeDamage(player.attackPower);
                    player.StartCoroutine(player.hitStopController.HitStopCoroutine());
                    SoundManager.Instance.PlaySound(player.damageSound, .75f);
                    player.Attacking = true;
                }

            }
            else
            {
                Debug.Log("No enemy hit");
            }
        }
        bool animFinished = IsAnimationFinished(player);
        if (animFinished)
        {
            player.animator.SetBool("isAttackWhip", false);
            player.Attacking = false;
            player.SwitchState(player.moveState);
        }
    }

    private bool IsAnimationFinished(PlayerStateManager player)
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f; // Animation is considered finished if the normalized time is equal to or greater than 1
    }

}
