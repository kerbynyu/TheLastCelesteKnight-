using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Script_movement : MonoBehaviour
{
    //recourses to load
    public Sprite spr_sword1;
    public Sprite spr_sword2;
    public Sprite spr_sword3;
    public AudioSource slashSound;
    public AudioSource hitSound;
    public AudioSource hurtSound;
    //Objects to link
    public LayerMask theMask;
    public Animator ani;
    public SpriteRenderer spr;
    public GameObject sword;
    public Rigidbody2D thisRigidbody2d;
    public Script_GroundCheck groundScript;
    public GameObject tele;
    //Variables: physics
    public float force = 10f;
    public float grav_ground = 7;
    public float grav_air = 7;
    public float attack_length;
    public float slashTime_normal = 0.5f;
    public float slashTime_hit = 1f;
    public float floatTime = 0.5f;
    public float thickness = 1f;
    Collider2D eCollider;

    //Varaiables: logic
    public float MaxHp = 5;
    public float Hp;
    public float damage = 1;
    public Boolean fly = true;
    public Vector2 direction;
    public int maxSlash;
    public int slashes;
    public float counter1;
    public float counter2;
    public float counter3;
    public bool hitted = false;
    public bool hurt = false;


    // Start is called before the first frame update
    private void OnDestroy()
    {
        Application.Quit();
    }
    void Start()
    {
        Hp = MaxHp;
    }

    private void Update()
    {

        if (hurt) { hurt = false; counter3 = 1; hurtSound.Play(); }
        if (counter3 > 0) { counter3-=Time.deltaTime; }
        //death and respawn
        if (Hp <= 0){
            transform.position = tele.transform.position - new Vector3(0, 0, 1f);
            thisRigidbody2d.velocity = Vector2.zero;
            Hp = MaxHp;
        }
        //Adjust gravity sacle and animation according to movement and grounded or not
        transform.position = new Vector3(transform.position.x, transform.position.y, -9);
        if (groundScript.grounded){
            ani.SetBool("onGround", true);
            fly = true;
            if (counter1 < 1) { slashes = maxSlash; }//can't restore during the move
            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
            {
                ani.SetBool("Moving",false);

            }
            else {
                ani.SetBool("Moving", true);

            }
            
            thisRigidbody2d.gravityScale = grav_ground;
            if (Input.GetKeyDown(KeyCode.Space)) {
                thisRigidbody2d.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            }
        }
        else {
            thisRigidbody2d.gravityScale = grav_air;
            ani.SetBool("onGround", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ani.SetBool("onGround", true);//for looking
                fly = false;
                thisRigidbody2d.AddForce(Vector2.up * force *1f, ForceMode2D.Impulse);
            }
        }

        //Teleport back to home
        if (Input.GetKeyDown(KeyCode.Q)) {
            transform.position = tele.transform.position-new Vector3(   0,0,1f);
        }

        //Facing according to the sword
        if (sword.transform.rotation.z > 0) { spr.flipX = true; }
        else { spr.flipX = false; }

        //Calculate the aiming
        Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mPos - transform.position;
        direction = direction.normalized;
        Ray ray1 = new Ray(transform.position, direction);
        //RaycastHit2D hit= Physics2D.CircleCast(ray1.origin,thickness,ray1.direction,attack_length, theMask);
        RaycastHit2D hit = Physics2D.Raycast(ray1.origin, ray1.direction, attack_length, theMask);
        Debug.DrawRay(ray1.origin, ray1.direction * attack_length, Color.white);

        //Rotate the sword
        if (counter1 <=0) { sword.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg); }

        //initallize the slash
        if (true)
        {
            if (Input.GetMouseButtonDown(0) && slashes > 0 && counter1 < 1)
            {
                slashSound.Play();
                slashes--;
                print("attempted flying");
                thisRigidbody2d.velocity = Vector2.zero;//clear force
                if (hit.collider != null && hit.collider.gameObject.CompareTag("enemy"))
                {
                    hitSound.Play();
                    //print("hitted");
                    //Destroy(hit.collider.gameObject);
                    eCollider = hit.collider;
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), eCollider, true);
                    if (hit.collider.gameObject.GetComponent<Script_Hp>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Script_Hp>().Hp -= damage;
                    }

                    counter1 = slashTime_hit;
                    slashes++;
                    hitted = true;
                    thisRigidbody2d.AddForce(direction * force * 4f, ForceMode2D.Impulse);
                }
                else
                {
                    counter1 = slashTime_normal;
                    thisRigidbody2d.AddForce(direction * force * 2f, ForceMode2D.Impulse);
                }
                //thisRigidbody2d.AddForce(Vector2.up * thisRigidbody2d.gravityScale, ForceMode2D.Impulse);

            }
        }
        //Slash Traveling
        if (counter1 > 0)
        {
            counter1-=Time.deltaTime;
            //if going on
            if (counter1 > 1) { thisRigidbody2d.gravityScale = 0; }
            //if ends
            else if (counter1 <0.05)
            {
                //clear force
                thisRigidbody2d.velocity = Vector2.zero;
                //if successfully hitted the enemy, enter phase 2 
                if (hitted)
                {
                    counter2 = floatTime;
                }
            }
            if (hitted) { sword.GetComponent<SpriteRenderer>().sprite = spr_sword3; }
            else { sword.GetComponent<SpriteRenderer>().sprite = spr_sword2; }
        }
        else
        {
            sword.GetComponent<SpriteRenderer>().sprite = spr_sword1;
            hitted = false;
            if (eCollider != null)
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), eCollider, false);
            }
        }

        //after float
        if (counter2 > 0 && counter1 < 0.05)
        {
            counter2-=Time.deltaTime;
            thisRigidbody2d.velocity = Vector2.zero;
        }

        //apperance accoridng to the states
        if (counter1 > 0 || counter2 > 0)
        {
            ani.speed = 0;
        }
        else { ani.speed = 1; }


    }

    void FixedUpdate()
    {
        //General Movement
        if (Input.GetKey(KeyCode.A)) {
            thisRigidbody2d.AddForce(Vector2.left * force * Time.fixedDeltaTime, ForceMode2D.Impulse);

            //spr.flipX = true;
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            thisRigidbody2d.AddForce(Vector2.right * force * Time.fixedDeltaTime, ForceMode2D.Impulse);

            //spr.flipX = false;
        }


    }


}
