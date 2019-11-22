using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IACharge : InteractiveAttack {

	Slider slider;
	Denigen currentAttacker;

	float decayRate = 0.5f;
	float increaseRate = 0.1f;

	float timer = 0.0f;
	float timeLimit = 5.0f;
	public Text timerText;

	public void Init(Denigen attacker, List<float> damage){
		print ("iacharge init");
		base.Init (damage);
		currentAttacker = attacker;
		slider = GetComponentInChildren<Slider> ();
		timerText.text = timer.ToString("0.00");
	}

	// Update is called once per frame
	new void Update () {
		//print ("update -- timer: " + timer);
		if (timer <= timeLimit) {
			timer += Time.deltaTime;
			slider.value -= decayRate * Time.deltaTime * (100f / currentAttacker.CurrentAttack.Accuaracy);

			if (Input.GetButtonDown ("Submit"))
				slider.value += increaseRate;
		}
		else
		{
			Quality quality;
			if (slider.value >= 0.9f)
				quality = Quality.PERFECT;
			else if (slider.value >= 0.75f)
				quality = Quality.GREAT;
			else if (slider.value >= 0.6f)
				quality = Quality.GOOD;
			else if (slider.value >= 0.3f)
				quality = Quality.OKAY;
			else if (slider.value >= 0.1f)
				quality = Quality.POOR;
			else
				quality = Quality.MISS;

			SetAttack(GameObject.FindGameObjectWithTag("MainCanvas"), quality);
			Destroy(gameObject);
		}
	}
}
