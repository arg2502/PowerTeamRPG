﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UI;

public class GameControl : MonoBehaviour {

	//This class is sort of a singleton, but not really.
	//This will hold our persistent data between rooms, like the heroes, and equipment

	//attributes
	public static GameControl control;

	//the player's name (Jethro)
	public string playerName = "Jethro";

    public static UIManager UIManager;
    public static SkillTreeManager skillTreeManager;
    public static ItemManager itemManager;
    public static AudioManager audioManager;

    //Info to be saved and used throughout the game
    public int totalGold; // the player's total gold
	public int totalKeys;
	public List<int> keysObtainedInDungeons = new List<int>();
	public int numOfDungeons;
	//items
//	public List<GameObject> consumables;// = new List<ConsumableItem>() { };
//	public List<GameObject> equipment;
//	public List<GameObject> weapons;
//	public List<GameObject> reusables;
//    public bool itemAdded;
	//----------new code------------------
	public List<InventoryItem> consumables;
	public List<InventoryItem> equipment;
	public List<InventoryItem> weapons;
	public List<InventoryItem> key;
	public bool itemAdded;
	//------------------------------------
    
    //room information
    public roomControl currentRoom; // keep track of the literal room object where the player currently is
	public string currentScene; // the place where the player currently is (outside of battle)
	public string previousScene; // the place where the player is coming from (outside of battle)
	public string savedScene; // the room where the player last saved
	public bool taggedStatue;
	public Vector2 savedStatue; // the position of where the player last saved -- dungeon entrance as default
	public Vector2 currentPosition; // the exact spot the player is in a room before a battle
	public Vector2 areaEntrance; // where the player will be kicked back to if they die
    public Gateway currentEntranceGateway;

	public List<HeroData> heroList = new List<HeroData>() { }; // stores all of our hero's stats

	public List<RoomControlData> rooms = new List<RoomControlData>() { }; // stores all of the data for areas the player has been to, like block positions, etc.
	// probably going to need a roomData class for long term saving of the abouve list
	public int areaLevel; // mean level of enemies, determined by an enemyControl obj
	public int numOfEnemies; // number of enemies in battle, determined by an enemyControl obj
	public List<EnemyData> enemies; // the type of enemies in battle, determined by an enemyControl obj

	// things for saving the state of the pause menu - temporary, not to be written to a file
	public bool isPaused;
	public tempMenu pause;
	public tempMenu teamSub;
	public tempMenu heroSub;
	public tempMenu inventSub;

	// for temporarily saving the state of enemies when pausing and unpausing the game
	public List<SerializableVector3> enemyPos = new List<SerializableVector3>();

	// for telling the pause menu which list of items to use
	//internal string whichInventory;    
    public enum WhichInventory { Consumables, Weapons, Equipment, KeyItems }
    internal WhichInventory whichInventoryEnum;
    public string CurrentInventory
    {
        get
        {
            if (whichInventoryEnum == WhichInventory.Consumables)
                return "consumable";
            else if (whichInventoryEnum == WhichInventory.Weapons)
                return "weapon";
            else if (whichInventoryEnum == WhichInventory.Equipment)
                return "armor";
            else
                return "key";
        }
    }

    // tells shopkeeper whether to open buy or sell menu
    // false - buy
    // true - sell
    public bool isSellMenu;
    	
    // check if an attack animation is playing
    public bool isAnimating = false;
    public bool isDying = false;

    // variable to access skill tree functions
    SkillTree skillTreeAccessor = new SkillTree();

    // var for keeping track of gateways between scenes
    public string sceneStartGateName;
    public void AssignEntrance(string gatewayName)
    {
        sceneStartGateName = gatewayName;
    }

    // room transition state --- to tell RoomControl how to handle positioning with loading the scene
    //public enum RoomTransitionState { menu, gateway }
    //public RoomTransitionState roomState;
    public characterControl.CharacterState currentCharacterState;

    public void SetCharacterState(characterControl.CharacterState newState)
    {
        currentCharacterState = newState;
    }
    
    public void WaitAFrameAndSetCharacterState(characterControl.CharacterState newState)
    {        
        StartCoroutine(IESetCharacterState(newState));
    }

    IEnumerator IESetCharacterState(characterControl.CharacterState newState)
    {
        yield return null; // waits one frame to let GetButtonDown finish, then set state (mainly for coming out of menu/dialogue)
        
        // In certain circumstances, when we pop out of a menu, a new one is opened
        // ex: shopkeepers, all menus are popped and then a new one opens
        // this all occurs during the one frame that we wait,
        // so before we set the state, make sure there are no menus open first
        if (UIManager.list_currentMenus.Count == 0)
            currentCharacterState = newState;
    }

    // states for checking if the player is in a menu/talking
    //bool isInMenu;
    //public bool IsInMenu { get { return IsInMenu; } set { isInMenu = value; } }
    characterControl.CharacterState prevState;
    public characterControl.CharacterState PrevState { get { return prevState; } set { prevState = value; } }

    public characterControl.HeroCharacter currentCharacter;
    public int currentCharacterInt { get { return (int)currentCharacter; } }

