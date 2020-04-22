using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour {

    CutsceneDialogue cutsceneDialogue;
    PlayableDirector director;

    private void Start()
    {
        cutsceneDialogue = GetComponent<CutsceneDialogue>();
        director = GetComponentInParent<PlayableDirector>();
        Go();
    }

    void Go()
    {
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
}
