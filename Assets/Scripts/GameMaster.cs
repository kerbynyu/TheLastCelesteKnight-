using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private static GameMaster instance;
    public Vector2 lastCheckPointPos;

    private SimplePhysicsController sc;
    private PlayerAttack pa;
    private Health hl;
    public Image health;
    public Image energyBar;

    public int score = 0;

    private void Start() {
        sc = GameObject.FindGameObjectWithTag("Player").GetComponent<SimplePhysicsController>();
        pa = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        hl = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update() {
        //newScore = sc.score;
        health.rectTransform.sizeDelta = new Vector2(hl.Hp * 10f, 10f);
        energyBar.rectTransform.sizeDelta = new Vector2(50f, (50f/9f)*pa.energy);
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
