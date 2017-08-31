using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

    //attributes
    float timer; // the amount of time that the effect has existed
    public float lifeTime; // the total amount of time that the effect should exist
    public float risingSpeed;
    Color fade = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    public string damage;

    public TextMesh labelMesh;
    public GameObject textObject;

    protected SpriteRenderer sr;

	// Use this for initialization
	public void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1000;
        textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
        textObject.name = "EffectText";
        textObject.GetComponent<MeshRenderer>().sortingOrder = 1000;
        labelMesh = textObject.GetComponent<TextMesh>();

        // give them a random offset for cases where multiple effects appear around the same time
        transform.position += new Vector3(Random.Range(-1, 2) * 0.78f, Random.Range(-1, 2)* 0.78f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        sr.sortingOrder = (int)transform.position.y + 500;
        textObject.GetComponent<MeshRenderer>().sortingOrder = (int)transform.position.y + 500;

        labelMesh.text = damage;

	    //increase the timer every frame
        timer += Time.deltaTime;

        if (timer >= lifeTime) { Destroy(textObject); Destroy(this.gameObject); }
        if (timer >= lifeTime / 2) { sr.color -= fade * Time.deltaTime; labelMesh.color -= fade * Time.deltaTime; }

        transform.position += new Vector3(0.0f, risingSpeed * Time.deltaTime, 0.0f);
        labelMesh.transform.position = transform.position;
	}
}
