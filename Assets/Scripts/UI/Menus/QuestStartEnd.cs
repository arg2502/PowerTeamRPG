using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestStartEnd : MonoBehaviour {

    public Text startEnd;
    public Text questName;
    Image image;
    float fadingRate = 0.01f;
    float lifeCycleTime = 3f;

	public void Init(bool startQuest, string newName)
    {
        image = GetComponentInChildren<Image>();
        startEnd.text = (startQuest ? "Quest Start" : "Quest Complete");
        questName.text = newName;
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        var startingColor = image.color;
        startingColor.a = 0f;
        image.color = startingColor;

        while (image.color.a < 1f)
        {
            var a = image.color.a + Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            yield return new WaitForSeconds(fadingRate);
        }
        startingColor.a = 1f;
        image.color = startingColor;
        yield return new WaitForSeconds(lifeCycleTime);
        
        while (image.color.a > 0f)
        {
            var a = image.color.a - Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            yield return new WaitForSeconds(fadingRate);
        }

        Destroy(gameObject);
    }
		
}
