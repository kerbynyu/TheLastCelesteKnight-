using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSoundManager : MonoBehaviour
{
	public AudioClip[] dropSounds;
	public AudioSource bgm;
	public AudioSource dropMaker;

	public float maxValue=1000f;
	public float stopNumber=50f;
	public float increaseRate=1f;
	public float counter=0f;
    // Start is called before the first frame update
    void Start()
    {
        bgm=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        counter+=increaseRate;
        if(counter>stopNumber) counter=stopNumber;
        float rng=Random.Range(0f,maxValue);
        if(rng<=counter && !dropMaker.isPlaying){
        	int k=Random.Range(0,9);
        	dropMaker.clip=dropSounds[k];
        	dropMaker.Play();
        	counter=0f;
        	print(k);
        }
    }
}
