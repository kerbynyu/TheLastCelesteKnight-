using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offStage : MonoBehaviour
{
    public GameObject locator;
    public GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Health>()) { collision.gameObject.GetComponent<Health>().Hp -= 1; }
            collision.gameObject.transform.position = locator.transform.position;
            gm.blackScreen();
        }
    }
}