    // Current NPC
    public NPCDialogue currentNPC; // we can only talk to one NPC at a time, this variable will keep that one in focus
    public NPCPathwalkControl CurrentNPCPathwalk { get { return currentNPC.GetComponentInParent<NPCPathwalkControl>(); } }
	public StationaryNPCControl CurrentStationaryNPC { get { return currentNPC.GetComponentInParent<StationaryNPCControl>(); } }
    public ShopKeeperDialogue CurrentShopkeeper { get { return currentNPC.GetComponent<ShopKeeperDialogue>(); } }

    public AudioClip battleIntro;
    public AudioClip battleLoop;

    //awake gets called before start
    void Awake () {
		if (control == null)
		{
			//this keeps the game object from being destroyed between scenes
			DontDestroyOnLoad(gameObject);
			control = this;

            InitHeroes();

            //Debug.Log(heroList);

            UIManager = new UIManager();
            skillTreeManager = new SkillTreeManager();
            itemManager = new ItemManager();
            audioManager = GetComponentInChildren<AudioManager>();
            audioManager.Init();
            
			totalGold = 0;
			totalKeys = 0;

			// create list keeping track of dungeon keys, start off at zero keys
			numOfDungeons = 2;
			for (int i = 0; i < numOfDungeons; i++)
			{
				keysObtainedInDungeons.Add(0);
			}




            //Spell hellfire = new Spell();
            //hellfire.Name = "HellFire";
            //hellfire.Pm = 8;
            //hellfire.Description = "An all-encompassing spell with considerable power and accuracy. \nCost 8pm Str 40, Crit 03, Acc 100";
            //GameControl.control.heroList[1].spellsList.Add(hellfire);

            //Spell splashflame = new Spell();
            //splashflame.Name = "Splash Flame";
            //splashflame.Pm = 3;
            //splashflame.Description = "An explosive fireball that deals light damage to enemies adjacent to the target. \nCost 3pm Str 60, Crit 05, Acc 85";
            //GameControl.control.heroList[1].spellsList.Add(splashflame);

            //Spell testtarget = new Spell();
            //testtarget.Name = "Test Target";
            //testtarget.Pm = 0;
            //testtarget.Description = "Targets a hero. \n Str 0, Crit 0, Acc 100";
            //GameControl.control.heroList[1].spellsList.Add(testtarget);
            
			//Add Items to the inventory for testing
			GameControl.control.AddItem("Lesser Restorative", "consumable", 3);
			GameControl.control.AddItem("Restorative", "Consumable", 3);
			GameControl.control.AddItem("Lesser Elixir", "consumable", 3);
			GameControl.control.AddItem("Elixir", "Consumable", 2);
			GameControl.control.AddItem("Elixir", "Consumable", 2);
			GameControl.control.AddItem("Purple Prince", "consumable", 1);
			GameControl.control.AddItem("Huge Ass Heals", "Consumable", 2);
			GameControl.control.AddItem("Tincture", "Consumable", 5);
			GameControl.control.AddItem("Strengthening Draught", "Consumable", 5);
			GameControl.control.AddItem("Copper Tonic", "Consumable", 5);
			GameControl.control.AddItem("Captain Power's BIG Booster", "Consumable", 5);
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

            //Load();
		}
		else if (control != this)
		{
			//If a gameControl already exists, we don't want 2 of them
			Destroy(gameObject);
		}
	}
   
    public void AddItem(ScriptableItem _item, int _quantity)
    {
        AddItem(_item.name, _item.Type, _quantity);
    }

