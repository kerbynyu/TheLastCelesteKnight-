using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck2 : MonoBehaviour {

    public bool isGrounded = false;


    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            isGrounded = true;

        }
        if (other.gameObject.CompareTag("Platform")) {
            isGrounded = true;

        }

        if (other.gameObject.CompareTag("Enemy")) {
            isGrounded = true;
        }
    }

    public void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Ground")) {
            isGrounded = true;

        }
        if (other.gameObject.CompareTag("Platform")) {
            isGrounded = true;

        }

        if (other.gameObject.CompareTag("Enemy")) {
            isGrounded = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other) {
        isGrounded = false;

        if (other.gameObject.CompareTag("Enemy")) {
            isGrounded = true;
        }

    }
}
