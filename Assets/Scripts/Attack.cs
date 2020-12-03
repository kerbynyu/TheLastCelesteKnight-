using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{



    public Health health;
    public bool hitted=false;
    // Start is called before the first frame update

    public virtual void Update()
    {
        if (health.Hp <= 0)
        {
            Destroy(gameObject);
        }
        //print("atttack update");
        hitted = false;
    }


}
