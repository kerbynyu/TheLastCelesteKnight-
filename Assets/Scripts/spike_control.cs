using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike_control : MonoBehaviour
{
    public Animator ani;
    public eHit_Box hitbox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("spike2"))
        {
            hitbox.gameObject.SetActive(true);
        }
        else { hitbox.gameObject.SetActive(false); }
    }
}
