using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int side = 0;//0 for neutral, 1 for player, -1 for enemy
    public GameObject owner;
    public float damage = 0;
    public float life = 1;
    public float force = 5f;
    public Vector2 dir = Vector2.left;
    public float xMovement=5f;
    public float yMovement=5f;
    public List<GameObject> whiteList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if the tag is hurtbox and they really have hurboxes
        if (collision.gameObject.CompareTag("hurtBox")&& collision.gameObject.GetComponent<HurtBox>())
        {
            HurtBox hurtbox = collision.gameObject.GetComponent<HurtBox>();
            if (hurtbox.side!=side&&!whiteList.Contains(collision.gameObject)) {
                hurtbox.owner.GetComponent<Attack>().hitted=true;
                hurtbox.owner.GetComponent<Health>().Hp -= damage;

                whiteList.Add(collision.gameObject);
                //if hitted solid hurtBox instead of grass
                if (hurtbox.solid) {
                    owner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //owner.GetComponent<Rigidbody2D>().AddForce(force*dir, ForceMode2D.Impulse);
                    owner.transform.Translate(xMovement, yMovement, 0);
                }
                //if the hitted box is not steel
                if (!hurtbox.steel)
                {
                    hurtbox.thisRigidbody2d.AddForce(force * dir, ForceMode2D.Impulse);
                }


            }
            
        }
    }
}
