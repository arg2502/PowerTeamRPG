using UnityEngine;
using System.Collections;

public class MyButton : MonoBehaviour {

    // attributes
    //public Rect position;
    public enum MyButtonTextureState { normal, hover, active, disabled, inactive, inactiveHover };
    public MyButtonTextureState state = MyButtonTextureState.normal;
    //protected string label;
    public int width = 200;
    public int height = 50;
    public TextMesh labelMesh;
    public GameObject textObject;
    public GameObject textPrefab;
    // textures
    public Sprite normalTexture;
    public Sprite hoverTexture;
    public Sprite activeTexture;
    public Sprite disabledTexture;
    public Sprite inactiveHoverTexture;
    protected SpriteRenderer renderer;

    // properties
    //public Rect Position { get { return position; } set { position = value; } }
    //public string Label { get { return label; } set { label = value; } }
    //public TextMesh LabelMesh { get { return labelMesh; } set { labelMesh = value; } }

	// Use this for initialization
	void Start () {
        //position = new Rect(0, 0, Screen.width, Screen.height);

        renderer = gameObject.AddComponent<SpriteRenderer>();

        // load button sprites
        normalTexture = Resources.Load("Sprites/normalButton", typeof(Sprite)) as Sprite;
        hoverTexture = Resources.Load("Sprites/hoverButton", typeof(Sprite)) as Sprite;
        activeTexture = Resources.Load("Sprites/activeButton", typeof(Sprite)) as Sprite;
        disabledTexture = Resources.Load("Sprites/disabledButton", typeof(Sprite)) as Sprite;
        inactiveHoverTexture = Resources.Load("Sprites/inactiveHoverButton", typeof(Sprite)) as Sprite;

        renderer.sprite = normalTexture;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == MyButtonTextureState.normal) { renderer.sprite = normalTexture; } // button is in standby
        else if (state == MyButtonTextureState.hover) { renderer.sprite = hoverTexture; } // cursor is over button
        else if (state == MyButtonTextureState.active) { renderer.sprite = activeTexture; } // player has selected button
        else if (state == MyButtonTextureState.disabled) { renderer.sprite = disabledTexture; } // there is no option/button to display
        else if (state == MyButtonTextureState.inactive) { renderer.sprite = disabledTexture; } // player cannot select button
        else if (state == MyButtonTextureState.inactiveHover) { renderer.sprite = inactiveHoverTexture; } // player hovers over button but cannot select it
	}
}
