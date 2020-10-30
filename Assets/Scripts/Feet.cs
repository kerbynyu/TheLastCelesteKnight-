using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{

    public GameObject player;
    SimplePhysicsController playerCtr1;

    void Start(){
        playerCtr1 = GetComponentInParent<SimplePhysicsController>();

    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Ground") && playerCtr1.isJumping) {
            playerCtr1.isJumping = false;

        }
        if (other.gameObject.CompareTag("Platform")&& playerCtr1.isJumping) {
            playerCtr1.isJumping = false;
            player.transform.parent = other.gameObject.transform;

        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform")) {
            player.transform.parent = null; 
        }
    }


}
       

