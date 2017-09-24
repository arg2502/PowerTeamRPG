using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public List<Enemy> possibleEnemies;// Array of possible enemy types
	public List<enemyControl> enemies = new List<enemyControl>();
	public GameObject enemyControlPrefab; // a generic enemy control object
	public int minEnemiesPerBattle;
	public int maxEnemiesPerBattle;
	public List<MovableOverworldObject> movables = new List<MovableOverworldObject>();
	public List<TreasureChest> treasureChests = new List<TreasureChest>();
	public List<DoorQuestion> doorsInRoom = new List<DoorQuestion>();
	public List<Switch> switchesInRoom = new List<Switch> ();
	public List<ColorBridge> colorBridgesInRoom = new List<ColorBridge>();
	public List<Drawbridge> drawbridgesInRoom = new List<Drawbridge>();


	// Use this for initialization
	void Start () {

		// convert arrays to lists
        
		foreach(MovableOverworldObject m in GameObject.FindObjectsOfType<MovableOverworldObject>())
		{
			movables.Add(m);
		}
		foreach(TreasureChest tc in GameObject.FindObjectsOfType<TreasureChest>())
		{
			treasureChests.Add(tc);
		}
		foreach(DoorQuestion dq in GameControl.FindObjectsOfType<DoorQuestion>())
		{
			doorsInRoom.Add(dq);
		}
		foreach (Switch s in GameControl.FindObjectsOfType<Switch>()) {
			switchesInRoom.Add (s);
		}
		foreach (ColorBridge sb in GameControl.FindObjectsOfType<ColorBridge>()) {
			colorBridgesInRoom.Add (sb);
		}
		foreach (Drawbridge db in GameControl.FindObjectsOfType<Drawbridge>()) {
			drawbridgesInRoom.Add (db);
		}
		// add any manually placed enemies first
		foreach (enemyControl e in GameObject.FindObjectsOfType<enemyControl>()) {
			enemies.Add (e);
		}

		//create the appropriate amount of enemies
		if (!GameControl.control.isPaused)
		{


			for (int i = 0; i < numOfEnemies; i++)
			{
				GameObject temp = GameObject.Instantiate(enemyControlPrefab);
                temp.name = "EnemyControl";
				temp.transform.position = new Vector2(Random.Range(-1000.0f, 1000.0f), Random.Range(-1000.0f, 1000.0f)); // hopefully we will have a better way of placing enemies
				if(temp.GetComponent<enemyControl>().minEnemies == 0) temp.GetComponent<enemyControl>().minEnemies = minEnemiesPerBattle;
				if(temp.GetComponent<enemyControl>().maxEnemies == 0) temp.GetComponent<enemyControl>().maxEnemies = maxEnemiesPerBattle; // maybe this shouldn't be here
				enemies.Add(temp.GetComponent<enemyControl>());
			}
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
					movables[i].transform.position = new Vector3(rc.blockData[i].position.x, rc.blockData[i].position.y, rc.blockData[i].position.z);
					movables [i].isActivated = rc.blockData [i].isActivated;
					//movables [i].gameObject.SetActive (movables [i].isActivated);
				}
				// sync open chests
				for (int i = 0; i < treasureChests.Count; i++)
				{
					for (int j = 0; j < treasureChests.Count; j++)
					{
						if (treasureChests[i].name == rc.chestData[j].chestName)
						{
							treasureChests[i].isOpen = rc.chestData[j].isChestOpen;
						}
					}
				}
				// sync doors
				for (int i = 0; i < doorsInRoom.Count; i++)
				{
					for (int j = 0; j < doorsInRoom.Count; j++)
					{
						if (doorsInRoom[i].name == rc.doorData[j].doorName)
						{
							doorsInRoom[i].gameObject.SetActive(rc.doorData[j].isLocked);
						}
					}
				}
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
									&& enemies [i].dist < enemies [i].safeDistance + 100.0f) {
									enemies [i].GetComponent<SpriteRenderer> ().enabled = false;
									enemies [i].enabled = false;
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


				return;
			}
		}
		// if we're here, this room must not be currently tracked. Lets add it
		GameControl.control.rooms.Add(new RoomControlData());
		GameControl.control.rooms[GameControl.control.rooms.Count - 1].areaIDNumber = areaIDNumber;
		GameControl.control.rooms[GameControl.control.rooms.Count - 1].dungeonID = dungeonID;

		// add the necessary data to store all movable object positions
		for(int i = 0; i < movables.Count; i++)
		{
			GameControl.control.rooms[GameControl.control.rooms.Count - 1].blockData.Add(new BlockData());
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].blockData [i].blockName = movables [i].name;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].blockData [i].isActivated = movables [i].isActivated;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].blockData [i].position.x = movables [i].transform.position.x;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].blockData [i].position.y = movables [i].transform.position.y;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].blockData [i].position.z = movables [i].transform.position.z;

		}
		for(int i = 0; i < treasureChests.Count; i++)
		{
			GameControl.control.rooms[GameControl.control.rooms.Count - 1].chestData.Add(new TreasureData());//isChestOpen.Add(tc.isOpen);
			GameControl.control.rooms[GameControl.control.rooms.Count - 1].chestData[i].isChestOpen = treasureChests[i].isOpen;
			GameControl.control.rooms[GameControl.control.rooms.Count - 1].chestData[i].chestName = treasureChests[i].name;
		}
		for(int i = 0; i < doorsInRoom.Count; i++)
		{
			GameControl.control.rooms[GameControl.control.rooms.Count - 1].doorData.Add(new DoorData());
			GameControl.control.rooms[GameControl.control.rooms.Count - 1].doorData[i].isLocked = doorsInRoom[i].gameObject.activeSelf;
			GameControl.control.rooms[GameControl.control.rooms.Count - 1].doorData[i].doorName = doorsInRoom[i].name;

		}
		for(int i = 0; i < switchesInRoom.Count; i++){
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].switchData.Add (new SwitchData ());
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].switchData [i].isActivated = switchesInRoom [i].isActivated;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].switchData [i].switchName = switchesInRoom [i].name;
		}
		for(int i = 0; i < colorBridgesInRoom.Count; i++){
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].colorBridgeData.Add (new ColorBridgeData ());
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].colorBridgeData [i].rotationZ = colorBridgesInRoom [i].transform.Find("Bridge").transform.eulerAngles.z;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].colorBridgeData [i].bridgeName = colorBridgesInRoom [i].name;
		}
		for (int i = 0; i < drawbridgesInRoom.Count; i++) {
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].drawbridgeData.Add (new DrawbridgeData ());
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].drawbridgeData [i].bridgeName = drawbridgesInRoom [i].name;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].drawbridgeData [i].positionY = drawbridgesInRoom [i].transform.position.y;
			//GameControl.control.rooms [GameControl.control.rooms.Count - 1].drawbridgeData [i].isActive = drawbridgesInRoom [i].isActive;
		}
		for (int i = 0; i < enemies.Count; i++) {
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].enemyData.Add (new EnemyControlData ());
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].enemyData [i].enemyName = enemies [i].name;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].enemyData [i].battledState = enemies [i].beenBattled;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].enemyData [i].position.x = enemies [i].transform.position.x;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].enemyData [i].position.y = enemies [i].transform.position.y;
			GameControl.control.rooms [GameControl.control.rooms.Count - 1].enemyData [i].position.z = enemies [i].transform.position.z;
		}


        AssignCurrentPosition();
    }

    void OnLevelWasLoaded(int level)
    {
        foreach (Gateway g in GameObject.FindObjectsOfType<Gateway>())
        {
            gatewaysInRoom.Add(g);
        }

        AssignCurrentPosition();

    }

    public Vector2 AssignEntrance(string exitedGatewayName)
    {
        // if string is null, then we did not enter through a gateway -- set to saved statue or an inspector set entrance
        if(string.IsNullOrEmpty(exitedGatewayName))
        {
            if (GameControl.control.taggedStatue) return entrance = GameControl.control.savedStatue;
            //else return entrance;
        }

        foreach (Gateway gateway in gatewaysInRoom)
        {
            if (string.Compare(gateway.gatewayName, exitedGatewayName) == 0)
            {
                return entrance = gateway.entrancePos;                
            }
        }
        // if we reached this point, we haven't found the gateway's twin -- so set it to first in list, just in case
        if (gatewaysInRoom.Count > 0)
        {
            return entrance = gatewaysInRoom[0].entrancePos;
        }
        else
            return entrance;

    }
    void AssignCurrentPosition()
    {
        //tell the gameControl object what it needs to know
        GameControl.control.areaEntrance = AssignEntrance(GameControl.control.sceneStartGateName);
        GameControl.control.currentPosition = GameControl.control.areaEntrance;
    }
}

