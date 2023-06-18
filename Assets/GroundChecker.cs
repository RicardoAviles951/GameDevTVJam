using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    private LayerMask groundLayer;

    void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }


    public bool CheckGround()
    {
        //Checking the ground
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);

        // If the raycast hits a ground object, consider the character grounded
        return hit.collider != null;
    }
}
