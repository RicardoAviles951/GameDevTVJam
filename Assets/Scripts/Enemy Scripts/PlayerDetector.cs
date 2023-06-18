using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float detectionDistanceX = 5.0f;
    [SerializeField] private float detectionDistanceY = 2.0f;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag assigned.");
        }
    }

    public bool IsPlayerDetected()
    {
        if (player != null)
        {
            //Cache positions
            Vector2 playerPos = player.transform.position;
            Vector2 enemyPos = transform.position;

            //Calculate distances between x and y 
            float distanceX = Mathf.Abs(enemyPos.x - playerPos.x); 
            float distanceY = Mathf.Abs(enemyPos.y - playerPos.y);

            //Check if player is within range
            if(distanceX <= detectionDistanceX && distanceY <= detectionDistanceY)
            {
                //print("Y Distance: " + distanceY);
                return true;
            }
        }

        return false;
    }

    public float hitdirection()
    {
        //Cache positions
            Vector2 playerPos = player.transform.position;
            Vector2 enemyPos = transform.position;

            float dir = Mathf.Sign(enemyPos.x - playerPos.x);
            return dir;
    }
}