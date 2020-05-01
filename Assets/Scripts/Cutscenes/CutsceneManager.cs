using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {

    public List<QuestCutscene> questCutscenes;

    private void OnEnable()
    {
        GameControl.control.cutsceneManager = this;        
    }

    public void PlayCutscene(string subquestID)
    {
        // find the cutscene that matches with the quest that's passed in            
        var qc = questCutscenes.Find(q => string.Equals(q.subquestID, subquestID));
        qc.cutscene.Play();     
    }

    
}
[System.Serializable]
public class QuestCutscene
{
    public string subquestID;
    public Cutscene cutscene;
    public TriggerType triggerType;

    public enum TriggerType { ROOM_ENTER, ON_TRIGGER }
}