using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Camera _cam;
    private CameraManager camManager;
    [SerializeField] private int section;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        camManager = _cam.GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            camManager.pos = section;
        }
    }
    
}
