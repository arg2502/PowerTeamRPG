using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // SUPER BASIC FOR NOW -- REFORMAT LATER

    [Header("Music")]
    public AudioClip battleIntro;
    public AudioClip battleLoop;

    [Header("SFX")]
    public AudioClip sfx_hit;
    public AudioClip sfx_block;

    [Header("Sources")]
    public AudioSource source_MUS;
    public AudioSource source_SFX;

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
        source_MUS.clip = battleIntro;
        source_MUS.loop = false;
        source_MUS.Play();

        yield return new WaitUntil(() => source_MUS.time >= battleIntro.length || !source_MUS.isPlaying);

        source_MUS.clip = battleLoop;
        source_MUS.loop = true;
        source_MUS.Play();
    }

    void PlaySFX(AudioClip clip)
    {
        // randomize pitch a little
        var low = 0.9f;
        var high = 1.1f;
        var random = Random.Range(low, high);
        source_SFX.pitch = random;
        print(source_SFX.pitch);

        source_SFX.clip = clip;
        source_SFX.loop = false;
        source_SFX.Play();
    }

    public void PlayHit()
    {
        PlaySFX(sfx_hit);
    }

    public void PlayBlock()
    {
        PlaySFX(sfx_block);
    }
}
