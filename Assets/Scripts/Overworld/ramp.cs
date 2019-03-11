using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ramp : MonoBehaviour {

	[Header("Ramps must have a 1x to 2y Scale")]
	//going up and to the left or right?
	public bool isRight = false;

	void OnTriggerEnter2D (Collider2D _other){
		if (_other.gameObject.GetComponent<characterControl> () != null) {
			_other.gameObject.GetComponent<characterControl> ().onRamp = true;
			_other.gameObject.GetComponent<characterControl> ().isRampRight = isRight;
		}
	}

	void OnTriggerStay2D (Collider2D _other){
		if (_other.gameObject.GetComponent<characterControl> () != null) {
			_other.gameObject.GetComponent<characterControl> ().onRamp = true;
		}
	}

	void OnTriggerExit2D (Collider2D _other){
		if (_other.gameObject.GetComponent<characterControl> () != null) {
			_other.gameObject.GetComponent<characterControl> ().onRamp = false;
		}
	}
}
