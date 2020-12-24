using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConfirmUseMenu : Menu
{
    public Button topButton, middleButton, bottomButton;
	internal InventoryItem item;
    InventoryMenu inventory;
        
    public override void TurnOnMenu()
    {
        // assign inventory
        var list = UIManager.list_currentMenus;
        var count = list.Count;
        inventory = UIManager.FindMenu(uiDatabase.InventoryMenu) as InventoryMenu;

        // assign variables from inventory
        this.descriptionText = inventory.descriptionText;
        this.descriptionText.text = inventory.currentDescription;
        item = inventory.chosenItem;

        AssignBasedOnCategory();
        SetButtonNavigation(); // reset button navigation
        rootButton = AssignRootButton();

        base.TurnOnMenu();
    }
    public override void Refocus()
    {
        base.Refocus();

        this.descriptionText.text = inventory.currentDescription;
    }
    protected override void AddButtons()
    {
        base.AddButtons();

        listOfButtons = new List<Button>() { topButton, middleButton, bottomButton };
    }
    public override Button AssignRootButton()
    {
        if (topButton.interactable)
            return topButton;
        else if (middleButton.interactable)
            return middleButton;
        else
            return bottomButton;
    }
        
    // class specific functions
    void AssignBasedOnCategory()
    {
        Debug.Log("which inventory: " + (int)gameControl.whichInventoryEnum);
        switch((int)gameControl.whichInventoryEnum)
        {
            // consumables
            case 0:
                AssignConsumables();
                break;

            // weapons/augmentations
            case 1:
            case 2:
                AssignWeaponOrEquipment();
                break;

            // key items
            case 3:
                AssignKeyItems();
                break;
        }
    }

    /// <summary>
    /// Consumables: Use & Cancel
    /// </summary>
    void AssignConsumables()
    {
        // activate only the first two buttons
        ActivateButtons(2);

        // remove all function calls
        RemoveAllListeners();

        // assign text to buttons
        AssignText(topButton, "Use");
        AssignText(middleButton, "Back");

        // set top button to call UseItem with Consumables
        topButton.onClick.AddListener(OnUse);

        // set middle button to pop the menu
        middleButton.onClick.AddListener(OnCancel);

        topButton.interactable = true;
        middleButton.interactable = true;
        bottomButton.interactable = true;

    }

    /// <summary>
    /// Weapons: Equip, Remove, & Cancel
    /// </summary>
    void AssignWeaponOrEquipment()
    {
        // activate all buttons
        ActivateButtons(3);

        // remove
        RemoveAllListeners();

        // text
        AssignText(topButton, "Equip");
        AssignText(middleButton, "Remove");
        AssignText(bottomButton, "Back");

        // determine if the item can be equipped to any team member
        if(item.quantity - item.uses <= 0)
        {
            topButton.interactable = false;
        }
        else
        {
            topButton.interactable = true;
            topButton.onClick.AddListener(OnEquip);
        }

        // determine if any hero has the item that can be removed
        if(item.uses <= 0)
        {
            middleButton.interactable = false;
        }
        else
        {
            middleButton.interactable = true;
            middleButton.onClick.AddListener(OnRemove);
        }

        bottomButton.onClick.AddListener(OnCancel);
    }

    void AssignKeyItems()
    {
        // TEMP
        // for now just one button that's CANCEL
        ActivateButtons(1);
        RemoveAllListeners();
        AssignText(topButton, "Back");
        topButton.onClick.AddListener(OnCancel);
    }

    void OnUse()
    {
        PushUseItem(UseItemMenu.MenuState.Use);
    }
    void OnEquip()
    {
        PushUseItem(UseItemMenu.MenuState.Equip);
    }
    void OnRemove()
    {
        PushUseItem(UseItemMenu.MenuState.Remove);
    }
    void OnCancel()
    {
        UIManager.PopMenu();
    }


    UseItemMenu PushUseItem(UseItemMenu.MenuState menuState)
    {
        // open the ConfirmUse menu
        UIManager.PushMenu(uiDatabase.UseItemMenu);

        //         // set the Item Use menu's item to the one chosen
        //         var count = UIManager.list_currentMenus.Count;
        //         var useItem = UIManager.list_currentMenus[count - 1].GetComponent<UseItemMenu>();
        //         useItem.item = item;
        //         useItem.descriptionText = descriptionText;
        //useItem.icon.sprite = ItemDatabase.GetItemSprite(item.name);
        //         useItem.itemName.text = item.name;
        //         useItem.menuState = menuState;
        //         useItem.Setup();     

        //return useItem;
        return null;
    }

    /// <summary>
    /// Set "count" number of buttons to active, every button after to false.
    /// </summary>
    /// <param name="count">The number of buttons you want active.</param>
    void ActivateButtons(int count)
    {
        for(int i = 0; i < count; i ++)
        {
            listOfButtons[i].gameObject.SetActive(true);
        }
        for(int i = count; i < listOfButtons.Count; i++)
        {
            listOfButtons[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Assign the button's text.
    /// </summary>
    /// <param name="button">Button whose text you want changed.</param>
    /// <param name="text">Desired new text.</param>
    void AssignText(Button button, string text)
    {
        button.GetComponentInChildren<Text>().text = text;
    }

    /// <summary>
    /// Clears all the buttons from calling any function when clicked. Used when preparing the buttons for other uses
    /// </summary>
    void RemoveAllListeners()
    {
        foreach(var button in listOfButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
