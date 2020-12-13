using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : Attack
{

    public bool isAttacking= false;
    public float energy = 0;
    public float maxEnergy = 9;
    public float eUsed = 0;//energy used for this charge
    public float eSpeed = 0;//energy using speed
    public float eSpeed_Max = 3;//max energy using speed in sec
    public float eSpeed_Acc = 3;//the acceleration of energy using in sec

    public float counter1 = 0;//count the hitbox duration
    public float counter2 = 0;//count the invicible frame 
    public float counter3 = 0;//count the stun frame 
    public float counter4 = 0;//count the being shot away time
    public float counter5 = 0;//count the pushed back duration
    public float counter6 = 0;//count the attack CD

    public float attackCD = 0.5f;
    public float invicibleTime = 1.5f;
    public float stunTime = 0.5f;

    public float launchTime = 1f;
    public float pushed_back_time = 0.5f;

    public Vector2 pushed_direction;
    public float pushed_back_speed;
    public bool pushed_movable = false;
    public Vector3 lPosition;
    public Vector3 shift;

    public SoundManager2 soundManager;
    public SimplePhysicsController cont;
    public GroundCheck2 feet;
    public Animator thisAnimator;
    public SpriteRenderer pSprite;
    public HitBox hitbox1;
    public HitBox hitbox2;
    public HitBox hitbox3;
    public HitBox hitbox4;
    public HitBox cHitbox;//record the current hitbox
    public HurtBox hutbox;

    private GameMaster gm;
    

    // Start is called before the first frame update
    void Start()
    {
        hitbox1.gameObject.SetActive(false);
        hitbox2.gameObject.SetActive(false);
        hitbox3.gameObject.SetActive(false);
        hitbox4.gameObject.SetActive(false);
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameMaster>();
    }

    private void LateUpdate()
    {
        base.Update();
    }
    // Update is called once per frame
    public override void Update()
    {
        if (!alive)
        {
            
            gm.deathScreen();
            Destroy(gameObject);
        }
        else
        {
            energy = Mathf.Min(energy, maxEnergy);
            energy = Mathf.Max(energy, 0);

            if (hitted)
            {
                thisAnimator.SetBool("hurting",true);
                gm.shakeCounter=gm.shakeFrames;
                soundManager.playHitted();
                hitted = false;
                counter2 = invicibleTime;
                counter3 = stunTime;
                counter4 = launchTime;
                shift = transform.position - lPosition;
            }

            if(counter3<=stunTime/2f) thisAnimator.SetBool("hurting",false);
            //if player is not hitted
            if (counter3 <= 0)
            {

                //if player is pushed back by the attack
                if (counter5 >= 0)
                {
                    counter5 -= Time.deltaTime;
                    transform.Translate(new Vector3(-pushed_back_speed * pushed_direction.x * Time.deltaTime, -pushed_back_speed * pushed_direction.y * Time.deltaTime, 0));
                    pushed_back_speed -= pushed_back_speed * Time.deltaTime / pushed_back_time;
                }
                else
                {
                    pushed_movable = false;
                }
                //When it is not during one attack
                if (!isAttacking)
                {

                    thisAnimator.SetBool("slash", false);
                    //the defualt attack direction will be the current facing
                    if (pSprite.flipX) { cHitbox = hitbox3; }
                    else { cHitbox = hitbox1; }

                    if (counter6 > 0) { counter6 -= Time.deltaTime; }
                    //if pressed attack button and the cool down is ready
                    if (Input.GetKeyDown(KeyCode.J) && counter6 <= 0)
                    {
                        counter6 = attackCD;
                        thisAnimator.SetBool("slash", true);
                        //print("attack!");
                        isAttacking = true;
                        //attack direction will be overwrite by directional inputs
                        if (Input.GetKey(KeyCode.D)) { cHitbox = hitbox1; thisAnimator.SetInteger("direction", 0); }
                        else if (Input.GetKey(KeyCode.A)) { cHitbox = hitbox3; thisAnimator.SetInteger("direction", 0); }
                        if (Input.GetKey(KeyCode.S) && !feet.isGrounded) { cHitbox = hitbox2; thisAnimator.SetInteger("direction", -1); }
                        else if (Input.GetKey(KeyCode.W)) { cHitbox = hitbox4; thisAnimator.SetInteger("direction", 1); }

                        //Initialize the choosen hitbox
                        cHitbox.gameObject.SetActive(true);
                        counter1 = cHitbox.life;
                    }
                    //if pressed regenerate button and onground
                    if (Input.GetKey(KeyCode.O) && feet.isGrounded && (energy >= 3 || eUsed > 0))
                    {

                        if (eSpeed == 0)
                        {
                            print("charge");
                            thisAnimator.SetTrigger("charge");
                            gm.zooming=true;
                        }
                        thisAnimator.SetBool("charging", true);
                        if (eUsed % 1 <0.01&&eUsed>0.1 ) { soundManager.playCharging(); }
                        //print("KaMe");
                        eSpeed += eSpeed_Acc * Time.deltaTime;
                        eSpeed = Mathf.Min(eSpeed, eSpeed_Max);
                        energy -= eSpeed * Time.deltaTime;
                        eUsed += eSpeed * Time.deltaTime;

                        if (eUsed > 2.9)
                        {
                            print("charged");
                            thisAnimator.SetTrigger("charged");
                            eUsed = 0;
                            health.Hp += 1;
                            print("Hp plus 1");
                            soundManager.playCharge();
                            gm.shakeCounter=gm.shakeFrames;
                        }
                    }
                    else if (eUsed > 0)
                    {

                        eUsed = 0;
                        //print("Ha");
                    }
                    else
                    {
                        eSpeed = 0;
                        eUsed = 0;
                        thisAnimator.SetBool("charging", false);
                        gm.zooming=false;
                    }
                }
                else
                {
                    //count down the time
                    if (counter1 > 0) { counter1 -= Time.deltaTime; }
                    //when it's end, deactivate the hitbox
                    else { cHitbox.gameObject.SetActive(false); isAttacking = false; cHitbox.whiteList.Clear(); }

                }
            }
            //if hitted
            else
            {
                //no hitbox
                hitbox1.gameObject.SetActive(false);
                hitbox2.gameObject.SetActive(false);
                hitbox3.gameObject.SetActive(false);
                hitbox4.gameObject.SetActive(false);

                counter3 -= Time.deltaTime;

            }

            if (counter2 > 0)
            {
                counter2 -= Time.deltaTime;
                Color tmp = pSprite.color;
                tmp.a = 0.5f;
                pSprite.color = tmp;
                hutbox.gameObject.SetActive(false);
            }
            else
            {
                Color tmp = pSprite.color;
                tmp.a = 1f;
                pSprite.color = tmp;
                hutbox.gameObject.SetActive(true);

            }
            if (counter4 > 0)
            {
                counter4 -= Time.deltaTime;
                transform.Translate(shift.x * Time.deltaTime, shift.y * Time.deltaTime, 0);
                //print(shift.x);
                //simulate force
                shift.x -= shift.x * Time.deltaTime / launchTime;
                shift.y -= shift.y * Time.deltaTime / launchTime;

            }

            lPosition = transform.position;
            if (cont.darkDash)
            {
                hutbox.gameObject.SetActive(false);
            }
            else
            {
                hutbox.gameObject.SetActive(true);
            }
        }
    }
}
