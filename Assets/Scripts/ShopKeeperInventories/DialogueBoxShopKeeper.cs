using UnityEngine;
using System.Collections;

public class DialogueBoxShopKeeper : MonoBehaviour {

	// object for text
	GameObject titleTextGO;
	GameObject spokenTextGO;

	// string for actual text
	TextMesh titleText;
	public TextMesh spokenText;

	// position in list of text
	public int listPosition;

	// position the text in relation to the camera
	Vector3 titlePosition;
	Vector3 textPosition;

	// camera obj
	GameObject mainCamera;
	Vector2 dialogueOffset;
	Vector2 titleOffset;

	// character image
	public SpriteRenderer sr;
	SpriteRenderer portraitSr;

	// scroll typing lines
	bool isTyping;
	bool cancelTyping;
	int desiredLength = 30;

	// shop keeper
	ShopKeeper shopKeeper;

	// keep track of list position
	int prevPosition;

	// check whether a question is being asked
	//public bool isAsking;
	//public bool isDialogue;
	//public bool isResponse;

	// coroutine currently running
	IEnumerator currentRoutine;

	// Use this for initialization
	void Start () {

		shopKeeper = GameObject.FindObjectOfType<ShopKeeper> ().GetComponent<ShopKeeper> ();

		//Create the text objects
		titleTextGO = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		titleTextGO.GetComponent<TextMesh> ().fontSize = 100;
		titleTextGO.GetComponent<TextMesh> ().alignment = TextAlignment.Left;

		spokenTextGO = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
		spokenTextGO.GetComponent<TextMesh>().fontSize = 100;
		spokenTextGO.GetComponent<TextMesh>().alignment = TextAlignment.Left;

		// set text
		titleText = titleTextGO.GetComponent<TextMesh>();
		spokenText = spokenTextGO.GetComponent<TextMesh>();

		// set position to start of list
		listPosition = 0;

		// set to main camera
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

		sr = GetComponent<SpriteRenderer> ();
		portraitSr = gameObject.transform.Find("Portrait").GetComponent<SpriteRenderer> ();
		portraitSr.sprite = shopKeeper.portraitImages [listPosition];

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

		prevPosition = listPosition;
		currentRoutine = ScrollText (shopKeeper.flavorText [listPosition]);
		StartCoroutine (currentRoutine);
	}

	// Update is called once per frame
	void Update () {

		// only update text when position changes
		if (prevPosition != listPosition) {
			// stop coroutine that's currently running, to prevent mumbled up text
			StopCoroutine (currentRoutine);

			// reset text
			spokenText.text = "";

			// set current coroutine to the appropriate coroutine
			currentRoutine = ScrollText (shopKeeper.flavorText [listPosition]);

			// start new flavor text
			StartCoroutine (currentRoutine);
			prevPosition = listPosition;
		}

	}

