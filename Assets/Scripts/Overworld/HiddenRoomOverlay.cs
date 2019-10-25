using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenRoomOverlay : MonoBehaviour {

	public SpriteRenderer sr; // stores obj's sprite renderer
	bool isRevealed = false;
	float fadeTime = 0.0f;
	Color tmp; // stores temporary color to allow alteration of sr.color
	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>();
		tmp = sr.color;
		if (isRevealed) {
			tmp.a = 0f;
		} else {
			tmp.a = 1f;
		}
		sr.color = tmp;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartFading(bool _isReveal, float _speed){
		isRevealed = _isReveal;
		StopFading ();
		if (isRevealed && sr.color.a != 0f) {
			fading = StartCoroutine (Fade (sr.color.a, 0f, _speed));
		} else if (!isRevealed && sr.color.a != 1.0f) {
			fading = StartCoroutine (Fade (sr.color.a, 1f, _speed));
		}
	}

	public Coroutine fading; // for referencing active coroutines
	bool isFading{ get { return fading != null; } } // return whether or not a coroutine is active

	public IEnumerator Fade(float _oldColor, float _newColor, float _speed){

		//fadeTime = _oldColor;

		while (fadeTime <= 1.0f) {
			fadeTime += Time.deltaTime * _speed;
			
			tmp = sr.color;
			tmp.a = Mathf.Lerp (_oldColor, _newColor, fadeTime);

			sr.color = tmp;
			yield return null;
		}

		StopFading ();
	}

	public void StopFading(){
		if (isFading) {
			StopCoroutine(fading);
			fading = null;
			fadeTime = 0f;
		}
	}
}
