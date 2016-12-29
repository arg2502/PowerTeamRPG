using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueMenu : Menu {

    // list to pass into content array
    //public List<string> questionList;
    public NPCQuestion npc;

    // reference to dialogueBox
    public DialogueBox dBox;

	// Use this for initialization
	void Start () {      

	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        PressButton(KeyCode.Space);
	}

    public override void ButtonAction(string label)
    {
        if (enabled)
        {
            for (int i = 0; i < npc.answerList.Count; i++)
            {
                // if the button is hover when this is called, it's the button that was just pressed
                if (buttonArray[i].GetComponent<MyButton>().state == MyButton.MyButtonTextureState.hover)
                {
                    // set the dialogue box text to corresponding position in response list
                    npc.ResponseAction(i);
                    dBox.isAsking = false;
                    dBox.isDialogue = false;


					// stop response if shop keeper
					if (npc.GetComponent<ShopKeeperQuestion> () != null) {
						if (!npc.GetComponent<ShopKeeperQuestion> ().canBuy) {
							if (npc.possibleResponses.Count > 0) {
								npc.responseList = npc.possibleResponses [0];
								dBox.isResponse = true;
								dBox.outerListPosition = 0;
								dBox.innerListPosition = 0;
								StartCoroutine (dBox.ScrollText (npc.responseList.dialogue [dBox.innerListPosition]));
							}
							DisableQuestionMenu ();
						}
					}

					// if there are any responses (and not going to another scene), set it up here
					else {
						if (npc.possibleResponses.Count > 0) {
							npc.responseList = npc.possibleResponses [i];
							dBox.isResponse = true;
							dBox.outerListPosition = 0;
							dBox.innerListPosition = 0;
							StartCoroutine (dBox.ScrollText (npc.responseList.dialogue [dBox.innerListPosition]));
						}
						DisableQuestionMenu ();
					}
                }
            }
        }
    }

    public void EnableQuestionMenu()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        npc = GetComponent<NPCQuestion>();
        numOfRow = npc.answerList.Count;
        buttonArray = new GameObject[numOfRow];
        selectedIndex = 0;
        scrollIndex = 0;
        dBox = GameObject.FindObjectOfType<DialogueBox>();

        for (int i = 0; i < numOfRow; i++)
        {
            // create a button
            buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y - (375 - b.height) + (i * -(b.height + b.height / 2)));

            // assign text
            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
            
            // set to show in front of dialogue box
            //b.GetComponent<Renderer>().sortingOrder = 900;
            //b.textObject.GetComponent<Renderer> ().sortingOrder = 900;
        }
            // set selected button
            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

    }
    public void DisableQuestionMenu()
    {
        for (int i = 0; i < numOfRow; i++ )
        {
            Destroy(buttonArray[i].GetComponent<MyButton>().textObject);
            Destroy(buttonArray[i]);
        }
        enabled = false;
    }
    
    
}
