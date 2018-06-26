using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // SUPER BASIC FOR NOW -- REFORMAT LATER

    public AudioClip battleIntro;
    public AudioClip battleLoop;
    AudioSource mus_Source;

	// Use this for initialization
	void Start () {
        mus_Source = GetComponent<AudioSource>();
        StartCoroutine(StartThenLoop());		
	}
	
    IEnumerator StartThenLoop()
    {
        mus_Source.clip = battleIntro;
        mus_Source.loop = false;
        mus_Source.Play();

        yield return new WaitUntil(() => mus_Source.time >= battleIntro.length);

        mus_Source.clip = battleLoop;
        mus_Source.loop = true;
        mus_Source.Play();
    }
}
