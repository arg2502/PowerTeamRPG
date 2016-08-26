using UnityEngine;
using System.Collections;

public class characterControl : OverworldObject {
    float moveSpeed = 0;
    float walkSpeed = 200;
    float runSpeed = 300;
    public Vector2 speed;
    //public Vector3 lastPos;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        transform.position = GameControl.control.currentPosition; // this is just temporary, as the final version will have to be more nuanced
        base.Start();
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        sr.sortingOrder = (int)-transform.position.y;
        speed = new Vector2(0f, 0f);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { moveSpeed = runSpeed; }
        else { moveSpeed = walkSpeed; }

        if (Input.GetKey(KeyCode.W))
        {
            speed = new Vector2(0, moveSpeed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            speed = new Vector2(0, -moveSpeed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            speed = new Vector2(-moveSpeed, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            speed = new Vector2(moveSpeed, 0) * Time.deltaTime;
        }
        RaycastHit2D topHit = Physics2D.Raycast(new Vector3(transform.position.x + 15.0f, transform.position.y + 5.0f, transform.position.z), speed, 30.0f);
        RaycastHit2D bottomHit = Physics2D.Raycast(new Vector3(transform.position.x - 15.0f, transform.position.y - 10.0f, transform.position.z), speed, 30.0f);
        if (topHit.collider == null && bottomHit.collider == null)
        {
            transform.Translate(speed);
        }

        //flip the horizontal walking sprite based on speed
        if (speed.x < 0 && transform.localScale.x < 0) { transform.localScale = new Vector3(1, 1, 1); }
        else if (speed.x > 0 && transform.localScale.x > 0) { transform.localScale = new Vector3(-1, 1, 1); }

        anim.SetFloat("vSpeed", speed.y);
        anim.SetFloat("hSpeed", Mathf.Abs(speed.x));
    }

}
