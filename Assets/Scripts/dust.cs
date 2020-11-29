using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dust : MonoBehaviour
{
	public ParticleSystem runDust;
	private Animator anim;
	private GameObject player;
	private bool dusting;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        //runDust=transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        player=GameObject.FindGameObjectWithTag("Player");
        anim=player.GetComponent<Animator>();
        offset=player.transform.position-runDust.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    	var childTransform=runDust.transform;
        if(anim.GetBool("walking") && anim.GetBool("grounded") && !dusting){
        	runDust.Play();
        	dusting=true;
        }else if((!anim.GetBool("walking") || !anim.GetBool("grounded")) && dusting){
        	dusting=false;
        	float t=0f;
        	if(!anim.GetBool("grounded")) t=0.2f;
        	StartCoroutine(stop(runDust,t));
        	// GameObject copy=Instantiate(runDust.gameObject,childTransform.position,childTransform.rotation);
        	// Destroy(runDust.gameObject,5);
        	// runDust=copy.GetComponent<ParticleSystem>();
        }
        childTransform.position=player.transform.position-offset;
        if((anim.gameObject.GetComponent<SpriteRenderer>().flipX && childTransform.localScale.x>0)
         || (!anim.gameObject.GetComponent<SpriteRenderer>().flipX && childTransform.localScale.x<0)){
        	childTransform.localScale=new Vector3(-childTransform.localScale.x,childTransform.localScale.y,childTransform.localScale.z);
        	//childTransform.localPosition=new Vector3(-childTransform.localPosition.x,childTransform.localPosition.y,childTransform.localPosition.z);
       		offset=new Vector3(-offset.x,offset.y,offset.z);
       		// GameObject copy=Instantiate(runDust.gameObject,childTransform.position,childTransform.rotation);
       		// copy.transform.localScale=new Vector3(-childTransform.localScale.x,childTransform.localScale.y,childTransform.localScale.z);
       		// runDust.Stop();
       		// Destroy(runDust.gameObject,5);
       		// runDust=copy.GetComponent<ParticleSystem>();
       		// if(anim.GetBool("walking") && anim.GetBool("grounded")) runDust.Play();
       		// offset=new Vector3(-offset.x,offset.y,offset.z);

        }

    }

    IEnumerator stop(ParticleSystem pSystem, float t){
    	yield return new WaitForSeconds(t);
    	if(!dusting) runDust.Stop();
    }

}
