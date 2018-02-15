using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "UIDatabase", menuName = "Database/HeroData", order = 2)]
public class HeroData : DenigenData {


    //public HeroData() { Debug.Log("Inside HeroData -- Hello, my name is " + denigenName); }
    

   

    /// <summary>
    /// Searches through the hero's equipment/armor and returns true if the hero already the item.
    /// </summary>
    /// <param name="itemToCheck"></param>
    /// <returns></returns>
    public bool EquipmentContainsItem(Item itemToCheck)
    {
        foreach (var itemObj in equipment)
        {
            if (itemObj.GetComponent<Item>() == itemToCheck)
                return true;
        }

        return false;
    }

}
