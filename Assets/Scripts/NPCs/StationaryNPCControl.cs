using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryNPCControl : OverworldObject {

	Animator anim;

	public enum State{
		idle, //not talking, NPC faces default direction
		talking //NPC faces player
	}

	public State currentState = State.idle; //set state to idle by default

	public enum Direction {
		up,
		left,
		down,
		right
	}

	public Direction defaultDirection = Direction.down;

	private BoxCollider2D boxCollider; // Variable to reference our collider

	// Use this for initialization
	void Start () {

		//Find the NPC's animator
		anim = GetComponent<Animator>();
		
		//Find the NPC's collider
		boxCollider = GetComponent<BoxCollider2D> ();

		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FaceCharacter(Vector2 directionToFace)
	{
		currentState = State.talking; // set NPC to talking
		canMove = false; // Stop moving, to set to idle sprites
		//lastMovement = directionToFace; // set direction to face based on opposite player's dir
		print ("Face " + directionToFace.ToString());
		// set the blend tree animator to face the proper direction
		anim.SetBool ("canMove", canMove);
		anim.SetFloat("lastHSpeed", directionToFace.x);
		anim.SetFloat("lastVSpeed", directionToFace.y);
	}
	public void BackToNormal()
	{
		canMove = true;
		currentState = State.idle;
		anim.SetBool ("canMove", canMove);
	}
}
