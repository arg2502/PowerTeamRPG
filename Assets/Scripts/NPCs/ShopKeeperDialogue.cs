using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperDialogue : NPCDialogue {

    public List<ScriptableItem> shopInventory;

    public void Buy()
    {
        //print("BUY");
        GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.ShopkeeperBuyMenu);
        var buyMenu = GameControl.UIManager.FindMenu(GameControl.UIManager.uiDatabase.ShopkeeperBuyMenu).GetComponent<UI.ShopkeeperBuyMenu>();
        buyMenu.FillSlots(this);
    }

    public void Sell()
    {
        print("NOT YET");
    }
}
