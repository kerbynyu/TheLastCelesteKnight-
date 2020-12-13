using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePhysicsController : MonoBehaviour {

    public SpriteRenderer thisSprite;
    public Rigidbody2D thisRigidbody2D;
    public PlayerAttack playerAattack;
    public float force = 10f;
    public float jumpforce = 10;
    public bool onplatform;
    public Vector3 movementVector;
    Vector3 thisVelocty = Vector3.zero*20;
    public float smoothTime = 0.3f;
    public GroundCheck2 feet;

    //move variables

    public bool movingRight = false;
    private bool isMoving = false;


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
    public bool isDashing = false;
    private  bool dashed = false;
    public int dashAgainCounter = 0;
    public int dashAgainCounterMax = 25;
    //dark dash
    public bool allowDarkDash = false;
    public bool darkDashEnabled = false;
    public int darkDashCounter = 0;
    public int darkDashCounterMax = 100;
    public bool darkDash = false;
    public GameObject darkCoolDown;
    public ParticleSystem dark;
    public Animator CDAnim;
    //double jump variables
    public bool doubleJumpEnabled;
    public bool isDoubleJumping;
    public float doubleJumpHeight = 0.5f;
    public int doubleJumpCounter = 0;
    public bool doubleJumped = false;
    //wall jump variables
    public wallJump WJ1;
    public wallJump WJ2;
    public bool isOnWall;
    public int wjCounter = 0;
    public bool wjCount = false;
    public bool leaveTheWall = false;
    public bool toLeft = false;
    public bool toRight = false;
    public bool wallDash = false;
    public bool dashRight = false;
    public bool dashLeft = false;
    public bool dirChanged = false;
    public bool moveFromWall = false;
    public int moveFromWallCounter = 0;

    public GameObject wings; 

    private Animator anim;
    private GameMaster gm;

    void Start() {
        darkCoolDown.SetActive(false);
        GetComponent<Rigidbody2D>();
        darkDashCounter = darkDashCounterMax;
        anim = GetComponent<Animator>();
        //gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameMaster>();

        //transform.position = gm.lastCheckPointPos;
       // transform.position = Vector3.SmoothDamp(gm.lastCheckPointPos, gm.lastCheckPointPos, ref thisVelocty, smoothTime);
        //THIS IS THE CODE 

    }

    void FixedUpdate() {
        //if not using charging
        if (playerAattack.eUsed <= 0)
        {

            if (feet.isGrounded)
            {
                anim.SetBool("grounded", true);
                anim.SetBool("dJump", false);
            }

            if (transform.rotation.eulerAngles.z != 0)
            {
                transform.rotation = Quaternion.identity;
            }

            if (thisRigidbody2D.velocity.y < -40)
            {
                thisRigidbody2D.velocity = new Vector2(thisRigidbody2D.velocity.x, -40);
            }

            //if not during a push back onground
            if (playerAattack.counter5 <= 0 || playerAattack.pushed_movable)
            {
                //move left with 'a'
                if (Input.GetKey(KeyCode.A) && !isDashing)
                {
                    moveFromWallCounter += 1;
                    if (!leaveTheWall)
                    {
                        anim.SetBool("walking", true);
                        anim.SetBool("idle", false);
                        //walking animation
                        transform.Translate(-20 * Time.deltaTime, 0, 0);
                        isMoving = true;
                    }
                    else
                    {
                        if (wjCounter > 5)
                        {
                            transform.Translate(-20 * Time.deltaTime, 0, 0);
                        }
                    }

                    if (!isOnWall)
                    {
                        movingRight = false;
                    }

                }

                //move right with 'd' 
                if (Input.GetKey(KeyCode.D) && !isDashing)
                {
                    moveFromWallCounter += 1;
                    if (!leaveTheWall)
                    {
                        //walking animation
                        anim.SetBool("walking", true);
                        anim.SetBool("idle", false);
                        transform.Translate(20 * Time.deltaTime, 0, 0);
                        isMoving = true;
                    }
                    else
                    {
                        if (wjCounter > 5)
                        {
                            transform.Translate(20 * Time.deltaTime, 0, 0);
                        }
                    }
                    if (!isOnWall)
                    {
                        movingRight = true;
                    }
                }
            }
            if (!isOnWall)
            {
                if (movingRight)
                {
                    if (thisSprite.flipX == true)
                    {
                        thisSprite.flipX = false;
                    }
                }
                else
                {
                    if (thisSprite.flipX == false)
                    {
                        thisSprite.flipX = true;
                    }
                }
            }

            

            if (isMoving)
            {
                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    isMoving = false;
                    anim.SetBool("idle", true);
                    anim.SetBool("walking", false);
                    if (feet.isGrounded)
                    {
                        //idle animation  
                        anim.SetBool("grounded", true);
                    }
                }
            }
            else
            {
                anim.SetBool("idle", true);
                anim.SetBool("walking", false);
            }



            //jump control in fixed update

            if (!isJumping && !feet.isGrounded)
            {
                thisRigidbody2D.gravityScale = fallingGravity;
            }

            if (isJumping && releasedJump == false && !isOnWall)
            {
                //jumping animation
                anim.SetBool("grounded", false);
                anim.SetBool("falling", false);
                thisRigidbody2D.gravityScale = jumpingGravity;
                jumpcounter += 1;

                int minusIdx = 100;
                if (jumpcounter > 8)
                {
                    minusIdx = 110;
                }

                    if (!leaveTheWall)
                    {
                        thisRigidbody2D.AddForce(Vector2.up * (1800-jumpcounter*minusIdx) * Time.deltaTime, ForceMode2D.Impulse);
                    }
                    else
                    {
                        minusIdx = 80;
                        thisRigidbody2D.AddForce(Vector2.up * (1200 - jumpcounter * minusIdx) * Time.deltaTime, ForceMode2D.Impulse);
                    }

                if (thisRigidbody2D.velocity.y > 40)
                {
                    thisRigidbody2D.velocity = new Vector2(0, 40);
                }


            }
            else
            {
                //falling animation
                anim.SetBool("falling", true);
                transform.Translate(0, 0, 0);
                jumpcounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
            }

            //double jump control in fixed update

            if (isDoubleJumping && !isOnWall)
            {

                if (doubleJumpCounter == 0)
                {
                    wings.SetActive(false);
                    wings.SetActive(true);
                }

                //double jump animation
                anim.SetBool("dJump", true);
                anim.SetBool("falling", false);
                anim.SetBool("grounded", false);

                jumped = false;
                doubleJumpCounter += 1;
                int minusIdx = 110;
                if (doubleJumpCounter > 8)
                {
                    minusIdx = 120;
                }
                if (doubleJumpCounter < doubleJumpHeight)
                {
                    thisRigidbody2D.AddForce(Vector2.up * (1800-minusIdx*doubleJumpCounter) * Time.deltaTime, ForceMode2D.Impulse);
                }

                if (thisRigidbody2D.velocity.y > 30)
                {
                    thisRigidbody2D.velocity = new Vector2(0, 30);
                }
                if (Input.GetKeyUp(KeyCode.K) || doubleJumpCounter > doubleJumpHeight + 6)
                {
                    //falling animation
                    anim.SetBool("dJump", true);
                    anim.SetBool("falling", true);
                    thisRigidbody2D.velocity = new Vector2(0, 0);
                    doubleJumpCounter = 0;
                    thisRigidbody2D.gravityScale = fallingGravity;
                    isDoubleJumping = false;
                }
            }

            if (anim.GetBool("falling") == true && anim.GetBool("grounded") == true && anim.GetBool("walking") == false)
            {
                if (!isMoving || !isDashing)
                {
                    anim.SetBool("falling", false);
                    anim.SetBool("idle", true);
                }
            }

            //dash control in fixed update

                //dark dash
            if (darkDashCounter < darkDashCounterMax + 10)
            {
                darkDashCounter += 1;
            }
            if (darkDashCounter > darkDashCounterMax - 10)
            {
                if (CDAnim.gameObject.activeSelf)
                {
                    CDAnim.SetBool("ready", true);
                }

            }

            if (darkDashCounter < darkDashCounterMax)
            {
                darkDashEnabled = false;
            }
            else
            {
                darkDashEnabled = true;
                darkCoolDown.SetActive(false);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("charging")|| anim.GetCurrentAnimatorStateInfo(0).IsName("aboutToCharge"))
            {
                darkCoolDown.SetActive(false);
            }

            if (dashAgainCounter > -5)
            {
                dashAgainCounter -= 1;
            }
            if (isDashing)
            {
                wings.SetActive(false);
                //dash animation
                dashed = true;
                if (!darkDash)
                {
                    anim.SetBool("dash", true);
                }
                else
                {
                    dark.Play();
                    anim.SetBool("darkDash", true);
                }

                anim.SetBool("idle", false);
                thisRigidbody2D.velocity = new Vector2(thisRigidbody2D.velocity.x, 0);
                if (!wallDash)
                {
                    if (movingRight)
                    {
                        thisRigidbody2D.AddForce(Vector2.right * 800 * Time.deltaTime, ForceMode2D.Impulse);
                    }
                    else
                    {
                        thisRigidbody2D.AddForce(Vector2.right * -800 * Time.deltaTime, ForceMode2D.Impulse);
                    }
                    dashCounter += 1;
                    isJumping = false;
                    if (dashCounter > 6)
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
                }
                else
                {
                    if (dashRight)
                    {
                        thisRigidbody2D.AddForce(Vector2.right * 800 * Time.deltaTime, ForceMode2D.Impulse);
                    }
                    else if (dashLeft)
                    {
                        thisRigidbody2D.AddForce(Vector2.right * -800 * Time.deltaTime, ForceMode2D.Impulse);
                    }
                    dashCounter += 1;
                    isJumping = false;
                    if (dashCounter > 6)
                    {
                        if (dashRight)
                        {
                            thisRigidbody2D.AddForce(Vector2.right * -500 * Time.deltaTime, ForceMode2D.Impulse);
                        }
                        else if (dashLeft)
                        {
                            thisRigidbody2D.AddForce(Vector2.right * 500 * Time.deltaTime, ForceMode2D.Impulse);
                        }
                    }
                }
                if (dashCounter > 8)
                {
                    wallDash = false;
                    isDashing = false;
                    dashCounter = 0;
                }
            }
            else
            {
                dark.Stop();
                thisRigidbody2D.velocity = new Vector2(0, thisRigidbody2D.velocity.y);
            }

            //wall jump control in fixed update
            if (isOnWall)
            {
                if (dirChanged == false)
                {
                    if (movingRight)
                    {
                        movingRight = false;
                        dirChanged = true;
                    }
                    else
                    {
                        movingRight = true;
                        dirChanged = true;
                    }
                }
                //climbing on the wall animation
                anim.SetBool("walking", false);
                anim.SetBool("jump2wall", true);
                anim.SetBool("falling", false);
                anim.SetBool("onWall", true);
                anim.SetBool("grounded", false);
                anim.SetBool("dJump", false);
                wings.SetActive(false);
                dashed = false;
                transform.Translate(0, -8 * Time.deltaTime, 0);
                thisRigidbody2D.velocity = new Vector2(0, 0);
                //reset jump
                thisRigidbody2D.velocity = new Vector2(0, 0);
                doubleJumpCounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
                isDoubleJumping = false;
                jumpcounter = 0;
                thisRigidbody2D.gravityScale = fallingGravity;
                isJumping = false;
                releasedJump = true;
                if (wjCount)
                {
                    leaveTheWall = true;
                    doubleJumpEnabled = true;
                    releasedJump = false;
                    isJumping = true;
                    jumped = true;
                }
                moveFromWall = false;
                moveFromWallCounter = 0;
            }

            if (moveFromWallCounter < 15 && moveFromWallCounter > 0)
            {
                moveFromWall = true;
            }else if (feet.isGrounded || isOnWall)
            {
                moveFromWall = false;
            }
            else
            {
                moveFromWall = false;
            }


            if (moveFromWall)
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    leaveTheWall = true;
                    doubleJumpEnabled = true;
                    releasedJump = false;
                    isJumping = true;
                    jumped = true;
                }
            }
            
         


            //jump from wall
            if (leaveTheWall)
            {
                thisRigidbody2D.gravityScale = fallingGravity;
                anim.SetBool("jump2wall", false);
                dirChanged = false;
                wjCount = false;
                isOnWall = false;
                wjCounter += 1;

                if (WJ1.onWall)
                {
                    toRight = false;
                    toLeft = true;
                }
                else if (WJ2.onWall)
                {
                    toLeft = false;
                    toRight = true;
                }
                int jumpIdx = 17;

                if (wjCounter < jumpIdx)
                {
                    if (toLeft)
                    {
                        thisRigidbody2D.AddForce(Vector2.left * 600 * Time.deltaTime, ForceMode2D.Impulse);
                        doubleJumpEnabled = true;
                    }
                    else if (toRight)
                    {
                        thisRigidbody2D.AddForce(Vector2.right * 600 * Time.deltaTime, ForceMode2D.Impulse);
                        doubleJumpEnabled = true;
                    }
                }
                else
                {
                    
                    wjCounter = 0;
                    leaveTheWall = false;
                }

            }

            if (feet.isGrounded)
            {
                dirChanged = false;
                wings.SetActive(false);
                dashed = false;
                doubleJumpEnabled = true;
                jumped = false;
            }
            
            //reset dash

            if (WJ1.onWall)
            {
                dashLeft = true;
                dashRight = false;
            }
            else if (WJ2.onWall)
            {
                dashRight = true;
                dashLeft = false;
            }
            //dash from wall
            if (wallDash)
            {
                isDashing = true;
                dashAgainCounter = dashAgainCounterMax;
                isOnWall = false;


            }
        }

    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("idle", true);
            anim.SetBool("walking", false);
        }
        if (!isDashing)
        {
            if (darkDash)
            {
                darkDash = false;
                anim.SetBool("darkDash", false);
            }
            else
            {
                anim.SetBool("dash", false);
            }
        }
        if (!isOnWall)
        {
            anim.SetBool("onWall", false);
        }
        //wall jump control in update
        if (WJ1.onWall == true && Input.GetKey(KeyCode.D))
        {
            if (isJumping == false && feet.isGrounded == false)
            {
                isOnWall = true;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                wjCount = true;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                wallDash = true;
                jumped = true;
            }


        }
        else if (WJ2.onWall == true && Input.GetKey(KeyCode.A))
        {
            if (isJumping == false && feet.isGrounded == false)
            {
                isOnWall = true;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                wjCount = true;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                wallDash = true;
                jumped = true;
            }
        }
        else

        if (isOnWall == true)
        {
            doubleJumpEnabled = true;
            if (Input.GetKeyDown(KeyCode.K))
            {
                wjCount = true;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                wallDash = true;
                jumped = true;
            }
            if (WJ1.onWall == true || WJ2.onWall == true)
            {
                if (!feet.isGrounded)
                {
                    isOnWall = true;
                }
            }
            else
            {
                isOnWall = false;
            }
        }



        //dash control in update

        if (Input.GetKeyDown(KeyCode.L) && !isDashing && !dashed && dashAgainCounter < 0)
        {
            isDashing = true;
            dashAgainCounter = dashAgainCounterMax;
            if (darkDashEnabled && allowDarkDash)
            {
                darkDashCounter = 0;
                darkDash = true;
                darkCoolDown.SetActive(true);
                darkDashEnabled = false;
            }
        }

        if (dashed && feet.isGrounded)
        {
            dashed = false;
        }

        //jump control in update
        if (!isDashing || dashCounter < 4)
        {
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

            if (!feet.isGrounded)
            {
                if (!doubleJumped || !leaveTheWall)
                {
                    jumped = true;
                    //doubleJumpEnabled = true;
                }
            }
            else
            {
                doubleJumped = false;
            }

            if (Input.GetKeyUp(KeyCode.K) && jumped)
            {
                releasedJump = true;
                thisRigidbody2D.gravityScale = fallingGravity;
            }

            if ((feet.isGrounded || isOnWall) && !releasedJump)
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

            if (Input.GetKey(KeyCode.K) && (feet.isGrounded || isOnWall) && releasedJump == true)
            {
                doubleJumpEnabled = true;
                releasedJump = false;
                isJumping = true;
                jumped = true;
                transform.Translate(0, 0, 0);
            }

            if (isJumping && !releasedJump && !isDashing)
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

            if ((doubleJumpEnabled && Input.GetKey(KeyCode.K) && releasedJump))
            {

                isDoubleJumping = true;
                releasedJump = false;
                thisRigidbody2D.gravityScale = jumpingGravity;
                doubleJumpCounter = 0;
                doubleJumped = true;
                doubleJumpEnabled = false;
                Debug.Log("double");


            }

            if (isDoubleJumping)
            {
                wings.SetActive(true);
                if (Input.GetKeyUp(KeyCode.K) || doubleJumpCounter > doubleJumpHeight + 6)
                {
                    thisRigidbody2D.velocity = new Vector2(0, 0);
                    doubleJumpCounter = 0;
                    thisRigidbody2D.gravityScale = fallingGravity;
                    isDoubleJumping = false;

                }
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
        if (other.name == "darkCircle")
        {
            GameObject darkCircle = other.gameObject;
            darkCircleScr scr = darkCircle.GetComponent<darkCircleScr>();
            scr.ifEmission = true;
            allowDarkDash = true;
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