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
    protected List<string> questAlreadyTalked;
    public List<string> QuestAlreadyTalked { get { return questAlreadyTalked; } }
    string interactionMessage = "";
    bool show = false;
    public float TalkingSpeed
    {
        get
        {
            return GetComponentInChildren<NPCDialogue>().talkingSpeed;
        }
        set
        {
            GetComponentInChildren<NPCDialogue>().talkingSpeed = value;
        }
    }

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

    protected bool showNotificationAgain = true;

    // Use this for initialization
    protected void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
		sr.sortingOrder = (int)(-transform.position.y * 10.0f) + sortingOffset;
        questAlreadyTalked = new List<string>();
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
        questAlreadyTalked.Add(currentQuestID);
    }

    private InteractionNotification interactionNotification;
    public InteractionNotification InteractionNotification { get { return interactionNotification; } set { interactionNotification = value; } }

    public virtual void ShowInteractionNotification(string message = "")
    {
        if (GetComponentInChildren<NPCDialogue>() != null && !GetComponentInChildren<NPCDialogue>().canTalk) return;

        if (!string.IsNullOrEmpty(message))
            interactionMessage = message;

        if(interactionNotification == null)
        {
            interactionNotification = GameControl.UIManager.ShowInteractionNotification(transform, message);
        }
        else
        {
            show = true;
            interactionNotification.gameObject.SetActive(show);
            interactionNotification.SetText(interactionMessage);
            interactionNotification.GetComponent<Animator>().Play("FadeIn");
        }
    }
    public void HideInteractionNotification(bool instant = false)
    {
        //if (interactionNotification != null)
        //{

        //interactionNotification = null;
        //}
        show = false;
        if (instant)
        {
            interactionNotification.gameObject.SetActive(show);
        }
        else
        {
            StartCoroutine(HideNotification());
        }
    }    
    IEnumerator HideNotification()
    {
        var anim = interactionNotification.GetComponent<Animator>();        
        anim.Play("FadeOut");
        var timeToWait = anim.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(timeToWait);
        interactionNotification.gameObject.SetActive(show);
    }

    public virtual void PostDialogueInvoke(string functionName)
    {
        Invoke(functionName, 0f);
    }

    public virtual void BackToNormal() { if(showNotificationAgain) ShowInteractionNotification(); }
    
    /// <summary>
    /// Called through dialogue response
    /// </summary>
    public void Buy()
    {
        GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.ShopkeeperBuyMenu);
    }

    /// <summary>
    /// Called through dialogue response
    /// </summary>
    public void Sell()
    {
        GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.ShopkeeperSellMenu);
    }

    public void Talk()
    {
        GameControl.UIManager.PushNotificationMenu(
            "This should open up a menu with dialogue options...but not yet"
            );
    }

    protected void Update()
    {
        if (sr == null) return;
        sr.sortingOrder = (int)(-transform.position.y * 10.0f) + sortingOffset;
    }
}
