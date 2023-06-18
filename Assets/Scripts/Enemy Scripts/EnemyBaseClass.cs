using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    public int damage = 1;
    [SerializeField] protected int maxHP = 10;
    [SerializeField] protected float moveSpeed;
    protected Rigidbody2D rb;
    [SerializeField] protected int currentHP;
    protected bool isStomped;

    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Knockback
    }

    protected EnemyState currentState;

    // Abstract methods for different behaviors
    protected abstract void IdleBehavior();
    protected abstract void PatrolBehavior();
    protected abstract void ChaseBehavior();
    protected abstract void AttackBehavior();
    protected abstract void KnockbackBehavior();

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
            case EnemyState.Knockback:
                KnockbackBehavior();
                break;
        }
    }

    
    
    protected abstract void Attack(int damageAmount);

    protected abstract void Die();

    protected abstract void Move();

}
