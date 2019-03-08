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

    float fadeOutRate = 5f;
    float fadeInRate = 1.5f;
    
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

    public void StartMusic(AudioClip mainClip) { private_StartMusic(mainClip, null, true, true); }
    public void StartMusic(AudioClip introClip, AudioClip loopClip) { private_StartMusic(introClip, loopClip, true, true); }
    public void StartMusic(AudioClip mainClip, bool fadeOut) { private_StartMusic(mainClip, null, fadeOut, true); }
    public void StartMusic(AudioClip introClip, AudioClip mainClip, bool fadeOut) { private_StartMusic(introClip, mainClip, fadeOut, true); }
    public void StartMusic(AudioClip mainClip, bool fadeOut, bool fadeIn) { private_StartMusic(mainClip, null, fadeOut, fadeIn); }
    public void StartMusic(AudioClip introClip, AudioClip mainClip, bool fadeOut, bool fadeIn) { private_StartMusic(introClip, mainClip, fadeOut, fadeIn); }

    void private_StartMusic(AudioClip clip, AudioClip clip2 = null, bool fadeOut = true, bool fadeIn = true)
    {
        // if current music is null, there is nothing to fade out
        if(currentMusic != null)
        {
            // FADE OUT CALL HERE
            if (fadeOut)
                StartCoroutine(FadeOut(currentMusic));
            else
                currentMusic.Stop();
        }

        // if clip2 is not null then that means we have an intro clip as well
        if (clip2 != null)
        {
            StartCoroutine(StartThenLoop(clip, clip2, fadeIn));
        }
        else
        {
            PlayMusic(clip, true, fadeIn);
        }
    }

    IEnumerator StartThenLoop(AudioClip startClip, AudioClip loopClip, bool fade = true)
    {
        /*source_MUS.clip = startClip;
        source_MUS.loop = false;
        source_MUS.Play();*/
        var startSource = PlayMusic(startClip, false, fade);

        yield return new WaitUntil(() => startSource.time >= startClip.length || !startSource.isPlaying);

        /*source_MUS.clip = loopClip;
        source_MUS.loop = true;
        source_MUS.Play();*/
        PlayMusic(loopClip, true, false);
    }

    AudioSource PlayMusic(AudioClip clip, bool isLooped = true, bool fade = true)
    {
        var curMus = PlaySound(source_MUS, clip, isLooped, false, fade);
        currentMusic = curMus;
        return currentMusic;
    }

    public AudioSource PlaySFX(AudioClip clip, bool randomPitch = true)
    {
        return PlaySound(source_SFX, clip, false, randomPitch);
    }

    AudioSource PlaySound(List<AudioSource> sources, AudioClip clip, bool isLooped, bool randomPitch = true, bool fade = false)
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
        sources[sourceIndex].volume = 1f;
        if (fade)
        {
            StartCoroutine(FadeIn(sources[sourceIndex]));
        }
        else
        {
            sources[sourceIndex].Play();
        }

        return sources[sourceIndex];
    }

    IEnumerator FadeIn(AudioSource source)
    {
        source.volume = 0f;
        source.Play();
        while (source.volume < 1f)
        {
            source.volume += Time.deltaTime * fadeInRate;
            yield return null;
        }

        source.volume = 1f;
    }
    
    IEnumerator FadeOut(AudioSource source)
    {
        while(source.volume > 0f)
        {
            source.volume -= Time.deltaTime * fadeOutRate;
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
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
