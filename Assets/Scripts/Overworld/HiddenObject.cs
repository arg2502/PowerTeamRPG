using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : OverworldObject {

    [SerializeField] private Color showColor = Color.blue;
	[SerializeField] private Color hideColor = Color.white;

    [System.Serializable]
    public enum HiddenType { COLOR, SPARKLE }

    [SerializeField] private HiddenType hiddenType;
    [SerializeField] private ParticleSystem sparkles;

    [System.NonSerialized] public bool hasBeenSeen = false;

    private void Awake()
    {
        sparkles?.gameObject.SetActive(false);
    }

    private new void Start()
    {
        base.Start();
        ToggleDialogue();
    }

    private void ToggleDialogue()
    {
        var dialogue = GetComponentInChildren<NPCDialogue>();
        if (dialogue == null) return;
        dialogue.canTalk = hasBeenSeen;        
    }

    public void Show()
    {
        hasBeenSeen = true;
        ToggleDialogue();

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
