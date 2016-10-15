﻿using UnityEngine;
using System.Collections;

public class MyButton : MonoBehaviour {

    // attributes
    //public Rect position;
    public enum MyButtonTextureState { normal, hover, active, disabled, inactive, inactiveHover, disabledHover};
    public MyButtonTextureState state = MyButtonTextureState.normal;
    public int width;
    public int height;
    public TextMesh labelMesh;
    public GameObject textObject;
    public GameObject textPrefab;
    // textures
    public Sprite normalTexture;
    public Sprite hoverTexture;
    public Sprite activeTexture;
    //public Sprite disabledTexture;
    public Sprite inactiveTexture;
    public Sprite inactiveHoverTexture;
    //public Sprite disabledHoverTexture;
    public SpriteRenderer sr;

    // skill tree specific
    // if the Technique has a next, this next takes control of the next button's state
    public MyButton next;
    public SpriteRenderer contentSr;
    
	// Use this for initialization
	void Start () {        
        sr.sprite = normalTexture;
        //contentSr = gameObject.transform.Find("Content").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == MyButtonTextureState.normal) { sr.sprite = normalTexture; } // button is in standby
        else if (state == MyButtonTextureState.hover) { sr.sprite = hoverTexture; } // cursor is over button
        else if (state == MyButtonTextureState.active) { sr.sprite = activeTexture; } // player has selected button
        else if (state == MyButtonTextureState.disabled) { sr.sprite = inactiveTexture; } // there is no option/button to display
        else if (state == MyButtonTextureState.inactive) { sr.sprite = inactiveTexture; } // player cannot select button
        else if (state == MyButtonTextureState.inactiveHover) { sr.sprite = inactiveHoverTexture; } // player hovers over button but cannot select it
        else if (state == MyButtonTextureState.disabledHover) { sr.sprite = inactiveHoverTexture; } // skill tree specific - hover over technique you cannot buy yet
	}
}
