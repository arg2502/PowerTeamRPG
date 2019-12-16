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
		var dist = Mathf.Abs(targetBox.bounds.center.y - swordBox.bounds.max.y);
        //print("target center y: " + targetBox.bounds.center.y);
        //print("sword center y: " + swordBox.bounds.center.y);
        //print ("dist: " + dist);
        var perfectSize = targetBox.bounds.size.y / 10f;
        var greatSize = targetBox.bounds.size.y / 8f;
        var goodSize = targetBox.bounds.size.y / 5f;
        var okaySize = targetBox.bounds.size.y / 2f;

		if (dist <= perfectSize)
			quality = Quality.PERFECT;
        else if (dist <= greatSize)
            quality = Quality.GREAT;
        else if (dist <= goodSize)
            quality = Quality.GOOD;
        else if (dist <= okaySize)
            quality = Quality.OKAY;
        else
			quality = Quality.POOR;

		SetAttack(GameObject.FindGameObjectWithTag("MainCanvas"));
		Destroy(gameObject);
	}
}
