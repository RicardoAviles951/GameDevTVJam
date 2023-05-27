using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 leftPoint;
    private Vector2 rightPoint;
    [SerializeField] private float patrolDistance = 5.0f;
    [SerializeField] private float patrolSpeed = 2.0f;
    private float movement;

    public bool isMovingRight = true;


    void Awake()
    {
        startPosition = transform.position;
        leftPoint = startPosition + (Vector2.left*patrolDistance);
        rightPoint = startPosition + (Vector2.right*patrolDistance);
        
    }
    void Start()
    {
        
    }

    public Vector2 PatrolMovement(Rigidbody2D rb)
    {
        return new Vector2(movement,rb.velocity.y);
        
    }


    public void Patrol()
    {
        if (isMovingRight)
        {
            movement = patrolSpeed;
        }
        else
        {
            movement = -patrolSpeed;
        }

        // Check if reached the patrol point, then switch direction
        if (transform.position.x >= rightPoint.x)
        {
            isMovingRight = false;
        }
        else if (transform.position.x <= leftPoint.x)
        {
            isMovingRight = true;
        }
    }

    public void NewStartPosition()
    {
        startPosition = transform.position;
        leftPoint = startPosition + (Vector2.left*patrolDistance);
        rightPoint = startPosition + (Vector2.right*patrolDistance);
    }

    
}