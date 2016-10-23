using UnityEngine;
using System.Collections;

public class FontSort : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().sortingOrder = 9900;
	}

    void Update()
    {
        gameObject.GetComponent<Renderer>().sortingOrder = 9900;
    }
}
