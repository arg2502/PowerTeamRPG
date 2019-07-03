using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoleSwitch : MonoBehaviour {

    public List<Hole> connectedHoles;
    int numOfFilledHoles;
	bool isActivated;

	// Update is called once per frame
	void Update () {
		if (!isActivated) {
			// reset every update
			numOfFilledHoles = 0;

			// check if all the holes are full
			foreach (Hole h in connectedHoles) {
				if (h.isFull) {
					numOfFilledHoles++;
				}
			}

			// if all the holes are filled, unlock door
			if (numOfFilledHoles >= connectedHoles.Count) {
				gameObject.GetComponent<SpriteRenderer> ().enabled = !gameObject.GetComponent<SpriteRenderer> ().enabled;
				gameObject.GetComponent<BoxCollider2D> ().enabled = !gameObject.GetComponent<BoxCollider2D> ().enabled;
				isActivated = true;
			}
		}
	}
}
