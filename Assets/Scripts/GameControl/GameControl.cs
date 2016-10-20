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
    //items
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

    // things for saving the state of the pause menu - temporary, not to be written to a file
    public bool isPaused;
    public tempMenu pause;
    public tempMenu teamSub;
    public tempMenu heroSub;
    public tempMenu inventSub;

    // for temporarily saving the state of enemies when pausing and unpausing the game
    public List<SerializableVector3> enemyPos = new List<SerializableVector3>();

    // for telling the pause menu which list of items to use
    public string whichInventory; 

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
            heroList[0].passiveList = new List<Passive>();
            // Passives are non serializable now because they inherit from something with a my button variable
            heroList[0].passiveList.Add(new LightRegeneration());
            heroList[0].weapon = null;
            heroList[0].equipment = new List<GameObject>();

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

            pause = new tempMenu();
            teamSub = new tempMenu();
            heroSub = new tempMenu();
            inventSub = new tempMenu();
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
            SavableHeroData temp = new SavableHeroData();
            temp.identity = heroList[i].identity;
            temp.statBoost = heroList[i].statBoost;
            temp.skillTree = heroList[i].skillTree;
            temp.name = heroList[i].name;
            temp.level = heroList[i].level;
            temp.exp = heroList[i].exp;
            temp.expToLvlUp = heroList[i].expToLvlUp;
            temp.levelUpPts = heroList[i].levelUpPts;
            temp.techPts = heroList[i].techPts;
            temp.hp = heroList[i].hp;
            temp.hpMax = heroList[i].hpMax;
            temp.pm = heroList[i].pm;
            temp.pmMax = heroList[i].pmMax;
            temp.atk = heroList[i].atk;
            temp.def = heroList[i].def;
            temp.mgkAtk = heroList[i].mgkAtk;
            temp.mgkDef = heroList[i].mgkDef;
            temp.luck = heroList[i].luck;
            temp.evasion = heroList[i].evasion;
            temp.spd = heroList[i].spd;
            temp.skillsList = heroList[i].skillsList;
            temp.spellsList = heroList[i].spellsList;
            temp.passiveList = heroList[i].passiveList;
            temp.statusState = (SavableHeroData.Status)heroList[i].statusState;

            if (heroList[i].weapon != null)
            {
                Item item = heroList[i].weapon.GetComponent<Item>();
                ItemData id = new ItemData();
                id.name = item.name;
                id.quantity = item.quantity;
                temp.weapon = id;
            }

            temp.equipment = new List<ItemData>();
            for (int j = 0; j < heroList[i].equipment.Count; j++)
            {
                print("Index: " + j);
                print("Saving item:" + heroList[i].equipment[j].name);
                Item item = heroList[i].equipment[j].GetComponent<Item>();
                ItemData id = new ItemData();
                id.name = item.name;
                id.quantity = item.quantity;
                temp.equipment.Add(id);
                print("Saved Item data: " + temp.equipment[j].name);
            }

            data.heroList.Add(temp);
        }
        //Save which scene the player is in
        data.currentScene = currentScene;
        data.totalGold = totalGold;

        // Save all of the player's inventory
        foreach (GameObject i in consumables)
        {
            Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = item.name;
            id.quantity = item.quantity;
            data.consumables.Add(id);
        }
        foreach (GameObject i in reusables)
        {
            Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = item.name;
            id.quantity = item.quantity;
            data.reusables.Add(id);
        }
        foreach (GameObject i in weapons)
        {
            Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = item.name;
            id.quantity = item.quantity;
            data.weapons.Add(id);
        }
        foreach (GameObject i in equipment)
        {
            Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = item.name;
            id.quantity = item.quantity;
            data.equipment.Add(id);
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
                for(int i = 0; i < rc.treasureChests.Count; i++)
                {
                    rcd.chestData[i].isChestOpen = rc.treasureChests[i].isOpen;
                    rcd.chestData[i].chestName = rc.treasureChests[i].name;
                }
            }
        }
    }

    public void RecordEnemyPos()
    {
        enemyControl[] enemies = GameObject.FindObjectsOfType<enemyControl>();
        enemyPos = new List<SerializableVector3>();
        SerializableVector3 temp = new SerializableVector3();
        for (int i = 0; i < enemies.Length; i++)
        {
            temp = new SerializableVector3();
            temp.x = enemies[i].transform.position.x;
            temp.y = enemies[i].transform.position.y;
            temp.z = enemies[i].transform.position.z;
            enemyPos.Add(temp);
        }
    }

    // save the menu for when you transition to an external portion of the menu
    public void RecordPauseMenu()
    {
        PauseMenu tempPause = GameObject.FindObjectOfType<PauseMenu>();
        pause.isActive = tempPause.isActive;
        pause.isVisible = tempPause.isVisible;
        pause.selectedIndex = tempPause.SelectedIndex;
        pause.position = tempPause.transform.position;

        TeamSubMenu tempTeamSub = GameObject.FindObjectOfType<TeamSubMenu>();
        teamSub.isActive = tempTeamSub.isActive;
        teamSub.isVisible = tempTeamSub.isVisible;
        teamSub.selectedIndex = tempTeamSub.SelectedIndex;
        teamSub.position = tempTeamSub.transform.position;

        HeroSubMenu tempHeroSub = GameObject.FindObjectOfType<HeroSubMenu>();
        heroSub.isVisible = tempHeroSub.isVisible;
        heroSub.selectedIndex = tempHeroSub.SelectedIndex;
        heroSub.position = tempHeroSub.transform.position;

        InventorySubMenu tempInventSub = GameObject.FindObjectOfType<InventorySubMenu>();
        inventSub.isVisible = tempInventSub.isVisible;
        inventSub.selectedIndex = tempInventSub.SelectedIndex;
        inventSub.position = tempInventSub.transform.position;
    }

    // called if the isPaused variable is set to true
    public void RestorePauseMenu()
    {
        PauseMenu tempPause = GameObject.FindObjectOfType<PauseMenu>();
        TeamSubMenu tempTeamSub = GameObject.FindObjectOfType<TeamSubMenu>();
        HeroSubMenu tempHeroSub = GameObject.FindObjectOfType<HeroSubMenu>();
        InventorySubMenu tempInventSub = GameObject.FindObjectOfType<InventorySubMenu>();

        if (pause.isVisible) 
        {
            tempPause.isVisible = true;
            tempPause.EnablePauseMenu();
            tempPause.player.ToggleMovement();
            tempPause.isActive = pause.isActive;
            tempPause.SelectedIndex = pause.selectedIndex;
            tempPause.HighlightButton();
            tempPause.transform.position = pause.position;
        }
        if (teamSub.isVisible)
        {
            tempTeamSub.isVisible = true;
            tempTeamSub.EnableSubMenu();
            tempTeamSub.isActive = teamSub.isActive;
            tempTeamSub.SelectedIndex = teamSub.selectedIndex;
            tempTeamSub.HighlightButton();
            tempTeamSub.transform.position = teamSub.position;
        }
        if (heroSub.isVisible)
        {
            tempHeroSub.isVisible = true;
            tempHeroSub.EnableSubMenu();
            tempHeroSub.SelectedIndex = heroSub.selectedIndex;
            tempHeroSub.HighlightButton();
            tempHeroSub.transform.position = heroSub.position;
        }
        if (inventSub.isVisible)
        {
            tempInventSub.isVisible = true;
            tempInventSub.EnableSubMenu();
            tempInventSub.SelectedIndex = inventSub.selectedIndex;
            tempInventSub.HighlightButton();
            tempInventSub.transform.position = inventSub.position;
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
           
            //Make sure the item lists are cleared before adding more
            consumables.Clear();
            reusables.Clear();
            weapons.Clear();
            equipment.Clear();

            //Find all items and destroy them
            Item[] items = FindObjectsOfType<Item>();
            foreach (Item i in items) { Destroy(i.gameObject); }

            // Read in all consumable items
            foreach (ItemData id in data.consumables) { LoadConsumableItem(id); }

            //Read in all reusable items
            foreach (ItemData id in data.reusables) { LoadReusableItem(id); }

            //read in all weapons
            foreach (ItemData id in data.weapons) { LoadWeaponItem(id, null); }

            //read in all equipment
            foreach (ItemData id in data.equipment) { LoadArmorItem(id, null); }

            //load all of the heroes
            for (int i = 0; i < data.heroList.Count; i++)
            {
                HeroData temp = new HeroData();
                temp.identity = data.heroList[i].identity;
                temp.statBoost = data.heroList[i].statBoost;
                temp.skillTree = data.heroList[i].skillTree;
                temp.name = data.heroList[i].name;
                temp.level = data.heroList[i].level;
                temp.exp = data.heroList[i].exp;
                temp.expToLvlUp = data.heroList[i].expToLvlUp;
                temp.levelUpPts = data.heroList[i].levelUpPts;
                temp.techPts = data.heroList[i].techPts;
                temp.hp = data.heroList[i].hp;
                temp.hpMax = data.heroList[i].hpMax;
                temp.pm = data.heroList[i].pm;
                temp.pmMax = data.heroList[i].pmMax;
                temp.atk = data.heroList[i].atk;
                temp.def = data.heroList[i].def;
                temp.mgkAtk = data.heroList[i].mgkAtk;
                temp.mgkDef = data.heroList[i].mgkDef;
                temp.luck = data.heroList[i].luck;
                temp.evasion = data.heroList[i].evasion;
                temp.spd = data.heroList[i].spd;
                temp.skillsList = data.heroList[i].skillsList;
                temp.spellsList = data.heroList[i].spellsList;
                temp.passiveList = data.heroList[i].passiveList;
                temp.statusState = (HeroData.Status)data.heroList[i].statusState;

                if (data.heroList[i].weapon != null) { print("Loading Item: " + data.heroList[i].weapon.name); LoadWeaponItem(data.heroList[i].weapon, temp); }
                temp.equipment = new List<GameObject>();
                if (data.heroList[i].equipment.Count > 0) { for (int j = 0; j < data.heroList[i].equipment.Count; j++) { LoadArmorItem(data.heroList[i].equipment[j], temp); } }

                heroList.Add(temp);
            }

            //put the player back where they were
            UnityEngine.SceneManagement.SceneManager.LoadScene(data.currentScene);
            // Put their position vector here if we choose
            currentPosition = new Vector2(data.posX, data.posY);
            taggedStatue = data.taggedStatue;
            totalGold = data.totalGold;

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

    public void LoadReusableItem(ItemData id)
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

    public void LoadArmorItem(ItemData id, HeroData hd)
    {
        GameObject temp = null;
        switch (id.name)
        {
            case "Helmet of Fortitude":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/HelmetOfFortitude"));
                break;
            default:
                print("Error: Incorrect item name - " + id.name);
                break;
        }
        if (hd == null)
        {
            GameControl.control.AddItem(temp);
            GameControl.control.equipment[equipment.Count - 1].GetComponent<ArmorItem>().quantity = id.quantity;
        }
        else { hd.equipment.Add(temp); DontDestroyOnLoad(temp); }
    }

    public void LoadWeaponItem(ItemData id, HeroData hd)
    {
        GameObject temp = null;
        switch (id.name)
        {
            case "Spare Sword":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/SpareSword"));
                break;
            case "Tome of Practical Spells":
                temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/TomeOfPractical"));
                break;
            default:
                print("Error: Incorrect item name - " + id.name);
                break;
        }
        if (hd == null)
        {
            GameControl.control.AddItem(temp);
            GameControl.control.weapons[weapons.Count - 1].GetComponent<WeaponItem>().quantity = id.quantity;
        }
        else { hd.weapon = temp; DontDestroyOnLoad(temp); }
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
    public List<SavableHeroData> heroList = new List<SavableHeroData>() { };
    public List<RoomControlData> rooms = new List<RoomControlData>() { }; // stores all of the data for areas the player has been to, like block positions, etc.

    // All of the player's items
    public List<ItemData> consumables = new List<ItemData>() { };
    public List<ItemData> reusables = new List<ItemData>() { };
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
    // Need a creative way to store which items are equipped since items are non-serializable
    public GameObject weapon;
    public List<GameObject> equipment;
}

// this class exists to save the hero item
[Serializable]
public class SavableHeroData
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
    // Need a creative way to store which items are equipped since items are non-serializable
    public ItemData weapon;
    public List<ItemData> equipment;
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
    public List<TreasureData> chestData = new List<TreasureData>();
}

[Serializable]
public class TreasureData
{
    public bool isChestOpen;
    public string chestName;
}

[Serializable]
public class SerializableVector3
{
    public float x, y, z;
}

[Serializable]
public class tempMenu
{
    public bool isVisible;
    public bool isActive;
    public int selectedIndex;
    public bool isDisabled;
    public Vector2 position;
}