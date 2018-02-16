using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

    List<Denigen> denigenList = new List<Denigen>();

    public Transform heroContainer;

    // starting positions
    Vector3 jethroStart = new Vector3(1.5f, -3f);
    Vector3 coleStart = new Vector3(-3f, -1f);
    Vector3 eleanorStart = new Vector3(0f, 3f);
    Vector3 joulietteStart = new Vector3(4.5f, 1f);
    List<Vector3> startingPositions;

	void Start ()
    {
        startingPositions = new List<Vector3>() { jethroStart, coleStart, eleanorStart, joulietteStart };
        AddHeroes();
        CreateEnemies();
        PrintHeroes();
	}

    void AddHeroes()
    {
        foreach(var hero in GameControl.control.heroList)
        {
            CreateHero(hero.denigenName, hero.identity);
        }
    }
    
    void CreateHero(string heroName, int index) 
    {
        // special case -- if we're Jethro, our player name may not be Jethro, but the prefab is
        if (index == 0)
            heroName = "Jethro";

        var heroObj = GameObject.Instantiate(Resources.Load("Prefabs/HeroesBattle/" + heroName + "Prefab")) as GameObject;
        heroObj.transform.SetParent(heroContainer);
        var hero = heroObj.GetComponent<Hero>();
        hero.Data = GameControl.control.heroList[index];
        hero.transform.localPosition = startingPositions[index];
        denigenList.Add(hero);
    }
    
    void CreateEnemies()
    {
        // TEST TEST TEST OMG THIS IS JUST TO SEE IF THIS WORKS
        // THE CREATION OF ENEMIES AND THEIR DATA SHOULD BE HANDLED SEPARATELY INSIDE AN ENEMYMANAGER OF SORTS
        // THIS IS ONLY TO TEMPORARILY CREATE A GOIKKO ON THE SPOT -- PLEASE MOVE THIS LATER

        // create Enemy object
        var enemyObj = Instantiate(Resources.Load("Prefabs/EnemiesBattle/Goikko")) as GameObject;
        var enemy = enemyObj.GetComponent<Enemy>();

        // create data obj
        var enemyData = Resources.Load<EnemyData>("Data/Enemies/Goikko");

        // set Enemy's data obj to enemyData
        enemy.Data = enemyData;

        // set up enemy stats
        enemy.Init();

        // set position -- LATER

        // add to denigen list
        denigenList.Add(enemy);
    }

    void PrintHeroes()
    {
        foreach(var denigen in denigenList)
        {
            print("object: " + denigen.name);
            print("data: " + denigen.Data.denigenName);
        }
    }
}
