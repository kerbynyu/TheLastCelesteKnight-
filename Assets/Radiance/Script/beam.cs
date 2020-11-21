using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beam : MonoBehaviour
{
    public Animator thisAnim;
    private int destroyCounter = 0;
    private int lastCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (thisAnim.GetCurrentAnimatorStateInfo(0).IsName("beamBurst2"))
        {
            lastCounter += 1;
            if (lastCounter > 25)
            {
                thisAnim.SetTrigger("launched");
            }
        }

        if (thisAnim.GetCurrentAnimatorStateInfo(0).IsName("beamBurst3"))
        {
            destroyCounter += 1;
            if (destroyCounter > 2)
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
