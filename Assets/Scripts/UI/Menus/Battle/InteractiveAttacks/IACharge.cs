using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IACharge : InteractiveAttack {

	public Slider slider;
	Denigen currentAttacker;

	float decayRate = 0.5f;
	float increaseRate = 0.1f;

	float timer = 0.0f;
	float timeLimit = 3.0f;
	public Text timerText;

	public void Init(Denigen attacker, List<float> damage)
    {		
		base.Init (damage);
		currentAttacker = attacker;
        timerText.text = GetTimerText();
	}

    string GetTimerText()
    {
        return (timeLimit - timer).ToString("0.00");
    }

	new void Update () {
		if (timer <= timeLimit)
        {
			timer += Time.deltaTime;
            timerText.text = GetTimerText();
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
