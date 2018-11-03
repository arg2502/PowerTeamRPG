using UnityEngine;
using System.Collections;

public class MovableOverworldObject : OverworldObject {

    public float collisionOffset; //probably delet
    public bool isActivated; //whether or not a toggle has caused the object to appear
	public bool isCarried = false; //whether or not Jethro is carrying the object
    public float raycastDist = 0.125f; //probably delet

    void Start()
    {		
        base.Start();
        GetComponent<SpriteRenderer>().enabled = isActivated; 
		GetComponent<BoxCollider2D> ().enabled = isActivated;
    }

	// Update is called once per frame
	void Update () {
		//GetComponent<SpriteRenderer>().enabled = isActivated; 
		//GetComponent<BoxCollider2D> ().enabled = isActivated;
		if (isActivated && !isCarried) // this pertains to whether or not a switch has caused the object to appear
        {
            //gameObject.SetActive(true);
            sr.sortingOrder = (int)-transform.position.y;
        }
	}
}
