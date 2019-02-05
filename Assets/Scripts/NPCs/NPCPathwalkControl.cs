using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathwalkControl : MovableOverworldObject {

	Animator anim;
	
	bool isMoving;
	Vector2 lastMovement;

	//set to public temporarily
	public Waypoint[] waypoints; //stores all of the possible waypoints

	//public Waypoint nextWaypoint; //the next point to navigate to
	public Waypoint currentWaypoint; //the current target
	public Waypoint prevWaypoint; //the waypoint we just came from

	//the timer that determines how long the npc should collide with a waypoint before being considered satisfactory
	public float waypointTargetTime = 0.0f;
	float waypointTimer = 0.0f; //counts up until it meets waypointTargetTime

	//Create an enumeration for the state of NPC's walking
	//this will modify the NPC's behavior
	public enum State {
		walking, //moving toward the currentWaypoint
		avoiding, //avoiding an obstacle in the way of the waypoint
        talking // dialogue is occuring -- NPC stops and faces player
	}
	//set the state to walking by default
	public State walkingState = State.walking;
    State prevState = State.walking; // for talking -- store the previous state before talking began

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

	//A timer to make the NPC re-evaluate the direction it's headed
	//makes sure it doesn't deviate too far from it's path while avoiding collisions
	public float redirectTime = 0.0f;
	float redirectTimer = 0.0f;

	public float moveSpeed = 4.0f; //the walk speed of the NPC

	private BoxCollider2D boxCollider; // Variable to reference our collider
	private RaycastHit2D hit; //checks for collisions

	// Use this for initialization
	void Start () {
		//Find the NPC's animator
		anim = GetComponent<Animator>();

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

		//select the direction to start moving toward the waypoint
		ChooseDirection ();

		base.Start();
	}

	void Move (float move_x, float move_y){

		isMoving = true;

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
				transform.Translate (0, direction.y, 0); // move in the y direction
				//anim.SetFloat("vSpeed", direction.y);
				//anim.SetFloat("hSpeed", direction.x);
			}
			else if (hit.collider.tag != "Player") { //if we hit something, set the walking state to avoid, except for the player
				if(walkingState != State.avoiding){
					walkingState = State.avoiding;
					//reset the redirect timer for use as an avoid timer
					redirectTimer = 0.0f;
				}
			}
			else if (hit.collider.tag == "Player"){ // stop walking so the player can talk to you
				isMoving = false;
				direction = Vector2.zero;
			}
			
			//stuff for the animator
			//isMoving = true;
			//lastMovement = new Vector2 (0f, move_y);
		}
		
		//cast a box to see if NPC will collide horizontally
		// horizontal input
		if (move_x != 0) {
			hit = Physics2D.BoxCast (adjustedPosition + (offsetMultiple * boxCollider.bounds.size.magnitude), boxCollider.size, 0,
			                         new Vector2 (direction.x, 0), Mathf.Abs (direction.x));
			if (hit.collider == null) { // if the collider is null, no collision
				transform.Translate (direction.x, 0, 0);// move in the x direction
				//anim.SetFloat("hSpeed", direction.x);
			}
			else if (hit.collider.tag != "Player") { //if we hit something, set the walking state to avoid, except for the player
				if(walkingState != State.avoiding){
					walkingState = State.avoiding;
					//reset the redirect timer for use as an avoid timer
					redirectTimer = 0.0f;
				}
			}
			else if (hit.collider.tag == "Player"){ // stop walking so the player can talk to you
				isMoving = false;
				direction = Vector2.zero;
			}
			
			//stuff for the animator
			//isMoving = true;
			//lastMovement = new Vector2 (move_x, move_y);
		}

		//stuff for the animator
		anim.SetFloat("vSpeed", direction.y);
		anim.SetFloat("hSpeed", direction.x);
		//isMoving = true;
		lastMovement = new Vector2 (move_x, move_y);
	}

	//a method used during the avoid state to check if the desired path is clear
	bool ClearPath (float move_x, float move_y){
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
				return true; } //let the NPC know that the desired path is clear
			else { return false;} //let the npc know the path is not clear
		}
		
		//cast a box to see if NPC will collide horizontally
		// horizontal input
		if (move_x != 0) {
			hit = Physics2D.BoxCast (adjustedPosition + (offsetMultiple * boxCollider.bounds.size.magnitude), boxCollider.size, 0,
			                         new Vector2 (direction.x, 0), Mathf.Abs (direction.x));
			if (hit.collider == null) { // if the collider is null, no collision
				return true;
			} //let the NPC know that the desired path is clear
			else {
				return false;
			} //let the npc know the path is not clear
		} else {
			return false; // an exception catch
		}
	}

	void ChooseDirection () {
		// compare the coordinates of the current waypoint to the NPCs coordinates
		float xDifference = 0.0f;
		float yDifference = 0.0f;
		//find the differences in the coordinates
		xDifference = currentWaypoint.transform.position.x - transform.position.x;
		yDifference = currentWaypoint.transform.position.y - transform.position.y;

		//comapare the absolute values of x and y differences to determine horizontal or vertical movement
		if (Mathf.Abs (xDifference) >= Mathf.Abs (yDifference)) //if x is greater, move horizontally
		{
			if(xDifference > 0.0f) { desiredDirection = Direction.right; }
			else if (xDifference < 0.0f) { desiredDirection = Direction.left; }
		} 
		else // if y is greater, move vertically
		{
			if(yDifference > 0.0f) { desiredDirection = Direction.up; }
			else if(yDifference < 0.0f) { desiredDirection = Direction.down; }
		}
	}

	void ChooseNextWaypoint () {
		//randomly decide the next waypoint to travel to from a list of adjacent points
		//avoiding the previous waypoint

		int index = 0;

		if (prevWaypoint == null) { // if this is the first waypoint, there is no prevWaypoint
			//pick any valid index
			index = Random.Range (0, currentWaypoint.adjacentPoints.Count);
		} 
		else { //if this is not our first waypoint, we have an index we need to avoid
			//loop until the random number generator picks a point that is not the previous point
			do{
				index = Random.Range(0,currentWaypoint.adjacentPoints.Count);
			}while(currentWaypoint.adjacentPoints[index] == prevWaypoint);
		}

		//set previous waypoint to the waypoint we just reached
		prevWaypoint = currentWaypoint;
		//set current waypoint to the adjacent waypoint at the random index
		currentWaypoint = currentWaypoint.adjacentPoints [index];

		//pick the next direction to travel in based on the location of the new currentWaypoint
		ChooseDirection ();
	}
	
	// Update is called once per frame
	void Update () {
		if (walkingState == State.walking) {

			//every x amount of seconds, re-check the direction we should head
			redirectTimer += Time.deltaTime;
			if (redirectTimer >= redirectTime) {
				//reset the timer
				redirectTimer = 0.0f;
				//choose a new direction
				ChooseDirection ();
			}

			//move towards the current waypoint until we enter the trigger
			if (desiredDirection == Direction.up) {
				Move (0.0f, 1.0f);
			} else if (desiredDirection == Direction.left) {
				Move (-1.0f, 0.0f);
			} else if (desiredDirection == Direction.down) {
				Move (0.0f, -1.0f);
			} else if (desiredDirection == Direction.right) {
				Move (1.0f, 0.0f);
			}
		} else if (walkingState == State.avoiding) {

			//avoid with a rotation based on desired direction until the desired path is clear
			//if desired direction is up, avoid to the right
			if(desiredDirection == Direction.up){
				Move (1.0f, 0.0f);
				//set the state back to walking if the path is clear
				if(ClearPath(0.0f, 1.0f) == true){walkingState = State.walking;}
			}
			//if desired direction is right, avoid downward
			else if (desiredDirection == Direction.right){
				Move (0.0f, -1.0f);
				//set the state back to walking if the path is clear
				if(ClearPath(1.0f, 0.0f) == true){walkingState = State.walking;}
			}
			//if desired direction is down, avoid to the left
			else if (desiredDirection == Direction.down){
				Move (-1.0f, 0.0f);
				//set the state back to walking if the path is clear
				if(ClearPath(0.0f, -1.0f) == true){walkingState = State.walking;}
			}
			//if desired direction is left, avoid upward
			else if (desiredDirection == Direction.left){
				Move (0.0f, 1.0f);
				//set the state back to walking if the path is clear
				if(ClearPath(-1.0f, 0.0f) == true){walkingState = State.walking;}
			}
			//if the NPC collides with something else while avoiding, stop until desired path is clear (handled in move)
			//if avoiding for too long, head back to the previous waypoint
			redirectTimer += Time.deltaTime;
			if(redirectTimer >= redirectTime && prevWaypoint != null){
				//print (this.name + " is STUCK, redirecting.");
				redirectTimer = 0.0f; // reset the timer for use by walking state
				walkingState = State.walking; //set state to walking
				currentWaypoint = prevWaypoint; //turn the stuck NPC around
				ChooseDirection(); // let it choose the best direction
			}
		}

		// set values for animator to determine movement/idle animations
		if (canMove)
		{
			//anim.SetFloat("vSpeed", Input.GetAxisRaw("Vertical"));
			//anim.SetFloat("hSpeed", Input.GetAxisRaw("Horizontal"));
			anim.SetBool("isMoving", isMoving);
			anim.SetFloat("lastHSpeed", lastMovement.x);
			anim.SetFloat("lastVSpeed", lastMovement.y);
		}
		else
		{
			anim.SetBool("isMoving", canMove);
		}

		sr.sortingOrder = (int)(-transform.position.y * 10.0f);
	
	}

	//THis method checks npc's collision with a trigger, assumedly a waypoint
	void OnTriggerEnter2D(Collider2D other){
        
        //Check if the trigger is the current target waypoint
        if (other == currentWaypoint.box)
        {
            // Start the timer in a coroutine
            StartCoroutine(StartCountdown());
        }
	}

    /// <summary>
    /// This coroutine once started will loop until the waypointTimer has met its target time.
    /// Then it will choose the next waypoint, reset the timer, and the coroutine will end.
    /// </summary>
    IEnumerator StartCountdown()
    {
        //check if our target time has been met
        while (waypointTimer < waypointTargetTime)
        {
            waypointTimer += Time.deltaTime; //increase time within trigger
            yield return null; // required for coroutine -- wait one frame, then repeat loop
        }        
        
        //if the target time is met, pick a new waypoint to travel to
        ChooseNextWaypoint();

        //reset the timer
        waypointTimer = 0.0f;
    }

    public void FaceCharacter(Vector2 directionToFace)
    {
        prevState = walkingState; // store what the current state is
        walkingState = State.talking; // set NPC to talking
        canMove = false; // Stop moving, to set to idle sprites
        lastMovement = directionToFace; // set direction to face based on opposite player's dir

        // set the blend tree animator to face the proper direction
        anim.SetFloat("lastHSpeed", lastMovement.x);
        anim.SetFloat("lastVSpeed", lastMovement.y);
    }
    public void BackToNormal()
    {
        canMove = true;
        walkingState = prevState;
    }
}
