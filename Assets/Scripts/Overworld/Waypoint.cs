using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public BoxCollider2D box; //Store our boxCollider for easy access
	public List<Waypoint> adjacentPoints; //any waypoints adjacent to this waypoint

	// Use this for initialization
	void Start () {

		//get our box collider so NPCs can reference it easily
		box = GetComponent<BoxCollider2D> (); 
	}
}
