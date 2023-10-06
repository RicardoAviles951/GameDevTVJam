using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        //Play attack sound
        SoundManager.Instance.PlaySound(player.cutterSound);
        //Stop player movement
        player.rb.velocity = Vector2.zero;

        //Set hitbox size
        player.attackRangeLength = 1;
        player.attackRangeHeight = 1;

        //Set animator bool
        player.animator.SetBool("isAttackCutter", true);

        //Debug Information
        Debug.Log("Cutter Attack State");
        Debug.Log("Cutter Attacking ?" + player.Attacking);

        
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //Define size of hitbox
        Vector2 size = new Vector2(player.attackRangeLength, player.attackRangeHeight);
        //Store colliders of everything colliding with hitbox
        Collider2D[] enemiesToAttack = Physics2D.OverlapBoxAll(player.attackPoint.position, size, 0);

        //Loops through all objects to check if enemies were hit
        foreach (Collider2D enemyColliders in enemiesToAttack)
        {
            //Deal damage to enemies
            IDamageable enemy = enemyColliders.GetComponent<IDamageable>();
            if (enemy != null)
            {
                //player.animator.SetBool("")
                if(player.Attacking == false)
                {
                    player.attackHitParticles.Play();
                    player.cam.GetComponent<CameraManager>().CameraShake(player.shakeDur, player.shakeMag);
                    enemy.TakeDamage(player.attackPower);
                    player.StartCoroutine(player.hitStopController.HitStopCoroutine());
                    SoundManager.Instance.PlaySound(player.damageSound, .75f);
                    player.Attacking = true;
                }
                
            }
            else //No enemies hit
            {
                Debug.Log("No enemy hit");
            }
        }
        //Checks if animation is finished
        bool animFinished = IsAnimationFinished(player);
        if(animFinished)
        {
            player.animator.SetBool("isAttackCutter", false);
            player.Attacking = false;
            player.SwitchState(player.moveState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        //Nothing here
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision2D collision)
    {
        //Nothing here
    }

    private bool IsAnimationFinished(PlayerStateManager player)
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f; // Animation is considered finished if the normalized time is equal to or greater than 1
    }

    public override void OnTriggerEnter(PlayerStateManager player, Collider2D other)
    {
        throw new System.NotImplementedException();
    }
}
