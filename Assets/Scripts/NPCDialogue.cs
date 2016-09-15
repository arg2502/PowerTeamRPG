using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour {

    // title of the person speaking
    public List<string> title;
	public float typingSpeed;

	// list of phrases a character will say when you talk to them
	public List<ListOfStrings> dialogueList;

	// could have multiple lists for conditional conversations
	// based on the progression of the story or player
	// these could be in child classes


}
[System.Serializable]
public class ListOfStrings
{
    // actual dialogue spoken by character
    public List<string> dialogue;

    // pictures for every line, in case they change expression in the middle
    public List<Sprite> charImages;
}
