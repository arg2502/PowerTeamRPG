using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorQuestion : NPCQuestion {

	// Use this for initialization
	void Start () {
        // ask the player if they would like to unlock the door
        ListOfStrings doorQuestion = new ListOfStrings();
        doorQuestion.dialogue = new List<string>();
        doorQuestion.charImages = new List<Sprite>();
        doorQuestion.dialogue.Add("Would you like to use a key and unlock the door?");
        dialogueList.Add(doorQuestion);
        
        // set the possible responses
        answerList.Add("Yes");
        answerList.Add("No");

        title = new List<string>();
        title.Add("");
        base.Start();	
	}
	
    public override void ResponseAction(int responseIndex)
    {
        // if the response is the "Yes"(0), unlock the door
        if(responseIndex == 0)
        {
            gameObject.SetActive(false); // disable door
            GameControl.control.totalKeys--; // remove key
        }
    }
}
