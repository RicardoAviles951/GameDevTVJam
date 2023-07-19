using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPath : MonoBehaviour
{
    public PlayerStateManager stateManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(attackPoint.position,attackRange);
        Vector2 pos = new Vector2(stateManager.attackRangeLength, stateManager.attackRangeHeight);
        Gizmos.DrawWireCube(stateManager.attackPoint.position, pos);
    }
}
