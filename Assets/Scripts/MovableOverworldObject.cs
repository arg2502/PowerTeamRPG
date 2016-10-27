﻿using UnityEngine;
using System.Collections;

public class MovableOverworldObject : OverworldObject {

    public float collisionOffset;
    public bool isActivated;

    void Start()
    {
        base.Start();
        if (!isActivated) { gameObject.SetActive(false); }
    }

    //check collisions
    public bool CheckCollisions(Vector3 dir)
    {
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector3(transform.position.x + collisionOffset, transform.position.y, transform.position.z), dir, 8.0f, mask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x - collisionOffset, transform.position.y, transform.position.z), dir, 8.0f, mask);
        RaycastHit2D hit3 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - collisionOffset - 8.0f, transform.position.z), dir, 8.0f, mask);
        RaycastHit2D hit4 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + collisionOffset, transform.position.z), dir, 8.0f, mask);
        RaycastHit2D hit5 = Physics2D.Raycast(new Vector3(transform.position.x + collisionOffset, transform.position.y + collisionOffset, transform.position.z), dir, 8.0f, mask);
        RaycastHit2D hit6 = Physics2D.Raycast(new Vector3(transform.position.x - collisionOffset, transform.position.y - collisionOffset - 8.0f, transform.position.z), dir, 8.0f, mask);
        RaycastHit2D hit7 = Physics2D.Raycast(new Vector3(transform.position.x + collisionOffset, transform.position.y - collisionOffset - 8.0f, transform.position.z), dir, 8.0f, mask);
        RaycastHit2D hit8 = Physics2D.Raycast(new Vector3(transform.position.x - collisionOffset, transform.position.y + collisionOffset, transform.position.z), dir, 8.0f, mask);

        if ((hit1.collider != null && hit1.collider != this.GetComponent<Collider2D>()) || (hit2.collider != null && hit2.collider != this.GetComponent<Collider2D>())
            || (hit3.collider != null && hit3.collider != this.GetComponent<Collider2D>()) || (hit4.collider != null && hit4.collider != this.GetComponent<Collider2D>())) { return true; }
        else if((hit5.collider != null && hit5.collider != this.GetComponent<Collider2D>()) || (hit6.collider != null && hit6.collider != this.GetComponent<Collider2D>())
            || (hit7.collider != null && hit7.collider != this.GetComponent<Collider2D>()) || (hit8.collider != null && hit8.collider != this.GetComponent<Collider2D>())) { return true; }
        else { return false; }
    }
	// Update is called once per frame
	void Update () {
        if (isActivated)
        {
            //gameObject.SetActive(true);
            sr.sortingOrder = (int)-transform.position.y;
        }
	}
}
