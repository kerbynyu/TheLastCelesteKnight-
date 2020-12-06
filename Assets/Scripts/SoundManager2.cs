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
    public AudioSource swoosh;
    public AudioSource clang;
    public AudioSource boneCrush;
    public AudioSource dash;
    public AudioSource hurt;
    public AudioSource charging;
    public AudioSource charged;
    public AudioClip[] boneCrushSounds;
	public float initialLength;
    public float lowpitch=0.7f;
    public float highpitch=1.3f;
	private bool stepping=false;
    private bool previousGrnd;
    private bool previousDJump;
    private PlayerAttack pAttack;
    private bool isAttacking=false;
    private float timeSinceSlash=0f;
    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
        pAttack=player.GetComponent<PlayerAttack>();
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

        if(!isAttacking && pAttack.isAttacking && boneCrush.isPlaying){
            boneCrush.Stop();
        }

        if(pAttack.isAttacking && !boneCrush.isPlaying){
            /*
            var attacks=FindObjectsOfType<enemy_rush_attack>();
            foreach(enemy_rush_attack a in attacks){
                if(a.alive && (a.hitted || a.counter3>0)){
                    if(Time.time-timeSinceSlash<2f){
                        boneCrush.clip=boneCrushSounds[0];
                    }else{
                        var i=Random.Range(0,4);
                        if(i!=0) i=1;
                        boneCrush.clip=boneCrushSounds[i];
                    }
                    boneCrush.Play();
                    timeSinceSlash=Time.time;
                }
            }
            var attacks2=FindObjectsOfType<enemy_crow_attack>();
            foreach(enemy_crow_attack a in attacks2){
                if(a.alive && (a.hitted || a.counter1>0)){
                    if(Time.time-timeSinceSlash<2f){
                        boneCrush.clip=boneCrushSounds[0];
                    }else{
                        var i=Random.Range(0,4);
                        if(i!=0) i=1;
                        boneCrush.clip=boneCrushSounds[i];
                    }
                    boneCrush.Play();
                    timeSinceSlash=Time.time;
                }
            }

            var attacks3 = FindObjectsOfType<gruzzerAttack>();
            foreach (gruzzerAttack a in attacks3)
            {
                if (a.alive && (a.hitted))
                {
                    if (Time.time - timeSinceSlash < 2f)
                    {
                        boneCrush.clip = boneCrushSounds[0];
                    }
                    else
                    {
                        var i = Random.Range(0, 4);
                        if (i != 0) i = 1;
                        boneCrush.clip = boneCrushSounds[i];
                    }
                    boneCrush.Play();
                    timeSinceSlash = Time.time;
                }
            }

            var attacks4 = FindObjectsOfType<radianceMovement>();
            foreach (radianceMovement a in attacks4)
            {
                if (a.alive && (a.hitted))
                {
                    if (Time.time - timeSinceSlash < 2f)
                    {
                        boneCrush.clip = boneCrushSounds[0];
                    }
                    else
                    {
                        var i = Random.Range(0, 4);
                        if (i != 0) i = 1;
                        boneCrush.clip = boneCrushSounds[i];
                    }
                    boneCrush.Play();
                    timeSinceSlash = Time.time;
                }
            }
            */
        }

        if(player.GetComponent<SimplePhysicsController>().dashCounter>0 && !dash.isPlaying){
            dash.Play();
        }

        if(pAttack.isAttacking && !isAttacking){
            swoosh.Stop();
            swoosh.Play();
        }

        //     GameObject clone=Instantiate(pAttack.cHitbox.gameObject,player.transform);
        //     print(clone);
        //     clone.layer=0;
        //     List<Collider2D> contacts=new List<Collider2D>();
        //     clone.GetComponent<BoxCollider2D>().GetContacts(contacts);
        //     foreach(Collider2D c in contacts){
        //         print(c);
        //         if(c.gameObject.tag=="Platform" || c.gameObject.tag=="Ground"){
        //             clang.Stop();
        //             clang.Play();
        //             break;
        //         }
        //     }
        //     //Destroy(clone);            
        //     // var cast=Physics2D.BoxCast(pAttack.cHitbox.GetComponent<BoxCollider2D>().bounds.center,pAttack.cHitbox.GetComponent<BoxCollider2D>().bounds.extents,0f,Vector2.right,1f,LayerMask.GetMask("Platform"));

        //     // if ();
        //     // {

        //     //     clang.Stop();
        //     //     clang.Play();
        //     // }
        // }

        isAttacking=pAttack.isAttacking;
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

    public void playHit()
    {
        if (Time.time - timeSinceSlash < 2f)
        {
            boneCrush.clip = boneCrushSounds[0];
        }
        else
        {
            var i = Random.Range(0, 4);
            if (i != 0) i = 1;
            boneCrush.clip = boneCrushSounds[i];
        }
        boneCrush.Play();
        timeSinceSlash = Time.time;
    }

    public void playCharge()
    {
        charged.Play();
    }

    public void playCharging()
    {
        charging.Play();
    }

    public void playHitted()
    {
        hurt.Play();
    }
}
