using UnityEngine;
using System.Collections;

public class OverworldObject : MonoBehaviour {

    protected SpriteRenderer sr;

	// Use this for initialization
	protected void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)-transform.position.y;
	}
}
