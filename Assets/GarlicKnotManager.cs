using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GarlicKnotManager : MonoBehaviour
{
    public bool Active = true;
    [SerializeField] ParticleSystem _particleRef;
    private ParticleSystem _particleSystem;
    private Vector2 _position;

    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate an object to play the particle system if not in scene
        if(_particleSystem == null)
        {
            var parentPos = gameObject.transform;
            _particleSystem = Instantiate(_particleRef,parentPos);
        }  
    }

    public void Reset()
    {
        _particleSystem.transform.position = gameObject.transform.position;
    }

    //Moves the garlic knots out of the scene of the frame
    public void MoveOutPosition()
    {
        gameObject.transform.position = new Vector3(0, -15f, 0);
    }

    public void FireParticles()
    {
        Debug.Log("Firing particles!");

        _particleSystem.transform.SetParent(null);
        _particleSystem.Play();
    }

}
