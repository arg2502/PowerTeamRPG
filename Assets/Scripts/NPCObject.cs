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

	protected NPCDialogue npcDialogue;

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
		npcDialogue = GetComponent<NPCDialogue> ();
	}

	// begin conversation when player collides and presses space
	protected void Update(){
		if (distFromPlayer < 150.0f && Input.GetKeyUp(KeyCode.Space) && canTalk && player.gameObject.GetComponent<characterControl>().canMove) {
			canTalk = false;
			// if first time, set equal to existing box
			if (dBox == null && dBoxGO == null) {				
				if (GameObject.FindObjectOfType<DialogueBox> () == null) {
					dBoxGO = (GameObject)Instantiate (Resources.Load ("Prefabs/DialogueBoxPrefab"));
					dBox = dBoxGO.GetComponent<DialogueBox> ();
					dBox.npc = this;
                    //dBox.transform.Find("Portrait").GetComponent<SpriteRenderer>().sprite = charImage;
				} else {						
					dBox = GameObject.FindObjectOfType<DialogueBox> ();
					dBox.npc = this;
                    //dBox.transform.Find("Portrait").GetComponent<SpriteRenderer>().sprite = charImage;
					dBox.EnableBox ();
				}
			} else {
				dBox.npc = this;
                //dBox.transform.Find("Portrait").GetComponent<SpriteRenderer>().sprite = charImage;
				dBox.EnableBox ();
			}
			
		}
	}

	protected void FixedUpdate(){
		distFromPlayer = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
			+ ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));
	}
}