    public void AddItem(string _name, string _type, int _quantity)
	{
		//make all letters in type lowercase to avoid input errors
		_type = _type.ToLower ();

		// loop through the inventory to see if the player already has this type of item
		// if so, increase the quantity of that item
		if (_type == "consumable") 
		{
			foreach(InventoryItem i in consumables)
			{
				//If the item is found, simply increase the quantity and exit the method
				if(i.name == _name){
					i.quantity += _quantity;
					return;}
			}

			//If the item is not found, add it to the inventory
			consumables.Add(new InventoryItem(_name, _quantity, _type));
		}

		else if (_type == "weapon") 
		{
			foreach(InventoryItem i in weapons)
			{
				//If the item is found, simply increase the quantity and exit the method
				if(i.name == _name){
					i.quantity += _quantity;
					return;}
			}
			
			//If the item is not found, add it to the inventory
			weapons.Add(new InventoryItem(_name, _quantity, _type));
		}

		else if (_type == "armor") 
		{
			foreach(InventoryItem i in equipment)
			{
				//If the item is found, simply increase the quantity and exit the method
				if(i.name == _name){
					i.quantity += _quantity;
					return;}
			}
			
			//If the item is not found, add it to the inventory
			equipment.Add(new InventoryItem(_name, _quantity, _type));
		}

		if (_type == "key") 
		{
			foreach(InventoryItem i in key)
			{
				//If the item is found, simply increase the quantity and exit the method
				if(i.name == _name){
					i.quantity += _quantity;
					return;}
			}
			
			//If the item is not found, add it to the inventory
			key.Add(new InventoryItem(_name, _quantity, _type));
		}

		itemAdded = true;

//		if (item.GetComponent<ReusableItem>() != null)
//		{
//			foreach (GameObject i in reusables)
//			{
//				if (i.GetComponent<ReusableItem>().name == item.GetComponent<ReusableItem>().name) { i.GetComponent<ReusableItem>().quantity++; Destroy(item); return; }
//			}
//
//			// if this type of item is not already in the inventory, add it now
//			reusables.Add(item);
//			DontDestroyOnLoad(reusables[reusables.Count - 1]);
//		}
//		if (item.GetComponent<ConsumableItem>() != null)
//		{
//			foreach (GameObject i in consumables)
//			{
//				if (i.GetComponent<ConsumableItem>().name == item.GetComponent<ConsumableItem>().name) { i.GetComponent<ConsumableItem>().quantity++; Destroy(item); return; }
//			}
//
//			// if this type of item is not already in the inventory, add it now
//			consumables.Add(item);
//			DontDestroyOnLoad(consumables[consumables.Count - 1]);
//		}
//		if (item.GetComponent<ArmorItem>() != null)
//		{
//			foreach (GameObject i in equipment)
//			{
//				if (i.GetComponent<ArmorItem>().name == item.GetComponent<ArmorItem>().name) { i.GetComponent<ArmorItem>().quantity++; Destroy(item); return; }
//			}
//
//			// if this type of item is not already in the inventory, add it now
//			equipment.Add(item);
//			DontDestroyOnLoad(equipment[equipment.Count - 1]);
//		}
//		if (item.GetComponent<WeaponItem>() != null)
//		{
//			foreach (GameObject i in weapons)
//			{
//				if (i.GetComponent<WeaponItem>().name == item.GetComponent<WeaponItem>().name) { i.GetComponent<WeaponItem>().quantity++; Destroy(item); return; }
//			}
//
//			// if this type of item is not already in the inventory, add it now
//			weapons.Add(item);
//			DontDestroyOnLoad(weapons[weapons.Count - 1]);
//		}
//
//        // flag the inventory that an item was added and needs to be updated
//        itemAdded = true;

	}

    public void RemoveItem(InventoryItem _item, int _quantity = 1)
    {
        // decrease the quantity first. If there's none left, then remove.
        _item.quantity -= _quantity;
        if (_item.quantity > 0)
            return;
        
		if (consumables.Contains (_item))
            consumables.Remove(_item);
		else if (equipment.Contains (_item))
			equipment.Remove (_item);
		else if (weapons.Contains (_item))
			weapons.Remove (_item);
		else if (key.Contains (_item))
			key.Remove (_item);
//        if (item.GetComponent<ReusableItem>() != null)
//            reusables.Remove(item);
//
//        if (item.GetComponent<ConsumableItem>() != null)
//            consumables.Remove(item);
//
//        if (item.GetComponent<ArmorItem>() != null)
//            equipment.Remove(item);
//
//        if (item.GetComponent<WeaponItem>() != null)
//            weapons.Remove(item);
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
			temp.name = heroList[i].denigenName;
			temp.level = heroList[i].level;
			temp.exp = heroList[i].exp;
			temp.expToLvlUp = heroList[i].expToLvlUp;
            temp.expCurLevel = heroList[i].expCurLevel;
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
				id.uses = item.uses;
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
				id.uses = item.uses;
				temp.equipment.Add(id);
				print("Saved Item data: " + temp.equipment[j].name);
			}

