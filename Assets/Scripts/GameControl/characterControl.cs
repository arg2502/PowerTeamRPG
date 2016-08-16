using UnityEngine;
using System.Collections;

public class characterControl : OverworldObject {
    float moveSpeed = 0;
    float walkSpeed = 200;
    float runSpeed = 300;
    Vector2 speed;
    public Vector3 lastPos;
	// Use this for initialization
	void Start () {
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
            speed += new Vector2(0, moveSpeed) * Time.deltaTime;
            //transform.Translate( speed );
        }
        if (Input.GetKey(KeyCode.S))
        {
            speed += new Vector2(0, -moveSpeed) * Time.deltaTime;
            //transform.Translate(speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            speed += new Vector2(-moveSpeed, 0) * Time.deltaTime;
           // transform.Translate(new Vector2(-moveSpeed, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            speed += new Vector2(moveSpeed, 0) * Time.deltaTime;
            //transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, speed, 30.0f);
        if (hit.collider == null)
        {
            transform.Translate(speed);
        }
    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    lastPos = transform.position - new Vector3(2* speed.x, 2* speed.y, 0f);
    //}

    //void OnCollisionStay2D(Collision2D col)
    //{
    //    print("Collision");
    //    //transform.Translate(new Vector2(0, -moveSpeed) * Time.deltaTime);
    //    transform.position = lastPos;
    //}
}
