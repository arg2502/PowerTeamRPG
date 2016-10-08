using UnityEngine;
using System.Collections;

public class characterControl : OverworldObject {
    float moveSpeed = 0;
    float walkSpeed = 200;
    float runSpeed = 360;
    public Vector2 speed;
	public bool canMove;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        transform.position = GameControl.control.currentPosition; // this is just temporary, as the final version will have to be more nuanced
		canMove = true;
        base.Start();
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        sr.sortingOrder = (int)-transform.position.y;
        speed = new Vector2(0f, 0f);

		if (canMove) {
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
				moveSpeed = runSpeed;
			} else {
				moveSpeed = walkSpeed;
			}

			if (Input.GetKey (KeyCode.W)) {
				speed += new Vector2 (0, moveSpeed) * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.S)) {
				speed += new Vector2 (0, -moveSpeed) * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.A)) {
				speed += new Vector2 (-moveSpeed, 0) * Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.D)) {
				speed += new Vector2 (moveSpeed, 0) * Time.deltaTime;
			}
			RaycastHit2D topHit = Physics2D.Raycast (new Vector3 (transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), speed, 32.0f);
			RaycastHit2D bottomHit = Physics2D.Raycast (new Vector3 (transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), speed, 32.0f);
			if (topHit.collider == null && bottomHit.collider == null) {
				transform.Translate (speed);
			}
            else if (topHit.collider != null && topHit.collider.tag == "Movable")
            {
                if (transform.position.y - 24.0f + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset <= topHit.transform.position.y) // up - W
                {
                    if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, moveSpeed / 2)) == false)
                    {
                        topHit.transform.Translate(new Vector2(0, moveSpeed / 2) * Time.deltaTime);
                    }
                }
                else if (transform.position.x + topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset <= topHit.transform.position.x) // right - D
                {
                    if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(moveSpeed / 2, 0)) == false)
                    {
                        topHit.transform.Translate(new Vector2(moveSpeed / 2, 0) * Time.deltaTime);
                    }
                }
                else if (transform.position.y + 24.0f - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset >= topHit.transform.position.y) // down - S
                {
                    if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, -moveSpeed / 2)) == false)
                    {
                        topHit.transform.Translate(new Vector2(0, -moveSpeed / 2) * Time.deltaTime);
                    }
                }
                else if (transform.position.x - topHit.collider.GetComponent<MovableOverworldObject>().collisionOffset >= topHit.transform.position.x) // left - A
                {
                    if (topHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(-moveSpeed / 2, 0)) == false)
                    {
                        topHit.transform.Translate(new Vector2(-moveSpeed / 2, 0) * Time.deltaTime);
                    }
                }

                topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), speed, 32.0f);
                bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), speed, 32.0f);
                if (topHit.collider == null && bottomHit.collider == null) { transform.Translate(speed/2); }
            }
            else if (bottomHit.collider != null && bottomHit.collider.tag == "Movable")
            {
                if (transform.position.y + 24.0f - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset >= bottomHit.transform.position.y) // down - S
                {
                    if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, -moveSpeed / 2)) == false)
                    {
                        bottomHit.transform.Translate(new Vector2(0, -moveSpeed / 2) * Time.deltaTime);
                    }
                }
                else if (transform.position.x - bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset >= bottomHit.transform.position.x) // left - A
                {
                    if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(-moveSpeed / 2, 0)) == false)
                    {
                        bottomHit.transform.Translate(new Vector2(-moveSpeed / 2, 0) * Time.deltaTime);
                    }
                }
                else if (transform.position.y - 24.0f + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset <= bottomHit.transform.position.y) // up - W
                {
                    if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(0, moveSpeed / 2)) == false)
                    {
                        bottomHit.transform.Translate(new Vector2(0, moveSpeed / 2) * Time.deltaTime);
                    }
                }
                else if (transform.position.x + bottomHit.collider.GetComponent<MovableOverworldObject>().collisionOffset <= bottomHit.transform.position.x) // right - D
                {
                    if (bottomHit.collider.GetComponent<MovableOverworldObject>().CheckCollisions(new Vector2(moveSpeed / 2, 0)) == false)
                    {
                        bottomHit.transform.Translate(new Vector2(moveSpeed / 2, 0) * Time.deltaTime);
                    }
                }

                topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y - 32.0f, transform.position.z), speed, 32.0f);
                bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 48.0f, transform.position.z), speed, 32.0f);
                if (topHit.collider == null && bottomHit.collider == null) { transform.Translate(speed/2); }
            }
		}
        //flip the horizontal walking sprite based on speed
        if (speed.x < 0 && transform.localScale.x < 0) { transform.localScale = new Vector3(1, 1, 1); }
        else if (speed.x > 0 && transform.localScale.x > 0) { transform.localScale = new Vector3(-1, 1, 1); }

        anim.SetFloat("vSpeed", speed.y);
        anim.SetFloat("hSpeed", Mathf.Abs(speed.x));
    }

}
