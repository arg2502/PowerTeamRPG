using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCDialogue : MonoBehaviour {

	// title of the person speaking
	public string title;
	public float typingSpeed;

	// list of phrases a character will say when you talk to them
	public List<string> dialogueList;

	// could have multiple lists for conditional conversations
	// based on the progression of the story or player
	// these could be in child classes


}
