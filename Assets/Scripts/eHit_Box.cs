using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eHit_Box : MonoBehaviour {
    public int side = 0;//0 for neutral, 1 for player, -1 for enemy
    public GameObject owner;
    public float damage = 0;
    public float force = 5f;
    public float xMovement = 5f;
    public float yMovement = 5f;
    public bool hitPlayer = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay2D(Collider2D collision) {
        //if the tag is hurtbox and they really have hurboxes
        if (collision.gameObject.CompareTag("hurtBox") && collision.gameObject.GetComponent<HurtBox>()) {
            HurtBox hurtbox = collision.gameObject.GetComponent<HurtBox>();
            if (hurtbox.side != side) {
                hurtbox.owner.GetComponent<Attack>().hitted = true;
                hurtbox.owner.GetComponent<Health>().Hp -= damage;
                //print("hitted");
                hurtbox.owner.transform.Translate(new Vector3(xMovement * Mathf.Sign(hurtbox.owner.transform.position.x - owner.transform.position.x), yMovement, 0));

                hitPlayer = true;
            }

        }
    }
}
