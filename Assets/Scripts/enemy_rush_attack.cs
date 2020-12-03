using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_rush_attack : Attack
{
    public LayerMask thisMask;
    public LayerMask playerMask;
    public float ray_offX = 0;
    public float ray_offY = 0;
    public float detection_length= 100f; //length of player detection
    public float reverse_length = 50f;//length of returning when facing wall
    public float fall_length;
    public float walk_speed = 2f;
    public float run_speed = 5f;
    public bool isAttacking = false;
    public int direction = 1;

    public float chargeTime=0.5f;
    public float runTime=2f;
    public float stunTime = 1f;
    public float counter1=0;//charge counter
    public float counter2=0;//run counter
    public float counter3 = 0;//stun counter

    public Animator thisAnim;
    public SpriteRenderer thisSpriteRenderer;
    public GroundCheck2 feet;
    // Start is called before the first frame update
    void Start()
    {
        fall_length = Mathf.Sqrt(2) * reverse_length;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (feet.isGrounded)
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
            thisAnim.SetBool("grounded", true);
        }
        else {
            GetComponent<Rigidbody2D>().gravityScale = 10;
            thisAnim.SetBool("grounded", false);
        }
        if (counter3<=0&&!hitted)
        {
            if (thisSpriteRenderer.flipX) { direction = 1; } else { direction = -1; }
            //if no player spotted
            if (!isAttacking)
            {
                thisAnim.SetBool("running", false);
                thisAnim.SetBool("walking", true);
                if (feet.isGrounded)
                {
                    
                    
                    counter1 = 0;
                    counter2 = 0;
                    transform.Translate(walk_speed * direction * Time.deltaTime, 0, 0);//walking
                                                                                       //reverse detection section
                    Ray2D reverseRay = new Ray2D(new Vector2(transform.position.x + ray_offX, transform.position.y + ray_offY), new Vector2(direction, 0) * reverse_length);
                    Ray2D fallRay = new Ray2D(new Vector2(transform.position.x + ray_offX, transform.position.y + ray_offY), new Vector2(direction, -1) * fall_length);
                    Debug.DrawRay(reverseRay.origin, new Vector2(direction, 0) * reverse_length);
                    Debug.DrawRay(reverseRay.origin, fallRay.direction * fall_length);
                    RaycastHit2D reverseHit = Physics2D.Raycast(reverseRay.origin, reverseRay.direction, reverse_length, thisMask);
                    RaycastHit2D fallHit = Physics2D.Raycast(fallRay.origin, fallRay.direction, fall_length, thisMask);
                    if ((reverseHit.collider != null && reverseHit.collider.gameObject.CompareTag("Ground")) || !(fallHit.collider != null && fallHit.collider.gameObject.CompareTag("Ground")))
                    {
                        thisAnim.SetTrigger("turn");
                        thisSpriteRenderer.flipX = !thisSpriteRenderer.flipX;
                    }

                    //player detection section front
                    Ray2D detectRay = new Ray2D(new Vector2(transform.position.x + ray_offX, transform.position.y + ray_offY), new Vector2(direction, 0) * detection_length);
                    Debug.DrawRay(detectRay.origin, new Vector2(direction, 0) * detection_length);
                    RaycastHit2D detectHit = Physics2D.Raycast(detectRay.origin, detectRay.direction, detection_length, playerMask);
                    if (detectHit.collider != null && detectHit.collider.gameObject.CompareTag("Player"))
                    {
                        thisAnim.SetTrigger("anticipation");
                        isAttacking = true;
                        counter1 = chargeTime;
                    }

                    //player detection section back
                    Ray2D detectRay2 = new Ray2D(new Vector2(transform.position.x + ray_offX, transform.position.y + ray_offY), new Vector2(-direction, 0) * detection_length);
                    Debug.DrawRay(detectRay2.origin, new Vector2(-direction, 0) * detection_length);
                    RaycastHit2D detectHit2 = Physics2D.Raycast(detectRay2.origin, detectRay2.direction, detection_length, playerMask);
                    if (detectHit2.collider != null && detectHit2.collider.gameObject.CompareTag("Player"))
                    {
                        thisAnim.SetTrigger("anticipation");
                        isAttacking = true;
                        counter1 = chargeTime;
                        thisSpriteRenderer.flipX = !thisSpriteRenderer.flipX;
                    }
                }

            }
            //if player spotted
            else
            {

                //if attack already start
                if (counter2 > 0)
                {
                    thisAnim.SetBool("running", true);
                    thisAnim.SetBool("walking", false);
                    counter2 -= Time.deltaTime;
                    transform.Translate(run_speed * direction * Time.deltaTime, 0, 0);//running
                }
                //if still charging
                else if (counter1 > 0)
                {
                    counter1 -= Time.deltaTime;
                    //charge finished
                    if (counter1 <= 0)
                    {
                        counter2 = runTime;
                    }
                }
                else
                {
                    isAttacking = false;
                }
            }
        }
        else {
            if (hitted)
            {
                counter3 = stunTime;
                hitted = false;
            }
            else
            {
                counter3 -= Time.deltaTime;
            }
        }
    }
}
