﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPrompt : InteractiveAttack {

    //List<string> buttonPromptOptions;
	List<Vector2> currentButtonPrompts;
    int currentButton = 0;

    public GameObject promptPrefab;
    List<GameObject> promptObjs;

    float timer = 0.0f;
    float timeLimit = 5.0f;
    public UnityEngine.UI.Text timerText;

	public void Init(List<float> damage, int promptCount = 3)
    {
        base.Init(damage);
        CreatePositionsOnLine(promptCount);

        //buttonPromptOptions = new List<string>() { "Submit", "Cancel", "Pause" };
		currentButtonPrompts = new List<Vector2>();
        //int random;
		int horOrVer, negOrPos;
        for(int i = 0; i < promptCount; i++)
        {
            //random = Random.Range(0, buttonPromptOptions.Count);
            //currentButtonPrompts.Add(buttonPromptOptions[random]);
			horOrVer = Random.Range (0, 2);
			negOrPos = Random.Range (0, 2);
			if (negOrPos == 0)
				negOrPos = -1;

			currentButtonPrompts.Add (
				new Vector2 (
				(horOrVer == 0 ? negOrPos : 0), 
				(horOrVer == 1 ? negOrPos : 0)
				)
			);
        }
        promptObjs = new List<GameObject>();
        for(int i = 0; i < currentButtonPrompts.Count; i++)
        {
            var obj = Instantiate(promptPrefab, parentRectTransform.transform);
            obj.transform.localPosition = new Vector2(xPosList[i], 0f);

            // TEMP            
			string objText;
			if (currentButtonPrompts [i].x != 0) {
				if (currentButtonPrompts [i].x == -1)
					objText = "<";
				else
					objText = ">";
			} else {
				if (currentButtonPrompts [i].y == -1)
					objText = "v";
				else
					objText = "^";
			}
			obj.GetComponentInChildren<UnityEngine.UI.Text> ().text = objText; //currentButtonPrompts[i];

            promptObjs.Add(obj);
        }
        timerText.text = timer.ToString("0.00");
    }

    private new void Update()
    {
        if(currentButton < currentButtonPrompts.Count
            && timer < timeLimit)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("0.00");
            //if (Input.GetButtonDown(currentButtonPrompts[currentButton]))
			if(Input.GetAxisRaw("Horizontal") == currentButtonPrompts[currentButton].x 
				&& Input.GetAxisRaw("Vertical") == currentButtonPrompts[currentButton].y
				&& (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
			{
                promptObjs[currentButton].SetActive(false);
                currentButton++;
            }
        }
        else
        {
            print(timer);
            Quality quality;
            if (timer < timeLimit / 5f)
                quality = Quality.PERFECT;
            else if (timer < timeLimit * 2f / 5f)
                quality = Quality.GREAT;
            else if (timer < timeLimit * 3f / 5f)
                quality = Quality.GOOD;
            else if (timer < timeLimit * 4f / 5f)
                quality = Quality.OKAY;
            else if (timer < timeLimit)
                quality = Quality.POOR;
            else
                quality = Quality.MISS;

            SetAttack(GameObject.FindGameObjectWithTag("MainCanvas"), quality);
            Destroy(gameObject);
        }
    }
}