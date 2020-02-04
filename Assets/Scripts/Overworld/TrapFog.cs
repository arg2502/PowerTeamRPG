using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapFog : MonoBehaviour {

	public bool isHorizontal = false; // determines which direction to send player on respawn
	public TrapFogRespawn respawnPoint = null;
	public RawImage overlayCanvas;
	public Texture transitionEffect;
	public Texture colorTexture;
	public float speed;
	characterControl hero;

	void OnTriggerEnter2D(Collider2D _other){
		if (_other.tag == "Player") {
			//move the player back to the respawn point
			//probably can be something fancy, but for now we'll just set the position
			if(respawnPoint != null){
				hero = _other.GetComponent<characterControl>();
				hero.canMove = false;
				//StartCoroutine(Fade(false));
				StartFading(false);
			}
		}
	}

	public Coroutine fading; // for referencing active coroutines
	bool isFading{ get { return fading != null; } } // return whether or not a coroutine is active

	public IEnumerator Fade(bool show){
		float targVal = show ? 1 : 0;
		float curVal = show ? 0 : 1;

		//overlayCanvas.material.SetTexture ("_MainTex", colorTexture);
		overlayCanvas.material.color = Color.white;
		overlayCanvas.material.SetTexture ("_AlphaTex", transitionEffect);

		while (curVal != targVal) {
			curVal = Mathf.MoveTowards(curVal, targVal, speed * Time.deltaTime);
			overlayCanvas.material.SetFloat("_Cutoff", curVal);
			yield return null;
		}

		if (show == false) {
			if(isHorizontal){
				hero.gameObject.transform.position = new Vector3(respawnPoint.transform.position.x,
				                                                   hero.transform.position.y, 0f);
			}else{
				hero.gameObject.transform.position = new Vector3(hero.transform.position.x,
				                                                   respawnPoint.transform.position.y, 0f);
			}

			StartFading(true);
		}
		hero.canMove = show;
	}

	public void StartFading(bool show){
		StopFading ();
		fading = StartCoroutine(Fade(show));
	}

	public void StopFading(){
		if (isFading) {
			StopCoroutine(fading);
			fading = null;
		}
	}
}
