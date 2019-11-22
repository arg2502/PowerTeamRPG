using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBeggar : StationaryNPCControl {

    public List<BeggarTalk> knownResponses; // responses where the player needs to be away of certain criteria to understand
    public List<BeggarTalk> unknownResponses; // when the player doesn't know certain criteria, or when it doesn't matter    
    public TextAsset defaultResponse;

    public void ShowBeggarMenu()
    {
        GameControl.UIManager.PushBeggarMenu(this);
    }

    public void SetGold(int gold)
    {
        TextAsset beggarResponse = null;
        if (GameControl.control.knownBeggarDialogues.Contains(gold))
        {
            var beggarTalk = knownResponses.Find(r => r.gold == gold);
            if (beggarTalk != null)
                beggarResponse = beggarTalk.dialogue;
        }
        else
        {
            var beggarTalk = unknownResponses.Find(r => r.gold == gold);
            if (beggarTalk != null)
                beggarResponse = beggarTalk.dialogue;
        }

        if (beggarResponse == null)
            beggarResponse = defaultResponse;

        GameControl.UIManager.PopMenu();
        GetComponentInChildren<NPCDialogue>().StartDialogue(beggarResponse);
    }

    
}

[System.Serializable]
public class BeggarTalk
{
    public int gold;
    public TextAsset dialogue;
}
