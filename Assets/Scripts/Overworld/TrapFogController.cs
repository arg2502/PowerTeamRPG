using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFogController : MonoBehaviour {

	public TrapFog[] arrayOfFog;
	public TrapFogRespawn[] arrayOfRespawn;
	// Use this for initialization
	void Start () {
		arrayOfFog = GetComponentsInChildren<TrapFog> ();
		arrayOfRespawn = GetComponentsInChildren<TrapFogRespawn> ();
		foreach (var respawn in arrayOfRespawn) {
			respawn.controller = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
