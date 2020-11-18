using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radianceMovement : MonoBehaviour
{

    public int floatCounter = 0;
    public bool floating = false;
    public bool teleport = false;
    public bool telOutRange = false;
    public bool casting = false;
    public float nextTeleportPosition = 0;
    public int nextFloatDuration = 0;
    public Transform leftMost;
    public Transform rightMost;
    // Start is called before the first frame update
    void Start()
    {
        floating = true;
        nextTeleportPosition = (leftMost.position.x+rightMost.position.x)/2;
        nextFloatDuration = Random.Range(50, 60);
    }

    void FixedUpdate()
    {
        if (floating)
        {
            nextTeleportPosition = Random.Range(leftMost.position.x, rightMost.position.x);
            floatCounter += 1;
            if (floatCounter > nextFloatDuration)
            {
                float isOut = Random.Range(1f, 10f);
                if (isOut > 9)
                {
                    telOutRange = true;
                }
                else
                {
                    telOutRange = false;
                }
                floatCounter = 0;
                teleport = true;
                floating = false;
            }
        }
        if (teleport)
        {
            nextFloatDuration = Random.Range(50, 100);
            if (!telOutRange)
            {
                transform.position = new Vector3(nextTeleportPosition, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(leftMost.position.x - 100, transform.position.y, transform.position.z);
            }
            teleport = false;
            floating = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
