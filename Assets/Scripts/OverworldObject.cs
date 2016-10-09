using UnityEngine;
using System.Collections;

public class OverworldObject : MonoBehaviour {

    protected SpriteRenderer sr;
    public LayerMask mask;
    public bool canMove = true;

	// Use this for initialization
	protected void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sortingOrder = (int)-transform.position.y;
	}

    public void ToggleMovement()
    {
        foreach (OverworldObject o in GameObject.FindObjectsOfType<OverworldObject>())
        {
            o.canMove = !canMove;
        }
    }
}
