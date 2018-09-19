//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class ConsumableItemSubMenu : SubMenu {

//    public InventoryMenu im;
//    ItemUseSubMenu use;
//    ItemRemoveSubMenu remove;
//    public string itemName; // the name of an item in inventory
//    public Item currentItem;

//    // Use this for initialization
//    void Start()
//    {
//        im = GameObject.FindObjectOfType<InventoryMenu>();
//        use = GameObject.FindObjectOfType<ItemUseSubMenu>();

//        // check if the button's name needs to be changed
//        if (GameControl.control.whichInventory == "weapons" || GameControl.control.whichInventory == "armor")
//        {
//            contentArray = new List<string>() { "Equip", "Remove" };
//            buttonDescription = new List<string>() { "Equip this item to one of your teammates.", "Remove this item from one of your teammates." };
//        }

//        base.Start();

//        // Create the use sub menu
//        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/ItemUseSubMenu"));
//        temp.name = "ItemUseSubMenu";
//        use = temp.GetComponent<ItemUseSubMenu>();
//        use.parentPos = buttonArray[selectedIndex].transform;

//        // create remove sub menu
//        temp = Instantiate(new GameObject());
//        //temp.AddComponent<ItemRemoveSubMenu>();
//        temp.name = "ItemRemoveSubMenu";
//        remove = temp.AddComponent<ItemRemoveSubMenu>();
//        remove.parentPos = buttonArray[selectedIndex].transform;
//    }

//    // deal with the button pressed
//    public override void ButtonAction(string label)
//    {
//        switch (label)
//        {
//            case "Use":
//            case "Equip":
//                use.EnableSubMenu();
//                break;
//            case "Remove":
//                remove.EnableSubMenu();
//                break;
//            default:
//                break;
//        }
//    }

//    public void EnableSubMenu()
//    {
//        im.DeactivateMenu();
//        CheckForInactive();
//        base.EnableSubMenu();
//    }

//    public void DeactivateMenu()
//    {
//        isActive = false;
//        for (int i = 0; i < buttonArray.Length; i++)
//        {
//            if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled; }
//        }
//    }

//    public void ActivateMenu()
//    {
//        frameDelay = 0.0f;
//        isActive = true;
//        im.descriptionText.GetComponent<Renderer>().enabled = true;
//        for (int i = 0; i < buttonArray.Length; i++)
//        {
//            if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
//        }

//        // deactive any buttons if necessary
//        CheckForInactive();

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //if (isVisible && frameDelay > 0)
//        if (isVisible && isActive)
//        {
//            im.descriptionText.GetComponent<TextMesh>().text = im.FormatText(buttonDescription[selectedIndex]);
//            if (Input.GetKeyUp(GameControl.control.backKey) || Input.GetKeyUp(GameControl.control.pauseKey))
//            {
//                im.ActivateMenu();
//            }
//        }
//        //update which position the submenu should appear in
//        use.parentPos = remove.parentPos = buttonArray[selectedIndex].transform;

//        base.Update();
//    }

//    void CheckForInactive()
//    {
//        // set the current item based on the inventory type
//        if (GameControl.control.whichInventory == "consumables")
//            foreach (GameObject go in GameControl.control.consumables) { if (go.GetComponent<Item>().name == itemName) { currentItem = go.GetComponent<Item>(); } }
//        else if (GameControl.control.whichInventory == "weapons")
//            foreach (GameObject go in GameControl.control.weapons) { if (go.GetComponent<Item>().name == itemName) { currentItem = go.GetComponent<Item>(); } }
//        else if (GameControl.control.whichInventory == "armor")
//            foreach (GameObject go in GameControl.control.equipment) { if (go.GetComponent<Item>().name == itemName) { currentItem = go.GetComponent<Item>(); } }

//        // if the player can't use anymore of the object, set button to inactive
//        if (GameControl.control.whichInventory == "weapons" || GameControl.control.whichInventory == "armor")
//        {
//            // if player has used up all the items, deactivate equip button
//            if (currentItem.quantity - currentItem.uses <= 0)
//            {
//                // hover if in first position
//                if (selectedIndex == 0)
//                    buttonArray[0].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
//                else
//                    buttonArray[0].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
//            }
//            // if the player has not used the item at all, deactivate remove button
//            if(currentItem.uses == 0)
//            {
//                // hover if in second position
//                if (selectedIndex == 1)
//                    buttonArray[1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover;
//                else
//                    buttonArray[1].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
//            }
//        }
//    }
//}