			data.heroList.Add(temp);
		}
		//Save which scene the player is in
		data.currentScene = currentScene;
		data.totalGold = totalGold;
		data.keysObtainedInDungeons = keysObtainedInDungeons;
        //data.totalKeys = totalKeys;



        // Save all of the player's inventory
        foreach (var i in consumables)
        {
            //Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = i.name;
            id.quantity = i.quantity;
            id.uses = i.uses;
            data.consumables.Add(id);
        }        
        foreach (var i in weapons)
        {
            //Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = i.name;
            id.quantity = i.quantity;
            id.uses = i.uses;
            data.weapons.Add(id);
        }
        foreach (var i in equipment)
        {
            //Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = i.name;
            id.quantity = i.quantity;
            id.uses = i.uses;
            data.equipment.Add(id);
        }
        foreach (var i in key)
        {
            //Item item = i.GetComponent<Item>();
            ItemData id = new ItemData();
            id.name = i.name;
            id.quantity = i.quantity;
            id.uses = i.uses;
            data.key.Add(id);
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
					rcd.blockData [i].blockName = rc.movables [i].name;
					rcd.blockData [i].isActivated = rc.movables [i].isActivated;
					rcd.blockData[i].position.x = rc.movables[i].transform.position.x;
					rcd.blockData[i].position.y = rc.movables[i].transform.position.y;
					rcd.blockData[i].position.z = rc.movables[i].transform.position.z;
				}
				//for(int i = 0; i < rc.treasureChests.Count; i++)
				//{
				//	rcd.chestData[i].isChestOpen = rc.treasureChests[i].isOpen;
				//	rcd.chestData[i].chestName = rc.treasureChests[i].name;
				//}
				//for (int i = 0; i < rc.doorsInRoom.Count; i++)
				//{
				//	rcd.doorData[i].isLocked = rc.doorsInRoom[i].gameObject.activeSelf;
				//	rcd.doorData[i].doorName = rc.doorsInRoom[i].name;
				//}
				for (int i = 0; i < rc.switchesInRoom.Count; i++) {
					rcd.switchData [i].isActivated = rc.switchesInRoom [i].isActivated;
					rcd.switchData [i].switchName = rc.switchesInRoom [i].name;
				}
				for (int i = 0; i < rc.colorBridgesInRoom.Count; i++) {
					rcd.colorBridgeData [i].rotationZ = rc.colorBridgesInRoom [i].transform.Find("Bridge").transform.eulerAngles.z;
					rcd.colorBridgeData [i].bridgeName = rc.colorBridgesInRoom [i].name;
				}
				for (int i = 0; i < rc.drawbridgesInRoom.Count; i++) {
					rcd.drawbridgeData [i].bridgeName = rc.drawbridgesInRoom [i].name;
					//rcd.drawbridgeData [i].isActive = rc.drawbridgesInRoom [i].isActive;
					rcd.drawbridgeData [i].positionY = rc.drawbridgesInRoom [i].transform.position.y;
				}
				for (int i = 0; i < rc.enemies.Count; i++) {
					rcd.enemyData [i].enemyName = rc.enemies [i].name;
					rcd.enemyData [i].battledState = rc.enemies [i].beenBattled;
					rcd.enemyData [i].position.x = rc.enemies [i].transform.position.x;
					rcd.enemyData [i].position.y = rc.enemies [i].transform.position.y;
					rcd.enemyData [i].position.z = rc.enemies [i].transform.position.z;
				}
				// save which dungeon the player is in
				// if >= 0, save the keys
				rcd.dungeonID = rc.dungeonID;
				if (rcd.dungeonID >= 0)
				{
					keysObtainedInDungeons[rcd.dungeonID] = totalKeys;
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
		//PauseMenu tempPause = GameObject.FindObjectOfType<PauseMenu>();
		//pause.isActive = tempPause.isActive;
		//pause.isVisible = tempPause.isVisible;
		//pause.selectedIndex = tempPause.SelectedIndex;
		//pause.position = tempPause.transform.position;

		//TeamSubMenu tempTeamSub = GameObject.FindObjectOfType<TeamSubMenu>();
		//teamSub.isActive = tempTeamSub.isActive;
		//teamSub.isVisible = tempTeamSub.isVisible;
		//teamSub.selectedIndex = tempTeamSub.SelectedIndex;
		//teamSub.position = tempTeamSub.transform.position;

		//HeroSubMenu tempHeroSub = GameObject.FindObjectOfType<HeroSubMenu>();
		//heroSub.isVisible = tempHeroSub.isVisible;
		//heroSub.selectedIndex = tempHeroSub.SelectedIndex;
		//heroSub.position = tempHeroSub.transform.position;

		//InventorySubMenu tempInventSub = GameObject.FindObjectOfType<InventorySubMenu>();
		//inventSub.isVisible = tempInventSub.isVisible;
		//inventSub.selectedIndex = tempInventSub.SelectedIndex;
		//inventSub.position = tempInventSub.transform.position;
	}

	// called if the isPaused variable is set to true
	public void RestorePauseMenu()
	{
		//PauseMenu tempPause = GameObject.FindObjectOfType<PauseMenu>();
		//TeamSubMenu tempTeamSub = GameObject.FindObjectOfType<TeamSubMenu>();
		//HeroSubMenu tempHeroSub = GameObject.FindObjectOfType<HeroSubMenu>();
		//InventorySubMenu tempInventSub = GameObject.FindObjectOfType<InventorySubMenu>();

		//if (pause.isVisible) 
		//{
		//	tempPause.isVisible = true;
		//	tempPause.EnablePauseMenu();
		//	tempPause.player.ToggleMovement();
		//	tempPause.isActive = pause.isActive;
		//	tempPause.SelectedIndex = pause.selectedIndex;
		//	tempPause.HighlightButton();
		//	tempPause.transform.position = pause.position;
		//}
		//if (teamSub.isVisible)
		//{
		//	tempTeamSub.isVisible = true;
		//	tempTeamSub.EnableSubMenu();
		//	tempTeamSub.isActive = teamSub.isActive;
		//	tempTeamSub.SelectedIndex = teamSub.selectedIndex;
		//	tempTeamSub.HighlightButton();
		//	tempTeamSub.transform.position = teamSub.position;
		//}
		//if (heroSub.isVisible)
		//{
		//	tempHeroSub.isVisible = true;
		//	tempHeroSub.EnableSubMenu();
		//	tempHeroSub.SelectedIndex = heroSub.selectedIndex;
		//	tempHeroSub.HighlightButton();
		//	tempHeroSub.transform.position = heroSub.position;
		//}
		//if (inventSub.isVisible)
		//{
		//	tempInventSub.isVisible = true;
		//	tempInventSub.EnableSubMenu();
		//	tempInventSub.SelectedIndex = inventSub.selectedIndex;
		//	tempInventSub.HighlightButton();
		//	tempInventSub.transform.position = inventSub.position;
		//}
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
            //reusables.Clear();
            weapons.Clear();
            equipment.Clear();
            key.Clear();

            //Find all items and destroy them
            //Item[] items = FindObjectsOfType<Item>();
            //foreach (Item i in items) { Destroy(i.gameObject); }

            // Read in all consumable items
            foreach (ItemData id in data.consumables) { LoadConsumableItem(id); }

            //Read in all reusable items
//            foreach (ItemData id in data.reusables) { LoadReusableItem(id); }

            //read in all weapons
            foreach (ItemData id in data.weapons) { LoadWeaponItem(id); }

            //read in all equipment
            foreach (ItemData id in data.equipment) { LoadArmorItem(id); }

            foreach(ItemData id in data.key) { LoadKeyItem(id); }

            //load all of the heroes
            for (int i = 0; i < data.heroList.Count; i++)
			{
				HeroData temp = new HeroData();
                string loadPath = "";
                switch(data.heroList[i].identity)
                {
                    case 0: loadPath = "Data/Heroes/Start_Jethro"; break;
                    case 1: loadPath = "Data/Heroes/Start_Cole"; break;
                    case 2: loadPath = "Data/Heroes/Start_Eleanor"; break;
                    case 3: loadPath = "Data/Heroes/Start_Jouliette"; break;
                }
                if(string.IsNullOrEmpty(loadPath))
                {
                    Debug.LogError("No hero found. Idenity not 0 - 4.");
                    return;
                }
                temp = Resources.Load<HeroData>(loadPath);
                temp = Instantiate(temp);

				temp.identity = data.heroList[i].identity;
				temp.statBoost = data.heroList[i].statBoost;
				temp.skillTree = data.heroList[i].skillTree;
				temp.denigenName = data.heroList[i].name;
				temp.level = data.heroList[i].level;
				temp.exp = data.heroList[i].exp;
				temp.expToLvlUp = data.heroList[i].expToLvlUp;
                temp.expCurLevel = data.heroList[i].expCurLevel;
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
				temp.statusState = (DenigenData.Status)data.heroList[i].statusState;
                print("LOADED");

//				if (data.heroList[i].weapon != null) 
//				{ 
//					foreach (GameObject w in weapons)
//					{
//						if (w.GetComponent<WeaponItem>().name == data.heroList[i].weapon.name) { temp.weapon = w; }
//					} 
//				}
//				temp.equipment = new List<GameObject>();
//				if (data.heroList[i].equipment.Count > 0) 
//				{ 
//					for (int j = 0; j < data.heroList[i].equipment.Count; j++)
//					{
//						foreach (GameObject e in equipment)
//						{
//							if (e.GetComponent<ArmorItem>().name == data.heroList[i].equipment[j].name) { temp.equipment.Add(e); }
//						}
//					} 
//				}

				heroList.Add(temp);
			}

            //put the player back where they were
            //UnityEngine.SceneManagement.SceneManager.LoadScene(data.currentScene);
            LoadSceneAsync(data.currentScene);
            // Put their position vector here if we choose
            //currentPosition = new Vector2(data.posX, data.posY);

            taggedStatue = data.taggedStatue;
            if (taggedStatue)
                savedStatue = new Vector2(data.posX, data.posY);

			totalGold = data.totalGold;
			keysObtainedInDungeons = data.keysObtainedInDungeons;
			// put all interactable item data back
			rooms = data.rooms;

			// load keys
			roomControl rc = GameObject.FindObjectOfType<roomControl>();
			foreach(RoomControlData rcd in rooms)
			{
				if(rcd.dungeonID == rc.dungeonID)
				{
					if(rcd.dungeonID < 0)
					{
						totalKeys = 0;
					}
					else
					{
						totalKeys = keysObtainedInDungeons[rcd.dungeonID];
					}
				}
			}
		}
	}

    void LoadConsumableItem(ItemData id)
    {
        AddItem(id.name, "consumable", id.quantity);
    }

    void LoadArmorItem(ItemData id)
    {
        AddItem(id.name, "armor", id.quantity);
    }

    void LoadWeaponItem(ItemData id)
    {
        AddItem(id.name, "weapon", id.quantity);
    }

    void LoadKeyItem(ItemData id)
    {
        AddItem(id.name, "key", id.quantity);
    }
