using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ListButton : MonoBehaviour {

    public Text techName;
    public Text pmCost;
    public Image techIcon;
    public Image typeIcon;

    BattleManager battleManager;
    Technique thisTech;
    public Technique ThisTech { get { return thisTech; } }
        
    public void SetTechnique(Technique tech)
    {
        if(battleManager == null)
            battleManager = FindObjectOfType<BattleManager>();

        thisTech = tech;
        techName.text = thisTech.Name;
        techIcon.sprite = thisTech.TreeImage;
        RefreshPMCost();

        // Hide techIcon for now if null
        if (techIcon.sprite == null)
            techIcon.gameObject.SetActive(false);

        // FOR NOW, JUST DISABLE TYPE ICON, AS I DON'T KNOW IF WE'RE HAVING ELEMENTAL TYPES FOR TECHNIQUES
        typeIcon.gameObject.SetActive(false);

    }

    public void RefreshPMCost()
    {
        // determine cost based on status state
        int cost = thisTech.Pm;
        Color textColor = pmCost.color;
        if (battleManager.CurrentDenigen.StatusState == DenigenData.Status.cursed)
        {
            cost = thisTech.Pm * 2;
            textColor = Color.red;
        }
        //else
        //{
        //    cost = thisTech.Pm;
        //    textColor = Color.white;
        //}

        pmCost.text = cost.ToString();
        pmCost.color = textColor;
    }

//    public void SetItem(Item item)
//    {
//        techName.text = item.name;
//        RefreshItemQuantity(item);
//        techIcon.sprite = item.sprite;
//        typeIcon.gameObject.SetActive(false);        
//    }

	public void SetItem(InventoryItem item)
    {
		var _item = ItemDatabase.GetItem ("Consumable", item.name);
	    techName.text = item.name;
        RefreshItemQuantity(item);
        techIcon.sprite = _item.sprite;
        typeIcon.gameObject.SetActive(false);        
    }

    //public void RefreshItemQuantity(Item item)
	public void RefreshItemQuantity(InventoryItem item)
    {
        //print("refresh quantity to: " + item.quantity);
		pmCost.text = item.quantity.ToString();
        //pmCost.text = (item.quantity - item.GetComponent<ConsumableItem>().inUse).ToString();
    }
}
