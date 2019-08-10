using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverworldObject : MonoBehaviour {

    protected SpriteRenderer sr;
    public LayerMask mask;
    public bool canMove = true;
    public Vector3 offset = new Vector3(256.0f, 0.0f, 0.0f); // offset for stuck enemies, default: right
	public int sortingOffset; //mainly for multi-object prefabs
    protected string currentQuestID;
    public string CurrentQuestID { get { return currentQuestID; } set { currentQuestID = value; } }

    // weight variables -- used only as references throughout objects (movables/switches/etc.)
    public enum Weight { NORMAL, LIGHT, HEAVY, CUSTOM }
    public Weight weightClass;
    // NOT SET IN STONE -- CAN BE CHANGED ONCE WE FIGURE OUT WHAT WE WANT TO DO WITH THEM
    // CURRENTLY
    // 3 lights = 1 normal
    // 5 lights = 1 heavy 
    // 2 normals = ~1 heavy   
    protected float LightWeight { get { return 1f; } }
    protected float NormalWeight { get { return 3f; } }
    protected float HeavyWeight { get { return 5f; } }
    
	// Use this for initialization
	protected void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
		sr.sortingOrder = (int)(-transform.position.y * 10.0f) + sortingOffset;
	}

    // move this to gameManager
    // change to just control this object's movement
    public void ToggleMovement()
    {
        foreach (OverworldObject o in GameObject.FindObjectsOfType<OverworldObject>())
        {
            o.canMove = !o.canMove;
        }
    }
    public virtual void Activate() {}

	public int SortingOrder { get { return this.sr.sortingOrder; } set { this.sr.sortingOrder = value; } }

    /// <summary>
    /// Function to be called after talking with an object that is required for a quest
    /// Simply increments the number of people you needed to talk to
    /// Call this function the the RESPONSES column of the conversation spreadsheet
    /// </summary>
    public void QuestTalk()
    {
        GameControl.questTracker.IncrementTalkToPeople(currentQuestID);
    }
}
