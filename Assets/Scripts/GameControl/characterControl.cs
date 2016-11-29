using UnityEngine;
using System.Collections;

public class characterControl : OverworldObject {
    float moveSpeed = 0;
    float walkSpeed = 200;
    float runSpeed = 360;
    public Vector2 speed;
    public Vector2 desiredSpeed;
    PauseMenu pm;
	//public bool canMove;

    public LayerMask movableMask;

    RaycastHit2D topHitCheck;
    RaycastHit2D bottomHitCheck;

    Animator anim;

	// Use this for initialization
	void Start () {

        pm = GameObject.FindObjectOfType<PauseMenu>();

        anim = GetComponent<Animator>();
        transform.position = GameControl.control.currentPosition; // this is just temporary, as the final version will have to be more nuanced
		canMove = true;
        base.Start();
	}

    public bool CheckCollision(Vector2 dir)
    {
        topHitCheck = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), dir, 32.0f, movableMask);
        bottomHitCheck = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), dir, 32.0f, movableMask);
        if (topHitCheck.collider == null && bottomHitCheck.collider == null) { return false; }
        else { return true; }
    }
	
	// Update is called once per frame
    void FixedUpdate()
    {
        sr.sortingOrder = (int)-transform.position.y;
        speed = new Vector2(0f, 0f);
        //desiredSpeed = Vector2.zero;

		if (canMove) {
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
				moveSpeed = runSpeed;
			} else {
				moveSpeed = walkSpeed;
			}

            // If not pushing or pulling
            if (!Input.GetKey(KeyCode.Space))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    desiredSpeed = new Vector2(0, moveSpeed) * Time.deltaTime;
                    if (CheckCollision(new Vector2(0, moveSpeed)) == false) { speed += new Vector2(0, moveSpeed) * Time.deltaTime; }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    desiredSpeed = new Vector2(0, -moveSpeed) * Time.deltaTime;
                    if (CheckCollision(new Vector2(0, -moveSpeed)) == false) { speed += new Vector2(0, -moveSpeed) * Time.deltaTime; }
                }

                if (Input.GetKey(KeyCode.A))
                {
                    desiredSpeed = new Vector2(-moveSpeed, 0) * Time.deltaTime;
                    if (CheckCollision(new Vector2(-moveSpeed, 0)) == false) { speed += new Vector2(-moveSpeed, 0) * Time.deltaTime; }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    desiredSpeed = new Vector2(moveSpeed, 0) * Time.deltaTime;
                    if (CheckCollision(new Vector2(moveSpeed, 0)) == false) { speed += new Vector2(moveSpeed, 0) * Time.deltaTime; }
                }
                RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), speed, 32.0f, mask);
                RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), speed, 32.0f, mask);
                if (topHit.collider == null && bottomHit.collider == null && speed != Vector2.zero)
                {
                    transform.Translate(speed);
                    desiredSpeed = Vector2.zero;
                }
            }
			
            // If pushing or pulling
            if (Input.GetKey(KeyCode.Space))
            {
                RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), desiredSpeed, 32.0f, mask);
                RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), desiredSpeed, 32.0f, mask);
                if (topHit.collider != null && topHit.collider.tag == "Movable")
                {
                    if (//transform.position.y <= topHit.transform.position.y &&
                        !((transform.position.x > topHit.transform.position.x + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.x < topHit.transform.position.x - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.W))) // up - W
                    {
                        if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, moveSpeed / 2)) == false)
                        {
                            if (!CheckCollision(new Vector2(0, moveSpeed) * Time.deltaTime))
                            {
                                topHit.transform.Translate(new Vector2(0, moveSpeed / 2) * Time.deltaTime);
                                speed += new Vector2(0, moveSpeed) * Time.deltaTime;
                            }
                        }
                    }
                    else if (//transform.position.y >= topHit.transform.position.y &&
                        !((transform.position.x > topHit.transform.position.x + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.x < topHit.transform.position.x - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.S))) // down - S
                    {
                        if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, -moveSpeed / 2)) == false)
                        {
                            if (!CheckCollision(new Vector2(0, -moveSpeed) * Time.deltaTime)) 
                            {
                                topHit.transform.Translate(new Vector2(0, -moveSpeed / 2) * Time.deltaTime);
                                speed += new Vector2(0, -moveSpeed) * Time.deltaTime;
                            }
                        }
                    }

                    else if (//transform.position.x <= topHit.transform.position.x &&
                        !((transform.position.y > topHit.transform.position.y + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.y < topHit.transform.position.y - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.D))) // right - D
                    {
                        if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(moveSpeed / 2, 0)) == false)
                        {
                            if (!CheckCollision(new Vector2(moveSpeed, 0) * Time.deltaTime))
                            {
                                topHit.transform.Translate(new Vector2(moveSpeed / 2, 0) * Time.deltaTime);
                                speed += new Vector2(moveSpeed, 0) * Time.deltaTime;
                            }
                        }
                    }
                    else if (//transform.position.x >= topHit.transform.position.x &&
                        !((transform.position.y > topHit.transform.position.y + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.y < topHit.transform.position.y - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.A))) // left - A
                    {
                        if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(-moveSpeed / 2, 0)) == false)
                        {
                            if (!CheckCollision(new Vector2(-moveSpeed, 0) * Time.deltaTime))
                            {
                                topHit.transform.Translate(new Vector2(-moveSpeed / 2, 0) * Time.deltaTime);
                                speed += new Vector2(-moveSpeed, 0) * Time.deltaTime;
                            }
                        }
                    }

                    topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), speed, 32.0f, mask);
                    bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), speed, 32.0f, mask);
                    if (topHit.collider == null && bottomHit.collider == null) { transform.Translate(speed / 2); }
                }
                else if (bottomHit.collider != null && bottomHit.collider.tag == "Movable")
                {
                    if (//transform.position.y >= bottomHit.transform.position.y &&
                        !((transform.position.x > bottomHit.transform.position.x + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.x < bottomHit.transform.position.x - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.S))) // down - S
                    {
                        if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, -moveSpeed / 2)) == false)
                        {
                            bottomHit.transform.Translate(new Vector2(0, -moveSpeed / 2) * Time.deltaTime);
                            speed += new Vector2(0, -moveSpeed) * Time.deltaTime;
                        }
                    }
                    else if (//transform.position.y <= bottomHit.transform.position.y &&
                        !((transform.position.x > bottomHit.transform.position.x + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.x < bottomHit.transform.position.x - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.W))) // up - W
                    {
                        if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, moveSpeed / 2)) == false)
                        {
                            bottomHit.transform.Translate(new Vector2(0, moveSpeed / 2) * Time.deltaTime);
                            speed += new Vector2(0, moveSpeed) * Time.deltaTime;
                        }
                    }

                    else if (//transform.position.x >= bottomHit.transform.position.x &&
                        !((transform.position.y > bottomHit.transform.position.y + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.y < bottomHit.transform.position.y - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.A))) // left - A
                    {
                        if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(-moveSpeed / 2, 0)) == false)
                        {
                            bottomHit.transform.Translate(new Vector2(-moveSpeed / 2, 0) * Time.deltaTime);
                            speed += new Vector2(-moveSpeed, 0) * Time.deltaTime;
                        }
                    }
                    else if (//transform.position.x <= bottomHit.transform.position.x &&
                        !((transform.position.y > bottomHit.transform.position.y + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset) ||
                        (transform.position.y < bottomHit.transform.position.y - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset))
                        && (Input.GetKey(KeyCode.D))) // right - D
                    {
                        if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(moveSpeed / 2, 0)) == false)
                        {
                            bottomHit.transform.Translate(new Vector2(moveSpeed / 2, 0) * Time.deltaTime);
                            speed += new Vector2(moveSpeed, 0) * Time.deltaTime; 
                        }
                    }

                    topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), speed, 32.0f, mask);
                    bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), speed, 32.0f, mask);
                    if (topHit.collider == null && bottomHit.collider == null) { transform.Translate(speed / 2); }
                }
            }
		}

        anim.SetFloat("vSpeed", speed.y);
        anim.SetFloat("hSpeed", Mathf.Abs(speed.x));
        if (speed.y == 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Jethro_OWalkSide"))
        {
            //flip the horizontal walking sprite based on speed
            if (speed.x < 0 && transform.localScale.x < 0 /*&& anim.GetCurrentAnimatorStateInfo(0).IsName("Jethro_OWalkSide")*/) { transform.localScale = new Vector3(1, 1, 1); }
            else if (speed.x > 0 && transform.localScale.x > 0 /*&& anim.GetCurrentAnimatorStateInfo(0).IsName("Jethro_OWalkSide")*/) { transform.localScale = new Vector3(-1, 1, 1); }
        }
    }

}
