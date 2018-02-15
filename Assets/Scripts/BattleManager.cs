using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager {

    List<Denigen> denigenList = new List<Denigen>();

	public BattleManager ()
    {
        AddHeroes();
        CreateEnemies();
	}

    void AddHeroes()
    {
        foreach(var hero in GameControl.control.heroList)
        {
            
        }
    }

    void CreateEnemies()
    {

    }
}
