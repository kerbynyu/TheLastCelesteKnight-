using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_hitBox : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //print("attacking player");
            GameObject player = collision.gameObject;
            if ( player.GetComponent<Script_movement>().counter1 <=0&& player.GetComponent<Script_movement>().counter2 <=0&&player.GetComponent<Script_movement>().counter3<=0)
            {
                Vector2 direction = player.transform.position - transform.position;
                direction = direction.normalized;
                player.GetComponent<Rigidbody2D>().AddForce(direction * 50f, ForceMode2D.Impulse);
                player.GetComponent<Script_movement>().Hp -= damage;
                player.GetComponent<Script_movement>().hurt = true;
                player.GetComponent<Script_movement>().counter2 = 0;
                player.GetComponent<Script_movement>().counter1 = 0;
            }
        }
    }
}
