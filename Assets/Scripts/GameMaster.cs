using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private static GameMaster instance;
    public Vector2 lastCheckPointPos;

    private SimplePhysicsController sc;
    public int score = 0;

    private void Start() {
        //sc = GameObject.FindGameObjectWithTag("Player").GetComponent<SimplePhysicsController>();
    }

    private void Update() {
        //newScore = sc.score;
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);

        }else {
            Destroy(gameObject);

        }
    }
}
