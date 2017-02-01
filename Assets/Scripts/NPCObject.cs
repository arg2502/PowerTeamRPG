using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCObject : OverworldObject {

	// variables to keep track of the player's
	// position relative to npc
	protected Transform player;
	protected float distFromPlayer;

	// enable dialogue box when player interacts with npc
	public DialogueBox dBox;
	protected GameObject dBoxGO;

    public NPCDialogue npcDialogue;
	protected List<NPCDialogue> dialogueList;
    int numOfTimesTalked;
    protected float distToTalk;

	// variable to determine whether the player
	// can talk to the npc
	// mainly so you can't start a dialogue if
	// one already exists
	public bool canTalk;
	//public Sprite charImage;
	protected void Start(){
        base.Start();
		player = GameObject.FindObjectOfType<characterControl>().transform;
		canTalk = true;
		//npcDialogue = GetComponent<NPCDialogue> ();
        dialogueList = new List<NPCDialogue>();
        foreach(NPCDialogue npcD in GetComponents<NPCDialogue>())
        {
            dialogueList.Add(npcD);
        }
        numOfTimesTalked = 0;
        npcDialogue = dialogueList[numOfTimesTalked];
        distToTalk = 120.0f;
        
	}

	// begin conversation when player collides and presses space
	protected void Update(){
		if (distFromPlayer < distToTalk 
			&& Input.GetKeyUp(GameControl.control.selectKey) 
			&& canTalk 
			&& player.gameObject.GetComponent<characterControl>().canMove
			&& gameObject.GetComponent<SpriteRenderer>().enabled) {
            
            // set which dialogue is spoken
            SetDialogue();
            
            canTalk = false;
			// if first time, set equal to existing box
			if (dBox == null && dBoxGO == null) {				
				if (GameObject.FindObjectOfType<DialogueBox> () == null) {
					dBoxGO = (GameObject)Instantiate (Resources.Load ("Prefabs/DialogueBoxPrefab"));
					dBox = dBoxGO.GetComponent<DialogueBox> ();
					dBox.npc = this;
				} else {						
					dBox = GameObject.FindObjectOfType<DialogueBox> ();
					dBox.npc = this;
					dBox.EnableBox ();
				}
			} else {
				dBox.npc = this;
				dBox.EnableBox ();
			}
			
		}
	}

	protected void FixedUpdate(){
		distFromPlayer = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
			+ ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));
	}
    protected virtual void SetDialogue()
    {
        // set which dialogue is spoken based on num of times talked
        // if out of range, say last option in list
        if (numOfTimesTalked >= dialogueList.Count)
        {
            npcDialogue = dialogueList[dialogueList.Count - 1];
        }
        else
        {
            npcDialogue = dialogueList[numOfTimesTalked];
            numOfTimesTalked++;
        }
    }
}
