using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class characterControl : OverworldObject {

    
    public enum CharacterState
    {
        Transition,
        Normal,
        Battle,
        Defeat,
        Menu,
        Cutscene
    }

	private BoxCollider2D boxCollider; // Variable to reference our collider
	private RaycastHit2D hit; //checks for collisions

    float moveSpeed = 0;
    float walkSpeed;// = 6f;
    float runSpeed { get { return walkSpeed * 1.5f; } }//9f;
    float acc = 0f;
    float vel = 0f;
    float maxAcc = 2f;
    float maxVel = 25f;

    float jethroWalk = 6f;
    float coleWalk = 6f;
    float eleanorWalk = 6f;
    float joulietteWalk = 8f;
    float[] characterSpeeds;

    public Vector2 speed;
    public Vector2 desiredSpeed;

	public bool onRamp = false;
	public bool isRampRight = false;

    // Jethro -- Carrying
	MovableOverworldObject carriedObject;
	bool isCarrying = false;
    bool canCarry = true;
    MovableOverworldObject readyForPickup;

    // Eleanor -- Hidden
    List<HiddenObject> hiddenObjFound;
    float hiddenRadius = 20f;

    // Jouliette -- Zap
    float zapRadius = 3f;

    Animator anim;

    bool isMoving;
    Vector2 lastMovement;

    // for room transition
    float xIncrementTransition = 0, yIncrementTransition = 0;    
    Vector2 desiredPos;
    UnityEvent OnDesiredPos;
    Gateway currentGateway;
    bool canEnterDoor;
    CameraController myCamera;

    float talkingDistance = 2f; // multiple of how far away to check if we can talk to something

    public enum HeroCharacter { JETHRO, COLE, ELEANOR, JOULIETTE }    

    InteractionNotification gatewayNotification;

    QuestCutscene currentCutscene;

    // Use this for initialization
    new void Start () {

        myCamera = FindObjectOfType<CameraController>();
        anim = GetComponent<Animator>();
        transform.position = GameControl.control.currentPosition; // this is just temporary, as the final version will have to be more nuanced
		canMove = true;
        base.Start();

		boxCollider = GetComponent<BoxCollider2D> ();
        OnDesiredPos = new UnityEvent();
        characterSpeeds = new float[4] { jethroWalk, coleWalk, eleanorWalk, joulietteWalk };
        ChangeHero();


        currentGateway = GameControl.control.currentEntranceGateway ? GameControl.control.currentEntranceGateway : GameControl.control.currentRoom.FindCurrentGateway(GameControl.control.areaEntrance);
        currentCutscene = GameControl.control.CheckForEnterRoomCutscenes();

        if (currentCutscene != null)
        {
            if (currentGateway == null || currentCutscene.cutscene.triggerType == Cutscene.TriggerType.ROOM_ENTER)
            {
                ToggleSpriteRenderers(false);
                GameControl.control.currentCharacterState = CharacterState.Cutscene;
                GameControl.control.PlayCutscene(currentCutscene);
            }
            else if (currentCutscene.cutscene.triggerType == Cutscene.TriggerType.AFTER_ENTRANCE)
            {                

                EnterRoom(currentGateway.transform.position, currentGateway.entrancePos, true);
            }
        }
        else if (currentGateway != null)
            EnterRoom(currentGateway.transform.position, currentGateway.entrancePos);
        else
            GameControl.control.currentCharacterState = CharacterState.Normal;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (myCamera == null)
            return;

        // for transition between areas
        if (other.GetComponent<Gateway>() && GameControl.control.currentCharacterState == CharacterState.Normal)
        {
            currentGateway = other.GetComponent<Gateway>();
            if(currentGateway.gatewayType != Gateway.Type.DOOR) // handle door transition in update
            {
                StartCoroutine(myCamera.Fade(false));
                ExitRoom(currentGateway.transform.position, currentGateway.exitPos);
            }
            else
            {
                // get direction
                if (IsFacingDoor())
                {
                    canEnterDoor = true;
                    if (gatewayNotification == null)
                        gatewayNotification = GameControl.UIManager.ShowInteractionNotification(currentGateway.transform, "Enter");
                    else
                    {
                        gatewayNotification.Init(currentGateway.transform, "Enter");
                        gatewayNotification.GetComponent<Animator>().Play("FadeIn");
                    }
                }   
                else if (canEnterDoor && !IsFacingDoor())
                {
                    canEnterDoor = false;
                    gatewayNotification?.GetComponent<Animator>()?.Play("FadeOut");
                }
            }
        }

        if (other.GetComponent<NPCDialogue>() || other.GetComponent<InteractiveObject>())
        {
            // While within an NPC's trigger area, constantly check for NPCs
            // We want to constantly check in case we are in situations where there are multiple NPCs
            // We want to be able to turn and talk to that NPC without any problems
            CheckInRangeObj();
        }

        if(other.GetComponent<Firewall>())
        {
            canCarry = false;
        }

        if (other.transform.parent?.tag == "Movable")
        {
            if (GameControl.control.currentCharacter == HeroCharacter.JETHRO)
            {
                //If the player is not carrying an object, check for one
                if (!isCarrying && canCarry)
                {
                    //create a box cast specifically for picking up objects, as per Alec's request
                    Vector2 direction = lastMovement;
                    direction.Normalize();
                    direction *= moveSpeed * Time.deltaTime;
                    hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, direction, Mathf.Abs(direction.magnitude) * 5.0f, mask);

                    if (hit.collider != null && hit.collider.tag == "Movable")
                    {
                        if (readyForPickup == null)
                        {
                            readyForPickup = hit.transform.GetComponent<MovableOverworldObject>();
                            readyForPickup.ShowInteractionNotification("Pick Up");
                        }
                    }
                    else
                    {
                        readyForPickup?.HideInteractionNotification();
                        readyForPickup = null;
                    }
                }
            }
            else
            {
                readyForPickup?.HideInteractionNotification();
                readyForPickup = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Gateway>())
        {
            canEnterDoor = false;
            gatewayNotification?.GetComponent<Animator>()?.Play("FadeOut");
        }
        // if we are no longer colliding with an NPC, & they were our currentNPC, set the current to null
        if ((collision.GetComponent<NPCDialogue>() && Equals(collision.GetComponent<NPCDialogue>(), GameControl.control.currentObj))
            ||(collision.GetComponent<InteractiveObject>() && Equals(collision.GetComponent<InteractiveObject>(), GameControl.control.currentObj)))
        {
            ResetCurrentNPC();
        }

        if(collision.GetComponent<Firewall>())
        {
            canCarry = true;
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

        // if the NPC has a pathwalk, set the NPC to stop and face the player
        if (GameControl.control.CurrentNPCPathwalk)
            GameControl.control.CurrentNPCPathwalk.FaceCharacter(-(lastMovement));
        else if (GameControl.control.CurrentStationaryNPC)
            GameControl.control.CurrentStationaryNPC.FaceCharacter(-(lastMovement));

        isMoving = false;

        // begin the NPC's dialogue
        GameControl.control.currentObj.GetComponentInChildren<NPCDialogue>().StartDialogue();
        
    }

    void ResetCurrentNPC(OverworldObject newCurrent = null)
    {
        // Set the previously current NPC back to normal        
        if(GameControl.control.currentObj != null && GameControl.control.currentObj != newCurrent)
        {
            GameControl.control.currentObj.GetComponentInParent<OverworldObject>()?.HideInteractionNotification();
        }

        // set the new current NPC if one was passed in
        if (newCurrent)
        {
            if (GameControl.control.currentObj != newCurrent)
            {
                GameControl.control.currentObj = newCurrent;
                               
                string text = "Read";
                if (GameControl.control.currentObj is NPCObject)
                    text = "Talk";
                GameControl.control.currentObj.ShowInteractionNotification(text);
            }
        }
        // if no NPC was passed in, then there's no one we can talk to
        else
        {
            GameControl.control.currentObj = null;            
        }
    }

    void CheckInRangeObj()
    {
        // Check if there is an NPC if front of us by casting a box based off of Jethro's lastMovement vector        
        var talkingVector = lastMovement * talkingDistance;
        var triggerHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, talkingVector, Mathf.Abs(talkingVector.magnitude), mask);
        
        // If there was a collision with an NPC that we can talk to, then set that to the current NPC
        if(triggerHit.collider)
        {
            if (triggerHit.collider.GetComponentInChildren<NPCDialogue>())
            {
                var npcDialogue = triggerHit.collider.GetComponentInChildren<NPCDialogue>();
                if (npcDialogue != null && npcDialogue.canTalk)
                    ResetCurrentNPC(npcDialogue.GetComponentInParent<OverworldObject>());
            }
            else if (triggerHit.collider.GetComponent<InteractiveObject>())
            {
                var io = triggerHit.collider.GetComponent<InteractiveObject>();
                if (io != null)
                    ResetCurrentNPC(io);
            }
        }
        // otherwise, there's no one in front of us, so we shouldn't be able to talk to anyone
        else
        {
            ResetCurrentNPC();
        }
    }
    // END NPC Talking interaction section ---------------------------

    //A method for determining movement, which also checks collisions
    public void Move(float move_x, float move_y){

        // if we're talking to an npc, don't move at all
        if (GameControl.control.currentCharacterState == CharacterState.Menu)
            return;

		//create the vector of the desired movement
		Vector2 direction = new Vector2 (move_x, move_y);
		direction.Normalize ();
		direction *= moveSpeed * Time.deltaTime;

		if (onRamp && isRampRight) { direction = new Vector2(direction.x, direction.y + direction.x);}
		if (onRamp && !isRampRight) { direction = new Vector2(direction.x, direction.y - direction.x);}


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
	
    void UpdateNormal()
    {
        if (Input.GetButtonDown("Pause"))
        {
            //GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.PauseMenu);         
            GameControl.UIManager.PushPauseCarousel();
        }

        if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") > 0)
            IncreaseHero();
        else if (Input.GetButtonDown("MenuNav") && Input.GetAxisRaw("MenuNav") < 0)
            DecreaseHero();

        if (Input.GetButtonDown("Submit"))
        {
            if (canEnterDoor && currentGateway?.gatewayType == Gateway.Type.DOOR)
            {
                StartCoroutine(myCamera.Fade(false));
                ExitRoom(currentGateway.transform.position, currentGateway.transform.position); // don't move
            }

            // If we have an NPC to talk to, check if the player has pressed select to talk to them
            if (GameControl.control.currentObj)
            {
                if (GameControl.control.currentObj.GetComponent<InteractiveObject>())
                    GameControl.control.currentObj.GetComponent<InteractiveObject>().PerformAction();
                else if (GameControl.control.currentObj.GetComponentInChildren<NPCDialogue>())
                    TalkToNPC();
            }

            if (GameControl.control.currentCharacter == HeroCharacter.ELEANOR
                && GameControl.control.currentCharacterState == CharacterState.Normal)
            {
                if (canMove)
                {
                    hiddenObjFound = new List<HiddenObject>();

                    // For now, I'll do a large overlap circle to detect any HiddenObjects
                    // within camera view (not precise, but around there)
                    // This is probably not the best way, but we'll see what happens
                    var hits = Physics2D.OverlapCircleAll(transform.position, hiddenRadius);
                    foreach (var h in hits)
                    {
                        if (h.GetComponent<HiddenObject>())
                            hiddenObjFound.Add(h.GetComponent<HiddenObject>());
                    }
                    if (hiddenObjFound.Count > 0)
                    {
                        foreach (var obj in hiddenObjFound)
                            obj.Show();
                    }

                    canMove = false;
                }
                else
                {
                    foreach (var obj in hiddenObjFound)
                        obj.Hide();

                    hiddenObjFound.Clear();
                    canMove = true;
                }
            }
        }

        // STATE == NORMAL
        if (canMove)
        {
            isMoving = false;
            moveSpeed = walkSpeed;
            if (GameControl.control.currentCharacter != HeroCharacter.JOULIETTE)
            {
                if (Input.GetButton("Run"))
                    moveSpeed = runSpeed;
            }
            else
            {
                if (Input.GetButton("Run"))
                {
                    if (acc < 0) acc = 0;
                    acc += Time.deltaTime;
                    if (acc >= maxAcc) acc = maxAcc;

                    vel += acc;
                    if (vel > maxVel) vel = maxVel;
                    moveSpeed = vel;

                }
                else
                {
                    //if (acc > 0) acc = 0;
                    //acc -= Time.deltaTime;
                    //if (acc <= -maxAcc) acc = -maxAcc;
                    //vel += acc;
                    //if (vel <= walkSpeed) vel = walkSpeed;
                    //moveSpeed = vel;
                    if(acc != 0 || vel != 0) { acc = 0; vel = 0; }
                    moveSpeed = walkSpeed;
                }
            }

            //Check for input
            if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
            {
                //call the move function
                Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            // special action
            if (Input.GetButtonDown("Submit"))
            {
                // If picking up or putting down an object
                if (GameControl.control.currentCharacter == HeroCharacter.JETHRO)
                {
                    //If the player is not carrying an object, check for one
                    if (!isCarrying && canCarry)
                    {
                        //create a box cast specifically for picking up objects, as per Alec's request
                        //Vector2 direction = lastMovement;
                        //direction.Normalize();
                        //direction *= moveSpeed * Time.deltaTime;
                        //hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, direction, Mathf.Abs(direction.magnitude) * 5.0f, mask);

                        //if (hit.collider != null && hit.collider.tag == "Movable")
                        if(readyForPickup != null)
                        {
                            //if it hits one, move the object to the position above Jethro's head and set iscarried to true
                            isCarrying = true;
                            //disable the object's collider, so it doesn't hinder movement
                            readyForPickup.GetComponent<Collider2D>().enabled = false;
                            readyForPickup.HideInteractionNotification();
                            //set carried object to the object we are picking up
                            carriedObject = readyForPickup;
                            readyForPickup = null;
                            carriedObject.isCarried = true; // set the object's isCarried to true
                            carriedObject.HideInteractionNotification(true);
                                                            //Move the object to the position above player's head
                            carriedObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.85f, transform.position.z);
                            //make sure the object is always rendered above the player
                            carriedObject.SortingOrder = sr.sortingOrder + 1;
                        }
                    }
                    //if an object is already being carried, drop the object in the direction that jethro is facing
                    else if (isCarrying)
                    {
                        //check that the area you are facing is open enough to drop the object
                        //Start by calculating the adjusted position of player's feet (since we're placing object "on the ground")
                        Vector2 adjustedPosition = new Vector2((transform.position.x + boxCollider.offset.x),
                                                               (transform.position.y + boxCollider.offset.y));
                        //get the carried object's collider (to avoid calling Get component more than necessary
                        BoxCollider2D carriedCollider = carriedObject.GetComponent<BoxCollider2D>();

                        //cast the collider forward from the player's adjusted position
                        hit = Physics2D.BoxCast(adjustedPosition, carriedCollider.size, 0,
                                                 lastMovement, Mathf.Abs(carriedCollider.size.x), mask);

                        //Put the object down if clear -- this part will prob have to be edited in the future
                        //to allow for switches and holes, etc, that the object can be placed on top of
                        if (hit.collider == null || hit.collider.GetComponentInChildren<Firewall>())
                        {
                            //calculate how much to translate the object
                            int xMultiple = 0;
                            int yMultiple = 0;
                            //make the xMultiple either 1, 0, or -1, for directional purposes
                            if (lastMovement.x != 0 && lastMovement.x < 0) { xMultiple = -1; }
                            else if (lastMovement.x != 0 && lastMovement.x > 0) { xMultiple = 1; }
                            //make the yMultiple either 1, 0, or -1, for directional purposes
                            if (lastMovement.y != 0 && lastMovement.y < 0) { yMultiple = -1; }
                            else if (lastMovement.y != 0 && lastMovement.y > 0) { yMultiple = 1; }

                            //move the object to it's final resting place
                            carriedObject.transform.position = new Vector3(((carriedCollider.size.x + carriedCollider.offset.x) * xMultiple) + adjustedPosition.x,
                                                                           ((carriedCollider.size.y + carriedCollider.offset.y + Math.Abs(carriedCollider.offset.y)) * yMultiple) + transform.position.y);
                            //reset the object's isCarried to false
                            carriedObject.isCarried = false;
                            //reset the object's collider to enabled
                            carriedCollider.enabled = true;
                            //reset the player's isCarrying to false
                            isCarrying = false;
                        }
                    }
                }

                else if (GameControl.control.currentCharacter == HeroCharacter.JOULIETTE)
                {
                    var hits = Physics2D.OverlapCircleAll(transform.position, zapRadius);
                    foreach (var h in hits)
                    {
                        if (h.GetComponent<Generator>())
                        {
                            h.GetComponent<Generator>().ActivateGenerator();
                        }
                    }

                }
            }

            //if we are carrying an object, move it to player's position
            if (isCarrying)
            {
                //Move the object to the position above player's head
                carriedObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f/*+ 0.85f*/, transform.position.z);
                //make sure the object is always rendered above the player
                carriedObject.SortingOrder = sr.sortingOrder + 1;
            }

        }
    }
    void UpdateMenu()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameControl.UIManager.list_currentMenus[0].GetComponent<UI.PauseMenu>() is UI.PauseMenu)
                GameControl.UIManager.PopAllMenus();
        }
    }
    void UpdateTransition()
    {
        if(currentGateway?.gatewayType != Gateway.Type.DOOR)
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
                transform.position = desiredPos;
                OnDesiredPos.Invoke();
            }
        }
    }
    void UpdateBattle()
    {

    }
    void UpdateDefeat()
    {

    }

    void SetAnimations()
    {
        // set values for animator to determine movement/idle animations
        if (canMove)
        {
            anim.SetFloat("vSpeed", Input.GetAxisRaw("Vertical"));
            anim.SetFloat("hSpeed", Input.GetAxisRaw("Horizontal"));
            anim.SetBool("isMoving", isMoving);
            if (GameControl.control.currentCharacter == HeroCharacter.JETHRO)
                anim.SetFloat("isCarry", System.Convert.ToSingle(isCarrying));
            anim.SetFloat("lastHSpeed", lastMovement.x);
            anim.SetFloat("lastVSpeed", lastMovement.y);
        }
        else
        {
            anim.SetBool("isMoving", canMove);
        }
    }

    void ToggleSpriteRenderers(bool show)
    {
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = show;
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = (int)(-transform.position.y * 10.0f);

        switch (GameControl.control.currentCharacterState)
        {
            case CharacterState.Normal:
                UpdateNormal(); break;
            case CharacterState.Menu:
                UpdateMenu(); break;
            case CharacterState.Transition:
                UpdateTransition(); break;
            case CharacterState.Battle:
                UpdateBattle(); break;
            case CharacterState.Defeat:
                UpdateDefeat(); break;
        }

        
        speed = new Vector2(0f, 0f);

        if (GameControl.control.currentCharacterState != CharacterState.Transition)
            SetAnimations();
    }
    
    void EnterRoom(Vector2 startPos, Vector2 endPos, bool playCutsceneAfter = false)
    {
        FindIncrementTransitionValues(startPos, endPos);
        desiredPos = endPos;

        if(playCutsceneAfter)
        {
            if (currentGateway.gatewayType != Gateway.Type.DOOR)
                OnDesiredPos.AddListener(FinishEntranceThenCutscene);
            else
            {
                transform.position = currentGateway.entrancePos;
                Invoke("FinishEntranceThenCutscene", 0.5f);
            }
        }
        else
        {
            if (currentGateway.gatewayType != Gateway.Type.DOOR)
                OnDesiredPos.AddListener(FinishEntrance);
            else
            {
                transform.position = currentGateway.entrancePos;
                Invoke("FinishEntrance", 0.5f);
            }
        }
        
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

        if (currentGateway.gatewayType != Gateway.Type.DOOR)
            OnDesiredPos.AddListener(FinishExit);
        else
            Invoke("FinishExit", 0.5f);

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

        var dist = Mathf.Abs(position - finalPos);
        if (currentGateway != null)
        {
            if (dist > 0.1f && dist < currentGateway.TransitionDist * 2f) // fail safe (not the best) -- if we get too far from where we should we need to stop eventually
                return false;
        }
        else
        {
            if (dist > 0.1f)
                return false;
        }

        return true;
    }

    void FinishEntrance()
    {
        lastMovement = new Vector2(xIncrementTransition, yIncrementTransition);
        GameControl.control.currentCharacterState = CharacterState.Normal;
        OnDesiredPos.RemoveAllListeners();
    }
    void FinishEntranceThenCutscene()
    {
        lastMovement = new Vector2(xIncrementTransition, yIncrementTransition);
        GameControl.control.currentCharacterState = CharacterState.Cutscene;
        OnDesiredPos.RemoveAllListeners();
        GameControl.control.PlayCutscene(currentCutscene);
    }
    void FinishExit()
    {
        if (currentGateway == null) return;
        currentGateway.GetComponent<Gateway>().NextScene();
        currentGateway = null;
        OnDesiredPos.RemoveAllListeners();
    }

    void IncreaseHero()
    {
        if (isCarrying) return;
        if (!canMove) return;
        GameControl.control.currentCharacter++;
        if (GameControl.control.currentCharacterInt > GameControl.control.heroList.Count - 1) GameControl.control.currentCharacter = 0;
        ChangeHero();
    }

    void DecreaseHero()
    {
        if (GameControl.control.currentCharacterState != CharacterState.Normal) return;
        if (!canMove) return;
        if (isCarrying) return;
        GameControl.control.currentCharacter--;
        if (GameControl.control.currentCharacterInt < 0) GameControl.control.currentCharacter = (HeroCharacter)GameControl.control.heroList.Count - 1;
        ChangeHero();
    }

    void ChangeHero()
    {
        GetComponent<Animator>().runtimeAnimatorController = GameControl.control.GetHeroAnimator();// heroAnimators[GameControl.control.currentCharacterInt];
        walkSpeed = characterSpeeds[GameControl.control.currentCharacterInt];
    }

    bool IsFacingDoor()
    {
        return (lastMovement.y > 0 && currentGateway.direction == Gateway.Direction.North)
                    || (lastMovement.y < 0 && currentGateway.direction == Gateway.Direction.South)
                    || (lastMovement.x > 0 && currentGateway.direction == Gateway.Direction.West)
                    || (lastMovement.x < 0 && currentGateway.direction == Gateway.Direction.East);
    }
}
