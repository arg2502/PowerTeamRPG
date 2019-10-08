using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPrompt : InteractiveAttack {

    List<string> buttonPromptOptions;
    List<string> currentButtonPrompts;
    int currentButton = 0;

    public GameObject promptPrefab;
    List<GameObject> promptObjs;

    float timer = 0.0f;
    float timeLimit = 5.0f;
    public UnityEngine.UI.Text timerText;

	public void Init(float damage, int promptCount = 3)
    {
        base.Init(damage);
        CreatePositionsOnLine(promptCount);

        buttonPromptOptions = new List<string>() { "Submit", "Cancel", "Pause" };
        currentButtonPrompts = new List<string>();
        int random;
        for(int i = 0; i < promptCount; i++)
        {
            random = Random.Range(0, buttonPromptOptions.Count);
            currentButtonPrompts.Add(buttonPromptOptions[random]);
        }
        promptObjs = new List<GameObject>();
        for(int i = 0; i < currentButtonPrompts.Count; i++)
        {
            var obj = Instantiate(promptPrefab, parentRectTransform.transform);
            obj.transform.localPosition = new Vector2(xPosList[i], 0f);

            // TEMP            
            obj.GetComponentInChildren<UnityEngine.UI.Text>().text = currentButtonPrompts[i];

            promptObjs.Add(obj);
        }
        timerText.text = timer.ToString();
    }

    private new void Update()
    {
        if(currentButton < currentButtonPrompts.Count
            && timer < timeLimit)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString();
            if (Input.GetButtonDown(currentButtonPrompts[currentButton]))
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

            SetAttack(parentRectTransform, quality);
            Destroy(gameObject);
        }
    }
}
