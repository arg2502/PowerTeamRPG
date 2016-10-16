using UnityEngine;
using System.Collections;

public class ConsumableItemSubMenu : SubMenu {

    InventoryMenu im;

    // Use this for initialization
    void Start()
    {
        im = GameObject.FindObjectOfType<InventoryMenu>();

        base.Start();
    }

    // deal with the button pressed
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            default:
                break;
        }
    }

    public void EnableSubMenu()
    {
        im.DeactivateMenu();
        base.EnableSubMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible && frameDelay > 0)
        {
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Q))
            {
                im.ActivateMenu();
            }
        }
        base.Update();
    }
}
