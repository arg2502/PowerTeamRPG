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
	public int prevPosition;

	// check whether a question is being asked
	//public bool isAsking;
	//public bool isDialogue;
	//public bool isResponse;

	// coroutine currently running
	IEnumerator currentRoutine;

	// change the text when the player is in the submenus
	public bool isBuying;
	public string currentText;
	public string prevText;


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

		if (isBuying) {
			if (currentText != prevText) {
				// stop coroutine that's currently running, to prevent mumbled up text
				StopCoroutine (currentRoutine);

				// reset text
				spokenText.text = "";

				// set current coroutine to the appropriate coroutine
				currentRoutine = ScrollText (currentText);

				// start new flavor text
				StartCoroutine (currentRoutine);
				prevText = currentText;
			}
		} else {
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

}
