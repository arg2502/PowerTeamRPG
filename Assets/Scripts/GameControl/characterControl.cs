using UnityEngine;
using System.Collections;
using System;

public class characterControl : OverworldObject {

    
    public enum CharacterState
    {
        Transition,
        Normal,
        Battle,
        Defeat,
        Menu
    }

	private BoxCollider2D boxCollider; // Variable to reference our collider
	private RaycastHit2D hit; //checks for collisions

    float moveSpeed = 0;
    float walkSpeed = 6f;
    float runSpeed = 9f;
    public Vector2 speed;
    public Vector2 desiredSpeed;
    //PauseMenu pm;
	//public bool canMove;

    //public LayerMask movableMask;

	MovableOverworldObject carriedObject;
	bool isCarrying = false;

    Animator anim;

    bool isMoving;
    Vector2 lastMovement;

    // for room transition
    float xIncrementTransition = 0, yIncrementTransition = 0;
    Vector2 desiredPos;
    Action OnDesiredPos;
    Gateway currentGateway;
    CameraController myCamera;

    NPCDialogue currentNPC; // we can only talk to one NPC at a time, this variable will keep that one in focus
    float talkingDistance = 2f; // multiple of how far away to check if we can talk to something

    // Use this for initialization
    void Start () {

        //pm = GetComponentInChildren<PauseMenu>();
        myCamera = FindObjectOfType<CameraController>();
        anim = GetComponent<Animator>();
        transform.position = GameControl.control.currentPosition; // this is just temporary, as the final version will have to be more nuanced
		canMove = true;
        base.Start();

		//new code -------------------------------------------------
		boxCollider = GetComponent<BoxCollider2D> ();
		//end new code ---------------------------------------------

        //canMove = false;
        
        // if the character is transitioning through a gateway, call EnterRoom
        if (GameControl.control.currentCharacterState == CharacterState.Transition)
        {
            Debug.Log(GameControl.control.currentEntranceGateway);
            var gateway = GameControl.control.currentEntranceGateway ? GameControl.control.currentEntranceGateway : GameControl.control.currentRoom.FindCurrentGateway(GameControl.control.areaEntrance);

            if (gateway != null && !GameControl.control.taggedStatue)
                EnterRoom(gateway.transform.position, gateway.entrancePos);
            else
                GameControl.control.currentCharacterState = CharacterState.Normal;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // for transition between areas
        if (other.GetComponent<Gateway>() && GameControl.control.currentCharacterState == CharacterState.Normal)
        {
            currentGateway = other.GetComponent<Gateway>();
            StartCoroutine(myCamera.Fade());
            ExitRoom(currentGateway.transform.position, currentGateway.exitPos);            
        }

        // for NPC interaction
        if(other.GetComponent<NPCDialogue>())
        {
            // When we enter a trigger area for an NPC that will speak, start checking for collisions
            CheckInRangeNPC();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<NPCDialogue>())
        {
            // While within an NPC's trigger area, constantly check for NPCs
            // We want to constantly check in case we are in situations where there are multiple NPCs
            // We want to be able to turn and talk to that NPC without any problems
            CheckInRangeNPC();

            // If we have an NPC to talk to, check if the player has pressed select to talk to them
            if(currentNPC)
                TalkToNPC();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // if we are no longer colliding with an NPC, & they were our currentNPC, set the current to null
        if(collision.GetComponent<NPCDialogue>() && Equals(collision.GetComponent<NPCDialogue>(), currentNPC))
        {
            ResetCurrentNPC();
        }
    }

    // NPC Talking interaction section -------------------------
    void TalkToNPC()
    {
        // If we:
        //  press the select key,
        //  the NPC is not talking already
        //  and the character is not already talking
        // then begin talking to the NPC
        if(Input.GetKeyDown(GameControl.control.selectKey)// && !GameControl.control.IsInMenu
            && GameControl.control.currentCharacterState != CharacterState.Menu)
        {
            // if the NPC has a pathwalk, set the NPC to stop and face the player
            if (currentNPC.GetComponentInParent<NPCPathwalkControl>())
                currentNPC.GetComponentInParent<NPCPathwalkControl>().FaceCharacter(-(lastMovement));

            // begin the NPC's dialogue
            currentNPC.StartDialogue();
        }
    }

    void ResetCurrentNPC(NPCDialogue newCurrent = null)
    {
        // Set the previously current NPC back to normal
        if (currentNPC)
            currentNPC.GetComponentInParent<SpriteRenderer>().color = Color.white; // FOR NOW, we just changed the color

        // set the new current NPC if one was passed in
        if(newCurrent)
        {
            currentNPC = newCurrent;

            // Some sort of indicator to tell the player who they can talk to
            // FOR NOW, we just changed the color
            currentNPC.GetComponentInParent<SpriteRenderer>().color = Color.yellow;
        }
        // if no NPC was passed in, then there's no one we can talk to
        else
        {
            currentNPC = null;
        }
    }

    void CheckInRangeNPC()
    {
        // Check if there is an NPC if front of us by casting a box based off of Jethro's lastMovement vector        
        var talkingVector = lastMovement * talkingDistance;
        var triggerHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, talkingVector, Mathf.Abs(talkingVector.magnitude), mask);
        //Debug.DrawLine(boxCollider.bounds.center, boxCollider.bounds.center + new Vector3(talkingVector.x, talkingVector.y));
        
        // If there was a collision with an NPC that we can talk to, then set that to the current NPC
        if(triggerHit.collider && triggerHit.collider.GetComponentInChildren<NPCDialogue>())
        {
            ResetCurrentNPC(triggerHit.collider.GetComponentInChildren<NPCDialogue>());
        }
        // otherwise, there's no one in front of us, so we shouldn't be able to talk to anyone
        else
        {
            ResetCurrentNPC();
        }
    }
    // END NPC Talking interaction section ---------------------------

    //new code ----------------------------------------------------
    //A method for determining movement, which also checks collisions
    public void Move(float move_x, float move_y){

        // if we're talking to an npc, don't move at all
        if (GameControl.control.currentCharacterState == CharacterState.Menu)
            return;

		//create the vector of the desired movement
		Vector2 direction = new Vector2 (move_x, move_y);
		direction.Normalize ();
		direction *= moveSpeed * Time.deltaTime;

		//get the position of the boxcollider, adjusted for any offsets
		//Vector2 adjustedPosition = new Vector2((transform.position.x + boxCollider.offset.x),
		//                                       (transform.position.y + boxCollider.offset.y));

        hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, direction, Mathf.Abs(direction.magnitude), mask);

		// if there was a hit, test the individual axes, one after the other
        if (hit.collider) {
			//The clipping on corners occured because these 2 individual tests
			//created a false positive, where the 2 tests would cast in the cardinal
			//directions and miss the corner of a hitbox, and then the player
			//would move diagonally and hit the corner the sests missed
			//Performing a translate after each test fixes this problem

			var horHit = Physics2D.BoxCast (boxCollider.bounds.center, boxCollider.size, 0,
                                   new Vector2 (direction.x, 0), Mathf.Abs (direction.x), mask);
			if (horHit.collider == null){
				transform.Translate(new Vector2 (direction.x, 0.0f));
			}

			var vertHit = Physics2D.BoxCast (boxCollider.bounds.center, boxCollider.size, 0,
                                   new Vector2 (0, direction.y), Mathf.Abs (direction.y), mask);

			if (vertHit.collider == null){
				transform.Translate(new Vector2 (0.0f, direction.y));
			}
		}
		else {
			//if the collider is null, no collision, just translate
			transform.Translate (direction);
		}

        // vertical input
        if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < -0.5f) {			
			//stuff for the animator
			isMoving = true;
			lastMovement = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
		}

