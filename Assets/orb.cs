using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orb : Attack
{
    public Animator ani;
    public eHit_Box hitbox;

    private int waitCounter = 0;
    private bool action = false;
    private int lastCounter = 0;
    public GameObject middle;
    private GameObject player;
    private float spd = 0.7f;
    private Vector3 middleSpd = Vector3.zero;
    public Animator thisAnim;
    private float zRotation = 0;
    private int hitGroundCounter = 0;
    private bool hitGround = false;
    // Start is called before the first frame update
    void Start()
    {
        zRotation = Random.Range(0, 90);
    }

    void FixedUpdate()
    {
        waitCounter += 1;
        if (waitCounter > 30)
        {
            action = true;
        }
        if (action)
        {
            thisAnim.SetBool("action", true);
            zRotation += 5;
            transform.rotation = Quaternion.Euler(0, 0, zRotation);
            player = GameObject.Find("Player");
            middle.transform.position = Vector3.SmoothDamp(middle.transform.position, player.transform.position, ref middleSpd, 0.13f);
            transform.position = Vector3.MoveTowards(transform.position,middle.transform.position,spd);
        }
        if (hitGround)
        {
            hitGroundCounter += 1;
        }
        if (hitGroundCounter > 10)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("phase2"))
        {
            hitbox.gameObject.SetActive(true);
        }
        else { hitbox.gameObject.SetActive(false); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            hitGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            hitGround = false;
            hitGroundCounter = 0;
        }
    }

}
