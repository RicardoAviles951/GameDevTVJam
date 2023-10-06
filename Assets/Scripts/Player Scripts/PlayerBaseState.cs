using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnCollisionEnter(PlayerStateManager player, Collision2D collision);
    public abstract void FixedUpdateState(PlayerStateManager player);
    public abstract void OnTriggerEnter(PlayerStateManager player, Collider2D other);
    
}
