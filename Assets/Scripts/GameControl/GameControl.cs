﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

    //This class is sort of a singleton, but not really.
    //This will hold our persistent data between rooms, like the heroes, and equipment

    //attributes
    public static GameControl control;

    //the player's name (Jethro)
    public string playerName = "Jethro";

    //Info to be saved and used throughout the game
    public int totalGold; // the player's total gold
    //items -- add later
    public string currentScene; // the place where the player currently is (outside of battle)
    public string previousScene; // the place where the player is coming from (outside of battle)
    public string savedScene; // the room where the player last saved
    public Vector3 savedStatue; // the position of where the player last saved -- dungeon entrance as default
    public Vector3 currentPosition; // the exact spot the player is in a room before a battle

    public List<HeroData> heroList = new List<HeroData>() { };

    //awake gets called before start
    void Awake () {
        if (control == null)
        {
            //this keeps the game object from being destroyed between scenes
            DontDestroyOnLoad(gameObject);
            control = this;

            totalGold = 0;
            //test code for creating Jethro -- based on level 1 stats
            //We will have these stats stored in HeroData objs for consistency between rooms
            heroList.Add(new HeroData());
            heroList[0].name = "Jethro";
            heroList[0].level = 1;
            heroList[0].exp = 0;
            heroList[0].levelUpPts = 0;
            heroList[0].techPts = 0;
            heroList[0].hp = 8;
            heroList[0].hpMax = 8;
            heroList[0].pm = 4;
            heroList[0].pmMax = 4;
            heroList[0].atk = 8;
            heroList[0].def = 7;
            heroList[0].mgkAtk = 6;
            heroList[0].mgkDef = 5;
            heroList[0].luck = 7;
            heroList[0].evasion = 5;
            heroList[0].spd = 6;

            //test code for creating Cole -- based on level 2 stats
            //We will have these stats stored in HeroData objs for consistency between rooms
            //heroList.Add(new HeroData());
            //heroList[1].name = "Cole";
            //heroList[1].level = 2;
            //heroList[1].exp = 0;
            //heroList[1].levelUpPts = 0;
            //heroList[1].techPts = 0;
            //heroList[1].hp = 10;
            //heroList[1].hpMax = 10;
            //heroList[1].pm = 17;
            //heroList[1].pmMax = 17;
            //heroList[1].atk = 9;
            //heroList[1].def = 6;
            //heroList[1].mgkAtk = 18;
            //heroList[1].mgkDef = 13;
            //heroList[1].luck = 10;
            //heroList[1].evasion = 8;
            //heroList[1].spd = 9;
        }
        else if (control != this)
        {
            //If a gameControl already exists, we don't want 2 of them
            Destroy(gameObject);
        }
        
	}

    //this will save our game data to an external, persistent file
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        //data.health = health;
        //save all of the heroes
        for (int i = 0; i < heroList.Count; i++)
        {
            data.heroList.Add(heroList[i]);
        }
        //Save which scene the player is in
        data.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;


        //Writes our player class to a file
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            //clear any current data
            heroList = new List<HeroData>() { };
            //sets local data to the stored data
            //health = data.health;
            //load all of the heroes
            for (int i = 0; i < data.heroList.Count; i++)
            {
                heroList.Add(data.heroList[i]);
            }
            //put the player back where they were
            UnityEngine.SceneManagement.SceneManager.LoadScene(data.currentScene);
            // Put their position vector here if we choose
        }
    }
}

//this class is where all of our data will be sent in order to be saved
[Serializable]
class PlayerData
{
    public int totalGold; // the player's total gold
    //items -- add later
    public string currentScene; // the place where the currently is (outside of battle)
    public string savedScene; // the room where the player last saved
    public float statuePosX, statuePosY, statuePosZ; // the position of where the player last saved -- dungeon entrance as default
    public float posX, posY, posZ; //The exact position where the player was upon saving. This will probably be removed to avoid abuse and exploits
    public List<HeroData> heroList = new List<HeroData>() { };
}

//this class should hold all of the stuff necessary for a hero object
[Serializable]
public class HeroData
{
    public string name;
    public int level, exp, levelUpPts, techPts;
    public int hp, hpMax, pm, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
    public List<string> skillsList, skillsDescription, spellsList, spellsDescription;
    //Also need passives, equipment
}