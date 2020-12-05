using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beam : MonoBehaviour
{
    public Animator thisAnim;
    public eHit_Box hitbox;
    private int destroyCounter = 0;
    private int lastCounter = 0;
    public bool ifPlay = false;
    public AudioSource beamAudio;
    private bool its2 = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (!beamAudio.isPlaying && ifPlay && its2)
        {
            beamAudio.Play();
            ifPlay = false;
        }

        if (thisAnim.GetCurrentAnimatorStateInfo(0).IsName("beamBurst2"))
        {
            its2 = true;
            lastCounter += 1;
            if (lastCounter > 25)
            {
                thisAnim.SetTrigger("launched");
            }
        }

        if (thisAnim.GetCurrentAnimatorStateInfo(0).IsName("beamBurst3"))
        {
            destroyCounter += 1;
            if (destroyCounter > 2)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (thisAnim.GetCurrentAnimatorStateInfo(0).IsName("beamBurst2"))
        {
            hitbox.gameObject.SetActive(true);
        }
        else { hitbox.gameObject.SetActive(false); }
    }
}