	//Coroutine to type out lines
	public IEnumerator ScrollText(string line)
	{
		int letter = 0;
		spokenText.text = "";
		int lineLength = spokenText.text.Length;
		isTyping = true;
		cancelTyping = false;
		while (isTyping && !cancelTyping && letter < line.Length) {
			if (lineLength > desiredLength && line[letter-1] == ' ') {
				spokenText.text += "\n";
				lineLength = 0;
			}
			spokenText.text += line [letter];
			letter++;
			lineLength++;
			//spokenText.text = FormatText (spokenText.text);
			yield return new WaitForSeconds (shopKeeper.typingSpeed);
		}
		isTyping = false;
		cancelTyping = false;

	}
	/*public void WriteText(List<ListOfStrings> text)
	{
		// set portrait pic
		if (text[outerListPosition].charImages.Count > 0)
		{
			portraitSr.sprite = text[outerListPosition].charImages[innerListPosition];
		}

		// change text position
		// if the portrait exists, make room for it
		if (portraitSr.sprite) { dialogueOffset = new Vector2(-450, -250); }
		else { dialogueOffset = new Vector2(-600, -250); }

		titleText.text = npc.npcDialogue.title[outerListPosition];

		// display at correct locationif (titleText.text == "")
		if(titleText.text == "")
		{
			textPosition = new Vector3(mainCamera.transform.position.x + (dialogueOffset.x + titleOffset.x), mainCamera.transform.position.y + (dialogueOffset.y + titleOffset.y), -100);
		}
		else
		{
			textPosition = new Vector3(mainCamera.transform.position.x + dialogueOffset.x, mainCamera.transform.position.y + dialogueOffset.y, -100);
		}
		spokenTextGO.transform.position = textPosition;

		titlePosition = new Vector3(textPosition.x + titleOffset.x, textPosition.y + titleOffset.y, -100);
		titleTextGO.transform.position = titlePosition;

		transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 325, -900);

		// turn on renderer
		// if (sr.enabled == false) { sr.enabled = true; }

		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (!isTyping)
			{
				// continue list if space is pressed
				// continue inside list if there is more for that person to say
				if (innerListPosition < text[outerListPosition].dialogue.Count - 1)
				{
					innerListPosition++;
					StartCoroutine(ScrollText(text[outerListPosition].dialogue[innerListPosition]));
				}

				// move on to next person if there are more
				else if (outerListPosition < text.Count - 1 && !isAsking)
				{
					outerListPosition++; // increase outer list to next person
					innerListPosition = 0; // reset dialogue list to beginning
					portraitSr.sprite = text[outerListPosition].charImages[innerListPosition];// change picture
					StartCoroutine(ScrollText(text[outerListPosition].dialogue[innerListPosition])); // type text
				}

				// disable text and box when list is done
				else if(!isAsking || isResponse)
				{
					DisableBox();

				}
			}
			else if (isTyping && !cancelTyping)
			{
				cancelTyping = true;
				spokenText.text = FormatText(text[outerListPosition].dialogue[innerListPosition]);
			}
		}
		// display text
		//spokenText.text = text [innerListPosition];



		//sr.transform.position = new Vector3 (textPosition.x - 200, textPosition.y, -100);


		titleText.GetComponent<Renderer>().sortingOrder = 9900;
		spokenText.GetComponent<Renderer>().sortingOrder = 9900;
		sr.sortingOrder = 9899;
		portraitSr.sortingOrder = 9899;
	}
	public void WriteResponseText(ListOfStrings response)
	{
		// set portrait pic
		if (response.charImages.Count > 0)
		{
			portraitSr.sprite = response.charImages[innerListPosition];
		}

		// change text position
		// if the portrait exists, make room for it
		if (portraitSr.sprite) { dialogueOffset = new Vector2(-450, -250); }
		else { dialogueOffset = new Vector2(-600, -250); }

		titleText.text = npc.npcDialogue.responseTitle;

		// display at correct location
		if (titleText.text == "")
		{
			textPosition = new Vector3(mainCamera.transform.position.x + (dialogueOffset.x + titleOffset.x), mainCamera.transform.position.y + (dialogueOffset.y + titleOffset.y), -100);
		}
		else
		{
			textPosition = new Vector3(mainCamera.transform.position.x + dialogueOffset.x, mainCamera.transform.position.y + dialogueOffset.y, -100);
		}
		spokenTextGO.transform.position = textPosition;

		titlePosition = new Vector3(textPosition.x + titleOffset.x, textPosition.y + titleOffset.y, -100);
		titleTextGO.transform.position = titlePosition;

		transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 325, -900);

		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (!isTyping)
			{
				// continue list if space is pressed
				// continue inside list if there is more for that person to say
				if (innerListPosition < response.dialogue.Count - 1)
				{
					innerListPosition++;
					StartCoroutine(ScrollText(response.dialogue[innerListPosition]));
				}
				// disable text and box when list is done
				else if (!isAsking || isResponse)
				{
					DisableBox();

				}
			}
			else if (isTyping && !cancelTyping)
			{
				cancelTyping = true;
				spokenText.text = FormatText(response.dialogue[innerListPosition]);
			}
		}
		titleText.GetComponent<Renderer>().sortingOrder = 9900;
		spokenText.GetComponent<Renderer>().sortingOrder = 9900;
		sr.sortingOrder = 9899;
		portraitSr.sortingOrder = 9899;
	}
	public string FormatText(string str)
	{
		string formattedString = null;
		string[] wordArray = str.Split(' ');
		int lineLength = 0;
		foreach (string s in wordArray)
		{
			//if the current line plus the length of the next word and SPACE is greater than the desired line length
			if (lineLength > desiredLength)
			{
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
	}*/

}
