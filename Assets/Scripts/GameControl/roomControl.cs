using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class roomControl : MonoBehaviour {

	// store the default entrance of the room
	// this is where the player will be sent when they die if no statue has been tagged
	public Vector2 entrance;
    public List<Gateway> gatewaysInRoom = new List<Gateway>();

	// Attributes dealing with enemies
	public int areaIDNumber; // helps the gameControl obj know what data to synch
	public int dungeonID = -1;
	public int areaLevel; // the locked level of enemies in the area
	public int numOfEnemies; //number of enemies walking around
	public List<EnemyData> possibleEnemies;// Array of possible enemy types
	public List<enemyControl> enemies = new List<enemyControl>();
	public GameObject enemyControlPrefab; // a generic enemy control object
	public int minEnemiesPerBattle;
	public int maxEnemiesPerBattle;
	public List<MovableOverworldObject> movables = new List<MovableOverworldObject>();
	//public List<TreasureChest> treasureChests = new List<TreasureChest>();
	//public List<DoorQuestion> doorsInRoom = new List<DoorQuestion>();
	public List<Switch> switchesInRoom = new List<Switch> ();
	public List<ColorBridge> colorBridgesInRoom = new List<ColorBridge>();
	public List<Drawbridge> drawbridgesInRoom = new List<Drawbridge>();

    // room size limits    
    public RoomLimits roomLimits;
    [System.Serializable]
    public struct RoomLimits
    {
        public int minX, minY, maxX, maxY; // ints because the tilemap's position and size should always be whole numbers (??? right ???)
    }
    bool roomWasLoaded;

    [Header("Sound Bank")]
    public AudioClip music_start;
    public AudioClip music_loop;

	// Use this for initialization
	void Start () {
		// convert arrays to lists
        
		foreach(MovableOverworldObject m in FindObjectsOfType<MovableOverworldObject>())
		{
			movables.Add(m);
		}
		//foreach(TreasureChest tc in FindObjectsOfType<TreasureChest>())
		//{
		//	treasureChests.Add(tc);
		//}
		//foreach(DoorQuestion dq in FindObjectsOfType<DoorQuestion>())
		//{
		//	doorsInRoom.Add(dq);
		//}
		foreach (Switch s in FindObjectsOfType<Switch>()) {
			switchesInRoom.Add (s);
		}
		foreach (ColorBridge sb in FindObjectsOfType<ColorBridge>()) {
			colorBridgesInRoom.Add (sb);
		}
		foreach (Drawbridge db in FindObjectsOfType<Drawbridge>()) {
			drawbridgesInRoom.Add (db);
		}
		// add any manually placed enemies first
		foreach (enemyControl e in FindObjectsOfType<enemyControl>()) {
			enemies.Add (e);
		}

        // find the room's limits
        //roomLimits = new RoomLimits();
        //var tilemap = FindObjectOfType<Tiled2Unity.TiledMap>();
        //roomLimits.minX = (int)tilemap.transform.position.x;
        //roomLimits.minY = (int)tilemap.transform.position.y;
        //roomLimits.maxX = roomLimits.minX + tilemap.NumTilesWide;
        //roomLimits.maxY = roomLimits.minY - tilemap.NumTilesHigh;


        //create the appropriate amount of enemies
        if (!GameControl.control.isPaused)
		{
            // TEMP COMMENTED
			//for (int i = 0; i < numOfEnemies; i++)
			//{
			//	GameObject temp = GameObject.Instantiate(enemyControlPrefab);
   //             temp.name = "EnemyControl";
			//	temp.transform.position = new Vector2(Random.Range(-16.0f, 16.0f), Random.Range(-16.0f, 16.0f)); // hopefully we will have a better way of placing enemies
			//	if(temp.GetComponent<enemyControl>().minEnemies == 0) temp.GetComponent<enemyControl>().minEnemies = minEnemiesPerBattle;
			//	if(temp.GetComponent<enemyControl>().maxEnemies == 0) temp.GetComponent<enemyControl>().maxEnemies = maxEnemiesPerBattle; // maybe this shouldn't be here
			//	enemies.Add(temp.GetComponent<enemyControl>());
			//}
		}
		/*else if (GameControl.control.isPaused)
        {
            for (int i = 0; i < numOfEnemiesGameControl.control.enemyPos.Count; i++)
            {
                GameObject temp = GameObject.Instantiate(enemyControlPrefab);
                //temp.transform.position = new Vector2(GameControl.control.enemyPos[i].x, GameControl.control.enemyPos[i].y); // hopefully we will have a better way of placing enemies
                
                temp.GetComponent<enemyControl>().minEnemies = minEnemiesPerBattle;
                temp.GetComponent<enemyControl>().maxEnemies = maxEnemiesPerBattle; // maybe this shouldn't be here
                enemies.Add(temp.GetComponent<enemyControl>());
            }
        }*/

		// Check if this room is already tracked by the game control obj - if not, make it so!
		foreach (RoomControlData rc in GameControl.control.rooms)
		{
			if (areaIDNumber == rc.areaIDNumber) 
			{
				// synch the data
				// first the blocks -- since that's what ive got
				for (int i = 0; i < movables.Count; i++)
				{
                    for(int j = 0; j < movables.Count; j++)
                    {
                        if(movables[i].name == rc.blockData[j].blockName)
                        {
                            movables[i].transform.position = new Vector3(rc.blockData[j].position.x, rc.blockData[j].position.y, rc.blockData[j].position.z);
                            movables[i].isActivated = rc.blockData[j].isActivated;
                        }
                    }
					
					//movables [i].gameObject.SetActive (movables [i].isActivated);
				}
				// sync open chests
				//for (int i = 0; i < treasureChests.Count; i++)
				//{
				//	for (int j = 0; j < treasureChests.Count; j++)
				//	{
				//		if (treasureChests[i].name == rc.chestData[j].chestName)
				//		{
				//			treasureChests[i].isOpen = rc.chestData[j].isChestOpen;
				//		}
				//	}
				//}
				// sync doors
				//for (int i = 0; i < doorsInRoom.Count; i++)
				//{
				//	for (int j = 0; j < doorsInRoom.Count; j++)
				//	{
				//		if (doorsInRoom[i].name == rc.doorData[j].doorName)
				//		{
				//			doorsInRoom[i].gameObject.SetActive(rc.doorData[j].isLocked);
				//		}
				//	}
				//}
				// sync switches
				for (int i = 0; i < switchesInRoom.Count; i++) {
					for (int j = 0; j < switchesInRoom.Count; j++) {
						if (switchesInRoom [i].name == rc.switchData [j].switchName) {
							switchesInRoom[i].isActivated = rc.switchData[j].isActivated;
						}
					}
				}
				// sync switch bridges
				for (int i = 0; i < colorBridgesInRoom.Count; i++) {
					for (int j = 0; j < colorBridgesInRoom.Count; j++) {
						if (colorBridgesInRoom [i].name == rc.colorBridgeData [j].bridgeName) {
							colorBridgesInRoom [i].transform.Find("Bridge").transform.rotation = Quaternion.Euler (0.0f, 0.0f, rc.colorBridgeData [j].rotationZ);
						}
					}
				}
				// sync drawbridges
				for (int i = 0; i < drawbridgesInRoom.Count; i++) {
					for (int j = 0; j < drawbridgesInRoom.Count; j++) {
						if (drawbridgesInRoom [i].name == rc.drawbridgeData [j].bridgeName) {
							drawbridgesInRoom [i].transform.position = new Vector3 (drawbridgesInRoom [i].transform.position.x, rc.drawbridgeData [j].positionY, drawbridgesInRoom [i].transform.position.z);
							//drawbridgesInRoom [i].isActive = rc.drawbridgeData [j].isActive;
						}
					}
				}
				// sync enemy locations
				for (int i = 0; i < enemies.Count; i++) {
					for (int j = 0; j < enemies.Count; j++) {
						if (enemies [i].name == rc.enemyData [j].enemyName) {

							//enemies [i].CheckDistance (); // after state is saved, check the distance
							enemies[i].transform.position = new Vector3(rc.enemyData[j].position.x,rc.enemyData[j].position.y, rc.enemyData[j].position.z);
							enemies [i].beenBattled = rc.enemyData [j].battledState;

							// if the enemy has been battled and they are within the safe distance, deactivate them
							if (enemies [i] != null) {
								enemies [i].UpdateDist ();
								if (enemies [i].beenBattled 
									&& enemies [i].dist < enemies [i].safeDistance + 1.5f) {
                                    //enemies [i].GetComponent<SpriteRenderer> ().enabled = false;
                                    //enemies [i].enabled = false;
                                    enemies[i].gameObject.SetActive(false);
								} else {
									enemies [i].GetComponent<SpriteRenderer> ().enabled = true;
									enemies [i].enabled = true;
									enemies [i].beenBattled = false;
								}
							}
						}
					}
				}


				// set the amount of keys equal to the dungeon you are in
				// if < 0 (-1), then you are not in a dungeon. Set keys to 0
				if (dungeonID < 0)
				{
					GameControl.control.totalKeys = 0;
				}
				// set keys to the amount that you previously had in the current dungeon
				else
				{
					GameControl.control.totalKeys = GameControl.control.keysObtainedInDungeons[dungeonID];
				}

                // set the room limits
                roomLimits = rc.roomLimits;

				return;
			}
		}
		// if we're here, this room must not be currently tracked. Lets add it
		GameControl.control.rooms.Add(new RoomControlData());
        var newRoom = GameControl.control.rooms[GameControl.control.rooms.Count - 1];

        newRoom.areaIDNumber = areaIDNumber;
		newRoom.dungeonID = dungeonID;

		// add the necessary data to store all movable object positions
		for(int i = 0; i < movables.Count; i++)
		{
			newRoom.blockData.Add(new BlockData());
			newRoom.blockData [i].blockName = movables [i].name;
			newRoom.blockData [i].isActivated = movables [i].isActivated;
			newRoom.blockData [i].position.x = movables [i].transform.position.x;
			newRoom.blockData [i].position.y = movables [i].transform.position.y;
			newRoom.blockData [i].position.z = movables [i].transform.position.z;

		}
		//for(int i = 0; i < treasureChests.Count; i++)
		//{
		//	newRoom.chestData.Add(new TreasureData());//isChestOpen.Add(tc.isOpen);
		//	newRoom.chestData[i].isChestOpen = treasureChests[i].isOpen;
		//	newRoom.chestData[i].chestName = treasureChests[i].name;
		//}
		//for(int i = 0; i < doorsInRoom.Count; i++)
		//{
		//	newRoom.doorData.Add(new DoorData());
		//	newRoom.doorData[i].isLocked = doorsInRoom[i].gameObject.activeSelf;
		//	newRoom.doorData[i].doorName = doorsInRoom[i].name;

		//}
		for(int i = 0; i < switchesInRoom.Count; i++){
			newRoom.switchData.Add (new SwitchData ());
			newRoom.switchData [i].isActivated = switchesInRoom [i].isActivated;
			newRoom.switchData [i].switchName = switchesInRoom [i].name;
		}
		for(int i = 0; i < colorBridgesInRoom.Count; i++){
			newRoom.colorBridgeData.Add (new ColorBridgeData ());
			newRoom.colorBridgeData [i].rotationZ = colorBridgesInRoom [i].transform.Find("Bridge").transform.eulerAngles.z;
			newRoom.colorBridgeData [i].bridgeName = colorBridgesInRoom [i].name;
		}
		for (int i = 0; i < drawbridgesInRoom.Count; i++) {
			newRoom.drawbridgeData.Add (new DrawbridgeData ());
			newRoom.drawbridgeData [i].bridgeName = drawbridgesInRoom [i].name;
			newRoom.drawbridgeData [i].positionY = drawbridgesInRoom [i].transform.position.y;
			//newRoom.drawbridgeData [i].isActive = drawbridgesInRoom [i].isActive;
		}
		for (int i = 0; i < enemies.Count; i++) {
			newRoom.enemyData.Add (new EnemyControlData ());
			newRoom.enemyData [i].enemyName = enemies [i].name;
			newRoom.enemyData [i].battledState = enemies [i].beenBattled;
			newRoom.enemyData [i].position.x = enemies [i].transform.position.x;
			newRoom.enemyData [i].position.y = enemies [i].transform.position.y;
			newRoom.enemyData [i].position.z = enemies [i].transform.position.z;
		}

        newRoom.roomLimits = roomLimits;
        //Debug.Log(GameControl.control.currentCharacterState);

        if (!roomWasLoaded)
            AssignCurrentPosition();

    }

    public void SetEnemiesToNotBattled()
    {
        foreach(var e in enemies)
        {
            e.beenBattled = false;
        }
    }

    void OnEnable()
    {
        roomWasLoaded = true;

        //if (GameControl.control.currentCharacterState == characterControl.CharacterState.Battle) return;
        if (GameControl.control.isPaused) return;

        // find the room's limits
        roomLimits = new RoomLimits();
        var tilemap = FindObjectOfType<Tiled2Unity.TiledMap>();
        if (tilemap != null)
        {
            roomLimits.minX = (int)tilemap.transform.position.x;
            roomLimits.minY = (int)tilemap.transform.position.y;
            roomLimits.maxX = roomLimits.minX + tilemap.NumTilesWide;
            roomLimits.maxY = roomLimits.minY - tilemap.NumTilesHigh;
        }
                
        //GameControl.control.currentCharacterState = characterControl.CharacterState.Transition;
        AssignCurrentPosition();
        
        // start music
        GameControl.audioManager.StartMusic(music_loop);
    }

    public Gateway AssignEntrance(string exitedGatewayName)
    {
        // if string is null, then we did not enter through a gateway -- set to saved statue or an inspector set entrance
        //if(string.IsNullOrEmpty(exitedGatewayName))
        //{
        //    if (GameControl.control.taggedStatue) return entrance = GameControl.control.savedStatue;
        //    //else return entrance;
        //}

       
        if (string.IsNullOrEmpty(exitedGatewayName))
        {
            if (GameControl.control.currentCharacterState == characterControl.CharacterState.Battle)
            {
                GameControl.control.currentCharacterState = characterControl.CharacterState.Normal;
                return null;
            }
            // default send to the first gateway in the room's position
            else if (gatewaysInRoom.Count > 0) return gatewaysInRoom[0];

            // if there are no gateways for some reason, set to default current position
            //else return entrance = GameControl.control.currentPosition;
            else return null;

        }

        else
        {
            foreach (Gateway gateway in gatewaysInRoom)
            {
                if (string.Compare(gateway.gatewayName, exitedGatewayName) == 0)
                {
                    return gateway;
                }
            }
            // if we reached this point, we haven't found the gateway's twin -- so set it to first in list, just in case
            if (gatewaysInRoom.Count > 0)
            {
                return gatewaysInRoom[0];
            }
            else
                //return entrance = GameControl.control.currentPosition;
                return null;
        }
        

    }
    void AssignCurrentPosition()
    {
        if (gatewaysInRoom.Count <= 0)
        {
            foreach (Gateway g in FindObjectsOfType<Gateway>())
            {
                gatewaysInRoom.Add(g);
            }
        }

        //tell the gameControl object what it needs to know
        GameControl.control.currentRoom = this;
        
        //// if player is coming back from battle, then there is no entrance
        //if (GameControl.control.currentCharacterState == characterControl.CharacterState.Battle
        //    || GameControl.control.currentCharacterState == characterControl.CharacterState.Defeat)
        //{
        //    return;
        //}
        
        var gatewayEntrance = AssignEntrance(GameControl.control.sceneStartGateName);
        if (gatewayEntrance != null)
        {
            GameControl.control.currentEntranceGateway = gatewayEntrance;
            GameControl.control.areaEntrance = gatewayEntrance.transform.position;
        }
        else
        {
            GameControl.control.currentEntranceGateway = null; 
            GameControl.control.areaEntrance = GameControl.control.currentPosition;
        }


        //GameControl.control.characterControl.canMove = false;
        GameControl.control.currentPosition = GameControl.control.areaEntrance;
        GameControl.control.sceneStartGateName = "";
        
        var camera = FindObjectOfType<CameraController>();
        //camera.StayWithinRoomAtStart();
        //GameControl.control.characterControl.RoomTransition(gatewayEntrance.transform.position, gatewayEntrance.entrancePos);

    }

    // when you leave and reenter a scene from battle, Gateway becomes null
    // find the correct gateway based off of the area entrance
    public Gateway FindCurrentGateway(Vector2 areaEntrance)
    {
        if (GameControl.control.currentCharacterState == characterControl.CharacterState.Battle)
            return null;

        foreach(Gateway g in gatewaysInRoom)
        {
            if (areaEntrance.x == g.transform.position.x && areaEntrance.y == g.transform.position.y)
                return g;
        }
        return null;
    }
}

