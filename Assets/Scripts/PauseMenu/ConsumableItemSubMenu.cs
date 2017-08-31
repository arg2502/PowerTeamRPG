using UnityEngine;
using System.Collections;

public class ConsumableItemSubMenu : SubMenu {

    public InventoryMenu im;
    ItemUseSubMenu use;
    public string itemName; // the name of an item in inventory

    // Use this for initialization
    void Start()
    {
        im = GameObject.FindObjectOfType<InventoryMenu>();
        use = GameObject.FindObjectOfType<ItemUseSubMenu>();

        // check if the button's name needs to be changed
        if (GameControl.control.whichInventory == "weapons" || GameControl.control.whichInventory == "armor")
        { contentArray[0] = "Equip"; buttonDescription[0] = "Equip this item to one of your teammates."; }

        base.Start();

        // Create the use sub menu
        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/ItemUseSubMenu"));
        temp.name = "ItemUseSubMenu";
        use = temp.GetComponent<ItemUseSubMenu>();
        use.parentPos = buttonArray[selectedIndex].transform;
    }

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            case "Use":
            case "Equip":
                use.EnableSubMenu();
                break;
            default:
                break;
        }
    }

    public void EnableSubMenu()
    {
        im.DeactivateMenu();
        base.EnableSubMenu();
    }

    public void DeactivateMenu()
    {
        isActive = false;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.disabled; }
        }
    }

    public void ActivateMenu()
    {
        frameDelay = 0.0f;
        isActive = true;
        im.descriptionText.GetComponent<Renderer>().enabled = true;
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i != selectedIndex) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.normal; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (isVisible && frameDelay > 0)
        if (isVisible && isActive)
        {
            im.descriptionText.GetComponent<TextMesh>().text = im.FormatText(buttonDescription[selectedIndex]);
            if (Input.GetKeyUp(GameControl.control.backKey) || Input.GetKeyUp(GameControl.control.pauseKey))
            {
                im.ActivateMenu();
            }
        }
        //update which position the submenu should appear in
        use.parentPos = buttonArray[selectedIndex].transform;
        base.Update();
    }
}
