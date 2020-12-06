using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int side = 0;//0 for neutral, 1 for player, -1 for enemy
    public GameObject owner;
    public float damage = 0;
    public float life = 1;//the duration of this box
    public float force = 5f;//the force to the taker
    public float force2 = 40f;//the force to the owner
    public bool moveAble = false;//if movable once land a hit
    public Vector2 dir = Vector2.left;
    public List<GameObject> whiteList;
    public GameObject effect;
    public GameObject locator;
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
            //if hit a new box not hitted one
            if (hurtbox.side!=side&&!whiteList.Contains(collision.gameObject)) {
                
                //pass the damage and comfirm a hit
                hurtbox.owner.GetComponent<Attack>().hitted=true;
                hurtbox.owner.GetComponent<Health>().Hp -= damage;
                
                
                //add to whitelist
                whiteList.Add(collision.gameObject);
                //if hitted solid hurtBox instead of grass
                if (hurtbox.solid) {
                    //death fly
                    if (hurtbox.owner.GetComponent<Health>().Hp <= 0)
                    {
                        hurtbox.owner.GetComponent<Rigidbody2D>().gravityScale = 10;
                        hurtbox.owner.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        hurtbox.owner.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x+Random.Range(-1f,1f), dir.y+Random.Range(1, 2)) * force * 2, ForceMode2D.Impulse);
                    }
                    //create effect
                    Instantiate(effect, locator.transform.position, locator.transform.rotation);
                    if (owner.GetComponent<PlayerAttack>())
                    {
                        PlayerAttack pl = owner.GetComponent<PlayerAttack>();
                        pl.counter5 = pl.pushed_back_time;
                        pl.pushed_direction = dir;
                        pl.pushed_back_speed = force2;
                        pl.pushed_movable = moveAble;
                        pl.energy += 1;
                    }
                    owner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //owner.GetComponent<Rigidbody2D>().AddForce(force*dir, ForceMode2D.Impulse);
                    //owner.transform.Translate(xMovement, yMovement, 0);
                }
                //if the hitted box is not steel
                if (!hurtbox.steel)
                {
                    hurtbox.thisRigidbody2d.AddForce(force * dir, ForceMode2D.Impulse);
                }

                //if the hurtbox have it's own effect
                if (hurtbox.haveEffect)
                {
                    //print(Vector2.SignedAngle(Vector2.down, dir));
                    Instantiate(hurtbox.effect, hurtbox.transform.position, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, dir)));
                }


            }
            
        }
    }
}
