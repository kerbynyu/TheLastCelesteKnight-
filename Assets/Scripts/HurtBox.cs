using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public int side = 0;
    public bool solid = true;//wether this is a enemy with mass or just some grass
    public bool steel = false;//wether this is not moveable or not
    public GameObject owner;
    public Rigidbody2D thisRigidbody2d;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
