using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    public bool burst = false;
    private int burstCounter = 0;
    public float burstRotation = 0;
    public float burstX = 0;
    public float burstY = 0;

    public bool other = false;
    private int destroyCounter = 0;
    public AudioSource swordAudio;
    public bool ifPlay = false;
    public bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {

        if (burst)
        {
            if (ifPlay && !played)
            {
                swordAudio.Play();
                played = true;
            }
            burstCounter += 1;
            if (burstCounter > 40)
            {
                
                transform.position += transform.right*0.8f;
                transform.rotation = Quaternion.Euler(0, 0, burstRotation);
                burstRotation -= 0.4f;
            }
            if (burstCounter > 200)
            {
                Destroy(gameObject);
            }
        }else if (other)
        {
            destroyCounter += 1;
            if (destroyCounter > 40)
            {
                if (ifPlay && !played)
                {
                    swordAudio.Play();
                    played = true;
                }
            }
            transform.position += transform.right * 0.6f;
            if (destroyCounter > 300)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
