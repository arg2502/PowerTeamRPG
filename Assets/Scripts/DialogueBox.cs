using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// the box that will appear when talking with a character
public class DialogueBox : MonoBehaviour {

	// object for text
	GameObject spokenTextGO;

	// string for actual text
	TextMesh spokenText;

	// position in list of text
	int listPosition;

	// npc dialogue to get the list of text to write
	NPCDialogue dialogueNode;

	// npc object in order to turn
	// canTalk back to true
	NPCObject npc;

	// position the text in relation to the camera
	Vector2 textPosition;

	GameObject mainCamera;

	// Use this for initialization
	void Start () {
		//Create the text object
		spokenTextGO = (GameObject)Instantiate(Resources.Load("Prefabs/TextPrefab"));
		spokenTextGO.GetComponent<TextMesh>().fontSize = 250;
		spokenTextGO.GetComponent<TextMesh>().alignment = TextAlignment.Left;

		// set text
		spokenText = spokenTextGO.GetComponent<TextMesh>();

		// set the dialogue node to the one inside the npc inspector
		dialogueNode = GetComponent<NPCDialogue>();

		// set npc
		npc = GetComponent<NPCObject>();

		// set position to start of list
		listPosition = 0;

		// set to main camera
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

	}
	
	// Update is called once per frame
	void Update () {
		if (enabled) {
			// display text
			spokenText.text = dialogueNode.dialogueList [listPosition];

			// display at correct location
			textPosition = new Vector2(mainCamera.transform.position.x - Screen.width/4, mainCamera.transform.position.y - Screen.height/2);
			spokenTextGO.transform.position = textPosition;

			if (Input.GetKeyUp (KeyCode.Space)) {
				// continue list if space is pressed
				if (listPosition < dialogueNode.dialogueList.Count - 1) {
					listPosition++;
				} 
			// disable text and box when list is done
			else {
					npc.canTalk = true;
					spokenText.GetComponent<Renderer> ().enabled = false;
					enabled = false;
				}
			}
		}
	
	}

	// turn box on when player wants to talk
	public void EnableBox(){
		listPosition = 0;
		enabled = true;
		spokenText.GetComponent<Renderer> ().enabled = true;

	}
}
