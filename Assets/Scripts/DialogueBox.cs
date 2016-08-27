using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// the box that will appear when talking with a character
public class DialogueBox : MonoBehaviour {

	// object for text
	GameObject titleTextGO;
	GameObject spokenTextGO;

	// string for actual text
	TextMesh titleText;
	TextMesh spokenText;

	// position in list of text
	int listPosition;

	// npc dialogue to get the list of text to write
	NPCDialogue dialogueNode;

	// npc object in order to turn
	// canTalk back to true
	public NPCObject npc;

	// position the text in relation to the camera
	Vector3 titlePosition;
	Vector3 textPosition;

	// camera obj
	GameObject mainCamera;

	characterControl hero;

	// character image
	SpriteRenderer sr;
	SpriteRenderer portraitSr;

	// Use this for initialization
	void Start () {


		//Create the text objects
		titleTextGO = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		titleTextGO.GetComponent<TextMesh> ().fontSize = 500;
		titleTextGO.GetComponent<TextMesh> ().alignment = TextAlignment.Left;

		spokenTextGO = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		spokenTextGO.GetComponent<TextMesh>().fontSize = 500;
		spokenTextGO.GetComponent<TextMesh>().alignment = TextAlignment.Left;

		// set text
		titleText = titleTextGO.GetComponent<TextMesh>();
		spokenText = spokenTextGO.GetComponent<TextMesh>();

		// set the dialogue node to the one inside the npc inspector
		dialogueNode = npc.GetComponent<NPCDialogue>();

		// set npc
		//npc = GetComponent<NPCObject>();

		// set position to start of list
		listPosition = 0;

		// set to main camera
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");


		hero = GameObject.FindObjectOfType<characterControl> ();
		hero.canMove = false;


		sr = GetComponent<SpriteRenderer> ();
		portraitSr = gameObject.transform.Find("Portrait").GetComponent<SpriteRenderer> ();


		// display at correct location - initially
		textPosition = new Vector3(mainCamera.transform.position.x - 600, mainCamera.transform.position.y - 350, -100);
		spokenTextGO.transform.position = textPosition;

		titlePosition = new Vector3 (textPosition.x - 200, textPosition.y + 100, -100);
		titleTextGO.transform.position = titlePosition;

		transform.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y - 325, -900);
	}
	
	// Update is called once per frame
	void Update () {
		if (enabled) {
			// display text
			titleText.text = dialogueNode.title;
			spokenText.text = dialogueNode.dialogueList [listPosition];

			// display at correct location
			textPosition = new Vector3(mainCamera.transform.position.x - 600, mainCamera.transform.position.y - 350, -100);
			spokenTextGO.transform.position = textPosition;

			titlePosition = new Vector3 (textPosition.x - 200, textPosition.y + 100, -100);
			titleTextGO.transform.position = titlePosition;

			transform.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y - 325, -900);


			//sr.transform.position = new Vector3 (textPosition.x - 200, textPosition.y, -100);


			titleText.GetComponent<Renderer> ().sortingOrder = 900;
			spokenText.GetComponent<Renderer> ().sortingOrder = 900;
			sr.sortingOrder = 899;
			portraitSr.sortingOrder = 899;

			if (Input.GetKeyUp (KeyCode.Space)) {
				// continue list if space is pressed
				if (listPosition < dialogueNode.dialogueList.Count - 1) {
					listPosition++;
				} 
			// disable text and box when list is done
			else {
					npc.canTalk = true;
					npc = null;
					spokenText.GetComponent<Renderer> ().enabled = false;
					titleText.GetComponent<Renderer> ().enabled = false;
					sr.enabled = false;
					portraitSr.enabled = false;
					hero.canMove = true;
					enabled = false;

				}
			}
		}
	
	}

	// turn box on when player wants to talk
	public void EnableBox(){
		listPosition = 0;
		spokenText.GetComponent<Renderer> ().enabled = true;
		titleText.GetComponent<Renderer> ().enabled = true;
		sr.enabled = true;
		portraitSr.enabled = true;
		enabled = true;
		hero.canMove = false;

		// set the dialogue node to the one inside the npc inspector
		dialogueNode = npc.GetComponent<NPCDialogue>();

	}
}
