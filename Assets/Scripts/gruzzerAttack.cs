using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gruzzerAttack : Attack
{
    public Vector2 dir = new Vector2(-1, -1);
    public float speed = 3;
    public float rotateCD = 1;
    public float counter1 = 0;
    public float counter2 = 0;

    public GoundCheck3 feet1;
    public GoundCheck3 feet2;
    public GoundCheck3 feet3;
    public GoundCheck3 feet4;

    public SpriteRenderer thisRender;
    public eHit_Box hitbox;
    public HurtBox hurtBox;
    public Animator thisAnim;
    public Rigidbody2D thisBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        base.Update();
    }
    // Update is called once per frame
    public override void Update()
    {
        if (alive)
        {
            transform.Translate(dir.x * speed * Time.deltaTime, dir.y * speed * Time.deltaTime, 0);
            if ((feet1.isGrounded || feet2.isGrounded)&&counter1<=0) { dir.y = -dir.y;counter1 = rotateCD; }
            if ((feet3.isGrounded || feet4.isGrounded)&&counter2<=0) { dir.x = -dir.x;counter2 = rotateCD; }
            if (counter1 > 0) { counter1 -= Time.deltaTime; }
            if (counter2 > 0) { counter2 -= Time.deltaTime; }
            thisRender.flipX = (dir.x == 1);
        }
        else
        {
            thisAnim.SetBool("death", true);
            hitbox.gameObject.SetActive(false);
            hurtBox.gameObject.SetActive(false);
            thisBody.gravityScale = 5;
            thisBody.drag = 0;
        }
    }
}
