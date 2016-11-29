using UnityEngine;
using System.Collections;

public class ColorBridge : OverworldObject {

    public bool isRed;
    public bool isBlue;
    public bool isMoving;
    float rotationDegrees1 = 0.0f;
    float rotationDegrees2 = 90.0f;
	Transform player;
	float width = 240.0f;
	float height = 332.0f;

    void Start()
    {
        base.Start();
        sr.sortingOrder = (int)-transform.position.y - 2000;
        isMoving = false;
		player = GameObject.FindObjectOfType<characterControl>().transform;
    }

    public override void Activate()
    {
        StartCoroutine(RotateBridge());
        isMoving = true;
    }
    IEnumerator RotateBridge()
    {
        if (transform.eulerAngles.z < rotationDegrees2)
        {
            while (transform.eulerAngles.z < rotationDegrees2)
            {
                transform.Rotate(Vector3.forward, 1.0f);
                //print(transform.eulerAngles.z);

				// is player on bridge
				if (player.transform.position.x > (transform.position.x - width / 2)
				   && player.transform.position.x < (transform.position.x + width / 2)
				   && player.transform.position.y > (transform.position.y - height / 2)
				   && player.transform.position.y < (transform.position.y + height / 2)) {
					//player.transform.RotateAround (transform.position, Vector3.forward, 1.0f);
					//player.transform.Rotate (Vector3.forward, -1.0f);
				}

                // set isMoving to false
                if (transform.eulerAngles.z == rotationDegrees2)
                {
                    isMoving = false;
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
        else if(transform.eulerAngles.z > rotationDegrees1)
        {
            while (transform.eulerAngles.z > rotationDegrees1)
            {
                transform.Rotate(Vector3.back, 1.0f);
                //print(transform.eulerAngles.z);

				// is player on bridge
				if (player.transform.position.x > (transform.position.x - width / 2)
					&& player.transform.position.x < (transform.position.x + width / 2)
					&& player.transform.position.y > (transform.position.y - height / 2)
					&& player.transform.position.y < (transform.position.y + height / 2)) {
					//player.transform.RotateAround (transform.position, Vector3.back, 1.0f);
					//player.transform.Rotate (Vector3.back, -1.0f);

				}

                // to compensate for the error of shifting the block's rotation upon start
                // set isMoving to false
                if(transform.eulerAngles.z > 358.0f 
                    || transform.eulerAngles.z < 2.0f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    isMoving = false;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
