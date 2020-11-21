using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightWallScr : MonoBehaviour
{
    private int destroyCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        destroyCounter += 1;
        transform.position += transform.right * 0.6f;
        if (destroyCounter > 200)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
