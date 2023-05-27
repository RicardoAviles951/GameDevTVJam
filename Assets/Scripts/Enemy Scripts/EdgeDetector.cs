using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetector : MonoBehaviour
{
    [SerializeField] private EnemyPatroller patroller;

    public void OnTriggerExit2D(Collider2D collision)
    {
        //turn
        patroller.isMovingRight = ! patroller.isMovingRight;
    }
}