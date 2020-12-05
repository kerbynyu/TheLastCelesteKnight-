using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoundCheck3 : MonoBehaviour
{

    
    public bool isGrounded = false;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

        }
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;

        }

        if (other.gameObject.CompareTag("wall"))
        {
            isGrounded = true;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

        }
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;

        }

        if (other.gameObject.CompareTag("wall"))
        {
            isGrounded = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;


    }
}
