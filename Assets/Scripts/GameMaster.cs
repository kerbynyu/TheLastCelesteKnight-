using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

    private static GameMaster instance;
    public Vector2 lastCheckPointPos;

    private SimplePhysicsController sc;
    private PlayerAttack pa;
    private Health hl;
    public Image health;
    public Image energyBar;
    public Image dark;
    public Image white;
    public float transTime = 1;
    public float counter1 = 0;
    public float pauseTime = 2;
    public float counter2 = 0;
    public float counter3 = -1;
    public float counter4 = -1;
    public float theAlpha = 0f;
    public float theAlpha2 = 0f;
    public float deathTime = 2;

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
        if (pa.energy >= 3||pa.eUsed>0) { energyBar.GetComponent<Image>().color = new Color(1, 1, 1);  }
        else { energyBar.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);  }

        if (counter1 > 0)
        {
            
            counter1 -= Time.deltaTime;
            theAlpha -=1f/transTime*Time.deltaTime;
            theAlpha = Mathf.Max(0, theAlpha);
        }

        if (counter4 > 0)
        {
            counter4 -= Time.unscaledDeltaTime;
            theAlpha += 1 / deathTime * Time.unscaledDeltaTime;
            theAlpha = Mathf.Min(1, theAlpha);
        }
        else if (counter4 > -1)
        {
            counter4 = -1;
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            ResumeGame();
        }
        
        Color c = dark.color;
        c.a = theAlpha;
        dark.color = c;

        if (counter3 > 0)
        {

            counter3 -= Time.unscaledDeltaTime;
        }
        else if(counter3 >-1)
        {
            counter3 = -1;
            ResumeGame();
            counter2 = transTime;
        }
        if (counter2 > 0)
        {

            counter2 -= Time.unscaledDeltaTime;
            theAlpha2 -= 1 /transTime * Time.unscaledDeltaTime;
            theAlpha2 = Mathf.Max(0, theAlpha2);
        }

        Color c2 = white.color;
        c2.a = theAlpha2;
        white.color = c2;
    }

    private void Awake() {
        whiteScreen();
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(instance);
            /*DontDestroyOnLoad(health);
            DontDestroyOnLoad(energyBar);
            DontDestroyOnLoad(dark);*/

        }
        else {
            Destroy(gameObject);

        }
        
       
    }

    public void blackScreen()
    {
        counter1 = transTime;
        Color c = dark.color;
        theAlpha = 1f;
        c.a = theAlpha;
        dark.color = c;
    }

    public void deathScreen()
    {
        PauseGame();
        counter4 = deathTime;
        Color c = dark.color;
        theAlpha = 0f;
        c.a = theAlpha;
        dark.color = c;
    }

    public void whiteScreen()
    {
        PauseGame();
        counter3 = pauseTime;
        Color c = white.color;
        theAlpha2 = 1f;
        c.a = theAlpha2;
        white.color = c;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
