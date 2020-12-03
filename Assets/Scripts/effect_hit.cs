using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_hit : MonoBehaviour
{
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("end"))
        {
            Destroy(gameObject);
        }
    }
}
