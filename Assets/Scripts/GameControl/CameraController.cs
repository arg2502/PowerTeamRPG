using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject followTarget;
    Vector3 targetPos;
    public float moveSpeed;

    // jump immediately to player position at beginning
    void Start()
    {
        transform.position = new Vector3(GameControl.control.currentPosition.x, GameControl.control.currentPosition.y, transform.position.z);
    }

	// Update is called once per frame
	void LateUpdate () {
        targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);	
	}
}
