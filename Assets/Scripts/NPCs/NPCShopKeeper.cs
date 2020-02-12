using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShopKeeper : StationaryNPCControl {

    public TextAsset commentList;
    public List<ScriptableItem> shopInventory;    

    [System.Serializable]
    public enum ShopKeeperType { CONSUMABLE, ARMOR, AUGMENT, MISC }
    public ShopKeeperType currentType;

    public float consumablePer;
    public float armorPer;
    public float augmentPer;
    public float miscPer;

    float defaultSpecialized = 0.9f;
    float defaultOthers = 0.6f;
    
    bool useDefault = true;
    bool useCustom;
    
    public Dictionary<ScriptableItem, int> customItemPrices;
    
    public string npcName { get { return GetComponentInChildren<NPCDialogue>().npcName; } }
    public Sprite neutralSpr { get { return GetComponentInChildren<NPCDialogue>().neutralSpr; } }
    public Sprite happySpr { get { return GetComponentInChildren<NPCDialogue>().happySpr; } }
    public Sprite sadSpr { get { return GetComponentInChildren<NPCDialogue>().sadSpr; } }
    public Sprite angrySpr { get { return GetComponentInChildren<NPCDialogue>().angrySpr; } }


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
            armorPer = defaultOthers;
            augmentPer = defaultOthers;
            miscPer = defaultOthers;
        }
        else if (currentType == ShopKeeperType.ARMOR)
        {
            consumablePer = defaultOthers;
            armorPer = defaultSpecialized;
            augmentPer = defaultOthers;
            miscPer = defaultOthers;
        }
        else if (currentType == ShopKeeperType.AUGMENT)
        {
            consumablePer = defaultOthers;
            armorPer = defaultOthers;
            augmentPer = defaultSpecialized;
            miscPer = defaultOthers;
        }
        else
        {
            consumablePer = defaultOthers;
            armorPer = defaultOthers;
            augmentPer = defaultOthers;
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

    public void Talk()
    {
        GameControl.UIManager.PushNotificationMenu(
            "This should open up a menu with dialogue options...but not yet"
            );
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
        else if (s_item is ScriptableArmor)
            return (int)(Mathf.Ceil(s_item.value * armorPer));
        else if (s_item is ScriptableAugment)
            return (int)(Mathf.Ceil(s_item.value * augmentPer));
        else if (s_item is ScriptableKey)
            return (int)(Mathf.Ceil(s_item.value * miscPer));
        else
            return -1;
    }

    public void StartDialogue()
    {
        GetComponentInChildren<NPCDialogue>().StartDialogue();
    }
}
