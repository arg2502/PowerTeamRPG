using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : OverworldObject {

    [SerializeField] Color showColor = Color.blue;
	[SerializeField] Color hideColor = Color.white;

    [System.Serializable]
    public enum HiddenType { COLOR, SPARKLE }

    [SerializeField] HiddenType hiddenType;
    [SerializeField] ParticleSystem sparkles;

    [System.NonSerialized] public bool hasBeenSeen = false;

    private void Awake()
    {
        sparkles?.gameObject.SetActive(false);
    }

    private new void Start()
    {
        base.Start();
        UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        var dialogue = GetComponentInChildren<NPCDialogue>();
        if (dialogue == null) return;
        print("dialogue is not null");
        print("dialogue.cantalk: " + dialogue.canTalk);
        dialogue.canTalk = hasBeenSeen;
        print("dialogue.cantalk now: " + dialogue.canTalk);
    }

    public void Show()
    {
        hasBeenSeen = true;
        UpdateDialogue();

        switch(hiddenType)
        {
            case HiddenType.COLOR:
                ShowColor();
                break;
            case HiddenType.SPARKLE:
                ShowSparkle();
                break;
        }
    }

    public void Hide()
    {
        switch (hiddenType)
        {
            case HiddenType.COLOR:
                HideColor();
                break;
            case HiddenType.SPARKLE:
                HideSparkle();
                break;
        }
    }

    private void ShowColor()
    {
        GetComponent<SpriteRenderer>().color = showColor;
    }

    private void HideColor()
    {
        GetComponent<SpriteRenderer>().color = hideColor;
    }

    private void ShowSparkle()
    {
        sparkles.gameObject.SetActive(true);
    }

    private void HideSparkle()
    {
        sparkles.gameObject.SetActive(false);
    }

}
