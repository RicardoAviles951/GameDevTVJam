using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float chaseSpeed = 3.0f;
    [SerializeField] private float chaseRange = 10f;
    private float movement;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag assigned.");
        }
    }

    public bool isPlayerAttackable()
    {
        Vector2 playerPos = player.transform.position;
        float distanceBetween = Vector2.Distance(transform.position, playerPos);

        if(distanceBetween > attackRange)
        {
            if(playerPos.x < transform.position.x)
            {
               movement = -chaseSpeed; //Player on left
            }
            else
            {
               movement = chaseSpeed;//Player on right
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool isPlayerChaseable()
    {
        Vector2 playerPos = player.transform.position;
        float distanceBetween = Vector2.Distance(transform.position, playerPos);

         if(distanceBetween > chaseRange)
        {
            return false;
        }

        return true;

    }

    public Vector2 ChaseMovement(Rigidbody2D rb)
    {
        return new Vector2(movement,rb.velocity.y);
    }


}