using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPrompt : InteractiveAttack {

    List<string> buttonPromptOptions;
    List<string> currentButtonPrompts;
    int currentButton = 0;

    public GameObject promptPrefab;
    List<GameObject> promptObjs;

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
    }

    private new void Update()
    {
        if(currentButton < currentButtonPrompts.Count)
        {
            if(Input.GetButtonDown(currentButtonPrompts[currentButton]))
            {
                promptObjs[currentButton].SetActive(false);
                currentButton++;
            }
        }
        else
        {
            Attack(Quality.PERFECT);
            Destroy(gameObject);
        }
    }
}