//	public void LoadConsumableItem(ItemData id)
//	{
//		GameObject temp = null;
//		switch (id.name)
//		{
//		case "Lesser Restorative":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserRestorative"));
//                temp.name = "LesserRestorative";
//			break;
//		case "Restorative":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Restorative"));
//                temp.name = "Restorative";
//			break;
//		case "Gratuitous Restorative":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/GratuitousRestorative"));
//                temp.name = "GratuitousRestorative";
//			break;
//		case "Terminal Restorative":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/TerminalRestorative"));
//                temp.name = "TerminalRestorative";
//			break;
//		case "Lesser Elixir":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/LesserElixir"));
//                temp.name = "LesserElixir";
//			break;
//		case "Elixir":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/Elixir"));
//                temp.name = "Elixir";
//			break;
//		case "Gratuitous Elixir":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/GratuitousElixir"));
//                temp.name = "GratuitousElixir";
//			break;
//		case "Terminal Elixir":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/TerminalElixir"));
//                temp.name = "TerminalElixir";
//			break;
//		default:
//			print("Error: Incorrect item name - " + id.name);
//			break;
//		}
//		GameControl.control.AddItem(temp);
//		GameControl.control.consumables[consumables.Count - 1].GetComponent<ConsumableItem>().quantity = id.quantity;
//		GameControl.control.consumables[consumables.Count - 1].GetComponent<ConsumableItem>().uses = id.uses;
//	}
//
//	public void LoadReusableItem(ItemData id)
//	{
//		GameObject temp = null;
//		switch (id.name)
//		{
//		default:
//			print("Error: Incorrect item name - " + id.name);
//			break;
//		}
//		GameControl.control.AddItem(temp);
//		GameControl.control.reusables[reusables.Count - 1].GetComponent<ReusableItem>().quantity = id.quantity;
//		GameControl.control.reusables[reusables.Count - 1].GetComponent<ReusableItem>().uses = id.uses;
//
//	}
//
//	public void LoadArmorItem(ItemData id, DenigenData hd)
//	{
//		GameObject temp = null;
//		switch (id.name)
//		{
//		case "Helmet of Fortitude":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/HelmetOfFortitude"));
//                temp.name = "HelmetOfFortitude";
//			break;
//		case "Iron Armor":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/IronArmor"));
//                temp.name = "IronArmor";
//			break;
//		case "Iron Helm":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/IronHelm"));
//                temp.name = "IronHelm";
//			break;
//		case "Steel Armor":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/SteelArmor"));
//                temp.name = "SteelArmor";
//			break;
//		case "Steel Gauntlets":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/SteelGauntlets"));
//                temp.name = "SteelGauntlets";
//			break;
//		case "Steel Helm":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/SteelHelm"));
//                temp.name = "SteelHelm";
//			break;
//		default:
//			print("Error: Incorrect item name - " + id.name);
//			break;
//		}
//		if (hd == null)
//		{
//			GameControl.control.AddItem(temp);
//			GameControl.control.equipment[equipment.Count - 1].GetComponent<ArmorItem>().quantity = id.quantity;
//			GameControl.control.equipment[equipment.Count - 1].GetComponent<ArmorItem>().uses = id.uses;
//
//		}
//		else { hd.equipment.Add(temp); DontDestroyOnLoad(temp); }
//	}
//
//	public void LoadWeaponItem(ItemData id, DenigenData hd)
//	{
//		GameObject temp = null;
//		switch (id.name)
//		{
//		case "Spare Sword":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/SpareSword"));
//                temp.name = "SpareSword";
//			break;
//		case "Tome of Practical Spells":
//			temp = (GameObject)Instantiate(Resources.Load("Prefabs/Items/TomeOfPractical"));
//                temp.name = "TomeOfPractical";
//			break;
//		default:
//			print("Error: Incorrect item name - " + id.name);
//			break;
//		}
//		if (hd == null)
//		{
//			GameControl.control.AddItem(temp);
//			GameControl.control.weapons[weapons.Count - 1].GetComponent<WeaponItem>().quantity = id.quantity;
//			GameControl.control.weapons[weapons.Count - 1].GetComponent<WeaponItem>().uses = id.uses;
//
//		}
//		else { hd.weapon = temp; DontDestroyOnLoad(temp); }
//	}
    void AssignHeroStats(DenigenData dataToSet, Hero hero)
    {
        //dataToSet.level = hero.Level;
        //dataToSet.exp = hero.Exp;
        //dataToSet.expToLvlUp = hero.ExpToLevelUp;
        //dataToSet.levelUpPts = hero.LevelUpPts;
        //dataToSet.techPts = hero.TechPts;
        //dataToSet.hp = hero.hpChange;
        //dataToSet.hpMax = hero.hpMaxChange;
        //dataToSet.pm = hero.pmChange;
        //dataToSet.pmMax = hero.pmMaxChange;
        //dataToSet.atk = hero.Atk;
        //dataToSet.def = hero.Def;
        //dataToSet.mgkAtk = hero.MgkAtk;
        //dataToSet.mgkDef = hero.MgkDef;
        //dataToSet.luck = hero.Luck;
        //dataToSet.evasion = hero.Evasion;
        //dataToSet.spd = hero.Spd;
        //dataToSet.skillsList = hero.SkillsList;
        //dataToSet.spellsList = hero.SpellsList;
        //dataToSet.passiveList = hero.PassivesList;
        //// Passives are non serializable now because they inherit from something with a my button variable
        ////heroList[0].passiveList.Add(new LightRegeneration());
        //dataToSet.weapon = null;
        //dataToSet.equipment = new List<GameObject>();
    }

    /// <summary>
    /// This function is purely for testing purposes. 
    /// The heroes should only be created and added from their Start scriptable object once AND ONLY ONCE -- right when they're added to the party
    /// Any other subsequent time, their data will be read in from elsewhere, perhaps another HeroData scriptableObject???    /// 
    /// </summary>
    void InitHeroes()
    {
        AddJethro();
        AddCole();
        AddEleanor();
        AddJuliette();
        SetHeroesToLiving();
    }

