using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : OverworldObject {

    float timeActive = 5f;
    float timer = 0f;

    Color activeColor = Color.blue;
    Color inactiveColor = Color.white;

	public void ActivateGenerator()
    {
        GetComponent<SpriteRenderer>().color = activeColor;
        timer = timeActive;
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            
            if (timer <= 0f)
            {
                timer = 0f;
                DeActivateGenerator();
            }
        }
    }

    void DeActivateGenerator()
    {
        GetComponent<SpriteRenderer>().color = inactiveColor;
    }
}
