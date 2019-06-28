using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "UIDatabase", menuName = "Database/HeroData", order = 3)]
public class HeroData : DenigenData {


    //public HeroData() { Debug.Log("Inside HeroData -- Hello, my name is " + denigenName); }
    

   

    /// <summary>
    /// Searches through the hero's equipment/armor and returns true if the hero already the item.
    /// </summary>
    /// <param name="itemToCheck"></param>
    /// <returns></returns>
    //public bool EquipmentContainsItem(Item itemToCheck)
    //{
    //    foreach (var itemObj in equipment)
    //    {
    //        if (itemObj.GetComponent<Item>() == itemToCheck)
    //            return true;
    //    }

    //    return false;
    //}


    public void AddExp(int expToAdd)
    {
        exp += expToAdd;
        expToLvlUp -= expToAdd;

        //Level up the hero, if necessary
        if (expToLvlUp <= 0)
        {
            // keep the rollover experience
            int extraExp = Mathf.Abs(expToLvlUp);
            LevelUp(extraExp);
            //UpdateCard(h);
            //h.statBoost = true;
            //h.skillTree = true;
            //levelUp = true; // tells the game to go to the level up scene
        }
        else
        {
            expCurLevel += expToAdd;
        }

        // HANDLE GOING TO STAT BOOST AND SKILL TREE MENUS LATER
        // maybe have a callback function passed through constructor? if (levelUp) callback.Invoke(); ??
        // -- AG 4/29/18
    }

}