//    void AddHero(HeroData data)
//    {

//        // because scriptable objects are assets, changes made during runtime remain
//        // creating a copy resets the values, making it easier to test
//#if UNITY_EDITOR
//        data = Instantiate(data);
//#endif

//        heroList.Add(data);
//    }

    void AddJethro()
    {
        //test code for creating Jethro -- based on level 1 stats
        //We will have these stats stored in HeroData objs for consistency between rooms

        // since the hero stats are set in the Hero/Denigen classes, 
        // the quickest solution is to create a hero object, save the data with hero data, then destroy the object
        // same situation with the other heroes
        //var jethroObj = (GameObject)Instantiate(Resources.Load("Prefabs/JethroPrefab"));
        //var jethro = jethroObj.GetComponent<Jethro>();
        //heroList.Add(new HeroData());
        //heroList[0].denigenName = playerName;
        //heroList[0].identity = 0;
        //AssignHeroStats(heroList[0], jethro);
        //Destroy(jethroObj);

        var jethro = Resources.Load<HeroData>("Data/Heroes/Start_Jethro");
#if UNITY_EDITOR
        jethro = Instantiate(jethro);
#endif
        heroList.Add(jethro);
        //AddHero(jethro);
        jethro.denigenName = playerName;
        //jethro.Init();
        
        
    }
    void AddCole()
    {
        //var coleObj = (GameObject)Instantiate(Resources.Load("Prefabs/ColePrefab"));
        //var cole = coleObj.GetComponent<Cole>();
        //heroList.Add(new HeroData());
        //heroList[1].identity = 1;
        //heroList[1].denigenName = "Cole";
        //AssignHeroStats(heroList[1], cole);
        //Destroy(coleObj);

        var cole = Resources.Load<HeroData>("Data/Heroes/Start_Cole");
#if UNITY_EDITOR
        cole = Instantiate(cole);
#endif
        heroList.Add(cole);

        // the below code would probably be found in the "Add Spell" functions
        //skillTreeAccessor.ReadInfo("techniquesCole1.tsv");
        //skillTreeAccessor.AddTechnique(heroList[1], new Spell(skillTreeAccessor.FindTechnique("candleshot")));
        //skillTreeManager.AddColeStartingTechniques();
    }
    void AddEleanor()
    {
        // test eleanor -----THESE VALUES ARE COMPLETELY RANDOM AND ARBITRARY --- PLEASE CHANGE/REEVALUATE
        //var eleanorObj = (GameObject)Instantiate(Resources.Load("Prefabs/EleanorPrefab"));
        //var eleanor = eleanorObj.GetComponent<Eleanor>();
        //heroList.Add(new HeroData());
        //heroList[2].identity = 2;
        //heroList[2].denigenName = "Eleanor";
        //AssignHeroStats(heroList[2], eleanor);
        //Destroy(eleanorObj);

        var eleanor = Resources.Load<HeroData>("Data/Heroes/Start_Eleanor");
#if UNITY_EDITOR
        eleanor = Instantiate(eleanor);
#endif
        heroList.Add(eleanor);
    }
    void AddJuliette()
    {
        //// juliette test-----THESE VALUES ARE COMPLETELY RANDOM AND ARBITRARY --- PLEASE CHANGE/REEVALUATE
        //var julietteObj = (GameObject)Instantiate(Resources.Load("Prefabs/JuliettePrefab"));
        //var juliette = julietteObj.GetComponent<Juliette>();
        //heroList.Add(new HeroData());
        //heroList[3].identity = 3;
        //heroList[3].denigenName = "Juliette";
        //AssignHeroStats(heroList[3], juliette);
        //Destroy(julietteObj);

        var jouliette = Resources.Load<HeroData>("Data/Heroes/Start_Jouliette");
#if UNITY_EDITOR
        jouliette = Instantiate(jouliette);
#endif
        heroList.Add(jouliette);
    }

    void SetHeroesToLiving()
    {
        foreach (var hero in heroList)
            hero.statusState = DenigenData.Status.normal;
    }

    void SetHeroesToMaxHP()
    {
        foreach (var hero in heroList)
            hero.hp = hero.hpMax;
    }

    void SetHeroesToMaxPM()
    {
        foreach (var hero in heroList)
            hero.pm = hero.pmMax;
    }

    public void ReturnFromBattle()
    {
        //currentCharacterState = characterControl.CharacterState.Normal;
        if (string.IsNullOrEmpty(currentScene))
            currentScene = "testScene";
        //SceneManager.LoadScene(currentScene);        
        if (!ReadyForNextScene)
            ReadyForNextScene = true;
        else
            GameControl.control.LoadSceneAsync(currentScene);
    }

    public void LoadLastSavedStatue()
    {
        // reset heroes stats
        SetHeroesToLiving();
        SetHeroesToMaxHP();
        SetHeroesToMaxPM();

        // set all enemies "been battled" to false
        currentRoom.SetEnemiesToNotBattled();


        if (taggedStatue)
        {
            currentPosition = savedStatue;
            currentCharacterState = characterControl.CharacterState.Normal;
        }
        else
        {
            currentPosition = areaEntrance;
            currentCharacterState = characterControl.CharacterState.Transition;
        }

        SceneManager.LoadScene(currentScene);
    }

    public void AddGold(int gold)
    {
        totalGold += gold;
    }

    AsyncOperation currentOperation;
    public bool ReadyForNextScene
    {
        get { return currentOperation.allowSceneActivation; }
        set
        {
            currentOperation.allowSceneActivation = value;
            if(value == true)
            {
                //print("current progress: " + currentOperation.progress);
                if (currentOperation.progress < 0.9f)
                    UIManager.PushMenu(UIManager.uiDatabase.LoadingScreen);
            }
        }
    }
    public void LoadScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public void LoadSceneAsync(string _scene, bool waitToLoad = false)
    {
        //print("loading called");
        StartCoroutine(LoadAsync(_scene, waitToLoad));
    }

    IEnumerator LoadAsync(string _scene, bool waitToLoad = false)
    {
        currentOperation = SceneManager.LoadSceneAsync(_scene);
        if (waitToLoad)
            ReadyForNextScene = false;
        else
            UIManager.PushMenu(UIManager.uiDatabase.LoadingScreen);
        while (currentOperation.progress < 0.9f)
        {
            //print("loading: " + currentOperation.progress);
            yield return null;
        }        
    }

}

