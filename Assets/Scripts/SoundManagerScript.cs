using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    static AudioSource audioSrc;
    public static AudioClip jumpSound;

    void Start(){
        audioSrc = GetComponent<AudioSource>();
        jumpSound = Resources.Load<AudioClip>("jump");

    }

    public static void playSound(string clip) {

        if(clip == "jump"){
            audioSrc.pitch = (Random.Range(-0.3f,1.1f)); //changes pitch 
            audioSrc.PlayOneShot(jumpSound, 0.05F);
            
        }
    }
}
