using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : OverworldObject {

    [SerializeField] Color showColor = Color.blue;
	[SerializeField] Color hideColor = Color.white;

	public void Show()
    {
        GetComponent<SpriteRenderer>().color = showColor;
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().color = hideColor;
    }
}
