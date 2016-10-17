using UnityEngine;
using System.Collections;

public class ConsumableItemSubMenu : SubMenu {

    public InventoryMenu im;
    ItemUseSubMenu use;

    // Use this for initialization
    void Start()
    {
        im = GameObject.FindObjectOfType<InventoryMenu>();
        use = GameObject.FindObjectOfType<ItemUseSubMenu>();

        base.Start();

        // Create the use sub menu
        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/ItemUseSubMenu"));
        use = temp.GetComponent<ItemUseSubMenu>();
        use.parentPos = buttonArray[selectedIndex].transform;
    }

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            case "Use":
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
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Q))
            {
                im.ActivateMenu();
            }
        }
        //update which position the submenu should appear in
        use.parentPos = buttonArray[selectedIndex].transform;
        base.Update();
    }
}
