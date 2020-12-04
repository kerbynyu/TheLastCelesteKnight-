using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_crow_attack : Attack
{
    public float speed = 3f;
    public float rotationSpeed = -90f;
    public float stunTime = 1f;
    public float counter1 = 0;

    public Animator thisAnim;
    public GroundCheck2 feet1;
    public eHit_Box hitbox;
    public HurtBox hurtbox;

    private void LateUpdate()
    {
        base.Update();
    }
    public override void Update()
    {
        if (alive)
        {
            if (counter1 <= 0 && !hitted)
            {
                thisAnim.SetBool("stun", false);
                thisAnim.SetBool("grounded", true);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
                //ground attach
                if (!feet1.isGrounded)
                {
                    transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.World);
                }
            }
            else
            {
                if (hitted)
                {
                    hitted = false;
                    counter1 = stunTime;
                }
                else
                {
                    thisAnim.SetBool("stun", true);
                    counter1 -= Time.deltaTime;
                }

            }
        }
        else
        {
            thisAnim.SetBool("death", true);
            hitbox.gameObject.SetActive(false);
            hurtbox.gameObject.SetActive(false);
        }
    }
}
