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
    public Vector3 movementVector;
    Vector3 thisVelocty = Vector3.zero*20;
    public float smoothTime = 0.3f;
    public GroundCheck2 feet;

    //move variables

    private bool movingRight = false;

    //jump variables
    public float jumpingGravity;
    public float fallingGravity;
    public bool releasedJump = true;
    public float jumpHeight = 1.5f;
    public int jumpcounter = 0;
    public bool isJumping;
    public bool jumped;

    //dash variables
    public int dashCounter = 0;
    private bool isDashing = false;
    private  bool dashed = false;
    public int dashAgainCounter = 0;
    public int dashAgainCounterMax = 25;

    //double jump variables
    public bool doubleJumpEnabled;
    public bool isDoubleJumping;
    public float doubleJumpHeight = 0.5f;
    public int doubleJumpCounter = 0;

    //wall jump variables
    public wallJump WJ1;
    public wallJump WJ2;
    public bool isOnWall = false;

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
        if (Input.GetKey(KeyCode.A) && !isDashing) { 
            transform.Translate(-20*Time.deltaTime, 0, 0);

            if (thisSprite.flipX == false) {
                thisSprite.flipX = true;
            }
            movingRight = false;

        }

        //move right with 'd' 
        if (Input.GetKey(KeyCode.D) && !isDashing) {
            transform.Translate(20*Time.deltaTime, 0, 0);
            if (thisSprite.flipX == true) {
                thisSprite.flipX = false;
            }
            movingRight = true;
        }

        //jump control in fixed update

        if (!isJumping && !feet.isGrounded)
        {
            thisRigidbody2D.gravityScale = fallingGravity;
        }

        if (isJumping && releasedJump == false)
        {

            thisRigidbody2D.gravityScale = jumpingGravity;
            jumpcounter += 1;
            if (jumpcounter < jumpHeight)
            {
                thisRigidbody2D.AddForce(Vector2.up * 4500 * Time.deltaTime, ForceMode2D.Impulse);
            } else if (jumpcounter < jumpHeight + 3)
            {
                thisRigidbody2D.AddForce(Vector2.down * 600 * Time.deltaTime, ForceMode2D.Impulse);

            }
            if (thisRigidbody2D.velocity.y > 40)
            {
                thisRigidbody2D.velocity = new Vector2(0, 40);
            }


        }
        else
        {
            transform.Translate(0, 0, 0);
            jumpcounter = 0;
            thisRigidbody2D.gravityScale = fallingGravity;
        }
            
        //double jump control in fixed update

        if (isDoubleJumping)
        {
            jumped = false;
            doubleJumpCounter += 1;
            if (doubleJumpCounter < doubleJumpHeight)
            {
                thisRigidbody2D.AddForce(Vector2.up * 4500 * Time.deltaTime, ForceMode2D.Impulse);
            }
            else if (doubleJumpCounter < doubleJumpHeight + 1)
            {
                thisRigidbody2D.AddForce(Vector2.down * 600 * Time.deltaTime, ForceMode2D.Impulse);

            }
            if (thisRigidbody2D.velocity.y > 30)
            {
                thisRigidbody2D.velocity = new Vector2(0, 30);
            }
            if (Input.GetKeyUp(KeyCode.K) || doubleJumpCounter > doubleJumpHeight + 6)
            {
                thisRigidbody2D.velocity = new Vector2(0, 0);
                doubleJumpCounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
                isDoubleJumping = false;
            }
        }


        //dash control in fixed update
        if (dashAgainCounter > -5)
        {
            dashAgainCounter -= 1;
        }
        if (isDashing)
        {
            dashed = true;
            thisRigidbody2D.velocity = new Vector2(thisRigidbody2D.velocity.x, 0);
            if (movingRight)
            {
                thisRigidbody2D.AddForce(Vector2.right * 1500 * Time.deltaTime, ForceMode2D.Impulse);
            }else
            {
                thisRigidbody2D.AddForce(Vector2.right * -1500 * Time.deltaTime, ForceMode2D.Impulse);
            }
            dashCounter += 1;
            isJumping = false;
            if (dashCounter > 4)
            {
                if (movingRight)
                {
                    thisRigidbody2D.AddForce(Vector2.right * -500 * Time.deltaTime, ForceMode2D.Impulse);
                }
                else
                {
                    thisRigidbody2D.AddForce(Vector2.right * 500 * Time.deltaTime, ForceMode2D.Impulse);
                }
            }

            if (dashCounter > 6)
            {
                isDashing = false;
                dashCounter = 0;
            }
        }
        else
        {
            thisRigidbody2D.velocity = new Vector2(0, thisRigidbody2D.velocity.y);
        }

        //wall jump control in fixed update
        if (isOnWall)
        {
            
            transform.Translate(0, -8 * Time.deltaTime, 0);
            thisRigidbody2D.velocity = new Vector2(0, 0);
        }


    }


    private void Update() {
        //wall jump control in update
        if (WJ1.onWall == true || WJ2.onWall == true)
        {
            isOnWall = true;
        }
        else
        {
            isOnWall = false;
        }

        

        //dash control in update

        if (Input.GetKeyDown(KeyCode.L) && !isDashing && !dashed && dashAgainCounter < 0)
        {
            isDashing = true;
            dashAgainCounter = dashAgainCounterMax;
        }

        if (dashed && feet.isGrounded)
        {
            dashed = false;
        }

        //jump control in update

        if (isJumping && !releasedJump)
        {
            if (Input.GetKeyUp(KeyCode.K) || jumpcounter > jumpHeight + 3)
            {
                thisRigidbody2D.velocity = new Vector2(0, 0);
                jumpcounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
                isJumping = false;
                transform.Translate(0, 0, 0);
            }
        }

        if(Input.GetKeyUp(KeyCode.K) && jumped)
        {
            releasedJump = true;
            thisRigidbody2D.gravityScale = fallingGravity;
        }

        if(feet.isGrounded && !releasedJump)
        {
            if (!Input.GetKey(KeyCode.K))
            {
                thisRigidbody2D.velocity = new Vector2(0, 0);
                jumpcounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
                isJumping = false;
                releasedJump = true;
                transform.Translate(0, 0, 0);
            }
        }

        if (Input.GetKey(KeyCode.K) && feet.isGrounded == true && releasedJump == true)
        {
            doubleJumpEnabled = true;
            releasedJump = false;
            isJumping = true;
            jumped = true;
            transform.Translate(0, 0, 0);
        }

        if (isJumping && !releasedJump)
        {
            if (Input.GetKeyUp(KeyCode.K) || jumpcounter > jumpHeight + 3)
            {
                thisRigidbody2D.velocity = new Vector2(0, 0);
                jumpcounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
                isJumping = false;
            }
        }

        //double jump control in update
        
        if (doubleJumpEnabled && Input.GetKey(KeyCode.K) && releasedJump)
        {
            isDoubleJumping = true;
            releasedJump = false;
            thisRigidbody2D.gravityScale = jumpingGravity;
            doubleJumpCounter = 0;
        }

        if (isDoubleJumping)
        {
            if (Input.GetKeyUp(KeyCode.K) || doubleJumpCounter > doubleJumpHeight + 6)
            {
                thisRigidbody2D.velocity = new Vector2(0, 0);
                doubleJumpCounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
                isDoubleJumping = false;
            }
        }
    }

    public void Hurt() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


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