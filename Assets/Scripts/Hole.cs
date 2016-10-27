using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hole : OverworldObject {

    // attributes
    // store only the blocks relevant to this hole, so that we aren't checking the
    // distance between this hole and every block in existance
    public List<MovableOverworldObject> relevantBlocks;

    public bool isFull; // checks whether a block is in the hole or not

    float distFromBlock;

	// Use this for initialization
	void Start () {
        base.Start();
        sr.sortingOrder = -9999;
	}
	
	// Update is called once per frame
	void Update () {
        // if a block gets close enough, deactivate the block, and the hole collider
        foreach (MovableOverworldObject m in relevantBlocks)
        {
            distFromBlock = Mathf.Abs(Mathf.Sqrt(((transform.position.x - m.transform.position.x) * (transform.position.x - m.transform.position.x))
            + ((transform.position.y - m.transform.position.y) * (transform.position.y - m.transform.position.y))));

            if (distFromBlock < 30) { isFull = true; GetComponent<BoxCollider2D>().enabled = false; sr.color = Color.white; m.gameObject.SetActive(false); }
        }
        // also change the hole's sprite to look like a block is filling it
	}
}
