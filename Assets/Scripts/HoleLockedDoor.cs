using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoleLockedDoor : MonoBehaviour {

    public List<Hole> connectedHoles;
    int numOfFilledHoles;

	// Update is called once per frame
	void Update () {
        // reset every update
        numOfFilledHoles = 0;

        // check if all the holes are full
        foreach(Hole h in connectedHoles)
        {
            if(h.isFull)
            {
                numOfFilledHoles++;
            }
        }

        // if all the holes are filled, unlock door
        if(numOfFilledHoles >= connectedHoles.Count)
        {
            gameObject.SetActive(false);
        }
	}
}
