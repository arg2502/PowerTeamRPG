using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildIcon : MonoBehaviour {

    Image sr;
    //float lifecycleTime = 2f;
    float fadingRate = 0.01f;
    int flashTimes = 3;

	// Use this for initialization
	void Start () {
        sr = GetComponent<Image>();
        StartCoroutine(LifeCycle());
	}

    IEnumerator LifeCycle()
    {
        var startingColor = sr.color;
        startingColor.a = 0f;
        sr.color = startingColor;

        while(sr.color.a < 1f)
        {
            var a = sr.color.a + Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
            yield return new WaitForSeconds(fadingRate);
        }
        startingColor.a = 1f;
        sr.color = startingColor;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < flashTimes-1; i++)
        {
            while (sr.color.a > 0.5f)
            {
                var a = sr.color.a - Time.deltaTime;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
                yield return new WaitForSeconds(fadingRate);
            }
            startingColor.a = 0.5f;
            sr.color = startingColor;
            while (sr.color.a < 1f)
            {
                var a = sr.color.a + Time.deltaTime;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
                yield return new WaitForSeconds(fadingRate);
            }
            startingColor.a = 1f;
            sr.color = startingColor;
            yield return new WaitForSeconds(0.1f);
        }

        while (sr.color.a > 0f)
        {
            var a = sr.color.a - Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
            yield return new WaitForSeconds(fadingRate);
        }

        Destroy(gameObject);
    }
	
}
