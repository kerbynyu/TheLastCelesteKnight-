using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightWallScr : MonoBehaviour
{
    private int destroyCounter;
    public float movingSpd = 0.6f;
    public bool phase4 = false;
    private int lastingTime = 200;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        destroyCounter += 1;
        transform.position += transform.right * movingSpd;
        if (destroyCounter > lastingTime)
        {
            Destroy(gameObject);
        }
        if (phase4)
        {
            movingSpd = 0.4f;
            lastingTime = 400;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
