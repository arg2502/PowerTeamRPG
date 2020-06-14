using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneManager : MonoBehaviour {

    public List<QuestCutscene> questCutscenes;

    Cutscene currentCutscene;

    private void OnEnable()
    {
        GameControl.control.cutsceneManager = this;        
    }

    public void PlayCutscene(string subquestID)
    {
        // find the cutscene that matches with the quest that's passed in            
        var qc = questCutscenes.Find(q => string.Equals(q.subquestID, subquestID));
        currentCutscene = qc.cutscene;
        currentCutscene.Play();     
    }

    public void PlayCutscene(QuestCutscene qc)
    {
        currentCutscene = qc.cutscene;
        currentCutscene.Play();
    }

    public void Dialogue()
    {
        currentCutscene.Pause();
    }
    
}
[System.Serializable]
public class QuestCutscene
{
    public string subquestID;
    public Cutscene cutscene;

}