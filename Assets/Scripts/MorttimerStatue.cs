using UnityEngine;
using System.Collections;

public class MorttimerStatue : OverworldObject {

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {

    }

	void FixedUpdate () {
        distFromPlayer = Mathf.Abs(Mathf.Sqrt(((transform.position.x - player.position.x) * (transform.position.x - player.position.x))
                + ((transform.position.y - player.position.y) * (transform.position.y - player.position.y))));
	}
}
