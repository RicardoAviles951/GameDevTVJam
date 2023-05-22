using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 2.0f;
    [SerializeField] private float yOffset = 3.0f;
    [SerializeField] private float minY = -1.0f;
    [SerializeField] private float maxY = 1.0f;
    void LateUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x,target.position.y+yOffset,-10f);//Create vector position based on player position
        targetPos.y = Mathf.Clamp(targetPos.y, minY,maxY); // Limit the y position to minY


        transform.position = Vector3.Slerp(transform.position,targetPos,followSpeed*Time.deltaTime);
    }
}
