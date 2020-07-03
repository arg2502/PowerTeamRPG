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

    public NPCHero cs_jethro, cs_cole, cs_eleanor, cs_jouliette;

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

    protected IEnumerator MergePlayers()
    {
        if (GameControl.control.currentCharacterInt == 0)
        {
            FindObjectOfType<characterControl>().transform.position = cs_jethro.transform.position;
            cs_jethro.gameObject.SetActive(false);        
        }
        else if (GameControl.control.currentCharacterInt == 1)
        {
            FindObjectOfType<characterControl>().transform.position = cs_cole.transform.position;
            cs_cole.gameObject.SetActive(false);            
        }
        else if (GameControl.control.currentCharacterInt == 2)
        {
            FindObjectOfType<characterControl>().transform.position = cs_eleanor.transform.position;
            cs_eleanor.gameObject.SetActive(false);
        }
        else if (GameControl.control.currentCharacterInt == 3)
        {
            FindObjectOfType<characterControl>().transform.position = cs_jouliette.transform.position;
            cs_jouliette.gameObject.SetActive(false);
        }

        if (cs_jethro != null && cs_jethro.gameObject.activeSelf) StartCoroutine(cs_jethro.MergeIntoPlayer());
        if(cs_cole != null && cs_cole.gameObject.activeSelf) StartCoroutine(cs_cole.MergeIntoPlayer());
        if(cs_eleanor != null && cs_eleanor.gameObject.activeSelf) StartCoroutine(cs_eleanor.MergeIntoPlayer());
        if(cs_jouliette != null && cs_jouliette.gameObject.activeSelf) StartCoroutine(cs_jouliette.MergeIntoPlayer());

        yield return new WaitWhile(AnyCSHeroesAreActive);

        GameControl.control.SetCharacterState(characterControl.CharacterState.Normal);
        
    }

    bool AnyCSHeroesAreActive()
    {
        if (cs_jethro != null && cs_jethro.gameObject.activeSelf)
            return true;
        if (cs_cole != null && cs_cole.gameObject.activeSelf)
            return true;
        if (cs_eleanor != null && cs_eleanor.gameObject.activeSelf)
            return true;
        if (cs_jouliette != null && cs_jouliette.gameObject.activeSelf)
            return true;

        return false;
    }
}
