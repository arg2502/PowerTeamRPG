using UnityEngine;
using System.Collections;

public class testSceneChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Go to battle"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("testMenu");
        }
	}
}
