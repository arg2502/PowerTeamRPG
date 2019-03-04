using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour {

	public float moveSpeed;
	public float collisionOffset;
	public float maxDisplacement;
	private Vector2 originalPos;
	private bool inPosition = true;
	private float startTime;
	public float offsetTime;
	private float displacementDistance;
	public List<Sprite> variants = new List<Sprite>();

	// Use this for initialization
	void Start () {
		//record where the flower starts
		originalPos = transform.position;
		int r = Random.Range (0, 3);
		gameObject.GetComponent<SpriteRenderer>().sprite = variants[r];
	}
	
	// Update is called once per frame
	void Update () {
		if (!inPosition && Time.time - startTime >= 0.0f) {
			//do the lerp thing
			float distCovered = (Time.time - startTime) * moveSpeed;
			float fracJourney = distCovered / displacementDistance;
			if(fracJourney >= 1.0f){
				fracJourney = 1.0f;
				inPosition = true;
			}
			Vector2 oldPos = transform.position;
			transform.position = Vector2.Lerp(transform.position, originalPos, fracJourney);
		}
	}

	void OnTriggerStay2D(Collider2D _other){
		inPosition = true;
		if (_other.transform.position.x >= this.transform.position.x &&
		    this.transform.position.x > originalPos.x - maxDisplacement) {
			this.transform.position = new Vector2((transform.position.x - moveSpeed * Time.deltaTime), transform.position.y);
		}
		else if (_other.transform.position.x <= this.transform.position.x &&
		    this.transform.position.x < originalPos.x + maxDisplacement) {
			this.transform.position = new Vector2((transform.position.x + moveSpeed * Time.deltaTime), transform.position.y);
		}
		if (_other.transform.position.y >= this.transform.position.y + collisionOffset &&
		    this.transform.position.y > originalPos.y - maxDisplacement) {
			this.transform.position = new Vector2(transform.position.x, (transform.position.y - moveSpeed * Time.deltaTime));
		}
		else if (_other.transform.position.y <= this.transform.position.y + collisionOffset &&
		         this.transform.position.y < originalPos.y + maxDisplacement) {
			this.transform.position = new Vector2(transform.position.x, (transform.position.y + moveSpeed * Time.deltaTime));
		}
	}

	void OnTriggerExit2D(Collider2D _other){
		inPosition = false;
		startTime = Time.time + offsetTime;
		displacementDistance = Vector2.Distance (transform.position, originalPos);
	}
}
