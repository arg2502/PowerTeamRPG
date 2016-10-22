using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureChest : NPCObject {
    
    public bool isOpen; // check if the chest has been opened
    public Sprite openSprite;
    // three types of chest
    public int amountOfGold; // Will only be greater than zero if the chest gives gold
    public bool hasKey; // set to true if chest contains key
    public GameObject chestItem; // item that the chest contains

    ListOfStrings chestDialogue; // var to hold dialogue text that will be added to dialogue box
    GameObject temp;
    void Start()
    {
        base.Start();
        chestDialogue = new ListOfStrings();
        chestDialogue.dialogue = new List<string>();
        chestDialogue.charImages = new List<Sprite>();
        npcDialogue.title.Add("Treasure Chest");
        //openSprite = Resources.Load("Sprites/disabledButton", typeof(Sprite)) as Sprite;
    }
    // begin conversation when player collides and presses space
    protected void Update()
    {
        if (distFromPlayer < distToTalk
            && Input.GetKeyUp(KeyCode.Space) 
            && canTalk 
            && player.gameObject.GetComponent<characterControl>().canMove
            && !isOpen)
        {
            OpenChest();

            canTalk = false;
            // if first time, set equal to existing box
            if (dBox == null && dBoxGO == null)
            {
                if (GameObject.FindObjectOfType<DialogueBox>() == null)
                {
                    dBoxGO = (GameObject)Instantiate(Resources.Load("Prefabs/DialogueBoxPrefab"));
                    dBox = dBoxGO.GetComponent<DialogueBox>();
                    dBox.npc = this;
                }
                else
                {
                    dBox = GameObject.FindObjectOfType<DialogueBox>();
                    dBox.npc = this;
                    dBox.EnableBox();
                }
            }
            else
            {
                dBox.npc = this;
                dBox.EnableBox();
            }

        }
        else if(isOpen)
        {
            sr.sprite = openSprite;
        }
    }

	public void OpenChest()
    {
        // first check for gold in chest
        if(amountOfGold > 0)
        {
            // add gold to GameControl
            GameControl.control.totalGold += amountOfGold;
            chestDialogue.dialogue.Add("You got " + amountOfGold + " gold.");
            
        }
        // if no gold, then check for key
        else if(hasKey)
        {
            // add key
            GameControl.control.totalKeys++;
            chestDialogue.dialogue.Add("You got a key.");
        }
        // if no key, then the chest holds an item
        else
        {
            // add item to GameControl
            temp = (GameObject)Instantiate(chestItem);
            GameControl.control.AddItem(temp);
            chestDialogue.dialogue.Add("You got " + chestItem.GetComponent<Item>().name + ".");
        }

        npcDialogue.dialogueList.Add(chestDialogue); // add text to dialogue box
        isOpen = true; // set chest to open
        

    }
}
