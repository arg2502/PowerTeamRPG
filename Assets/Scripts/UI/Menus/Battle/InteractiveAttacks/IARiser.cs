using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IARiser : InteractiveAttack {

	public Slider targetSlider;
	public Slider swordSlider;

	float targetGravity = -1.3f;
	float targetVel = 1.5f;
	float swordVel = 3f;
	bool swordLaunched = false;
	bool targetHit = false;

	public void Init(List<float> damage){
		base.Init (damage);
	}

	void Update()
	{
		if (!targetHit) {
			targetSlider.value += targetVel * Time.deltaTime;
			targetVel += targetGravity * Time.deltaTime;
		}
		if (Input.GetButtonDown ("Submit") && !swordLaunched) {
			swordLaunched = true;
		}
		if (swordLaunched && !targetHit) {
			swordSlider.value += swordVel * Time.deltaTime;
		}

		if (swordSlider.value == swordSlider.maxValue ||
		   (targetSlider.value == targetSlider.minValue && targetVel < 0)) {
			quality = Quality.MISS;
			SetAttack(GameObject.FindGameObjectWithTag("MainCanvas"));
			Destroy(gameObject);
		}
	}

	public void HitTarget()
	{
		targetHit = true;
		var targetBox = targetSlider.GetComponentInChildren<BoxCollider2D> ();
		var swordBox = swordSlider.GetComponentInChildren<BoxCollider2D> ();
		var dist = targetBox.bounds.center.y - swordBox.bounds.center.y;
		print ("dist: " + dist);

		if (dist <= 2f)
			quality = Quality.PERFECT;
		else if (dist <= 7f)
			quality = Quality.GREAT;
		else if (dist <= 15f)
			quality = Quality.GOOD;
		else if (dist <= 25f)
			quality = Quality.OKAY;
		else
			quality = Quality.POOR;

		SetAttack(GameObject.FindGameObjectWithTag("MainCanvas"));
		Destroy(gameObject);
	}
}
