using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour {
    
    // each item in the list is a dialogue exchange you have with an NPC or object
    // if they only say one thing each time you talk, the list will have a count of 1.
    public List<TextAsset> dialogueList;

    int numOfTimesTalked;

    public string npcName;
    public Sprite neutralSpr;
    public Sprite happySpr;

    Dialogue dialogue;
    bool isTalking = false;
    public bool IsTalking { get { return isTalking; } }

    characterControl.CharacterState prevState;

    // Use this for initialization
    void Start () {
        dialogue = GetComponent<Dialogue>();

        if (dialogue == null)
            dialogue = gameObject.AddComponent<Dialogue>();
	}

    /// <summary>
    /// Send the appropriate conversation over to the Dialogue class to handle
    /// </summary>
    public void StartDialogue()
    {
        dialogue.StartDialogue(dialogueList[numOfTimesTalked]);
        isTalking = true;
        
        prevState = GameControl.control.currentCharacterState;
        GameControl.control.SetCharacterState(characterControl.CharacterState.Talking);
    }

    public void EndDialogue()
    {
        if (numOfTimesTalked < dialogueList.Count - 1)
            numOfTimesTalked++;
        isTalking = false;

        GameControl.control.SetCharacterState(prevState);

        // At the end of the dialogue, set the NPC's walking method back to normal
        // ...might not be the best place for this, as the NPCPathwalkControl function that's called
        // at the start of the conversation is called in characterControl, but that's because that 
        // function needs a characterControl variable
        // plus this location kinda makes sense...
        if (GetComponent<NPCPathwalkControl>())
            GetComponent<NPCPathwalkControl>().BackToNormal();
    }
}
