using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{

    public bool isAttacking= false;
    public float counter1 = 0;//count the hitbox duration
    public float counter2 = 0;//count the invicible frame 
    public float counter3 = 0;//count the stun frame 
    public float counter4 = 0;//count the being shot away time
    public float invicibleTime = 1.5f;
    public float stunTime = 0.5f;
    public float launchTime = 1f;
    public Vector3 lPosition;
    public Vector3 shift;
    public SpriteRenderer pSprite;
    public HitBox hitbox1;
    public HitBox hitbox2;
    public HitBox hitbox3;
    public HitBox hitbox4;
    public HitBox cHitbox;//record the current hitbox
    public HurtBox hutbox;

    // Start is called before the first frame update
    void Start()
    {
        hitbox1.gameObject.SetActive(false);
        hitbox2.gameObject.SetActive(false);
        hitbox3.gameObject.SetActive(false);
        hitbox4.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (hitted)
        {
            hitted = false;
            counter2 = invicibleTime;
            counter3 = stunTime;
            counter4 = launchTime;
            shift = transform.position-lPosition;
        }
        //if player is not hitted
        if (counter3<=0)
        {
            //When it is not during one attack
            if (!isAttacking)
            {
                //the defualt attack direction will be the current facing
                if (pSprite.flipX) { cHitbox = hitbox3; }
                else { cHitbox = hitbox1; }

                //if pressed attack button
                if (Input.GetKeyDown(KeyCode.J))
                {
                    //print("attack!");
                    isAttacking = true;
                    //attack direction will be overwrite by directional inputs
                    if (Input.GetKey(KeyCode.D)) { cHitbox = hitbox1; }
                    else if (Input.GetKey(KeyCode.A)) { cHitbox = hitbox3; }
                    if (Input.GetKey(KeyCode.S)) { cHitbox = hitbox2; }
                    else if (Input.GetKey(KeyCode.W)) { cHitbox = hitbox4; }

                    //Initialize the choosen hitbox
                    cHitbox.gameObject.SetActive(true);
                    counter1 = cHitbox.life;
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
        if (counter4 > 0) {
            counter4 -= Time.deltaTime;
            transform.Translate(shift.x * Time.deltaTime, shift.y * Time.deltaTime, 0);
            //print(shift.x);
            //simulate force
            shift.x -= shift.x * Time.deltaTime / launchTime;
            shift.y -= shift.y * Time.deltaTime / launchTime;

        }

        lPosition = transform.position;
    }
}
