using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour {

    CutsceneDialogue cutsceneDialogue;
    PlayableDirector director;
    public PlayableAsset timelineAsset;

    private void Start()
    {
        cutsceneDialogue = GetComponent<CutsceneDialogue>();
        director = GetComponentInParent<PlayableDirector>();
        //Go();
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
        // for now, set back to normal
        //GameControl.control.SetCharacterState(characterControl.CharacterState.Normal);
    }
}
