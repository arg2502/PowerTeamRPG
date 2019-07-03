using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "UIDatabase", menuName = "Database/HeroData", order = 3)]
public class HeroData : DenigenData {
    
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
        }
        else
        {
            expCurLevel += expToAdd;
        }
    }

}
