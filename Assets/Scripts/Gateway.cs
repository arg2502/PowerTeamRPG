using UnityEngine;
using System.Collections;

public class Gateway : MonoBehaviour {

    // the specific name of the gateway.
    // In the next room, this door will have a twin with the same way -- that is the gateway where the player will show up at
    public string gatewayName;

    // the scene that will load when the player enters the gateway
    public string sceneName;

    // place where the player will end up at
    public Vector2 entrancePos;

    public void NextScene()
    {
        GameControl.control.AssignEntrance(gatewayName);
        GameControl.control.RecordRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

    }

}
