using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Health : MonoBehaviour
{
    public float maxHp = 10;
    public float Hp;
    float lHp;



    // Start is called before the first frame update
    void Start()
    {
        Hp = maxHp;
        lHp = Hp;
    }

    // Update is called once per frame
    void Update()
    {
       

        if(Hp > maxHp) {
            Hp = Mathf.Min(maxHp, Hp);
        }
        


    }
}
