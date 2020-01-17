using UnityEngine;
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
    public static SpriteDatabase spriteDatabase;
    public static QuestTracker questTracker;

    //Info to be saved and used throughout the game
    public int totalGold; // the player's total gold
	public int totalKeys;
	public List<int> keysObtainedInDungeons = new List<int>();
	public int numOfDungeons;
	//items
	public List<InventoryItem> consumables;
	public List<InventoryItem> augments;
	public List<InventoryItem> armor;
	public List<InventoryItem> key;
	public bool itemAdded;
    
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

	// for temporarily saving the state of enemies when pausing and unpausing the game
	public List<SerializableVector3> enemyPos = new List<SerializableVector3>();

	// for telling the pause menu which list of items to use
	//internal string whichInventory;    
    public enum WhichInventory { Consumables, Armor, Augments, KeyItems }
    internal WhichInventory whichInventoryEnum;
    public string CurrentInventory
    {
        get
        {
            if (whichInventoryEnum == WhichInventory.Consumables)
                return "consumable";
            else if (whichInventoryEnum == WhichInventory.Armor)
                return "armor";
            else if (whichInventoryEnum == WhichInventory.Augments)
                return "augment";
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

    // var for keeping track of gateways between scenes
    public string sceneStartGateName;
    public void AssignEntrance(string gatewayName)
    {
        sceneStartGateName = gatewayName;
    }

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
    characterControl.CharacterState prevState;
    public characterControl.CharacterState PrevState { get { return prevState; } set { prevState = value; } }

    public characterControl.HeroCharacter currentCharacter;
    public int currentCharacterInt { get { return (int)currentCharacter; } }

    // Current NPC
    public NPCDialogue currentNPC; // we can only talk to one NPC at a time, this variable will keep that one in focus
    public NPCPathwalkControl CurrentNPCPathwalk { get { return currentNPC.GetComponentInParent<NPCPathwalkControl>(); } }
	public StationaryNPCControl CurrentStationaryNPC { get { return currentNPC.GetComponentInParent<StationaryNPCControl>(); } }
    public NPCShopKeeper CurrentShopkeeper { get { return currentNPC.GetComponentInParent<NPCShopKeeper>(); } }

    public AudioClip battleIntro;
    public AudioClip battleLoop;

    public List<int> knownBeggarDialogues;
    
    //awake gets called before start
    void Awake () {
		if (control == null)
		{
			//this keeps the game object from being destroyed between scenes
			DontDestroyOnLoad(gameObject);
			control = this;

            InitHeroes();
            UIManager = new UIManager();
            skillTreeManager = new SkillTreeManager();
            itemManager = new ItemManager();
            audioManager = GetComponentInChildren<AudioManager>();
            audioManager.Init();
            spriteDatabase = Resources.Load<SpriteDatabase>("Databases/SpriteDatabase");
            questTracker = new QuestTracker();
			totalGold = 0;
			totalKeys = 0;

			// create list keeping track of dungeon keys, start off at zero keys
			numOfDungeons = 2;
			for (int i = 0; i < numOfDungeons; i++)
			{
				keysObtainedInDungeons.Add(0);
			}

			//Add Items to the inventory for testing
			AddItem("Lesser Restorative", "consumable", 3);
			AddItem("Restorative", "Consumable", 3);
			AddItem("Lesser Elixir", "consumable", 3);
			AddItem("Elixir", "Consumable", 2);
			AddItem("Elixir", "Consumable", 2);
			AddItem("Purple Prince", "consumable", 1);
			AddItem("Huge Ass Heals", "Consumable", 2);
			AddItem("Tincture", "Consumable", 5);
			AddItem("Strengthening Draught", "Consumable", 5);
			AddItem("Copper Tonic", "Consumable", 5);
			AddItem("Captain Power's BIG Booster", "Consumable", 5);
            AddItem("Bleach", "Consumable", 1);
            AddItem("Blazing Stone", "Augment", 1);
            AddItem("Plat'num Gem", "Augment", 1);

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

		else if (_type == "armor") 
		{
			foreach(InventoryItem i in armor)
			{
				//If the item is found, simply increase the quantity and exit the method
				if(i.name == _name){
					i.quantity += _quantity;
					return;}
			}
			
			//If the item is not found, add it to the inventory
			armor.Add(new InventoryItem(_name, _quantity, _type));
		}

		else if (_type == "augment") 
		{
			foreach(InventoryItem i in augments)
			{
				//If the item is found, simply increase the quantity and exit the method
				if(i.name == _name){
					i.quantity += _quantity;
					return;}
			}
			
			//If the item is not found, add it to the inventory
			augments.Add(new InventoryItem(_name, _quantity, _type));
		}

		else if (_type == "key") 
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

	}

    public void RemoveItem(InventoryItem _item, int _quantity = 1)
    {
        // decrease the quantity first. If there's none left, then remove.
        _item.quantity -= _quantity;
        if (_item.quantity > 0)
            return;
        
		if (consumables.Contains (_item))
            consumables.Remove(_item);
		else if (augments.Contains (_item))
			augments.Remove (_item);
		else if (armor.Contains (_item))
			armor.Remove (_item);
		else if (key.Contains (_item))
			key.Remove (_item);
    }

	//this will save our game data to an external, persistent file
	public void Save(int index = 1) // 1 for test
	{
        currentScene = SceneManager.GetActiveScene().name;
        RecordRoom();

        BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo" + index.ToString() + ".dat");

		PlayerData data = new PlayerData();
		
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

			data.heroList.Add(temp);
		}
		//Save which scene the player is in
		data.currentScene = currentScene;
		data.totalGold = totalGold;
		data.keysObtainedInDungeons = keysObtainedInDungeons;
       

        // Save all of the player's inventory
        foreach (var i in consumables)
        {
            ItemData id = new ItemData();
            id.name = i.name;
            id.quantity = i.quantity;
            id.uses = i.uses;
            data.consumables.Add(id);
        }        
        foreach (var i in armor)
        {
            ItemData id = new ItemData();
            id.name = i.name;
            id.quantity = i.quantity;
            id.uses = i.uses;
            data.weapons.Add(id);
        }
        foreach (var i in augments)
        {
            ItemData id = new ItemData();
            id.name = i.name;
            id.quantity = i.quantity;
            id.uses = i.uses;
            data.equipment.Add(id);
        }
        foreach (var i in key)
        {
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
                for (int i = 0; i < rc.treasureChests.Count; i++)
                {
                    rcd.chestData[i].isChestOpen = rc.treasureChests[i].isOpen;
                    rcd.chestData[i].chestName = rc.treasureChests[i].name;
                }
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
    
	public void Load(int index)
	{
        string saveFile = Application.persistentDataPath + "/playerInfo" + index.ToString() + ".dat";
        if (File.Exists(saveFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFile, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            //clear any current data
            heroList = new List<HeroData>() { };

            //Make sure the item lists are cleared before adding more
            consumables.Clear();
            armor.Clear();
            augments.Clear();
            key.Clear();

            // Read in all consumable items
            foreach (ItemData id in data.consumables) { LoadConsumableItem(id); }

            //read in all weapons
            foreach (ItemData id in data.weapons) { LoadWeaponItem(id); }

            //read in all equipment
            foreach (ItemData id in data.equipment) { LoadArmorItem(id); }

            // read in all key items
            foreach (ItemData id in data.key) { LoadKeyItem(id); }

            //load all of the heroes
            for (int i = 0; i < data.heroList.Count; i++)
            {
                HeroData temp = new HeroData();
                string loadPath = "";
                switch (data.heroList[i].identity)
                {
                    case 0: loadPath = "Data/Heroes/Start_Jethro"; break;
                    case 1: loadPath = "Data/Heroes/Start_Cole"; break;
                    case 2: loadPath = "Data/Heroes/Start_Eleanor"; break;
                    case 3: loadPath = "Data/Heroes/Start_Jouliette"; break;
                }
                if (string.IsNullOrEmpty(loadPath))
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

                heroList.Add(temp);
            }

            //put the player back where they were
            LoadSceneAsync(data.currentScene);

            taggedStatue = data.taggedStatue;
            if (taggedStatue)
                savedStatue = new Vector2(data.posX, data.posY);

            totalGold = data.totalGold;
            keysObtainedInDungeons = data.keysObtainedInDungeons;
            // put all interactable item data back
            rooms = data.rooms;

            // load keys
            roomControl rc = GameObject.FindObjectOfType<roomControl>();
            foreach (RoomControlData rcd in rooms)
            {
                if (rcd.dungeonID == rc.dungeonID)
                {
                    if (rcd.dungeonID < 0)
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
        else print("file not found");
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

    void AddJethro()
    {
        var jethro = Resources.Load<HeroData>("Data/Heroes/Start_Jethro");
#if UNITY_EDITOR // test if this is needed
        jethro = Instantiate(jethro);
#endif
        heroList.Add(jethro);        
        jethro.denigenName = playerName;
                
        
    }
    void AddCole()
    {
        var cole = Resources.Load<HeroData>("Data/Heroes/Start_Cole");
#if UNITY_EDITOR // test if this is needed
        cole = Instantiate(cole);
#endif
        heroList.Add(cole);
    }
    void AddEleanor()
    {
        var eleanor = Resources.Load<HeroData>("Data/Heroes/Start_Eleanor");
#if UNITY_EDITOR // test if this is needed
        eleanor = Instantiate(eleanor);
#endif
        heroList.Add(eleanor);
    }
    void AddJuliette()
    {
        var jouliette = Resources.Load<HeroData>("Data/Heroes/Start_Jouliette");
#if UNITY_EDITOR // test if this is needed
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
        if (string.IsNullOrEmpty(currentScene))
            currentScene = "testScene";     
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

    public void AddGold(int gold, bool showNotification = false)
    {
        totalGold += gold;

        if (showNotification)
            UIManager.PushNotificationMenu("You got " + gold + " gold!");
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
            yield return null;
        }        
    }

    public static bool AnimatorHasParameter(Animator runAnim, string parameter)
    {
        foreach(var p in runAnim.parameters)
        {
            if (p.name == parameter)
                return true;
        }

        return false;
    }

    public void ShakeCamera(float shakeMagnitude = 0.05f, float shakeTime = 0.3f)
    {
        var camera = Camera.main;
        StartCoroutine(StartCameraShaking(camera, shakeMagnitude, shakeTime));
    }

    IEnumerator StartCameraShaking(Camera camera, float shakeMagnitude, float shakeTime)
    {
        var initialPos = camera.transform.position;
        float timer = 0.0f;
        float cameraShakingOffsetX;
        float cameraShakingOffsetY;
        
        while(timer < shakeTime)
        {
            cameraShakingOffsetX = UnityEngine.Random.value * shakeMagnitude * 2 - shakeMagnitude;
            cameraShakingOffsetY = UnityEngine.Random.value * shakeMagnitude * 2 - shakeMagnitude;
            Vector3 intermediatePos = camera.transform.position;
            intermediatePos.x += cameraShakingOffsetX;
            intermediatePos.y += cameraShakingOffsetY;
            camera.transform.position = intermediatePos;
            timer += Time.deltaTime;
            yield return null;
        }

        camera.transform.position = initialPos;
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
	public bool taggedStatue;
	public float posX, posY; //The exact position where the player was upon saving. This will probably be removed to avoid abuse and exploits
	public List<SavableHeroData> heroList = new List<SavableHeroData>() { };
	public List<RoomControlData> rooms = new List<RoomControlData>() { }; // stores all of the data for areas the player has been to, like block positions, etc.

	// All of the player's items
	public List<ItemData> consumables = new List<ItemData>() { };
	public List<ItemData> equipment = new List<ItemData>() { };
	public List<ItemData> weapons = new List<ItemData>() { };
    public List<ItemData> key = new List<ItemData>();
}

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