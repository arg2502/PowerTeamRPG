using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // SUPER BASIC FOR NOW -- REFORMAT LATER

    public AudioClip battleIntro;
    public AudioClip battleLoop;
    public AudioClip sfx_hit;
    public AudioSource mus_Source;
    public AudioSource sfx_Source;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        //mus_Source = GetComponent<AudioSource>();
        StartCoroutine(StartThenLoop());		
	}
	
    IEnumerator StartThenLoop()
    {
        mus_Source.clip = battleIntro;
        mus_Source.loop = false;
        mus_Source.Play();

        yield return new WaitUntil(() => mus_Source.time >= battleIntro.length || !mus_Source.isPlaying);

        mus_Source.clip = battleLoop;
        mus_Source.loop = true;
        mus_Source.Play();
    }

    public void PlayHit()
    {
        // randomize pitch a little
        var low = 0.9f;
        var high = 1.1f;
        var random = Random.Range(low, high);
        sfx_Source.pitch = random;
        print(sfx_Source.pitch);

        sfx_Source.clip = sfx_hit;
        sfx_Source.loop = false;
        sfx_Source.Play();
    }
}
