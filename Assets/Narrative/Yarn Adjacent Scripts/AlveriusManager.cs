using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlveriusManager : MonoBehaviour
{
    private static AlveriusManager instance;
    public static AlveriusManager Instance { get { return instance; } }
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; DontDestroyOnLoad(this.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
