using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkCircleScr : MonoBehaviour
{
    public int emissionCounter = 0;
    public bool ifEmission = false;
    public SpriteRenderer thisSR;
    public ParticleSystem par;
    public AudioSource au;
    private bool played = false;
    public GameObject one;
    public GameObject two;
    // Start is called before the first frame update
    void Start()
    {
        one.SetActive(false);
        two.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ifEmission)
        {
            one.SetActive(true);
            two.SetActive(true);
            if (!played)
            {
                au.Play();
                played = true;
            }
            if (emissionCounter < 50)
            {
                emissionCounter += 1;
                float alpha = thisSR.color.a;
                if (alpha > 0)
                {
                    alpha -= 0.05f;
                    thisSR.color = new Color(0, 0, 0, alpha);
                }
                if (emissionCounter > 30)
                {
                    par.Stop();
                }
            }
        }
        if (emissionCounter < 30)
        {
            par.Play();
        }
    }
}
