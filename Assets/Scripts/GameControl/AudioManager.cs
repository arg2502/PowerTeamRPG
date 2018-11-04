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
    public AudioClip sfx_miss;
    public AudioClip sfx_menuNav;
    public AudioClip sfx_menuSelect;

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

    void PlaySFX(AudioClip clip, bool randomPitch = true)
    {
        print("sfx play: " + clip);
        var pitchRandom = 1f;
        if (randomPitch)
        {
            // randomize pitch a little
            var low = 0.9f;
            var high = 1.1f;
            pitchRandom = Random.Range(low, high);
        }

        // find an empty source
        int sourceIndex = -1;

        for (int i = 0; i < source_SFX.Length; i++)
        {
            if (!source_SFX[i].isPlaying)
            {
                sourceIndex = i;
                break;
            }
        }
        // default to first just in case
        if (source_SFX[sourceIndex] == null)
            sourceIndex = 0;

        source_SFX[sourceIndex].pitch = pitchRandom;

        source_SFX[sourceIndex].clip = clip;
        source_SFX[sourceIndex].loop = false;
        source_SFX[sourceIndex].Play();
    }

    public void PlayHit()
    {
        PlaySFX(sfx_hit);
    }

    public void PlayBlock()
    {
        PlaySFX(sfx_block);
    }

    public void PlayMiss()
    {
        PlaySFX(sfx_miss, randomPitch: false);
    }

    public void PlayMenuNav()
    {
        // only play the navigation if the select sound is not playing
        if (!IsClipPlaying(sfx_menuSelect))
            PlaySFX(sfx_menuNav, randomPitch: false);
    }

    public void PlayMenuSelect()
    {
        PlaySFX(sfx_menuSelect, randomPitch: false);
    }

    public void Play(AudioClip clip)
    {
        PlaySFX(clip);
    }

    /// <summary>
    /// Checks all audio sources to see if it's currently playing the passed in audio clip.
    /// </summary>
    /// <param name="clip"></param>
    /// <returns>True if a match is found, meaning the clip is currently being played.
    /// False if no clip was found.</returns>
    bool IsClipPlaying(AudioClip clip)
    {
        // loop through all sources checking the clips
        foreach(var source in source_SFX)
        {
            // if the clip matches a source's currently playing clip, then return true
            if (source.clip == clip && source.isPlaying)
                return true;
        }

        // if the clip was not found, then return false
        return false;
    }
}
