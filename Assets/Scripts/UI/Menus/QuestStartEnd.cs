using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestStartEnd : MonoBehaviour {

    public Text startEnd;
    public Text questName;
    CanvasGroup canvasGroup;
    float fadingRate = 0.01f;
    float lifeCycleTime = 3f;

	public void Init(bool startQuest, string newName)
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        startEnd.text = (startQuest ? "Quest Start" : "Quest Complete");
        questName.text = newName;
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        canvasGroup.alpha = 0;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;            
            yield return new WaitForSeconds(fadingRate);
        }
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(lifeCycleTime);
        
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return new WaitForSeconds(fadingRate);
        }

        Destroy(gameObject);
    }
		
}
