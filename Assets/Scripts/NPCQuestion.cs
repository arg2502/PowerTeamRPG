using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCQuestion : NPCDialogue {

    // the character's actual response to the answer
    // private because it should not be set in inspector
    //List<string> response;
    public List<ListOfStrings> possibleResponses;

    void Start()
    {
        dMenu = GetComponent<DialogueMenu>();
        if (dMenu)
        {
            dMenu.contentArray = answerList;
        }
    }
	
}
