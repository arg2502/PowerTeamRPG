using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ListButton : MonoBehaviour {

    public Text techName;
    public Text pmCost;
    public Image techIcon;
    public Image typeIcon;

    public void SetTechnique(Technique tech)
    {
        techName.text = tech.Name;
        pmCost.text = tech.Pm.ToString();
        techIcon.sprite = tech.TreeImage;

        // FOR NOW, JUST DISABLE TYPE ICON, AS I DON'T KNOW IF WE'RE HAVING ELEMENTAL TYPES FOR TECHNIQUES
        typeIcon.gameObject.SetActive(false);

    }

    public void SetItem(Item item)
    {
        techName.text = item.name;
        pmCost.text = item.quantity.ToString();
        techIcon.sprite = item.sprite;
        typeIcon.gameObject.SetActive(false);


    }
}
