using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IARiserSword : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		GetComponentInParent<IARiser> ().HitTarget ();
	}
}
