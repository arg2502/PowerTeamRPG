using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueMenu : Menu {

    // list to pass into content array
    //public List<string> questionList;
    public NPCDialogue npc;

	// Use this for initialization
	void Start () {      

	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    public void EnableQuestionMenu()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        npc = GetComponent<NPCDialogue>();
        numOfRow = npc.questionList.Count;
        buttonArray = new GameObject[numOfRow];
        selectedIndex = 0;
        scrollIndex = 0;

        for (int i = 0; i < numOfRow; i++)
        {
            // create a button
            buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y - (250 - b.height) + (i * -(b.height + b.height / 2)));

            // assign text
            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);
            
            // set to show in front of dialogue box
            //b.GetComponent<Renderer>().sortingOrder = 900;
            //b.textObject.GetComponent<Renderer> ().sortingOrder = 900;
        }
            // set selected button
            buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

    }
    
}
