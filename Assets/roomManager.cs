using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class roomManager : MonoBehaviour
{
    public Image dark;
    public bool end = false;
    public float theAlpha;
    public float endTime = 1;
    public float counter1 = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            if (counter1 == -1)
            {
                counter1 = endTime;
                Time.timeScale = 0;
            }
            else if (counter1 > 0)
            {
                counter1 -= Time.unscaledDeltaTime;
                theAlpha += 1 / endTime * Time.unscaledDeltaTime;
                theAlpha = Mathf.Min(1, theAlpha);
                Color c = dark.color;
                c.a = theAlpha;
                dark.color = c;
            }
            else
            {
                Time.timeScale = 1;
                //counter1 = -1;
                SceneManager.LoadScene("radiance");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            end = true;
        }
    }
}
