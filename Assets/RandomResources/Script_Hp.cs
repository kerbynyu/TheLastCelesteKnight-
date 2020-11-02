using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Hp : MonoBehaviour
{
    public float MaxHp = 1;
    public float Hp;
    public AudioSource deathSound;
    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0) {
            AudioSource sound = Instantiate(deathSound, transform.position, transform.rotation);
            sound.Play();
            Destroy(gameObject);
        }
    }
}
