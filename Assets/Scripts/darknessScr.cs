using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darknessScr : MonoBehaviour
{
    public GameObject par1;
    public GameObject par2;
    public GameObject par3;
    public GameObject par4;
    public GameObject par5;
    public GameObject par6;
    public GameObject par7;
    public GameObject par8;
    public radianceMovement radiance;
    public Transform risingMax;
    List<GameObject> particles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        particles.Add(par1);
        particles.Add(par2);
        particles.Add(par3);
        particles.Add(par4);
        particles.Add(par5);
        particles.Add(par6);
        particles.Add(par7);
        particles.Add(par8);
        for (int i = 0; i < particles.Count; i++)
        {
            GameObject thisParticle = particles[i];
            thisParticle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (radiance.phase3 || radiance.phase4 || radiance.phase5)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                GameObject thisParticle = particles[i];
                thisParticle.SetActive(true);
            }
        }
        if (radiance.uniquePlatformCounter > 180 || radiance.phase4ready)
        {
            if (par1.transform.position.y < risingMax.position.y)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    GameObject thisParticle = particles[i];
                    thisParticle.SetActive(true);
                    transform.Translate(new Vector2(0, 0.8f*Time.deltaTime));
                }
                
            }
        }
    }
}
