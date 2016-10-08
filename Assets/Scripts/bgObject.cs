using UnityEngine;
using System.Collections;

public class bgObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sortingOrder = -10000;
	}
}
