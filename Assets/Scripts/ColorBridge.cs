using UnityEngine;
using System.Collections;

public class ColorBridge : OverworldObject {

    public bool isRed;
    public bool isBlue;
    public bool isMoving;
    float rotationDegrees1 = 0.0f;
    float rotationDegrees2 = 90.0f;
	Transform player;
	Transform bridgeTransform;
	float width = 240.0f;
	float height = 332.0f;
	roomControl room;

    void Start()
    {
        base.Start();
		bridgeTransform = gameObject.transform.Find ("Bridge").transform;
        sr.sortingOrder = (int)-transform.position.y - 2000;
		bridgeTransform.GetComponent<SpriteRenderer> ().sortingOrder = sr.sortingOrder - 1;
        isMoving = false;
		player = GameObject.FindObjectOfType<characterControl>().transform;

		room = GameObject.FindObjectOfType<roomControl> ();
    }
	void Update(){
		bridgeTransform = gameObject.transform.Find ("Bridge").transform;
	}
    public override void Activate()
    {
		isMoving = true;
		// freeze the player
		if(player.GetComponent<characterControl>().canMove){ToggleMovement();}
        StartCoroutine(RotateBridge());
        

    }
    IEnumerator RotateBridge()
    {
        if (bridgeTransform.eulerAngles.z < rotationDegrees2
			&& isMoving)
        {
			while (bridgeTransform.eulerAngles.z < rotationDegrees2)
            {
				bridgeTransform.Rotate(Vector3.forward, Time.deltaTime * 40.0f);
                //print(transform.eulerAngles.z);


                // set isMoving to false
				if (bridgeTransform.eulerAngles.z < rotationDegrees2 + 2.0f
					&& bridgeTransform.eulerAngles.z > rotationDegrees2 - 2.0f)
                {
					// reactivate player
					/*if (!player.GetComponent<characterControl> ().canMove) {
						ToggleMovement ();
						print ("toggle 1");
					}*/

					// reactivate enemies
					/*foreach(enemyControl e in room.enemies)
					{
						if (!e.GetComponent<enemyControl> ().canMove)
							ToggleMovement ();
					}*/
					bridgeTransform.rotation = Quaternion.Euler(0, 0, 90);
                    isMoving = false;
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
        else if(bridgeTransform.eulerAngles.z > rotationDegrees1
			&& isMoving)
        {
            while (bridgeTransform.eulerAngles.z > rotationDegrees1)
            {
				bridgeTransform.Rotate(Vector3.back, Time.deltaTime * 40.0f);
                //print(transform.eulerAngles.z);


                // to compensate for the error of shifting the block's rotation upon start
                // set isMoving to false
                if(bridgeTransform.eulerAngles.z > 358.0f 
                    || bridgeTransform.eulerAngles.z < 2.0f)
                {
					// reactivate player
					/*if (!player.GetComponent<characterControl> ().canMove) {
						ToggleMovement ();
						print ("toggle 2");
					}*/

                    bridgeTransform.rotation = Quaternion.Euler(0, 0, 0);
                    isMoving = false;

                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
