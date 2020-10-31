using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePhysicsController : MonoBehaviour {

    public SpriteRenderer thisSprite;
    public Rigidbody2D thisRigidbody2D;
    public float force = 10f;
    public float jumpforce = 10;
    public bool onplatform;
    public float gravityInAir;
    public float gravityScale;
    public Vector3 movementVector;
    Vector3 thisVelocty = Vector3.zero*20;
    public float smoothTime = 0.3f;
    public GroundCheck2 feet;

    //jump variables
    private bool releasedJump = true;
    private int jumpHeight = 320;
    int jumpcounter = 1;
    public bool doubleJump = false;
    public bool isJumping;
    private bool jumped;



    //Animator anim;
    private GameMaster gm;

    void Start() {
        GetComponent<Rigidbody2D>();

        //anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameMaster>();

        //transform.position = gm.lastCheckPointPos;
        transform.position = Vector3.SmoothDamp(gm.lastCheckPointPos, gm.lastCheckPointPos, ref thisVelocty, smoothTime);
        //THIS IS THE CODE 

    }


    void FixedUpdate() {

        if (transform.rotation.eulerAngles.z != 0) {
            transform.rotation = Quaternion.identity;
        }
        //move left with 'a'
        if (Input.GetKey(KeyCode.A)) {
            //this.thisRigidbody2D.AddForce(-Vector2.right * force * Time.deltaTime, ForceMode2D.Impulse);
            transform.Translate(-20*Time.deltaTime, 0, 0);

            if (thisSprite.flipX == false) {
                thisSprite.flipX = true;
            }

            /*
            if (anim.gameObject.activeSelf) {
                anim.SetTrigger("Walking");
            }
            */
        }

        //move right with 'd' 
        if (Input.GetKey(KeyCode.D)) {
            //this.thisRigidbody2D.AddForce(Vector2.right * force * Time.deltaTime, ForceMode2D.Impulse);
            transform.Translate(20*Time.deltaTime, 0, 0);
            if (thisSprite.flipX == true) {
                thisSprite.flipX = false;
            }


            /*
            if (anim.gameObject.activeSelf) {
                anim.SetTrigger("Walking");
            }
            */
        }

        /*
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            anim.SetTrigger("Idle");
        }
        */
    }


    private void Update() {
        //jump up with 'space' 

        if(Input.GetKeyUp(KeyCode.Space) && jumped)
        {
            releasedJump = true;
        }

        if(feet.isGrounded && !releasedJump)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                releasedJump = true;
            }
        }

        if (Input.GetKey(KeyCode.Space) && feet.isGrounded == true && releasedJump == true)
        {
            releasedJump = false;
            isJumping = true;
            jumped = true;
        }

        if (isJumping && releasedJump == false)
        {
            thisRigidbody2D.gravityScale = gravityInAir;
            jumpcounter += 1;
            if (jumpcounter < jumpHeight)
            {
                thisRigidbody2D.AddForce(Vector2.up * 2500 * Time.deltaTime, ForceMode2D.Impulse);
            }else if (jumpcounter < jumpHeight+30)
            {
                thisRigidbody2D.AddForce(Vector2.down * 1000 * Time.deltaTime, ForceMode2D.Impulse);
            }
            if (thisRigidbody2D.velocity.y > 45)
            {
                thisRigidbody2D.velocity = new Vector2(0, 45);
            }
            if (Input.GetKeyUp(KeyCode.Space) || jumpcounter > jumpHeight+50)
            {
                isJumping = false;
                thisRigidbody2D.velocity = new Vector2(0, 0);
                jumpcounter = 0;
            }
        }
        else
        {
            transform.Translate(0, 0, 0);
            jumpcounter = 0;
            thisRigidbody2D.gravityScale = gravityScale;
        }

        //double jump

        if (feet.isGrounded == true)
        {
            //thisRigidbody2D.gravityScale = 1;

            //SoundManagerScript.playSound("jump");
            thisRigidbody2D.gravityScale = 0;
            //this.thisRigidbody2D.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);

            doubleJump = true;
            jumped = false;



        }
        else if (Input.GetKeyDown(KeyCode.Space) && doubleJump == true && jumped)
        {
            thisRigidbody2D.gravityScale = 0;
            Debug.Log("double jump");
            this.thisRigidbody2D.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            //SoundManagerScript.playSound("jump");
            doubleJump = false;

        }


    }

    public void Hurt() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //sc.playerDead = true; 


    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Respawn")) {
            Hurt();

        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            isJumping = false;
        }

        if (other.gameObject.CompareTag("Platform")) {
            isJumping = false;
        }

        if (other.gameObject.CompareTag("Enemy")) {
            this.thisRigidbody2D.AddForce(Vector2.up * jumpforce / 2, ForceMode2D.Impulse);
            isJumping = false;
        }
      
        

        EnemyScript enemy = other.collider.GetComponent<EnemyScript>();
        if (enemy != null) {
            //Hurt(); //player is hurt
            foreach (ContactPoint2D point in other.contacts) {
                Debug.DrawLine(point.point, point.point + point.normal, Color.blue, 10);
            }
        }

    }


}