using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildIcon : MonoBehaviour {

    Image image;
    //float lifecycleTime = 2f;
    float fadingRate = 0.01f;
    int flashTimes = 3;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        StartCoroutine(LifeCycle());
	}

    IEnumerator LifeCycle()
    {
        var startingColor = image.color;
        startingColor.a = 0f;
        image.color = startingColor;

        while(image.color.a < 1f)
        {
            var a = image.color.a + Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            yield return new WaitForSeconds(fadingRate);
        }
        startingColor.a = 1f;
        image.color = startingColor;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < flashTimes-1; i++)
        {
            while (image.color.a > 0.5f)
            {
                var a = image.color.a - Time.deltaTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, a);
                yield return new WaitForSeconds(fadingRate);
            }
            startingColor.a = 0.5f;
            image.color = startingColor;
            while (image.color.a < 1f)
            {
                var a = image.color.a + Time.deltaTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, a);
                yield return new WaitForSeconds(fadingRate);
            }
            startingColor.a = 1f;
            image.color = startingColor;
            yield return new WaitForSeconds(0.1f);
        }

        while (image.color.a > 0f)
        {
            var a = image.color.a - Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            yield return new WaitForSeconds(fadingRate);
        }

        Destroy(gameObject);
    }
	
}
