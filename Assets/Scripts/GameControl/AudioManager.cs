using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour{

    [Header("Sources")]
    public GameObject mus_obj;
    public GameObject sfx_obj;

    List<AudioSource> source_MUS;    
    List<AudioSource> source_SFX;

    AudioSource currentMusic = null;

    float fadeRate = 1f;
    
    public void Init () {
        // instantiating music source obj
        var musList = mus_obj.GetComponentsInChildren<AudioSource>();
        source_MUS = new List<AudioSource>();
        foreach (var a in musList)
            source_MUS.Add(a);

        var sfxList = sfx_obj.GetComponentsInChildren<AudioSource>();
        source_SFX = new List<AudioSource>();
        foreach (var a in sfxList)
            source_SFX.Add(a);

    }

    public void StartMusic(AudioClip clip, AudioClip clip2 = null, bool fade = true)
    {
        // if current music is null, there is nothing to fade out
        if(currentMusic != null)
        {
            // FADE OUT CALL HERE
            //StartCoroutine(FadeOut(currentMusic));
            currentMusic.Stop();
        }

        // if clip2 is not null then that means we have an intro clip as well
        if (clip2 != null)
        {
            StartCoroutine(StartThenLoop(clip, clip2));
        }
        else
        {
            PlayMusic(clip);
        }
    }

    IEnumerator StartThenLoop(AudioClip startClip, AudioClip loopClip)
    {
        /*source_MUS.clip = startClip;
        source_MUS.loop = false;
        source_MUS.Play();*/
        var startSource = PlayMusic(startClip, false);

        yield return new WaitUntil(() => startSource.time >= startClip.length || !startSource.isPlaying);

        /*source_MUS.clip = loopClip;
        source_MUS.loop = true;
        source_MUS.Play();*/
        PlayMusic(loopClip);
    }

    AudioSource PlayMusic(AudioClip clip, bool isLooped = true)
    {
        var curMus = PlaySound(source_MUS, clip, isLooped, false);
        currentMusic = curMus;
        return currentMusic;
    }

    public AudioSource PlaySFX(AudioClip clip, bool randomPitch = true)
    {
        return PlaySound(source_SFX, clip, false, randomPitch);
    }

    AudioSource PlaySound(List<AudioSource> sources, AudioClip clip, bool isLooped, bool randomPitch = true)
    {
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

        for (int i = 0; i < sources.Count; i++)
        {
            if (!sources[i].isPlaying)
            {
                sourceIndex = i;
                break;
            }
        }
        // default to first just in case
        if (sources[sourceIndex] == null)
            sourceIndex = 0;

        sources[sourceIndex].pitch = pitchRandom;

        sources[sourceIndex].clip = clip;
        sources[sourceIndex].loop = isLooped;

        //if(sources == source_MUS)
        //{
        //    StartCoroutine(FadeIn(sources[sourceIndex]));
        //}
        sources[sourceIndex].Play();

        return sources[sourceIndex];
    }

    IEnumerator FadeIn(AudioSource source)
    {
        source.volume = 0f;
        while (source.volume < 1f)
        {
            source.volume += Time.deltaTime * fadeRate;
            yield return null;
        }

        source.volume = 1f;
    }

    IEnumerator FadeOut(AudioSource source)
    {
        while(source.volume > 0f)
        {
            source.volume -= Time.deltaTime * fadeRate;
            yield return null;
        }

        source.volume = 0f;
    }
    /*public void PlayHit()
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
    }*/
    
    /// <summary>
    /// Checks all audio sources to see if it's currently playing the passed in audio clip.
    /// </summary>
    /// <param name="clip"></param>
    /// <returns>True if a match is found, meaning the clip is currently being played.
    /// False if no clip was found.</returns>
    public bool IsClipPlaying(AudioClip clip)
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
