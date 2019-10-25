using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFogRespawn : MonoBehaviour {

	public TrapFogController controller = null;
	public bool isHorizontal = false;

	//this method will make sure that the player respawns at whichever side they approached the fog from
	void OnTriggerEnter2D(Collider2D _other){
		if (_other.tag == "Player") {
			foreach (var ffog in controller.arrayOfFog) {
				ffog.respawnPoint = this;
				ffog.isHorizontal = isHorizontal;
			}
		}
	}
}
