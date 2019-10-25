using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenRoomTrigger : MonoBehaviour {

	public float transitionSpeed = 1.0f;
	public List<HiddenRoomOverlay> listOfOverlays;
	public bool isExit = false;

	void OnTriggerExit2D(Collider2D _other){
		if (_other.tag == "Player") {
			if(isExit){
				foreach( var overlay in listOfOverlays)
				{
					//overlay.isRevealed = false;
					overlay.StartFading(false, transitionSpeed);
				}
			}
			else
			{
				foreach( var overlay in listOfOverlays)
				{
					//overlay.isRevealed = true;
					overlay.StartFading(true, transitionSpeed);
				}
			}
		}
	}
}
