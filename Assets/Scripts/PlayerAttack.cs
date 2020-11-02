using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool isAttacking= false;
    public float counter1 = 0;
    public SpriteRenderer pSprite;
    public HitBox hitbox1;
    public HitBox hitbox2;
    public HitBox hitbox3;
    public HitBox hitbox4;
    public HitBox cHitbox;//record the current hitbox

    // Start is called before the first frame update
    void Start()
    {
        hitbox1.gameObject.SetActive(false);
        hitbox2.gameObject.SetActive(false);
        hitbox3.gameObject.SetActive(false);
        hitbox4.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            if (pSprite.flipX) { cHitbox = hitbox3; }
            else { cHitbox = hitbox1; }

            if (Input.GetKeyDown(KeyCode.J))
            {
                if (Input.GetKey(KeyCode.D)) { cHitbox = hitbox1; }
                else if (Input.GetKey(KeyCode.A)) { cHitbox = hitbox3; }
                if (Input.GetKey(KeyCode.S)) { cHitbox = hitbox2; }
                else if (Input.GetKey(KeyCode.W)) { cHitbox = hitbox4; }
                //print("attack!");
                isAttacking = true;

                cHitbox.gameObject.SetActive(true);
                counter1 = cHitbox.life;
            }
        }
        else
        {
            if (counter1 > 0) { counter1 -= Time.deltaTime; }
            else { cHitbox.gameObject.SetActive(false); isAttacking = false; }

        }
    }
}
