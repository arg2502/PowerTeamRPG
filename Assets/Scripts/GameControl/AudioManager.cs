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
    public GameObject sfx_obj;
    AudioSource[] source_SFX;
    //public AudioSource source_SFX;

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
        source_SFX = sfx_obj.GetComponentsInChildren<AudioSource>();
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

        // find an empty source
        AudioSource source = null;

        for(int i = 0; i < source_SFX.Length; i++)
        {
            if (!source_SFX[i].isPlaying)
            {
                source = source_SFX[i];
                break;
            }
        }
        // default to first just in case
        if (source == null)
            source = source_SFX[0];

        source.pitch = random;
        //print(source_SFX.pitch);

        source.clip = clip;
        source.loop = false;
        source.Play();
    }

    public void PlayHit()
    {
        PlaySFX(sfx_hit);
    }

    public void PlayBlock()
    {
        PlaySFX(sfx_block);
    }

    public void Play(AudioClip clip)
    {
        PlaySFX(clip);
    }
}
