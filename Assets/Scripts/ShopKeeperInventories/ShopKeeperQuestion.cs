using UnityEngine;
using System.Collections;

public class ShopKeeperQuestion : NPCQuestion {

	characterControl player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<characterControl> ().GetComponent<characterControl> ();

		// set player's answers, since they should be the same no matter the shopkeeper
		answerList.Add ("Buy");
		answerList.Add ("Sell");
		answerList.Add ("Nothing");

		base.Start ();
	}
	
	public override void ResponseAction (int responseIndex)
	{
		// 0 - Buy
		if (responseIndex == 0) {
			GameControl.control.currentPosition = player.transform.position; //record the player's position
			GameControl.control.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; // record the current scene
			GameControl.control.RecordRoom();
			GameControl.control.RecordPauseMenu();
			GameControl.control.RecordEnemyPos();
			UnityEngine.SceneManagement.SceneManager.LoadScene("ShopKeeperInventory");
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
