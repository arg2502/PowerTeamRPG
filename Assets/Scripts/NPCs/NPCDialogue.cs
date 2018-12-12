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
        isTalking = true;        
        prevState = GameControl.control.currentCharacterState;
        GameControl.control.SetCharacterState(characterControl.CharacterState.Talking);

        // Start the actual Dialogue last, as this will determine whether we should end the dialogue as well
        // (Putting this line first caused issues where the dialogue would try to start again soon after
        // ending, but then the iterator would not be 0 yet and the conversation would end, BUT THEN 
        // the character was set to Talking and was stuck forever. This way, if there is a false start,
        // the character is set to Talking first, and then the dialogues starts & ends
        // not really a fix, more like hiding the bug)
        dialogue.StartDialogue(dialogueList[numOfTimesTalked]);
    }

    public void EndDialogue(Dialogue.Conversation currentConversation)
    {
        if (numOfTimesTalked < dialogueList.Count - 1)
            numOfTimesTalked++;
        isTalking = false;

        GameControl.control.SetCharacterState(prevState);
        
        // call any functions that need to occur after the dialogue has ended here
        if(!string.IsNullOrEmpty(currentConversation.actionName))
        {
            Invoke(currentConversation.actionName, 0f);
        }
        
        // At the end of the dialogue, set the NPC's walking method back to normal
        // ...might not be the best place for this, as the NPCPathwalkControl function that's called
        // at the start of the conversation is called in characterControl, but that's because that 
        // function needs a characterControl variable
        // plus this location kinda makes sense...
        if (GetComponentInParent<NPCPathwalkControl>())
            GetComponentInParent<NPCPathwalkControl>().BackToNormal();
    }

    void TestFunction()
    {
        print("TEST FUNCTION BABY");
    }
}
