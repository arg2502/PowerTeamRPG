using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableShadow : MonoBehaviour {
	protected SpriteRenderer sr; // stores shadow's sprite renderer
	protected SpriteRenderer psr; // stores parent's sprite renderer
	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>();
		psr = transform.parent.GetComponent<SpriteRenderer>();

		sr.sortingOrder = psr.sortingOrder - 1;
	}
	
	// Update is called once per frame
	void Update () {
		sr.sortingOrder = psr.sortingOrder - 1;
	}
}
