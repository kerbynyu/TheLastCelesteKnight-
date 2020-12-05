using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orb : MonoBehaviour
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
    public int hitGroundCounter = 0;
    private bool hitGround = false;
    public LayerMask playerMask;
    private int destroyCounter = 0;
    public eHit_Box orbHit;
    private int justCounter = 0;
    public AudioSource orbGenerate;
    public AudioSource orbDestroy;
    private bool hitPlay = false;
    public CircleCollider2D coll;
    public SpriteRenderer thisSR;
    public CircleCollider2D coll2;
    private bool stopGenerate = false;
    private bool generatePlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        zRotation = Random.Range(0, 90);
    }

    void FixedUpdate()
    {
        waitCounter += 1;
        if (!orbGenerate.isPlaying && !hitPlay && !stopGenerate && !generatePlayed)
        {
            orbGenerate.Play();
            generatePlayed = true;
        }
        if (waitCounter > 30)
        {
            action = true;
        }
        if (action)
        {
            
            justCounter += 1;
            thisAnim.SetBool("action", true);
            zRotation += 5;
            if (thisAnim.GetBool("hit") == false)
            {
                transform.rotation = Quaternion.Euler(0, 0, zRotation);
                player = GameObject.Find("Player");
                middle.transform.position = Vector3.SmoothDamp(middle.transform.position, player.transform.position, ref middleSpd, 0.13f);
                transform.position = Vector3.MoveTowards(transform.position, middle.transform.position, spd);
            }
        }

        if (justCounter > 80)
        {
            Destroy(coll2);
            Destroy(coll);
            float scX = transform.localScale.x - 0.05f;
            float scY = transform.localScale.y - 0.05f;
            transform.localScale = new Vector3(scX, scY, 1);
        }

        if (justCounter > 100)
        {
            
            Destroy(thisSR);
        }

        if (justCounter > 200)
        {
            Destroy(gameObject);
        }
        if (hitGroundCounter > 2)
        {
            thisAnim.SetBool("hit", true);
            hitPlay = true;
            if (orbGenerate.isPlaying)
            {
                orbGenerate.Stop();
                stopGenerate = true;
            }
            if (!orbDestroy.isPlaying && hitPlay)
            {
                orbDestroy.Play();
            }
        }

        if (thisAnim.GetBool("hit")==true)
        {
            
            destroyCounter += 1;
        }
        if (destroyCounter > 10)
        {
            Destroy(coll);
            Destroy(coll2);
            Destroy(thisSR);

           
        }
        if (destroyCounter > 50)
        {
            Destroy(gameObject);
        }

        if (orbHit.hitPlayer == true)
        {
            thisAnim.SetBool("hit", true);
            hitPlay = true;
            if (orbGenerate.isPlaying)
            {
                orbGenerate.Stop();
                stopGenerate = true;
            }
            if (!orbDestroy.isPlaying && hitPlay)
            {
                orbDestroy.Play();
            }
        }

        Ray2D detectRay = new Ray2D(new Vector2(transform.position.x, transform.position.y), Vector3.right * 1);
        RaycastHit2D detectHit = Physics2D.Raycast(detectRay.origin, detectRay.direction, 5, playerMask);
        Ray2D detectRay2 = new Ray2D(new Vector2(transform.position.x, transform.position.y), Vector3.left * 1);
        RaycastHit2D detectHit2 = Physics2D.Raycast(detectRay.origin, detectRay.direction, 5, playerMask);
        if (justCounter < 80)
        {
            if ((detectHit.collider != null && detectHit.collider.gameObject.CompareTag("Ground")) || (detectHit2.collider != null && detectHit2.collider.gameObject.CompareTag("Ground")))
            {
                hitGroundCounter += 1;
            }
            else
            {
                hitGroundCounter = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("phase2") && justCounter < 80)
        {
            hitbox.gameObject.SetActive(true);
        }
        else { hitbox.gameObject.SetActive(false); }
    }

    /*

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            hitGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            hitGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            hitGround = false;
            hitGroundCounter = 0;
        }
    }
    */
}
