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
	int innerListPosition;

    // position in list of lists
    int outerListPosition;

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
	Vector2 dialogueOffset;
	Vector2 titleOffset;

	characterControl hero;

	// character image
	SpriteRenderer sr;
	SpriteRenderer portraitSr;

	// scroll typing lines
	bool isTyping;
	bool cancelTyping;
	int desiredLength = 30;
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
		innerListPosition = 0;
        outerListPosition = 0;

		// set to main camera
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");


		hero = GameObject.FindObjectOfType<characterControl> ();
		hero.canMove = false;

		sr = GetComponent<SpriteRenderer> ();
		portraitSr = gameObject.transform.Find("Portrait").GetComponent<SpriteRenderer> ();

        // set correct image
        portraitSr.sprite = dialogueNode.charImages[outerListPosition];

		// display at correct location - initially
        if (portraitSr.sprite) { dialogueOffset = new Vector2(-450, -250); }
        else { dialogueOffset = new Vector2(-600, -250);}
		titleOffset = new Vector2 (0, 70);
		textPosition = new Vector3(mainCamera.transform.position.x + dialogueOffset.x, mainCamera.transform.position.y + dialogueOffset.y, -100);
		spokenTextGO.transform.position = textPosition;

		titlePosition = new Vector3 (textPosition.x + titleOffset.x, textPosition.y + titleOffset.y, -100);
		titleTextGO.transform.position = titlePosition;

		transform.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y - 325, -900);

		isTyping = false;
		cancelTyping = false;

		StartCoroutine (ScrollText (dialogueNode.dialogueList[outerListPosition].dialogue [innerListPosition]));
	}
	
	// Update is called once per frame
	void Update () {
		if (enabled) {
            // set portrait pic
            portraitSr.sprite = dialogueNode.charImages[outerListPosition];
            // change text position
            // if the portrait exists, make room for it
            if (portraitSr.sprite) { dialogueOffset = new Vector2(-450, -250); }
            else { dialogueOffset = new Vector2(-600, -250); }


            titleText.text = dialogueNode.title[outerListPosition];
            // display at correct location
            textPosition = new Vector3(mainCamera.transform.position.x + dialogueOffset.x, mainCamera.transform.position.y + dialogueOffset.y, -100);
            spokenTextGO.transform.position = textPosition;

            titlePosition = new Vector3(textPosition.x + titleOffset.x, textPosition.y + titleOffset.y, -100);
            titleTextGO.transform.position = titlePosition;

            transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 325, -900);

            // turn on renderer
           // if (sr.enabled == false) { sr.enabled = true; }

			if (Input.GetKeyUp (KeyCode.Space)) {
				if (!isTyping) {
					// continue list if space is pressed
                    // continue inside list if there is more for that person to say
					if (innerListPosition < dialogueNode.dialogueList[outerListPosition].dialogue.Count - 1) {
						innerListPosition++;
						StartCoroutine (ScrollText (dialogueNode.dialogueList [outerListPosition].dialogue[innerListPosition]));
					} 
                    // move on to next person if there are more
                    else if(outerListPosition < dialogueNode.dialogueList.Count - 1)
                    {
                        outerListPosition++; // increase outer list to next person
                        innerListPosition = 0; // reset dialogue list to beginning
                        portraitSr.sprite = dialogueNode.charImages[outerListPosition]; // change picture
                        StartCoroutine(ScrollText(dialogueNode.dialogueList[outerListPosition].dialogue[innerListPosition])); // type text
                    }
                    // move on to next person if 
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
				} else if (isTyping && !cancelTyping) {
					cancelTyping = true;
					spokenText.text = FormatText (dialogueNode.dialogueList[outerListPosition].dialogue [innerListPosition]);
				}
			}
			// display text
			//spokenText.text = dialogueNode.dialogueList [innerListPosition];

			

			//sr.transform.position = new Vector3 (textPosition.x - 200, textPosition.y, -100);


			titleText.GetComponent<Renderer> ().sortingOrder = 900;
			spokenText.GetComponent<Renderer> ().sortingOrder = 900;
			sr.sortingOrder = 899;
			portraitSr.sortingOrder = 899;


		}
	
	}
	string FormatText(string str)
	{
		string formattedString = null;
		string[] wordArray = str.Split(' ');
		int lineLength = 0;
		foreach (string s in wordArray)
		{
			//if the current line plus the length of the next word and SPACE is greater than the desired line length
			if (lineLength > desiredLength)
			{
				print (1 + lineLength);
				//go to new line
				formattedString += s + "\n";
				//starting a new line
				//lineLength = s.Length;
				lineLength = 0;
			}
			else
			{
				// first in array, no space
				//if (lineLength == 0) {
				//	formattedString += s + "";
				//} else {
					formattedString += s + " ";
				//}
				lineLength += s.Length + 1;
				//lineLength++;
			}
		}
		return formattedString;
	}
	//Coroutine to type out lines
	IEnumerator ScrollText(string line)
	{
		int letter = 0;
		spokenText.text = "";
		int lineLength = spokenText.text.Length;
		isTyping = true;
		cancelTyping = false;
		while (isTyping && !cancelTyping && letter < line.Length) {
			if (lineLength > desiredLength && line[letter-1] == ' ') {
				print ("cur char: " + line[letter] + "\nnext char: " + line [letter + 1]);
				spokenText.text += "\n";
				lineLength = 0;
			}
			spokenText.text += line [letter];
			letter++;
			lineLength++;
			//spokenText.text = FormatText (spokenText.text);
			yield return new WaitForSeconds (dialogueNode.typingSpeed);
		}
		isTyping = false;
		cancelTyping = false;
	}

	// turn box on when player wants to talk
	public void EnableBox(){
        // set the dialogue node to the one inside the npc inspector
        dialogueNode = npc.GetComponent<NPCDialogue>();

        // reset list positions
        outerListPosition = 0;
        innerListPosition = 0;

        // display at correct location
        // if the portrait exists, make room for it
        portraitSr = gameObject.transform.Find("Portrait").GetComponent<SpriteRenderer>();
        portraitSr.sprite = dialogueNode.charImages[outerListPosition]; // draw correct portrait
        if (portraitSr.sprite) { dialogueOffset = new Vector2(-450, -250); }
        else { dialogueOffset = new Vector2(-600, -250); }


        titleText.text = dialogueNode.title[outerListPosition];
        textPosition = new Vector3(mainCamera.transform.position.x + dialogueOffset.x, mainCamera.transform.position.y + dialogueOffset.y, -100);
        spokenTextGO.transform.position = textPosition;

        titlePosition = new Vector3(textPosition.x + titleOffset.x, textPosition.y + titleOffset.y, -100);
        titleTextGO.transform.position = titlePosition;

        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 325, -900);


        // now that the items are in the right position,
        // enable them
        enabled = true;
        

        spokenText.GetComponent<Renderer>().enabled = true;
        titleText.GetComponent<Renderer>().enabled = true;
        sr.enabled = true;
        if (portraitSr.sprite) { portraitSr.enabled = true; }

		hero.canMove = false;

		// start typing
		StartCoroutine (ScrollText (dialogueNode.dialogueList[outerListPosition].dialogue [innerListPosition]));

	}
}
