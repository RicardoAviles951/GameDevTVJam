using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int maxHP;
    [SerializeField] protected float moveSpeed;
    protected Rigidbody2D rb;
    protected int currentHP;

    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    protected EnemyState currentState;

    // Abstract methods for different behaviors
    protected abstract void IdleBehavior();
    protected abstract void PatrolBehavior();
    protected abstract void ChaseBehavior();
    protected abstract void AttackBehavior();

    protected virtual void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();

    }
    protected virtual void Update()
    {
        // Update behavior based on the current state
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleBehavior();
                break;
            case EnemyState.Patrol:
                PatrolBehavior();
                break;
            case EnemyState.Chase:
                ChaseBehavior();
                break;
            case EnemyState.Attack:
                AttackBehavior();
                break;
        }
    }

    
    
    protected abstract void Attack(int damageAmount);

    protected abstract void Die();

    protected abstract void Move();

}
