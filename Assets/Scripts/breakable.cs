using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakable : MonoBehaviour
{
	public Sprite brokenSprite;
	private PlayerAttack player;
	private Attack attack;
	private SpriteRenderer spr;
	private ParticleSystem par;
	private ParticleSystem par2;
	private bool destroyed=false;
    // Start is called before the first frame update
    void Start()
    {
    	player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        attack=gameObject.GetComponent<Attack>();
        spr=gameObject.GetComponent<SpriteRenderer>();
        par=gameObject.GetComponent<ParticleSystem>();
        par2=transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attack.hitted && !destroyed){
        	ParticleSystem.ShapeModule shape=par.shape;
        	ParticleSystem.VelocityOverLifetimeModule v=par2.velocityOverLifetime;
        	if(player.cHitbox==player.hitbox2){
        		shape.position=new Vector3(0f,1.48f,0f);
        		shape.rotation=new Vector3(0f,0f,200f);
        		shape.arc=140;
        		par2.gameObject.transform.rotation=Quaternion.Euler(0,0,-90);
        		AnimationCurve curveY = new AnimationCurve();
        		curveY.AddKey(0.0f, 0.0f);
        		curveY.AddKey(1.0f, 0.0f);
        		v.y=new ParticleSystem.MinMaxCurve(10.0f,curveY);
        		AnimationCurve curveX = new AnimationCurve();
        		curveX.AddKey(0.0f, 1.0f);
        		curveX.AddKey(1.0f, 0.0f);
        		v.x = new ParticleSystem.MinMaxCurve(10.0f, curveX);
        	}else if(player.cHitbox==player.hitbox3){
        		shape.rotation=new Vector3(0f,0f,90f);
        		par2.gameObject.transform.localScale=new Vector3(-par2.gameObject.transform.localScale.x,par2.gameObject.transform.localScale.y,par2.gameObject.transform.localScale.z);
        	}else if(player.cHitbox==player.hitbox4){
        		shape.rotation=new Vector3(0f,0f,50f);
        		par2.gameObject.transform.rotation=Quaternion.Euler(0,0,90);
        		AnimationCurve curveY = new AnimationCurve();
        		curveY.AddKey(0.0f, 0.0f);
        		curveY.AddKey(1.0f, 0.0f);
        		v.y=new ParticleSystem.MinMaxCurve(10.0f,curveY);
        		AnimationCurve curveX = new AnimationCurve();
        		curveX.AddKey(0.0f, 1.0f);
        		curveX.AddKey(1.0f, 0.0f);
        		v.x = new ParticleSystem.MinMaxCurve(10.0f, curveX);
        	}
        	spr.sprite=brokenSprite;
        	par.Play();
        	par2.Play();
        	destroyed=true;
        }
    }
}
