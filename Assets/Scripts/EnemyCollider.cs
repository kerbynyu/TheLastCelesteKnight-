using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{

    public bool isActive = true;

    public void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            isActive = false; 
            Debug.Log("DIE");
        }
    }

    public void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            isActive = false;
        }
    }
}
