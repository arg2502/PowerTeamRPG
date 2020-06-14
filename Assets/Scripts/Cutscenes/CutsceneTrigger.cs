using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour {

    [SerializeField] string subquestID;

	void Start () {

        var roomList = GameControl.control.CheckForTriggerCutscenes();
        gameObject.SetActive(roomList.Contains(subquestID));
	}	

    public void TriggerCutscene()
    {
        GameControl.control.PlayCutscene(subquestID);
        gameObject.SetActive(false);
    }
	
}