//this class is where all of our data will be sent in order to be saved
[Serializable]
class PlayerData
{
	public int totalGold; // the player's total gold
	public List<int> keysObtainedInDungeons = new List<int>();
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
	//public List<ItemData> reusables = new List<ItemData>() { };
	public List<ItemData> equipment = new List<ItemData>() { };
	public List<ItemData> weapons = new List<ItemData>() { };
    public List<ItemData> key = new List<ItemData>();
}

//this class should hold all of the stuff necessary for a hero object
//[Serializable]
//public class HeroData
//{
//	public int identity;
//	public bool statBoost = false;
//	public bool skillTree = false;
//	public string name;
//	public int level, exp, expToLvlUp, levelUpPts, techPts;
//	public int hp, hpMax, pm, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
//	public List<Skill> skillsList;
//	public List<Spell> spellsList;
//	public List<Passive> passiveList;
//	// status effect
//	public enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead, overkill };
//	public Status statusState;
//	// Need a creative way to store which items are equipped since items are non-serializable
//	public GameObject weapon;
//	public List<GameObject> equipment;

//    /// <summary>
//    /// Searches through the hero's equipment/armor and returns true if the hero already the item.
//    /// </summary>
//    /// <param name="itemToCheck"></param>
//    /// <returns></returns>
//    public bool EquipmentContainsItem(Item itemToCheck)
//    {
//        foreach(var itemObj in equipment)
//        {
//            if (itemObj.GetComponent<Item>() == itemToCheck)
//                return true;
//        }

