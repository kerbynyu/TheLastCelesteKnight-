using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GroundCheck : MonoBehaviour
{
    public Boolean grounded = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ground")){
            grounded = true;
        }
        
    }

    void OnTriggerExit2D()
    {
        grounded = false;
    }
}
