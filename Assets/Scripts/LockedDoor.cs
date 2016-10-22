using UnityEngine;
using System.Collections;

public class LockedDoor : NPCObject {

	protected override void SetDialogue()
    {
        // if the player does not have any keys, set node to dialogue
        if(GameControl.control.totalKeys <= 0)
        {
            npcDialogue = dialogueList[0];
        }
        // else, set to door question
        else
        {
            npcDialogue = dialogueList[1];
        }
    }
}
