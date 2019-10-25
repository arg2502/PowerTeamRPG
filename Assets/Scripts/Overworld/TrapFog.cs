using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFog : MonoBehaviour {

	public bool isHorizontal = false; // determines which direction to send player on respawn
	public TrapFogRespawn respawnPoint = null;

	void OnTriggerStay2D(Collider2D _other){
		if (_other.tag == "Player") {
			//move the player back to the respawn point
			//probably can be something fancy, but for now we'll just set the position
			if(respawnPoint != null){
				if(isHorizontal){
					_other.gameObject.transform.position = new Vector3(respawnPoint.transform.position.x,
					                                                   _other.transform.position.y, 0f);
				}else{
					_other.gameObject.transform.position = new Vector3(_other.transform.position.x,
					                                                   respawnPoint.transform.position.y, 0f);
				}
			}
		}
	}
}
