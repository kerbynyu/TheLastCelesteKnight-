using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSoundManager : MonoBehaviour
{
	public AudioClip[] dropSounds;
	public AudioClip[] pianoSounds;
	public AudioClip[] melodySounds;
	public AudioClip[] bellSounds;
	public AudioClip[] stringSounds;
	private AudioClip[][] musicClips;

	public AudioSource bgm;
	public AudioSource dropMaker;
	public AudioSource piano;
	public AudioSource melody;
	public AudioSource bells;
	public AudioSource strings;
	private AudioSource[] instruments;

	public float maxValue=1000f;
	public float stopNumber=50f;
	public float musicStopNumber=10f;
	public float increaseRate=1f;
	public float musicRate=0.005f;
	public float counter=0f;
	public float musicCounter=0f;
	private int currentChord=0;
	private bool crRunning=false;
    // Start is called before the first frame update
    void Start()
    {
        bgm=GetComponent<AudioSource>();
        musicClips=new AudioClip[][]{pianoSounds,melodySounds,bellSounds,stringSounds};
        instruments=new AudioSource[]{piano,melody,bells,strings};
    }

    // Update is called once per frame
    void Update()
    {
        counter+=increaseRate;
        musicCounter+=musicRate;
        if(counter>stopNumber) counter=stopNumber;
        float rng=Random.Range(0f,maxValue);
        if(rng<=counter && !dropMaker.isPlaying){
        	int k=Random.Range(0,9);
        	dropMaker.clip=dropSounds[k];
        	dropMaker.Play();
        	counter=0f;
        }
        if(musicCounter>stopNumber) musicCounter=stopNumber;
        rng=Random.Range(0f,maxValue);
        if(rng<=musicCounter){
        	int i=Random.Range(0,instruments.Length);
        	if(!instruments[i].isPlaying){
        		int p=Random.Range(0,musicClips[i].Length);
        		instruments[i].clip=musicClips[i][currentChord];
        		instruments[i].Play();
        		musicCounter=0f;
        		if(!crRunning) StartCoroutine(chordChange());
        	}
        	if(currentChord>3) currentChord=0;
        }
    }

    IEnumerator chordChange(){
    	float rng=Random.Range(0f,1f);
    	if(rng>0.2f){
	    	crRunning=true;
	    	yield return new WaitForSeconds(3);
	    	currentChord++;
	    	if(currentChord>3) currentChord=0;
	    	crRunning=false;
    	}
    }
}
