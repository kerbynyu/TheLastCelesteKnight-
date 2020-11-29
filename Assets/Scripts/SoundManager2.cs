using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
	private GameObject player;
	private Animator anim;
    private GroundCheck2 grnd;
	public AudioSource stepSound;
    public AudioSource jumpSound;
    public AudioSource landSound;
	public float initialLength;
    public float lowpitch=0.7f;
    public float highpitch=1.3f;
	private bool stepping=false;
    private bool previousGrnd;
    private bool previousDJump;
    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        grnd=player.GetComponentInChildren<GroundCheck2>();
        anim=player.GetComponent<Animator>();
        //initialLength=aud.clip.length;
    	initialLength=0.42f;
        previousGrnd=grnd.isGrounded;
        previousDJump=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.GetBool("walking") && grnd.isGrounded && !stepping){
        	StartCoroutine(step());
        }
        if((player.GetComponent<SimplePhysicsController>().isJumping && !jumpSound.isPlaying) || (!previousDJump && previousDJump!=player.GetComponent<SimplePhysicsController>().isDoubleJumping)){
            jumpSound.Stop();
            jumpSound.Play();
        }
        if(grnd.isGrounded && !previousGrnd){
            jumpSound.Stop();
            landSound.Play();
        }
        previousGrnd=grnd.isGrounded;
        previousDJump=player.GetComponent<SimplePhysicsController>().isDoubleJumping;
    }

    IEnumerator step(){
    	stepping=true;
    	while(anim.GetBool("walking") && grnd.isGrounded){
    		stepSound.Play();
    		stepSound.pitch=Random.Range(lowpitch,highpitch);
    		yield return new WaitForSeconds(initialLength);
    		stepSound.Stop();
    	} 	
    	stepping=false;
    }
}
