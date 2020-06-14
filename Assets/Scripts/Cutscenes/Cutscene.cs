using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(CutsceneDialogue))]
[RequireComponent(typeof(Animator))] 
public class Cutscene : MonoBehaviour {

    CutsceneDialogue cutsceneDialogue;
    PlayableDirector director;
    public PlayableAsset timelineAsset;
    public TriggerType triggerType;

    public enum TriggerType { ROOM_ENTER, AFTER_ENTRANCE, ON_TRIGGER }

    private void Start()
    {
        cutsceneDialogue = GetComponent<CutsceneDialogue>();
        director = GetComponentInParent<PlayableDirector>();
    }

    public virtual void Play()
    {
        cutsceneDialogue = GetComponent<CutsceneDialogue>();
        director = GetComponentInParent<PlayableDirector>();
        director.playableAsset = timelineAsset;
        director.Play();
    }

    public void Pause()
    {
        director.Pause();        
        cutsceneDialogue.StartDialogue();
    }

    public void Resume()
    {
        director.Resume();
    }

    public virtual void Stop()
    {
        director.Stop();
    }
}
