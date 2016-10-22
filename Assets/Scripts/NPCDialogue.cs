using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour {

    // title of the person speaking
    public List<string> title;
	public float typingSpeed;

    public List<ListOfStrings> dialogueList; // list of phrases a character will say when you talk to them
    public string responseTitle; // question variables
    public DialogueMenu dMenu; // needs to be here for DialogueBox to access
    public List<string> answerList; // answers for the player    
    public ListOfStrings responseList; // the final response of the npc - this is set inside dialogue menu  
    
}

[System.Serializable]
public class ListOfStrings
{
    // actual dialogue spoken by character
    public List<string> dialogue;

    // pictures for every line, in case they change expression in the middle
    public List<Sprite> charImages;

}
