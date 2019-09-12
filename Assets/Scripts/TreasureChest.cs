using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureChest : NPCObject
{
    public enum ChestType { NULL, GOLD, KEY, ITEM }
    public ChestType chestType;
    public int amountOfGold;
    [System.Serializable]
    public struct ChestItem
    {
        public ScriptableItem item;
        public int quantity;
    }
    public List<ChestItem> chestItems;
    public Sprite open, close;
    NPCDialogue dialogueComponent;
    public bool isOpen = false;

    private new void Start()
    {
        dialogueComponent = GetComponentInChildren<NPCDialogue>();
        SetText();
        GetComponent<SpriteRenderer>().sprite = isOpen ? open : close;
        base.Start();
    }

    void SetText()
    {
        string message = "";
        switch(chestType)
        {
            case ChestType.GOLD:
                message = "You got " + amountOfGold + " gold.";
                break;
            case ChestType.KEY:
                message = "You got a key.\nIt can open shit, I guess.";
                break;
            case ChestType.ITEM:
                message = "You got";
                if(chestItems.Count > 1)
                {
                    message += ":";
                    for(int i = 0; i < chestItems.Count; i++)
                    {
                        message += "\n" + chestItems[i].quantity.ToString() + " " + chestItems[i].item.name;
                    }
                }
                else if (chestItems.Count == 1)
                {
                    message += " " + chestItems[0].quantity.ToString() + " " + chestItems[0].item.name;
                }
                else
                {
                    message += " nothing...";
                }
                break;
            default:
                message = "This chest is empty. Such a disappointment.\nLike Mother always said..";
                break;
        }
        dialogueComponent.customDialogueList = new List<string>();
        dialogueComponent.customDialogueList.Add(message);
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            GetComponent<SpriteRenderer>().sprite = open;
            switch(chestType)
            {
                case ChestType.GOLD: OpenGold(); break;
                case ChestType.KEY: OpenKey(); break;
                case ChestType.ITEM: OpenItem(); break;
            }
        }
    }

    void OpenGold()
    {
        GameControl.control.AddGold(amountOfGold);
    }

    void OpenKey()
    {
        GameControl.control.totalKeys++;
    }

    void OpenItem()
    {
        for (int i = 0; i < chestItems.Count; i++)
        {
            GameControl.control.AddItem(chestItems[i].item, chestItems[i].quantity);
        }
    }

    public override void ShowInteractionNotification(string message)
    {
        message = "Open";
        base.ShowInteractionNotification(message);
    }
}
