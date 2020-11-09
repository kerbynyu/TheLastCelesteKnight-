using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJump : MonoBehaviour
{
    public bool onWall = false;
    public GroundCheck2 feet;
    public bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (feet.isGrounded == true)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGrounded && collision.tag == "wall")
        {
            onWall = true;
        }
        else if (isGrounded)
        {
            onWall = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "wall")
        {
            onWall = false;
        }
    }
}
