using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_JailDoor : InteractiveObject {

	public float travelDistance = 1.0f; // how far we want the door to move
	public float travelSpeed = 2.0f; // how fast we want the door to move
	float travelTime = 0.0f;
	float openPosX;
	float closePosX;

	private void Start () {
		base.Start ();
		notificationMessage = "Open"; // notification message always remains as it is set here?

		closePosX = gameObject.transform.position.x;
		openPosX = gameObject.transform.position.x + travelDistance;
	}

	public Coroutine moving; // for referencing active coroutines
	bool isMoving{ get { return moving != null; } } // return whether or not a coroutine is active

	public IEnumerator MoveDoor (float _currentPos, float _targetPos){
		while (travelTime <= 1.0f) {
			travelTime += Time.deltaTime * travelSpeed;

			transform.position = Vector2.Lerp(new Vector2(_currentPos, transform.position.y),
			                                  new Vector2(_targetPos, transform.position.y), travelTime);
			yield return null;
		}

		StopMoving ();
	}

	public void StopMoving(){
		if (isMoving) {
			StopCoroutine(moving);
			moving = null;
			travelTime = 0.0f;
		}
	}

	public override void PerformAction(){
		if (!isMoving) {
			if(transform.position.x == openPosX){				
                notificationMessage = "Open";
                HideInteractionNotification();
                moving = StartCoroutine (MoveDoor(openPosX, closePosX));
			}
			else if (transform.position.x == closePosX){
                notificationMessage = "Close";
                HideInteractionNotification();
                moving = StartCoroutine (MoveDoor(closePosX, openPosX));
			}
		}
	}
}