		// horizontal input
		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f) {
			//stuff for the animator
			isMoving = true;
			lastMovement = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
		}
	}
	//end new code -----------------------------------------------------
	
	// Update is called once per frame
    void Update()
    {
        // DEBUG - change time rate
        if (Input.GetKeyDown(KeyCode.O) && Time.timeScale > 0.1f)
            Time.timeScale -= 0.1f;
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale < 1.0f)
            Time.timeScale += 0.1f;

        if (Input.GetKeyUp(GameControl.control.pauseKey) && GameControl.control.currentCharacterState == CharacterState.Normal)
        {
            GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.PauseMenu);
        }

        sr.sortingOrder = (int)(-transform.position.y * 10.0f);
        speed = new Vector2(0f, 0f);
        //desiredSpeed = Vector2.zero;

        if (GameControl.control.currentCharacterState == CharacterState.Transition)
        {
            anim.SetBool("isMoving", true);
            anim.SetFloat("vSpeed", yIncrementTransition);
            anim.SetFloat("hSpeed", xIncrementTransition);

            if (!MoveToRoomTransitionSpot(desiredPos, xIncrementTransition, yIncrementTransition))
            {
                var transitionSpeed = new Vector2(xIncrementTransition, yIncrementTransition);
                transitionSpeed.Normalize();
                transitionSpeed *= walkSpeed * Time.deltaTime;

                transform.Translate(transitionSpeed);

                return;
            }
            else
            {
                OnDesiredPos.Invoke();
                
            }

        }



        // STATE == NORMAL
		if (canMove) {
            isMoving = false;

			if (Input.GetKey (GameControl.control.runKey) || Input.GetKey (KeyCode.RightShift)) {
				moveSpeed = runSpeed;
			} else {
				moveSpeed = walkSpeed;
			}

            //Check for input
			if(Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
			{
				//call the move function
				Move (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
            }

            // If picking up or putting down an object
            if (Input.GetKeyUp(GameControl.control.selectKey))
            {
				//If the player is not carrying an object, check for one
				if(!isCarrying)
				{
					//use the last calculated boxcast to see if we've clicked on a movable object
					if (hit.collider != null && hit.collider.tag == "Movable")
					{
						print("Hit " + hit.collider.name);
						//if it hits one, move the object to the position above Jethro's head and set iscarried to true
						isCarrying = true;
						//disable the object's collider, so it doesn't hinder movement
						hit.collider.enabled = false;
						//set carried object to the object we are picking up
						carriedObject = hit.collider.GetComponent<MovableOverworldObject>();
						carriedObject.isCarried = true; // set the object's isCarried to true
						//Move the object to the position above player's head
						carriedObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.85f, transform.position.z);
						//make sure the object is always rendered above the player
						carriedObject.SortingOrder = sr.sortingOrder + 1;
					}
				}
				//if an object is already being carried, drop the object in the direction that jethro is facing
				else if(isCarrying)
				{
					//check that the area you are facing is open enough to drop the object
					//Start by calculating the adjusted position of player's feet (since we're placing object "on the ground")
					Vector2 adjustedPosition = new Vector2((transform.position.x + boxCollider.offset.x),
					                                       (transform.position.y + boxCollider.offset.y));
					//get the carried object's collider (to avoid calling Get component more than necessary
					BoxCollider2D carriedCollider = carriedObject.GetComponent<BoxCollider2D>();

					//cast the collider forward from the player's adjusted position
					hit = Physics2D.BoxCast (adjustedPosition, carriedCollider.size, 0,
					                         lastMovement, Mathf.Abs (carriedCollider.size.x), mask);

					//Put the object down if clear -- this part will prob have to be edited in the future
					//to allow for switches and holes, etc, that the object can be placed on top of
					if(hit.collider == null){
						//calculate how much to translate the object
						int xMultiple = 0;
						int yMultiple = 0;
						//make the xMultiple either 1, 0, or -1, for directional purposes
						if (lastMovement.x != 0 && lastMovement.x < 0) {xMultiple = -1;}
						else if (lastMovement.x != 0 && lastMovement.x > 0) {xMultiple = 1;}
						//make the yMultiple either 1, 0, or -1, for directional purposes
						if (lastMovement.y != 0 && lastMovement.y < 0) {yMultiple = -1;}
						else if (lastMovement.y != 0 && lastMovement.y > 0) {yMultiple = 1;}

						//move the object to it's final resting place
						carriedObject.transform.position = new Vector3(((carriedCollider.size.x  + 0.1f) * xMultiple) + adjustedPosition.x,
						                                               ((carriedCollider.size.y  + carriedCollider.offset.y + 0.25f)* yMultiple) + transform.position.y);
						//reset the object's isCarried to false
						carriedObject.isCarried = false;
						//reset the object's collider to enabled
						carriedCollider.enabled = true;
						//reset the player's isCarrying to false
						isCarrying = false;
					}
				}
            }

			//if we are carrying an object, move it to player's position
			if(isCarrying)
			{
				//Move the object to the position above player's head
				carriedObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.85f, transform.position.z);
				//make sure the object is always rendered above the player
				carriedObject.SortingOrder = sr.sortingOrder + 1;
			}
			
		}

        // set values for animator to determine movement/idle animations
        if (canMove)
        {
            anim.SetFloat("vSpeed", Input.GetAxisRaw("Vertical"));
            anim.SetFloat("hSpeed", Input.GetAxisRaw("Horizontal"));
            anim.SetBool("isMoving", isMoving);
            anim.SetFloat("lastHSpeed", lastMovement.x);
            anim.SetFloat("lastVSpeed", lastMovement.y);
        }
        else
        {
            anim.SetBool("isMoving", canMove);
        }
    }
    
    void EnterRoom(Vector2 startPos, Vector2 endPos)
    {
        FindIncrementTransitionValues(startPos, endPos);
        desiredPos = endPos;
        OnDesiredPos = FinishEntrance;
        GameControl.control.currentCharacterState = CharacterState.Transition;
    }
    void ExitRoom(Vector2 startPos, Vector2 endPos)
    {
        FindIncrementTransitionValues(startPos, endPos);

        // determine the desiredPos value based on the increment transition values
        Vector2 newDesiredPos;

        // if xIncrement is not 0, then we're moving left/right
        // alter value of y
        if(xIncrementTransition != 0)
        {
            newDesiredPos = new Vector2(endPos.x, transform.position.y);
        }
        // otherwise, alter x
        else
        {
            newDesiredPos = new Vector2(transform.position.x, endPos.y);
        }

        // set desired value
        desiredPos = newDesiredPos;
        
        OnDesiredPos = FinishExit;

        GameControl.control.currentCharacterState = CharacterState.Transition;

    }

    public void FindIncrementTransitionValues(Vector2 startPos, Vector2 endPos)
    {
        // reset transition values
        xIncrementTransition = 0;
        yIncrementTransition = 0;
                
        // determine which direction Jethro will move
        // if x's are equal, then we need to move in y
        if (startPos.x == endPos.x)
        {
            // determine whether we're moving in positive or negative
            // if gate is less, than we're moving up
            if (startPos.y < endPos.y)
                yIncrementTransition = 1;
            // otherwise, we're going down
            else
                yIncrementTransition = -1;
        }
        // otherwise we need to move in x
        else
        {
            // determine whether we're moving in positive or negative
            // if gate is less, than we're moving right
            if (startPos.x < endPos.x)
                xIncrementTransition = 1;
            // otherwise, we're going left
            else
                xIncrementTransition = -1;
        }

        // continuously call the Move function until we are at the entrance position       
        //StartCoroutine(MoveToEntrance(entrancePos, xIncrementTransition, yIncrementTransition));

    }


    /// <summary>
    /// Called when entering a room. Moves Jethro from the Gateway entrance to the entrancePos.
    /// </summary>
    /// <param name="startPos">Position Jethro will start that frame.</param>
    /// <param name="endPos">The entrancePos and final destination. Never changes</param>
    /// <param name="xIncrement">How far Jethro will move in X direction. Either +, -, or 0.</param>
    /// <param name="yIncrement">How far Jethro will move in Y direction. Either +, -, or 0.</param>
    /// <returns></returns>
    bool MoveToRoomTransitionSpot(Vector3 endPos, float xIncrement, float yIncrement)
    {
        float position, finalPos;

        if (xIncrement != 0)
        {
            position = transform.position.x;
            finalPos = endPos.x;
        }
        else
        {
            position = transform.position.y;
            finalPos = endPos.y;
        }

        if((int)position != (int)finalPos)        
            return false;

        return true;
    }

    void FinishEntrance()
    {
        lastMovement = new Vector2(xIncrementTransition, yIncrementTransition);
        GameControl.control.currentCharacterState = CharacterState.Normal;
    }
    void FinishExit()
    {
        currentGateway.GetComponent<Gateway>().NextScene();
        currentGateway = null;
    }
}
