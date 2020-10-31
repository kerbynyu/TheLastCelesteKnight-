using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointScript : MonoBehaviour{

    private GameMaster gm;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameMaster>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            gm.lastCheckPointPos = transform.position; 
        }
    }
}
