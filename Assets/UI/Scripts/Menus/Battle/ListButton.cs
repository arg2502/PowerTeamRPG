using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ListButton : MonoBehaviour {

    public Text techName;
    public Text pmCost;
    public Image techIcon;
    public Image typeIcon;

    BattleManager battleManager;
    Technique currentTech;
        
    public void SetTechnique(Technique tech)
    {
        if(battleManager == null)
            battleManager = FindObjectOfType<BattleManager>();

        currentTech = tech;
        techName.text = currentTech.Name;
        techIcon.sprite = currentTech.TreeImage;
        RefreshPMCost();        

        // FOR NOW, JUST DISABLE TYPE ICON, AS I DON'T KNOW IF WE'RE HAVING ELEMENTAL TYPES FOR TECHNIQUES
        typeIcon.gameObject.SetActive(false);

    }

    public void RefreshPMCost()
    {
        // determine cost based on status state
        int cost;
        Color textColor;
        if (battleManager.CurrentHero.StatusState == DenigenData.Status.cursed)
        {
            cost = currentTech.Pm * 2;
            textColor = Color.red;
        }
        else
        {
            cost = currentTech.Pm;
            textColor = Color.black;
        }

        pmCost.text = cost.ToString();
        pmCost.color = textColor;
    }

    public void SetItem(Item item)
    {
        techName.text = item.name;
        RefreshItemQuantity(item);
        techIcon.sprite = item.sprite;
        typeIcon.gameObject.SetActive(false);        
    }

    public void RefreshItemQuantity(Item item)
    {
        print("refresh quantity to: " + item.quantity);
        pmCost.text = (item.quantity - item.GetComponent<ConsumableItem>().inUse).ToString();
    }
}
