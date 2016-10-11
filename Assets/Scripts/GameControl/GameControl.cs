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

    public List<RoomControlData> rooms = new List<RoomControlData>() { }; // stores all of the data for areas the player has been to, like block positions, etc.
    // probably going to need a roomData class for long term saving of the abouve list
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
            heroList[0].name = playerName;
            heroList[0].identity = 0;
            heroList[0].level = 1;
            heroList[0].exp = 0;
            heroList[0].expToLvlUp = 10;
            heroList[0].levelUpPts = 0;
            heroList[0].techPts = 0;
            heroList[0].hp = 11;
            heroList[0].hpMax = 11;
            heroList[0].pm = 3;
            heroList[0].pmMax = 3;
            heroList[0].atk = 6;
            heroList[0].def = 6;
            heroList[0].mgkAtk = 4;
            heroList[0].mgkDef = 4;
            heroList[0].luck = 6;
            heroList[0].evasion = 4;
            heroList[0].spd = 4;
            heroList[0].skillsList = new List<Skill>();
            heroList[0].spellsList = new List<Spell>();
            //heroList[0].skillsList.Add()

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
        data.currentScene = currentScene;

        // Save all of the player's inventory
        foreach (GameObject i in consumables)
        {
            Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = item.name;
            id.quantity = item.quantity;
            data.consumables.Add(id);
        }

        // record the player's position
        data.posX = currentPosition.x;
        data.posY = currentPosition.y;
        data.taggedStatue = taggedStatue;

        // save all of the changes player made in the overworld
        data.rooms = rooms;

        //Writes our player class to a file
        bf.Serialize(file, data);
        file.Close();
    }

    public void RecordRoom()
    {
        //save the current room, to acheive persistency while paused
        roomControl rc = GameObject.FindObjectOfType<roomControl>();
        foreach (RoomControlData rcd in rooms)
        {
            if (rcd.areaIDNumber == rc.areaIDNumber) // find the appropriate room
            {
                for (int i = 0; i < rc.movables.Count; i++)// save the movable block positions
                {
                    rcd.movableBlockPos[i].x = rc.movables[i].transform.position.x;
                    rcd.movableBlockPos[i].y = rc.movables[i].transform.position.y;
                    rcd.movableBlockPos[i].z = rc.movables[i].transform.position.z;
                }
            }
        }
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

            //Make sure the item lists are cleared before adding more
            consumables.Clear();
            reusables.Clear();
            weapons.Clear();
            equipment.Clear();

            //Find all items and destroy them
            Item[] items = FindObjectsOfType<Item>();
            foreach (Item i in items) { Destroy(i.gameObject); }

            // Read in all consumable items
            foreach (ItemData id in data.consumables)
            {
                LoadConsumableItem(id);
            }

            //Read in all reusable items
            //read in all weapons
            //read in all equipment

            //put the player back where they were
            UnityEngine.SceneManagement.SceneManager.LoadScene(data.currentScene);
            // Put their position vector here if we choose
            currentPosition = new Vector2(data.posX, data.posY);
            taggedStatue = data.taggedStatue;

            // put all interactable item data back
            rooms = data.rooms;
        }
    }

    public void LoadConsumableItem(ItemData id)
    {
        GameObject temp = null;
        switch (id.name)
        {
            case "Lesser Restorative":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserRestorative"));
                break;
            case "Restorative":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Restorative"));
                break;
            case "Gratuitous Restorative":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/GratuitousRestorative"));
                break;
            case "Terminal Restorative":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/TerminalRestorative"));
                break;
            case "Lesser Elixir":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserElixir"));
                break;
            case "Elixir":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Elixir"));
                break;
            case "Gratuitous Elixir":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/GratuitousElixir"));
                break;
            case "Terminal Elixir":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/TerminalElixir"));
                break;
            default:
                print("Error: Incorrect item name - " + id.name);
                break;
        }
        GameControl.control.AddItem(temp);
        GameControl.control.consumables[consumables.Count - 1].GetComponent<ConsumableItem>().quantity = id.quantity;
    }

    public void LoadReuseableItem(ItemData id)
    {
        GameObject temp = null;
        switch (id.name)
        {
            default:
                print("Error: Incorrect item name - " + id.name);
                break;
        }
        GameControl.control.AddItem(temp);
        GameControl.control.reusables[reusables.Count - 1].GetComponent<ReusableItem>().quantity = id.quantity;
    }

    public void LoadArmorItem(ItemData id)
    {
        GameObject temp = null;
        switch (id.name)
        {
            default:
                print("Error: Incorrect item name - " + id.name);
                break;
        }
        GameControl.control.AddItem(temp);
        GameControl.control.equipment[equipment.Count - 1].GetComponent<ArmorItem>().quantity = id.quantity;
    }

    public void LoadWeaponItem(ItemData id)
    {
        GameObject temp = null;
        switch (id.name)
        {
            default:
                print("Error: Incorrect item name - " + id.name);
                break;
        }
        GameControl.control.AddItem(temp);
        GameControl.control.weapons[weapons.Count - 1].GetComponent<WeaponItem>().quantity = id.quantity;
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
    public List<RoomControlData> rooms = new List<RoomControlData>() { }; // stores all of the data for areas the player has been to, like block positions, etc.

    // All of the player's items
    public List<ItemData> consumables = new List<ItemData>() { };
    public List<ItemData> reuseables = new List<ItemData>() { };
    public List<ItemData> equipment = new List<ItemData>() { };
    public List<ItemData> weapons = new List<ItemData>() { }; 
}

//this class should hold all of the stuff necessary for a hero object
[Serializable]
public class HeroData
{
    public int identity;
    public bool statBoost = false;
    public bool skillTree = false;
    public string name;
    public int level, exp, expToLvlUp, levelUpPts, techPts;
    public int hp, hpMax, pm, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
    public List<Skill> skillsList;
    public List<Spell> spellsList;
    public List<Passive> passiveList;
    // status effect
    public enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead };
    public Status statusState;
    //Also need passives, equipment
}

//this class should hold all of the stuff necessary for an item object
[Serializable]
public class ItemData
{
    public string name;
    public int quantity;
}

//this class will store all the stuff in a roomControl object that the player influences
[Serializable]
public class RoomControlData
{
    public int areaIDNumber;
    public List<SerializableVector3> movableBlockPos = new List<SerializableVector3>(); // the positions of all of the movable blocks
    //also store treasure boxes, doors, switches, antyhing important
}

[Serializable]
public class SerializableVector3
{
    public float x, y, z;
}