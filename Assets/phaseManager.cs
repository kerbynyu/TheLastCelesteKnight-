using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class phaseManager : MonoBehaviour
{
    public AudioSource bgm;
    public Image dark;
    public float theAlpha;
    public radianceMovement rad;
    public GameObject edge1;
    public GameObject respawn1;
    public GameObject edge2;
    public GameObject respawn2;
    public Vector2 p1;
    public Vector2 p21;
    public Vector2 p22;
    public Vector2 p3;
    public Vector2 p4;
    public Vector2 ep41;
    public Vector2 ep42;
    public float endTime = 3;
    public float counter1 = -1;
    public float slowAni = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (rad.phase1)
        {
            respawn1.transform.position = p1;
            respawn2.transform.position = p1;
        }
        if (rad.phase2)
        {
            respawn1.transform.position = p21;
            respawn2.transform.position = p22;
        }
        if (rad.phase3)
        {
            respawn1.transform.position = p3;
            respawn2.transform.position = p3;
        }

        if (rad.phase4ready)
        {
            respawn1.transform.position = p4;
            respawn2.transform.position = p4;
            edge1.transform.position = ep41;
            edge2.transform.position = ep42;
        }

        if (!rad.alive)
        {
            if (counter1 == -1)
            {
                counter1 = endTime;
                Time.timeScale = slowAni;
            }
            else if (counter1 > 0)
            {
                
                counter1 -= Time.unscaledDeltaTime;
                theAlpha += 1 / endTime * Time.unscaledDeltaTime;
                theAlpha = Mathf.Min(1, theAlpha);
                Color c = dark.color;
                c.a = theAlpha;
                dark.color = c;
                bgm.volume = (1 - theAlpha)/2;
            }
            else
            {
                Time.timeScale = 1;
                //counter1 = -1;
                SceneManager.LoadScene("End");
            }
        }

        
    }
}
