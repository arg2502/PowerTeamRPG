using UnityEngine;
using System.Collections;

public class ShopKeeperQuestion : NPCQuestion {

	characterControl player;
	ShopKeeper shopKeeper;
	public bool canBuy; // bool to determine whether the player has enough gold to buy

	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<characterControl> ().GetComponent<characterControl> ();

		// set player's answers, since they should be the same no matter the shopkeeper
		answerList.Add ("Buy");
		answerList.Add ("Sell");
		answerList.Add ("Nothing");

		shopKeeper = GetComponent<ShopKeeper> (); // both ShopKeeper & ShopKeeperQuestion should be attached to the same game object

		base.Start ();
	}

	void Update()
	{
		// check to see if the player has enough gold to buy anything
		// go to buy room return if player CAN afford at least one item
		foreach(GameObject g in shopKeeper.inventory)
		{
			if (GameControl.control.totalGold >= g.GetComponent<Item> ().price) {
				canBuy = true;
				return;
			}
		}
		// if we've reached here, then the player cannot afford to buy anything
		canBuy = false;


	}
	public override void ResponseAction (int responseIndex)
	{
		// 0 - Buy
		if (responseIndex == 0) {
			if (canBuy) {
				GameControl.control.currentPosition = player.transform.position; //record the player's position
				GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
				GameControl.control.RecordRoom();
				GameControl.control.RecordPauseMenu();
				GameControl.control.RecordEnemyPos();
				UnityEngine.SceneManagement.SceneManager.LoadScene(shopKeeper.sceneName);
			} else {
				//responseList = possibleResponses [responseIndex];
			}

			
		}
		// 1 - Sell
		else if (responseIndex == 1) {
			GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

		}
		// 2 - Nothing
		else {
		
		}
	}

}
