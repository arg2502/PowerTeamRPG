using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hole : OverworldObject {

    // attributes
    // store only the blocks relevant to this hole, so that we aren't checking the
    // distance between this hole and every block in existance
    public List<MovableOverworldObject> relevantBlocks;

    public bool isFull; // checks whether a block is in the hole or not
    float timer = 0.0f; // a timer to make sure the collider disappears when the block is fully in the hole

    public float distFromBlock;

    Animator anim;

	// Use this for initialization
	void Start () {
        base.Start();
        anim = GetComponent<Animator>();
        sr.sortingOrder = -9799;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isFull)
        { 
            // if a block gets close enough, deactivate the block, and the hole collider
            foreach (MovableOverworldObject m in relevantBlocks)
            {
                distFromBlock = Mathf.Abs(Mathf.Sqrt(((transform.position.x - m.transform.position.x) * (transform.position.x - m.transform.position.x))
                + ((transform.position.y - (m.transform.position.y - 30.0f)) * (transform.position.y - (m.transform.position.y - 30.0f)))));

                if (distFromBlock < 80) { isFull = true; anim.SetBool("isFull", true); m.gameObject.SetActive(false); }
            }
        }
        

        if (isFull) 
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f) { GetComponent<Collider2D>().enabled = false; }
        }
	}
}
