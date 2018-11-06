using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathwalkControl : OverworldObject {

	//set to public temporarily
	public Waypoint[] waypoints; //stores all of the possible waypoints

	public Waypoint nextWaypoint; //the next point to navigate to
	public Waypoint currentWaypoint; //the current target
	public Waypoint prevWaypoint; //the waypoint we just came from

	//Create an enumeration for the state of NPC's walking
	//this will modify the NPC's behavior
	public enum State {
		walking, //moving toward the currentWaypoint
		avoiding //avoiding an obstacle in the way of the waypoint
	}
	//set the state to walking by default
	public State walkingState = State.walking;

	//Create enum for the 4 directions an NPC can travel in
	//This acts like the input axis of the player character
	public enum Direction {
		up,
		left,
		down,
		right
	}
	//the desiredDirection variable tells us where the NPC wants to go
	public Direction desiredDirection = Direction.up;

	public float moveSpeed = 4.0f; //the walk speed of the NPC

	private BoxCollider2D boxCollider; // Variable to reference our collider
	private RaycastHit2D hit; //checks for collisions

	// Use this for initialization
	void Start () {
		//Find the NPC's collider
		boxCollider = GetComponent<BoxCollider2D> ();

		//find all waypoints in the scene
		waypoints = FindObjectsOfType<Waypoint> ();

		//find the closest waypoint an start navigating towards it
		float shortestDist = Mathf.Infinity; //stores shortest distance
		float dist = 0.0f; // distance between npc and current waypoint
		for (int i = 0; i < waypoints.Length; i++) {
			//calculate distance from npc to waypoint i
			dist = Vector2.Distance(transform.position, waypoints[i].transform.position);
			//Compare it to previous shortest distance
			//if this is shorter, make dist the new shortest distance
			if(dist < shortestDist)
			{
				shortestDist = dist;
				//set the current waypoint to waypoint i
				currentWaypoint = waypoints[i];
			}
		}
	}

	void Move (float move_x, float move_y){
		//create the vector of the desired movement
		Vector2 direction = new Vector2 (move_x, move_y);
		Vector2 offsetMultiple = direction.normalized;//this line is to solve a problem where the raycast hits npc's own collider
		direction.Normalize ();
		direction *= moveSpeed * Time.deltaTime;
		
		//get the position of the boxcollider, adjusted for any offsets
		Vector2 adjustedPosition = new Vector2((transform.position.x + boxCollider.offset.x),
		                                       (transform.position.y + boxCollider.offset.y));
		
		//cast a box to see if NPC will collide vertically
		// vertical input
		if (move_y != 0) {
			hit = Physics2D.BoxCast (adjustedPosition + (offsetMultiple * boxCollider.bounds.size.magnitude), boxCollider.size, 0,
			                         new Vector2 (0, direction.y), Mathf.Abs (direction.y));
			if (hit.collider == null) { // if the collider is null, no collision
				transform.Translate (0, direction.y, 0); }// move in the y direction
			else {print("Colliding with: " + hit.collider.name);}
			
			//stuff for the animator
			//isMoving = true;
			//lastMovement = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
		}
		
		//cast a box to see if NPC will collide horizontally
		// horizontal input
		if (move_x != 0) {
			hit = Physics2D.BoxCast (adjustedPosition + (offsetMultiple * boxCollider.bounds.size.magnitude), boxCollider.size, 0,
			                         new Vector2 (direction.x, 0), Mathf.Abs (direction.x));
			if (hit.collider == null) { // if the collider is null, no collision
				transform.Translate (direction.x, 0, 0); }// move in the x direction
			
			//stuff for the animator
			//isMoving = true;
			//lastMovement = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (walkingState == State.walking) {
			//move towards the current waypoint until we enter the trigger
			if(desiredDirection == Direction.up){
				Move(0.0f, 1.0f);
			}
			else if(desiredDirection == Direction.left){
				Move(1.0f, 0.0f);
			}
			else if(desiredDirection == Direction.down){
				Move(0.0f, -1.0f);
			}
			else if(desiredDirection == Direction.right){
				Move(1.0f, 0.0f);
			}


			//when entering the trigger, start a short timer
			//when the timer runs out, select the next waypoint
		}
	}

	//THis method checks npc's collision with a trigger, assumedly a waypoint
	void OnTriggerStay2D(Collider2D other){
		//Check if the trigger is the current target waypoint
		if (other == currentWaypoint.box) 
		{
			print ("AHHHHHHH! I'm here!");
		}
	}
}
