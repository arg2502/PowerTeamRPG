using UnityEngine;
using System.Collections;

public class MyButton : MonoBehaviour {

    // attributes
    //public Rect position;
    public enum MyButtonTextureState { normal, hover, active, disabled };
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

        renderer.sprite = normalTexture;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == MyButtonTextureState.normal) { renderer.sprite = normalTexture; }
        else if (state == MyButtonTextureState.hover) { renderer.sprite = hoverTexture; }
        else if (state == MyButtonTextureState.active) { renderer.sprite = activeTexture; }
        else if (state == MyButtonTextureState.disabled) { renderer.sprite = disabledTexture; }
	}
}
