﻿using UnityEngine;
using System.Collections;

public class characterControl : OverworldObject {
    float moveSpeed = 0;
    float walkSpeed = 4.5f;
    float runSpeed = 7f;
    public Vector2 speed;
    public Vector2 desiredSpeed;
    PauseMenu pm;
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

	// Use this for initialization
	void Start () {

        pm = GetComponentInChildren<PauseMenu>();

        anim = GetComponent<Animator>();
        transform.position = GameControl.control.currentPosition; // this is just temporary, as the final version will have to be more nuanced
		canMove = true;
        base.Start();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Gateway>())
        {
            other.GetComponent<Gateway>().NextScene();
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
	
	// Update is called once per frame
    void Update()
    {
        // DEBUG - change time rate
        if (Input.GetKeyDown(KeyCode.Minus) && Time.timeScale > 0.0f)
            Time.timeScale -= 0.1f;
        if (Input.GetKeyDown(KeyCode.Plus) && Time.timeScale < 1.0f)
            Time.timeScale += 0.1f;

        sr.sortingOrder = (int)-transform.position.y;
        speed = new Vector2(0f, 0f);
        //desiredSpeed = Vector2.zero;

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
                // vertical input
                if(Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
                {
                    desiredSpeed = new Vector2(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
                    if(CheckCollision(new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical")), movableMask) == false)
                    {
                        speed += new Vector2(0, moveSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime);
                    }
                    isMoving = true;
                    lastMovement = new Vector2(0f, Input.GetAxisRaw("Vertical"));
                }

                // horizontal input - may or may not be vertical input already
                if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
                {
                    desiredSpeed = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f);
                    if (CheckCollision(new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), 0f), movableMask) == false)
                    {
                        speed += new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime, 0f);
                    }
                    isMoving = true;
                    lastMovement = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
                }

                //RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + sideHitFloat, transform.position.y + topHitFloat, transform.position.z), speed, 0.5f, mask);
                //RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - sideHitFloat, transform.position.y - bottomHitFloat, transform.position.z), speed, 0.5f, mask);
                
                // if not colliding with object, move
                if (speed != Vector2.zero && !CheckCollision(speed, mask))
                {
                    // normalize speed and mult by moveSpeed to prevent super fastness
                    speed.Normalize();
                    speed *= moveSpeed * Time.deltaTime;

                    transform.Translate(speed);
                    desiredSpeed = Vector2.zero;
                }

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
                                bottomHit.transform.Translate(new Vector2(0, -moveSpeed * Input.GetAxisRaw("Vertical") / 2) * Time.deltaTime);
                                speed += new Vector2(0, -moveSpeed * Input.GetAxisRaw("Vertical")) * Time.deltaTime;
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
        anim.SetFloat("vSpeed", Input.GetAxisRaw("Vertical"));
        anim.SetFloat("hSpeed", Input.GetAxisRaw("Horizontal"));
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("lastHSpeed", lastMovement.x);
        anim.SetFloat("lastVSpeed", lastMovement.y);
    }

}
