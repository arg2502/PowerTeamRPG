using UnityEngine;
using System.Collections;

public class OverworldObject : MonoBehaviour {

    protected SpriteRenderer sr;
    public LayerMask mask;
    public bool canMove = true;
    public Vector3 offset = new Vector3(256.0f, 0.0f, 0.0f); // offset for stuck enemies, default: right
	// Use this for initialization
	protected void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)-transform.position.y;
	}

    public void ToggleMovement()
    {
        foreach (OverworldObject o in GameObject.FindObjectsOfType<OverworldObject>())
        {
            o.canMove = !o.canMove;
			//print (o.name);
        }
    }
    public virtual void Activate() {}
}
