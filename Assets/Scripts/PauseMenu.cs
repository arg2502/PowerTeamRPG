using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : Menu {

    protected List<string> buttonDescription;
    protected GameObject descriptionText;

	// Use this for initialization
	void Start () {
        base.Start();
        buttonArray = new GameObject[contentArray.Count];
        buttonDescription = new List<string>();

        buttonDescription.Add("View your team's stats, status ailments, and assign them equipment.");
        buttonDescription.Add("Access your entire inventory.");
        buttonDescription.Add("Save your game.");
        buttonDescription.Add("Return to the game.");

        for (int i = 0; i < contentArray.Count; i++)
        {
            // create a button
            buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(camera.transform.position.x - 600, camera.transform.position.y + (250 + b.height) + (i * -(b.height + b.height / 2)));

            // assign text
            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);

        }

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        //Create the description text object
        descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
        descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]);
        descriptionText.transform.position = new Vector2(camera.transform.position.x + 200, buttonArray[0].transform.position.y + 15);

        //call change text method to correctly size text and avoid a certain bug
        ChangeText();
	}

    string FormatText(string str)
    {
        string formattedString = null;
        int desiredLength = 40;
        string[] wordArray = str.Split(' ');
        int lineLength = 0;
        foreach (string s in wordArray)
        {
            //if the current line plus the length of the next word and SPACE is greater than the desired line length
            if (s.Length + 1 + lineLength > desiredLength)
            {
                //go to new line
                formattedString += "\n" + s + " ";
                //starting a new line
                lineLength = s.Length;
            }
            else
            {
                formattedString += s + " ";
                lineLength += s.Length + 1;
            }
        }
        return formattedString;
    }
    public override void ButtonAction(string label)
    {
        switch (label)
        {
            case "Team Info":
                break;
            case "Inventory":
                break;
            case "Save":
                break;
            case "Exit Menu":
                UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene);
                break;
        }
    }

	// Update is called once per frame
	void Update () {
        base.Update();

        // Update the description
        descriptionText.GetComponent<TextMesh>().text = FormatText(buttonDescription[selectedIndex]);

        PressButton(KeyCode.Space);
	}
}