//        return false;
//    }
//}

// this class exists to save the hero item
[Serializable]
public class SavableHeroData
{
	public int identity;
	public bool statBoost = false;
	public bool skillTree = false;
	public string name;
	public int level, exp, expToLvlUp, expCurLevel, levelUpPts, techPts;
	public int hp, hpMax, pm, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
	public List<Skill> skillsList;
	public List<Spell> spellsList;
	public List<Passive> passiveList;
	// status effect
	public enum Status { normal, bleeding, infected, cursed, blinded, petrified, dead, overkill };
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
	public int uses;
}

//this class will store all the stuff in a roomControl object that the player influences
[Serializable]
public class RoomControlData
{
	public int areaIDNumber;
	public int dungeonID;
	//public List<SerializableVector3> movableBlockPos = new List<SerializableVector3>(); // the positions of all of the movable blocks
	//also store treasure boxes, doors, switches, antyhing important
	public List<BlockData> blockData = new List<BlockData>();
	public List<TreasureData> chestData = new List<TreasureData>();
	public List<DoorData> doorData = new List<DoorData>();
	public List<SwitchData> switchData = new List<SwitchData> ();
	public List<ColorBridgeData> colorBridgeData = new List<ColorBridgeData>();
	public List<DrawbridgeData> drawbridgeData = new List<DrawbridgeData>();
	public List<EnemyControlData> enemyData = new List<EnemyControlData>();
    public roomControl.RoomLimits roomLimits = new roomControl.RoomLimits();
}
[Serializable]
public class BlockData
{
	public string blockName;
	public bool isActivated;
	public SerializableVector3 position = new SerializableVector3();
}
[Serializable]
public class TreasureData
{
	public bool isChestOpen;
	public string chestName;
}
[Serializable]
public class DoorData
{
	public bool isLocked;
	public string doorName;
}
[Serializable]
public class SwitchData
{
	public bool isActivated;
	public string switchName;
}
[Serializable]
public class ColorBridgeData
{
	public float rotationZ;
	public string bridgeName;
}
[Serializable]
public class DrawbridgeData
{
	public string bridgeName;
	public float positionY;
	//public bool isActive;
}
[Serializable]
public class SerializableVector3
{
	public float x, y, z;
}
[Serializable]
public class EnemyControlData
{
	public string enemyName;
	public bool battledState;
	public SerializableVector3 position = new SerializableVector3 ();
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

