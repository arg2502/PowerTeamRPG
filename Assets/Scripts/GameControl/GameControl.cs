using UnityEngine;
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
    public List<GameObject> consumables;// = new List<ConsumableItem>() { };
    public List<GameObject> equipment;
    public List<GameObject> weapons;
    public List<GameObject> reusables;

    //room information
    public string currentScene; // the place where the player currently is (outside of battle)
    public string previousScene; // the place where the player is coming from (outside of battle)
    public string savedScene; // the room where the player last saved
    public bool taggedStatue;
    public Vector2 savedStatue; // the position of where the player last saved -- dungeon entrance as default
    public Vector2 currentPosition; // the exact spot the player is in a room before a battle
    public Vector2 areaEntrance; // where the player will be kicked back to if they die

    public List<HeroData> heroList = new List<HeroData>() { }; // stores all of our hero's stats

    public int areaLevel; // mean level of enemies, determined by an enemyControl obj
    public int numOfEnemies; // number of enemies in battle, determined by an enemyControl obj
    public List<Enemy> enemies; // the type of enemies in battle, determined by an enemyControl obj

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
            heroList[0].expToLvlUp = 10;
            heroList[0].levelUpPts = 0;
            heroList[0].techPts = 0;
            heroList[0].hp = 14;
            heroList[0].hpMax = 14;
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

    public void AddItem(GameObject item)
    {
        // loop through the inventory to see if the player already has this type of item
        // if so, increase the quantity of that item
        if (item.GetComponent<ReusableItem>() != null)
        {
            foreach (GameObject i in reusables)
            {
                if (i.GetComponent<ReusableItem>().name == item.GetComponent<ReusableItem>().name) { i.GetComponent<ReusableItem>().quantity++; Destroy(item); return; }
            }

            // if this type of item is not already in the inventory, add it now
            reusables.Add(item);
            DontDestroyOnLoad(reusables[reusables.Count - 1]);
        }
        if (item.GetComponent<ConsumableItem>() != null)
        {
            foreach (GameObject i in consumables)
            {
                if (i.GetComponent<ConsumableItem>().name == item.GetComponent<ConsumableItem>().name) { i.GetComponent<ConsumableItem>().quantity++; Destroy(item); return; }
            }

            // if this type of item is not already in the inventory, add it now
            consumables.Add(item);
            DontDestroyOnLoad(consumables[consumables.Count - 1]);
        }
        if (item.GetComponent<ArmorItem>() != null)
        {
            foreach (GameObject i in equipment)
            {
                if (i.GetComponent<ArmorItem>().name == item.GetComponent<ArmorItem>().name) { i.GetComponent<ArmorItem>().quantity++; Destroy(item); return; }
            }

            // if this type of item is not already in the inventory, add it now
            equipment.Add(item);
            DontDestroyOnLoad(equipment[equipment.Count - 1]);
        }
        if (item.GetComponent<WeaponItem>() != null)
        {
            foreach (GameObject i in weapons)
            {
                if (i.GetComponent<WeaponItem>().name == item.GetComponent<WeaponItem>().name) { i.GetComponent<WeaponItem>().quantity++; Destroy(item); return; }
            }

            // if this type of item is not already in the inventory, add it now
            weapons.Add(item);
            DontDestroyOnLoad(weapons[weapons.Count - 1]);
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

        // record the player's position
        data.posX = currentPosition.x;
        data.posY = currentPosition.y;
        data.taggedStatue = taggedStatue;

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
            currentPosition = new Vector2(data.posX, data.posY);
            taggedStatue = data.taggedStatue;
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
    //public float statuePosX, statuePosY, statuePosZ; // the position of where the player last saved -- dungeon entrance as default
    public bool taggedStatue;
    public float posX, posY, posZ; //The exact position where the player was upon saving. This will probably be removed to avoid abuse and exploits
    public List<HeroData> heroList = new List<HeroData>() { };
    public List<Item> inventory = new List<Item>() { }; // All of the player's items
}

//this class should hold all of the stuff necessary for a hero object
[Serializable]
public class HeroData
{
    public string name;
    public int level, exp, expToLvlUp, levelUpPts, techPts;
    public int hp, hpMax, pm, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
    public List<string> skillsList, skillsDescription, spellsList, spellsDescription;
    // status effect
    public enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead };
    public Status statusState;
    //Also need passives, equipment
}
