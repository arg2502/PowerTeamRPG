using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCObject : OverworldObject {

	// variables to keep track of the player's
	// position relative to npc
	Transform player;
	float distFromPlayer;

	// enable dialogue box when player interacts with npc
	DialogueBox dBox;

	// variable to determine whether the player
	// can talk to the npc
	// mainly so you can't start a dialogue if
	// one already exists
	public bool canTalk;

	void Start(){
		player = GameObject.FindObjectOfType<characterControl>().transform;
		canTalk = true;
	}

	// begin conversation when player collides and presses space
	void Update(){
		if (distFromPlayer < 150.0f && Input.GetKeyUp(KeyCode.Space) && canTalk) {
			canTalk = false;
			// if dialogue box doesn't exist, add one
			if (dBox == null) {
				gameObject.AddComponent<DialogueBox> ();
				dBox = GetComponent<DialogueBox> ();
			} else {
				dBox.EnableBox ();
			}
		}
	}

	void FixedUpdate(){
		distFromPlayer = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
			+ ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));
	}
}
