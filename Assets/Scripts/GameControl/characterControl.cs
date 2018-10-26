﻿using UnityEngine;
using System.Collections;
using System;

public class characterControl : OverworldObject {

    
    public enum CharacterState
    {
        Transition,
        Normal,
        Battle,
        Defeat
    }

	//new code -------------------------------------------------------------
	private BoxCollider2D boxCollider; // Variable to reference our collider
	private RaycastHit2D hit; //checks for collisions
	// end new code --------------------------------------------------------

    float moveSpeed = 0;
    float walkSpeed = 4.5f;
    float runSpeed = 7f;
    public Vector2 speed;
    public Vector2 desiredSpeed;
    //PauseMenu pm;
	//public bool canMove;

    public LayerMask movableMask;

    RaycastHit2D topHitCheck;
    RaycastHit2D bottomHitCheck;

    float sideHitFloat = 0.3f; // 0.25
    float topHitFloat = 0.5f;
    float bottomHitFloat = 0.75f;

    Animator anim;

    bool isMoving;
    Vector2 lastMovement;

    // for room transition
    float xIncrementTransition = 0, yIncrementTransition = 0;
    Vector2 desiredPos;
    Action OnDesiredPos;
    Gateway currentGateway;
    CameraController myCamera;
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
        if (GameControl.control.currentCharacterState == CharacterState.Normal)
        {
            if (other.GetComponent<Gateway>())
            {
                currentGateway = other.GetComponent<Gateway>();
                StartCoroutine(myCamera.Fade());
                ExitRoom(currentGateway.transform.position, currentGateway.exitPos);
            }
        }
    }
    //void OnCollisionEnter2D(Collision2D otherCollider2d)
    //{
    //    print("ENTER");
    //    canMove = false;
    //}
    //void OnCollisionExit2D(Collision2D otherCollider2d)
    //{
    //    print("EXIT");
    //    canMove = true;
    //}

    public bool CheckCollision(Vector2 dir, LayerMask _mask)
    {
        topHitCheck = Physics2D.Raycast(new Vector3(transform.position.x + sideHitFloat, transform.position.y - topHitFloat, transform.position.z), dir, 0.5f, _mask);
        bottomHitCheck = Physics2D.Raycast(new Vector3(transform.position.x - sideHitFloat, transform.position.y - bottomHitFloat, transform.position.z), dir, 0.5f, _mask);
        if (topHitCheck.collider == null && bottomHitCheck.collider == null) { return false; }
        else { return true; }

    }

	//new code ----------------------------------------------------
	//A method for determining movement, which also checks collisions
	public void Move(float move_x, float move_y){
		//create the vector of the desired movement
		Vector2 direction = new Vector2 (move_x, move_y);
		direction.Normalize ();
		direction *= moveSpeed * Time.deltaTime;
		
		//cast a box to see if jethro will collide vertically
		hit = Physics2D.BoxCast (transform.position, boxCollider.size, 0,
		                         new Vector2 (0, direction.y), Mathf.Abs (direction.y));
		if (hit.collider == null) { // if the collider is null, no collision
			transform.Translate(0, direction.y, 0); // move in the y direction

			isMoving = true;
			lastMovement = new Vector2(0f, Input.GetAxisRaw("Vertical"));
		}
		
		//cast a box to see if jethro will collide horizontally
		hit = Physics2D.BoxCast (transform.position, boxCollider.size, 0,
		                         new Vector2 (direction.x, 0), Mathf.Abs (direction.x));
		if (hit.collider == null) { // if the collider is null, no collision
			transform.Translate(direction.x, 0, 0); // move in the x direction

			isMoving = true;
			lastMovement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
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

        sr.sortingOrder = (int)-transform.position.y;
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

            // If not pushing or pulling
            if (!Input.GetKey(GameControl.control.selectKey))
            {
				// new code --------------------------------------------------------------
				if(Input.GetAxisRaw("Vertical") != 0f || Input.GetAxisRaw("Horizontal") != 0f){
					Move (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
				}
				// end new code ----------------------------------------------------------


//                // vertical input
//                if(Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
//                {
//                    desiredSpeed = new Vector2(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
//                    if(CheckCollision(new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical")), movableMask) == false)
//                    {
//                        speed += new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime);
//                    }
//                    isMoving = true;
//                    lastMovement = new Vector2(0f, Input.GetAxisRaw("Vertical"));
//                }
//
//                // horizontal input - may or may not be vertical input already
//                if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
//                {
//                    desiredSpeed = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f);
//                    if (CheckCollision(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), 0f), movableMask) == false)
//                    {
//                        speed += new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime, 0f);
//                    }
//                    isMoving = true;
//                    lastMovement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
//                }
//
//                //RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + sideHitFloat, transform.position.y + topHitFloat, transform.position.z), speed, 0.5f, mask);
//                //RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - sideHitFloat, transform.position.y - bottomHitFloat, transform.position.z), speed, 0.5f, mask);
//                
//                // if not colliding with object, move
//                if (speed != Vector2.zero && !CheckCollision(speed, mask))
//                {
//                    // normalize speed and mult by moveSpeed to prevent super fastness
//                    speed.Normalize();
//                    speed *= moveSpeed * Time.deltaTime;
//
//                    transform.Translate(speed);
//                    desiredSpeed = Vector2.zero;
//                }

            }

            // If pushing or pulling
            if (Input.GetKey(GameControl.control.selectKey))
            {
                RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + sideHitFloat, transform.position.y - topHitFloat, transform.position.z), desiredSpeed, 0.5f, mask);
                RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - sideHitFloat, transform.position.y - bottomHitFloat, transform.position.z), desiredSpeed, 0.5f, mask);
                if (topHit.collider != null && topHit.collider.tag == "Movable")
                {
                    if (//transform.position.y <= topHit.transform.position.y &&
                        !((transform.position.x > topHit.transform.position.x + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.x < topHit.transform.position.x - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && ((Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f))) // up - W
                    {
                        if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical") / 2)) == false)
                        {
                            if (!CheckCollision(new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical")) * Time.deltaTime, movableMask))
                            {
                                topHit.transform.Translate(new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical") / 2) * Time.deltaTime);
                                print(Input.GetAxisRaw("Vertical"));
                                speed += new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical")) * Time.deltaTime;
                                isMoving = true;
                            }
                        }
                    }
                    else if (//transform.position.x <= topHit.transform.position.x &&
                        !((transform.position.y > topHit.transform.position.y + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.y < topHit.transform.position.y - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && ((Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f))) // right - D
                    {
                        if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal") / 2, 0)) == false)
                        {
                            if (!CheckCollision(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), 0) * Time.deltaTime, movableMask))
                            {
                                topHit.transform.Translate(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal") / 2, 0) * Time.deltaTime);
                                speed += new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), 0) * Time.deltaTime;
                                isMoving = true;
                            }
                        }
                    }
                    topHit = Physics2D.Raycast(new Vector3(transform.position.x + sideHitFloat, transform.position.y - topHitFloat, transform.position.z), speed, 0.5f, mask);
                    bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - sideHitFloat, transform.position.y - bottomHitFloat, transform.position.z), speed, 0.5f, mask);
                    if (topHit.collider == null && bottomHit.collider == null) { transform.Translate(speed / 2); }
                }
                else if (bottomHit.collider != null && bottomHit.collider.tag == "Movable")
                {
                    if (//transform.position.y >= bottomHit.transform.position.y &&
                        !((transform.position.x > bottomHit.transform.position.x + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.x < bottomHit.transform.position.x - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)) // down - S
                    {
                        if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, -moveSpeed * Input.GetAxisRaw("Vertical") / 2)) == false)
                        {
                            if (!CheckCollision(new Vector2(0, -moveSpeed * Input.GetAxisRaw("Vertical")) * Time.deltaTime, movableMask))
                            {
                                bottomHit.transform.Translate(new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical") / 2) * Time.deltaTime);
                                speed += new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical")) * Time.deltaTime;
                                isMoving = true;
                            }
                        }
                    }
                    else if (//transform.position.x >= bottomHit.transform.position.x &&
                        !((transform.position.y > bottomHit.transform.position.y + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.y < bottomHit.transform.position.y - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)) // left - A
                    {
                        if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal") / 2, 0)) == false)
                        {
                            if (!CheckCollision(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), 0) * Time.deltaTime, movableMask))
                            {
                                bottomHit.transform.Translate(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal") / 2, 0) * Time.deltaTime);
                                speed += new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), 0) * Time.deltaTime;
                                isMoving = true;
                            }
                        }
                    }
                    topHit = Physics2D.Raycast(new Vector3(transform.position.x + sideHitFloat, transform.position.y - topHitFloat, transform.position.z), speed, 0.5f, mask);
                    bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - sideHitFloat, transform.position.y - bottomHitFloat, transform.position.z), speed, 0.5f, mask);
                    if (topHit.collider == null && bottomHit.collider == null) { transform.Translate(speed / 2); }
                }
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
