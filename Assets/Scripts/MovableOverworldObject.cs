using UnityEngine;
using System.Collections;

public class MovableOverworldObject : OverworldObject {

    public bool isActivated; //whether or not a toggle has caused the object to appear
	public bool isCarried = false; //whether or not Jethro is carrying the object
         
    public float customWeight = 0f;
    private float objectWeight;
    public float ObjectWeight { get { return objectWeight; } }
    
    new void Start()
    {		
        base.Start();
        GetComponent<SpriteRenderer>().enabled = isActivated; 
		GetComponent<BoxCollider2D> ().enabled = isActivated;

        // assigning weight to movable object
        if (weightClass == Weight.NORMAL)
            objectWeight = NormalWeight;
        else if (weightClass == Weight.LIGHT)
            objectWeight = LightWeight;
        else if (weightClass == Weight.HEAVY)
            objectWeight = HeavyWeight;
        else
            objectWeight = customWeight;

    }

	// Update is called once per frame
	void Update () {
		if (isActivated && !isCarried) // this pertains to whether or not a switch has caused the object to appear
        {
            sr.sortingOrder = (int)(-transform.position.y * 10.0f);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<FloorSwitch>())
        {
            other.GetComponent<FloorSwitch>().PressSwitch(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<FloorSwitch>())
        {
            other.GetComponent<FloorSwitch>().UndoSwitch(this);
        }
    }
}
