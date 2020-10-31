using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour{

    public GameObject enemy;
    public Transform player;
    public float agroRange;
    public float speed;
    public bool isGrounded = false; 
    public Transform castPoint;
    public int jumpCount=0;
    public bool isFacingLeft;
    public Rigidbody2D rb2d;
    public bool isAgro;

    private bool isSearching;
    Animator anim;

    void Start(){
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }


    void Update() {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (CanSeePlayer(agroRange)) {
            isAgro = true; 

        }else {
            
            if(isAgro == true) {
                if (isSearching) {
                    isSearching = true;
                    Invoke("StopChasingPlayer", 5);

                }
            }

        }
        if (isAgro) {
            ChasePlayer();
        }

        if (GameObject.Find("Head").GetComponent<EnemyCollider>().isActive == false) {
            this.gameObject.SetActive(false);
        }
    }


    void ChasePlayer() {
        if (anim.gameObject.activeSelf) {
            anim.SetTrigger("Walking");
        }

        if (transform.position.x < player.position.x) {
            rb2d.velocity = new Vector2(speed, 0);
            transform.rotation = Quaternion.Euler(0, 180f, 0);

            isFacingLeft = false;

        } else {
            rb2d.velocity = new Vector2(-speed, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingLeft = true;
        }
    }


    void StopChasingPlayer(){
        isAgro = false;
        isSearching = false; 
        rb2d.velocity = new Vector2(0, 0);
        anim.SetTrigger("Idle");

    }


    bool CanSeePlayer(float distance) {
       
        bool val = false;
        float castDist = distance;

        if (isFacingLeft) {
            castDist = -distance; 
        }

        Vector2 endPos = castPoint.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos,1 << LayerMask.NameToLayer("Action"));

        if(hit.collider!= null) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                val = true;

            }else {
                val = false; 
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        } else {
            Debug.DrawLine(castPoint.position, endPos, Color.blue);

        }
        return val; 
    }

   
}
