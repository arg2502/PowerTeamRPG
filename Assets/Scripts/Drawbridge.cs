using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawbridge : OverworldObject {

    float height = 500.0f;
	float difference = 125.0f;
    float initialPos;
    public List<GameObject> colliders;
	public bool isActive;

    void Start()
    {
        base.Start();
        sr.sortingOrder = (int)-transform.position.y - 2000;
        initialPos = transform.position.y;

		//if (isActive) {

    }
	void Update()
	{
		if(transform.position.y != initialPos && !isActive){
			print ("change position");
			isActive = true;
			transform.position = new Vector3 (transform.position.x, initialPos - height, transform.position.z);
			foreach (GameObject go in colliders) {
				if (go.activeSelf) {

					go.SetActive (false);
				}
			}
		}
	}
    public override void Activate()
    {
		isActive = true;
        StartCoroutine(LowerBridge());
    }
    public IEnumerator LowerBridge()
    {
        // if the bridge is not at it's designated location, keep moving it
        while (transform.position.y > initialPos - height)
        {
            transform.Translate(new Vector3(0.0f, -2.0f, 0.0f));
            yield return new WaitForSeconds(0.01f);

        }
        // when bridge is in location, deactivate colliders
        foreach(GameObject go in colliders)
        {
            go.SetActive(false);
        }


    }
}
