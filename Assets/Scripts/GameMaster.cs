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
    //public Image health;
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
    public float disabledDarkness = 0.6f;
    public int score = 0;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Text textJ;
    public Text textK;
    public Text textL;
    public Text textO;
    public Image attackBlank;
    public Image jumpBlank;
    public Image dashBlank;
    public Image chargeBlank;

    public int shakeFrames=10;
    public int shakeCounter=0;
    public float shakeOffset=5f;



    private void Start() {
       
        sc = GameObject.FindGameObjectWithTag("Player").GetComponent<SimplePhysicsController>();
        pa = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        hl = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        
    }

    private void Update() {
        if (Input.GetKey(KeyCode.J)) { textJ.fontStyle = FontStyle.Bold; } else { textJ.fontStyle = FontStyle.Normal; }
        if (Input.GetKey(KeyCode.K)) { textK.fontStyle = FontStyle.Bold; } else { textK.fontStyle = FontStyle.Normal; }
        if (Input.GetKey(KeyCode.L)) { textL.fontStyle = FontStyle.Bold; } else { textL.fontStyle = FontStyle.Normal; }
        if (Input.GetKey(KeyCode.O)) { textO.fontStyle = FontStyle.Bold; } else { textO.fontStyle = FontStyle.Normal; }

        if (pa.counter6 > 0) { Color tc= attackBlank.color; tc.a = disabledDarkness; attackBlank.color = tc; } else { Color tc = attackBlank.color; tc.a = 0; attackBlank.color = tc; }
        if (!sc.doubleJumpEnabled) { Color tc = jumpBlank.color; tc.a = disabledDarkness; jumpBlank.color = tc; } else { Color tc = jumpBlank.color; tc.a = 0; jumpBlank.color = tc; }
        if (sc.dashAgainCounter>0) { Color tc = dashBlank.color; tc.a = disabledDarkness; dashBlank.color = tc; } else { Color tc = dashBlank.color; tc.a = 0; dashBlank.color = tc; }
        if (pa.energy<3&&pa.eUsed==0) { Color tc = chargeBlank.color; tc.a = disabledDarkness; chargeBlank.color = tc; } else { Color tc = chargeBlank.color; tc.a = 0; chargeBlank.color = tc; }
        //newScore = sc.score;
        //health.rectTransform.sizeDelta = new Vector2(hl.Hp * 10f, 10f);
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hl.Hp)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }


            if (i < hl.maxHp)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        energyBar.fillAmount = pa.energy/9;
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

        if(shakeCounter>0){
            shakeCounter--;            
            Camera.main.transform.position=Camera.main.transform.position + new Vector3(Random.Range(-shakeOffset,shakeOffset),Random.Range(-shakeOffset,shakeOffset),0);
        }
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
