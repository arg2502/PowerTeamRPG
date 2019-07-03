using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperDialogue : NPCDialogue {

    public TextAsset commentList;
    public List<ScriptableItem> shopInventory;    

    [System.Serializable]
    public enum ShopKeeperType { CONSUMABLE, WEAPON, ARMOR, MISC }
    public ShopKeeperType currentType;

    public float consumablePer;
    public float weaponPer;
    public float armorPer;
    public float miscPer;

    float defaultSpecialized = 0.9f;
    float defaultOthers = 0.6f;
    
    bool useDefault = true;
    bool useCustom;
    
    public Dictionary<ScriptableItem, int> customItemPrices;
    
    private new void Start()
    {
        base.Start();

        if(useDefault)
        {
            SetDefault();
        }
    }

    void SetDefault()
    {
        if (currentType == ShopKeeperType.CONSUMABLE)
        {
            consumablePer = defaultSpecialized;
            weaponPer = defaultOthers;
            armorPer = defaultOthers;
            miscPer = defaultOthers;
        }
        else if (currentType == ShopKeeperType.WEAPON)
        {
            consumablePer = defaultOthers;
            weaponPer = defaultSpecialized;
            armorPer = defaultOthers;
            miscPer = defaultOthers;
        }
        else if (currentType == ShopKeeperType.ARMOR)
        {
            consumablePer = defaultOthers;
            weaponPer = defaultOthers;
            armorPer = defaultSpecialized;
            miscPer = defaultOthers;
        }
        else
        {
            consumablePer = defaultOthers;
            weaponPer = defaultOthers;
            armorPer = defaultOthers;
            miscPer = defaultSpecialized;
        }
    }

    /// <summary>
    /// Called through dialogue response
    /// </summary>
    public void Buy()
    {
        GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.ShopkeeperBuyMenu);
    }

    /// <summary>
    /// Called through dialogue response
    /// </summary>
    public void Sell()
    {
        GameControl.UIManager.PushMenu(GameControl.UIManager.uiDatabase.ShopkeeperSellMenu);
    }

    public int GetItemPrice(InventoryItem item)
    {
        // find the scriptable object based on the inventory item
        var s_item = ItemDatabase.GetItem(GameControl.control.CurrentInventory, item.name);

        if(useCustom && customItemPrices.ContainsKey(s_item))
        {
            return customItemPrices[s_item];
        }
        

        // depending on the type of item, return based on the percentage
        if (s_item is ScriptableConsumable)
            return (int)(Mathf.Ceil(s_item.value * consumablePer));
        else if (s_item is ScriptableWeapon)
            return (int)(Mathf.Ceil(s_item.value * weaponPer));
        else if (s_item is ScriptableArmor)
            return (int)(Mathf.Ceil(s_item.value * armorPer));
        else if (s_item is ScriptableKey)
            return (int)(Mathf.Ceil(s_item.value * miscPer));
        else
            return -1;
    }
}
