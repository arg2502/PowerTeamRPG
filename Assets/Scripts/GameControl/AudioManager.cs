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
    bool fadingOut = false;
    bool fadingIn = false;    
    
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

    public void StartMusic(AudioClip mainClip) { private_StartMusic(mainClip, null, true, true, false); }
    public void StartMusic(AudioClip introClip, AudioClip loopClip) { private_StartMusic(introClip, loopClip, true, true, false); }
    public void StartMusic(AudioClip mainClip, bool fadeOut) { private_StartMusic(mainClip, null, fadeOut, true, false); }
    public void StartMusic(AudioClip introClip, AudioClip mainClip, bool fadeOut) { private_StartMusic(introClip, mainClip, fadeOut, true, false); }
    public void StartMusic(AudioClip mainClip, bool fadeOut, bool fadeIn) { private_StartMusic(mainClip, null, fadeOut, fadeIn, false); }
    public void StartMusic(AudioClip introClip, AudioClip mainClip, bool fadeOut, bool fadeIn) { private_StartMusic(introClip, mainClip, fadeOut, fadeIn, false); }
    public void PauseCurrentAndStartNewMusic(AudioClip mainClip) { private_StartMusic(mainClip, null, true, true, true); }
    public void PauseCurrentAndStartNewMusic(AudioClip introClip, AudioClip loopClip) { private_StartMusic(introClip, loopClip, true, true, true); }
    public void PauseCurrentAndStartNewMusic(AudioClip introClip, AudioClip loopClip, bool fadeOut, bool fadeIn) { private_StartMusic(introClip, loopClip, fadeOut, fadeIn, true); }

    void private_StartMusic(AudioClip clip, AudioClip clip2 = null, bool fadeOut = true, bool fadeIn = true, bool pause = false)
    {
        // if current music is null, there is nothing to fade out
        if(currentMusic != null)
        {
            // FADE OUT CALL HERE
            // if the clip we want to play is already currently playing, just stop the source
            //// this is to avoid getting stuck in a constant loop of fading out and fading in
            if (!fadeOut || currentMusic.clip == clip)
                currentMusic.Stop();
            else
                StartCoroutine(FadeOut(currentMusic, pause));

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

    AudioSource PlayMusic(AudioClip clip, bool isLooped = true, bool fade = true, bool pause = false)
    {
        var curMus = PlaySound(source_MUS, clip, isLooped, false, fade);
        currentMusic = curMus;
        return currentMusic;
    }

    public AudioSource PlaySFX(AudioClip clip, bool randomPitch = true)
    {
        return PlaySound(source_SFX, clip, false, randomPitch);
    }

    AudioSource PlaySound(List<AudioSource> sources, AudioClip clip, bool isLooped, bool randomPitch = true, bool fade = false, bool pause = false)
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

        // first, check if any sources already have the same clip
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].clip == clip) //!sources[i].isPlaying)
            {
                sourceIndex = i;
                break;
            }
        }

        // if no match, now check for any empty sources
        if (sourceIndex < 0)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                if (sources[i].clip == null)
                {
                    sourceIndex = i;
                    break;
                }
            }
        }

        // if still no match, find one that is not paused (time == 0)
        if (sourceIndex < 0)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                if (sources[i].time == 0)
                {
                    sourceIndex = i;
                    break;
                }
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
            StartCoroutine(FadeIn(sources[sourceIndex], pause));
        }
        else
        {
            sources[sourceIndex].Play();
        }

        return sources[sourceIndex];
    }

    IEnumerator FadeIn(AudioSource source, bool pause = false)
    {
        //if (fadingOut) yield break;
        //fadingIn = true;
        // ADD SOMETHING TO THE FADING CHECK SO THAT ONLY RETURN IF THE SAME SOURCE WANTS TO FADE IN AND OUT AT THE SAME TIME

        source.volume = 0f;
        //if (pause) source.UnPause();
        //else source.Play();
        source.Play();

        while (source.volume < 1f)
        {
            source.volume += Time.deltaTime * fadeInRate;
            yield return null;
        }

        source.volume = 1f;

        //fadingIn = false;
    }
    
    IEnumerator FadeOut(AudioSource source, bool pause = false)
    {
        //if (fadingIn) yield break;
        //fadingOut = true;

        while(source.volume > 0f)
        {
            source.volume -= Time.deltaTime * fadeOutRate;
            yield return null;
        }

        source.volume = 0f;
        if (pause) source.Pause();
        else source.Stop();
        
        //fadingOut = false;
    }
        
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
