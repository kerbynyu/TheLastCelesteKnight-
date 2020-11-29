using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private float length, starpos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        starpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(starpos + dist, transform.position.y, transform.position.z);
    }
}
