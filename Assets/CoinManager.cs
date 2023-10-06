using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinManager : MonoBehaviour
{
    [Header("Hover Parameters")]
    public float floatHeight = 1f;
    public float floatSpeed = 1f;
    private Transform coinTransform;
    private float initialY;

    //public static event CoinChanged;
    public UnityEvent coinChanged;
    

    // Start is called before the first frame update
    void Start()
    {
        coinTransform = gameObject.transform;
        initialY = coinTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Make the coin object float up and down

        float y = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        coinTransform.position = new Vector3 (coinTransform.position.x, initialY+y, coinTransform.position.z);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
   
    public void KillCoin()
    {
        Destroy(gameObject);
    }

}
