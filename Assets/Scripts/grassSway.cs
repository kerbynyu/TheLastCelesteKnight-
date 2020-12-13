using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassSway : MonoBehaviour
{
	public Sprite spriteLeft;
	public Sprite spriteRight;
	public int number=0;
	private int sway;
	private float duration=0.5f;
	private float time=0f;
	private SpriteRenderer spr;
	private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if(transform.localScale.x<1){
            var main=gameObject.GetComponent<ParticleSystem>().main;
            var rend=gameObject.GetComponent<ParticleSystemRenderer>();
            var scale=gameObject.GetComponent<ParticleSystem>().shape.scale;
            var shape=gameObject.GetComponent<ParticleSystem>().shape;
            main.startSize=main.startSize.constant/transform.localScale.x;
            rend.maxParticleSize=rend.maxParticleSize/transform.localScale.x;
            shape.scale=new Vector3(scale.x/transform.localScale.x,scale.y/transform.localScale.y,0f);
        }
        spr=gameObject.GetComponent<SpriteRenderer>();
        player=GameObject.FindGameObjectWithTag("Player");
        sway=0;
        gameObject.GetComponent<Animator>().SetInteger("number",number);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Attack>().health=gameObject.GetComponent<Health>();
    	if(!gameObject.GetComponent<Attack>().alive) sway=0;
        if(sway==1){
        	spr.sprite=spriteLeft;
        }else if(sway==-1){
        	spr.sprite=spriteRight;
        }
        if(Time.time>time+duration){
        	sway=0;
    		if(gameObject.GetComponent<Attack>().alive) gameObject.GetComponent<Animator>().enabled=true;
    	}
    }

    void OnTriggerEnter2D(Collider2D col){
    	if(col.gameObject.tag=="Player"){
    		if(player.GetComponent<SpriteRenderer>().flipX) sway=1;
    		else sway=-1;
    		gameObject.GetComponent<Animator>().enabled=false;
    		time=Time.time;
    	}
    }
}
