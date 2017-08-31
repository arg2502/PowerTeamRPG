using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelUpMenu : Menu {

    // Attributes
    protected List<int> originalStats; // stores what the hero's current stats are
    protected List<GameObject> statNumbers; // shows what the character's projected stats will be
    protected List<GameObject> statBoosts; // shows how much the stats will increase by
    protected List<int> statBoostInts; // stores how much the player wants to increase each stat by
    protected List<string> statDescription; // explanation of the stat
    protected GameObject descriptionText;
    protected HeroData hero; // the current hero who is leveling up. This will be provided by the GameControl obj
    protected int remainingPoints;// = 10; // this should be determined by the hero thsat is passed in by GameControl

    // Bool for knowing if we should continue with the level up menu -- this should probably be found in the skill tree section
    protected bool levelUp = false;

    //color green
    Color myGreen = new Color(0.1f, 0.6f, 0.2f);

	// Use this for initialization
	void Start () {

        originalStats = new List<int>();
        statBoostInts = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0};
        statNumbers = new List<GameObject>();
        statBoosts = new List<GameObject>();
        statDescription = new List<string>();

        //Set the hero
        for (int i = 0; i < GameControl.control.heroList.Count; i++)
        {
            if (GameControl.control.heroList[i].statBoost) 
            {
                GameControl.control.heroList[i].statBoost = false;
                hero = GameControl.control.heroList[i];
                break;
            }
        }
        originalStats.Add(hero.hpMax);
        originalStats.Add(hero.pmMax);
        originalStats.Add(hero.atk);
        originalStats.Add(hero.def);
        originalStats.Add(hero.mgkAtk);
        originalStats.Add(hero.mgkDef);
        originalStats.Add(hero.luck);
        originalStats.Add(hero.evasion);
        originalStats.Add(hero.spd);
        remainingPoints = hero.levelUpPts;

        // fill the stat description list with a description for each stat
        statDescription.Add(hero.name + "'s maximum health points. This stat determines how much damage " + hero.name
            + " can take before falling in combat.");
        statDescription.Add(hero.name + "'s maximum Power Magic points. This is a pool of magical strength that " +
            hero.name + " calls upon to use skills and spells. When depleted, " + hero.name + " must rely solely on their ability to strike.");
        statDescription.Add(hero.name + "'s physical strength. " + hero.name + "'s skills will inflict more damage as this" +
            " stat increases.");
        statDescription.Add(hero.name + "'s physical defense. " + hero.name + " will become more resistant to physical damage" +
            " as this stat increases.");
        statDescription.Add(hero.name + "'s magical might. " + hero.name + "'s spells will become more effective as" +
            " this stat increases. Attack spells will deal more damage and healing spells will restore more health.");
        statDescription.Add(hero.name + "'s magical defense. " + hero.name + " will become more resistant to magical damage" +
            " as this stat increases.");
        statDescription.Add(hero.name + "'s luck. " + hero.name + "'s chances of landing a critical hit increases" +
            " as this stat increases. Critical hits deal 1.5 times the usual damage.");
        statDescription.Add(hero.name + "'s evasiveness. " + hero.name + "'s ability to avoid enemy attacks increases" +
            " as this stat increases.");
        statDescription.Add(hero.name + "'s speed. This stat determines the turn order in battle.");
        statDescription.Add("Finalize the changes made to " + hero.name + "'s stats and proceed to skill trees.");

        numOfRow = 9; // the number of stats - 9

        base.Start();
        buttonArray = new GameObject[10];
        
        for (int i = 0; i < numOfRow; i++)
        {
            // create a button
            buttonArray[i] = (GameObject)Instantiate(Resources.Load("Prefabs/LevelUpButtonPrefab"));
            buttonArray[i].name = "LevelUpButton" + i;
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(camera.transform.position.x - 9.375f, camera.transform.position.y + (3.9f + b.height) + (i * -(b.height + b.height / 2)));

            // assign text
            b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.textObject.name = "LevelUpText" + i;
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);

            // create the text objects that will display the hero's stats, as well as the points they are adding
            statNumbers.Add((GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab")));
            statNumbers[i].name = "StatNumbers" + i;
            statNumbers[i].GetComponent<TextMesh>().text = "" + originalStats[i];  // should initially be equal to the original stats
            statNumbers[i].GetComponent<TextMesh>().color = Color.black;
            statNumbers[i].transform.position = new Vector2(buttonArray[i].transform.position.x + 3.9f, buttonArray[i].transform.position.y);

            // create the text objects that will display the amount that the player is adding to the stats
            statBoosts.Add((GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab")));
            statBoosts[i].name = "StatBoosts" + i;
            statBoosts[i].GetComponent<TextMesh>().text = "( +" + (statBoostInts[i]) + ")";
            statBoosts[i].GetComponent<TextMesh>().color = Color.black;
            statBoosts[i].transform.position = new Vector2(buttonArray[i].transform.position.x + 7.8f, buttonArray[i].transform.position.y);
        }

        // create the labels above the columns
        statNumbers.Add((GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab")));
        statNumbers[statNumbers.Count - 1].name = "StatPoints";
        statNumbers[statNumbers.Count - 1].GetComponent<TextMesh>().text = "Stat Points";
        statNumbers[statNumbers.Count - 1].GetComponent<TextMesh>().color = Color.black;
        statNumbers[statNumbers.Count - 1].transform.position = new Vector2(statNumbers[0].transform.position.x, statNumbers[0].transform.position.y + 1.2f);

        statBoosts.Add((GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab")));
        statBoosts[statBoosts.Count - 1].name = "AllocatedPoints";
        statBoosts[statBoosts.Count - 1].GetComponent<TextMesh>().text = "Allocated Points";
        statBoosts[statBoosts.Count - 1].GetComponent<TextMesh>().color = Color.black;
        statBoosts[statBoosts.Count - 1].transform.position = new Vector2(statBoosts[0].transform.position.x, statBoosts[0].transform.position.y + 1.2f);

        // create the label that shows remaining points
        statBoosts.Add((GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab")));
        statBoosts[statBoosts.Count - 1].name = "RemainingPoints";
        statBoosts[statBoosts.Count - 1].GetComponent<TextMesh>().text = "Remaining Points: " + remainingPoints;
        statBoosts[statBoosts.Count - 1].GetComponent<TextMesh>().color = Color.black;
        statBoosts[statBoosts.Count - 1].transform.position = new Vector2(statBoosts[8].transform.position.x, statBoosts[8].transform.position.y - 1.95f);

        // create the final button
        buttonArray[buttonArray.Length - 1] = (GameObject)Instantiate(Resources.Load("Prefabs/ButtonPrefab"));
        buttonArray[buttonArray.Length - 1].name = "FinalButton";
        MyButton bt = buttonArray[9].GetComponent<MyButton>();
        buttonArray[9].transform.position = new Vector2(buttonArray[8].transform.position.x, buttonArray[8].transform.position.y - 1.95f);
        // assign text
        bt.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
        bt.textObject.name = "FinalButtonText";
        bt.labelMesh = bt.textObject.GetComponent<TextMesh>();
        bt.labelMesh.text = contentArray[9];
        bt.labelMesh.transform.position = new Vector3(buttonArray[9].transform.position.x, buttonArray[9].transform.position.y, -1);

        //Create the description text object
        descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/LeftTextPrefab"));
        descriptionText.name = "DescriptionText";
        descriptionText.GetComponent<TextMesh>().text = FormatText(statDescription[selectedIndex]);
        descriptionText.transform.position = new Vector2(camera.transform.position.x + 3.125f, statNumbers[0].transform.position.y + 0.23f);

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        //call change text method to correctly size text and avoid a certain bug
        ChangeText();

        // If the player enters here with no points, make the buttons reflect that
        if (remainingPoints == 0)
        {
            DisableButtons();
        }
	}

    // decide on what action to take/mode to change depending on the button pressed
    public override void ButtonAction(string label)
    {
        if (label == "Allocate Stat Points")
        {
            // Actually add the allocated points and move on to the next step of leveling up -- skills
            foreach (HeroData hd in GameControl.control.heroList)
            {
                if (hd.identity == hero.identity)
                {
                    hd.hpMax += statBoostInts[0];
                    hd.pmMax += statBoostInts[1];
                    hd.atk += statBoostInts[2];
                    hd.def += statBoostInts[3];
                    hd.mgkAtk += statBoostInts[4];
                    hd.mgkDef += statBoostInts[5];
                    hd.luck += statBoostInts[6];
                    hd.evasion += statBoostInts[7];
                    hd.spd += statBoostInts[8];
                    hd.levelUpPts = remainingPoints;
                }
                // also use this loop to figure out if we need to level up another hero
                //if (hd.levelUp) { levelUp = true; }
            }

            // Now go to the skill tree portion
            if (hero.skillTree) { UnityEngine.SceneManagement.SceneManager.LoadScene("SkillTreeMenu"); }
            else { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); }

            // Either go back to current room, or move to level up the next hero
            // This should be in the skills area, but it is here since I haven't done the skills yet
            //if (levelUp == true) { UnityEngine.SceneManagement.SceneManager.LoadScene("LevelUpMenu"); }
            //else { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); }
            //else { UnityEngine.SceneManagement.SceneManager.LoadScene("SkillTreeMenu"); }
        }
        else // Change all of the labels and necessary sprites
        {
            if (Input.GetKeyUp(GameControl.control.rightKey))
            {
                // Add to the appropriate stat boost number based on button index
                if (remainingPoints > 0)
                {
                    statBoostInts[selectedIndex] += 1;
                    remainingPoints -= 1;

                    // change the color of the stat label to green if it is getting points
                    statNumbers[selectedIndex].GetComponent<TextMesh>().color = myGreen;
                    statBoosts[selectedIndex].GetComponent<TextMesh>().color = myGreen;

                    // Change sprite values for the button to reflect that the left button works
                    //buttonArray[selectedIndex].GetComponent<MyButton>().normalTexture = Resources.Load("Sprites/lvlUpMenu/lvlUpButton", typeof(Sprite)) as Sprite;
                    buttonArray[selectedIndex].GetComponent<MyButton>().hoverTexture = Resources.Load("Sprites/lvlUpMenu/hoverlvlUpButton", typeof(Sprite)) as Sprite;
                    buttonArray[selectedIndex].GetComponent<MyButton>().activeTexture = Resources.Load("Sprites/lvlUpMenu/activelvlUpButton", typeof(Sprite)) as Sprite;
                    //buttonArray[selectedIndex].GetComponent<MyButton>().disabledTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
                    
                }
            }

            if (Input.GetKeyUp(GameControl.control.leftKey))
            {
                // subtract from the appropriate stat only if it has had points added
                if (statBoostInts[selectedIndex] > 0)
                {
                    statBoostInts[selectedIndex] -= 1;
                    remainingPoints += 1;

                    // If this makes the remaining points jump from 0 to 1, we must reassign some sprites
                    if (remainingPoints == 1)
                    {
                        for (int i = 0; i < numOfRow; i++)
                        {
                            // if statboost has been applied to this index, then the left button should be enabled
                            if (statBoostInts[i] > 0)
                            {
                                buttonArray[i].GetComponent<MyButton>().normalTexture = Resources.Load("Sprites/lvlUpMenu/lvlUpButton", typeof(Sprite)) as Sprite;
                                buttonArray[i].GetComponent<MyButton>().hoverTexture = Resources.Load("Sprites/lvlUpMenu/hoverlvlUpButton", typeof(Sprite)) as Sprite;
                                buttonArray[i].GetComponent<MyButton>().activeTexture = Resources.Load("Sprites/lvlUpMenu/activelvlUpButton", typeof(Sprite)) as Sprite;
                                //buttonArray[i].GetComponent<MyButton>().disabledTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
                            }

                            // if not, only the right arrow should be enabled
                            else if (statBoostInts[i] == 0)
                            {
                                buttonArray[i].GetComponent<MyButton>().normalTexture = Resources.Load("Sprites/lvlUpMenu/lvlUpButton", typeof(Sprite)) as Sprite;
                                buttonArray[i].GetComponent<MyButton>().hoverTexture = Resources.Load("Sprites/lvlUpMenu/hoverDisabledLeftlvlUpButton", typeof(Sprite)) as Sprite;
                                buttonArray[i].GetComponent<MyButton>().activeTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledLeftlvlUpButton", typeof(Sprite)) as Sprite;
                                //buttonArray[i].GetComponent<MyButton>().disabledTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
                            }
                        }
                    }
                }

                // Change the color of the stat label back to white if it is no longer recieving points
                if (statBoostInts[selectedIndex] == 0)
                {
                    statNumbers[selectedIndex].GetComponent<TextMesh>().color = Color.black;
                    statBoosts[selectedIndex].GetComponent<TextMesh>().color = Color.black;

                    // Change sprite values for the button to reflect that the left button no longer works
                    //buttonArray[selectedIndex].GetComponent<MyButton>().normalTexture = Resources.Load("Sprites/lvlUpMenu/disabledLeftlvlUpButton", typeof(Sprite)) as Sprite;
                    buttonArray[selectedIndex].GetComponent<MyButton>().hoverTexture = Resources.Load("Sprites/lvlUpMenu/hoverDisabledLeftlvlUpButton", typeof(Sprite)) as Sprite;
                    buttonArray[selectedIndex].GetComponent<MyButton>().activeTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledLeftlvlUpButton", typeof(Sprite)) as Sprite;
                    //buttonArray[selectedIndex].GetComponent<MyButton>().disabledTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
                }
                
            }
            
            // Update the labels
            statBoosts[selectedIndex].GetComponent<TextMesh>().text = "( +" + (statBoostInts[selectedIndex]) + ")";
            statNumbers[selectedIndex].GetComponent<TextMesh>().text = "" + (originalStats[selectedIndex] + statBoostInts[selectedIndex]);
            statBoosts[statBoosts.Count - 1].GetComponent<TextMesh>().text = "Remaining Points: " + remainingPoints; // the remaining points label

            // If there are no more allocation points remaining, all of the buttons' sprites need to be changed
            if (remainingPoints == 0)
            {
                DisableButtons();
            }
        }
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

    void DisableButtons()
    {
        for (int i = 0; i < numOfRow; i++)
        {
            // if statboost has been applied to this index, then the left button should be enabled
            if (statBoostInts[i] > 0)
            {
                //buttonArray[i].GetComponent<MyButton>().normalTexture = Resources.Load("Sprites/lvlUpMenu/disabledRightlvlUpButton", typeof(Sprite)) as Sprite;
                buttonArray[i].GetComponent<MyButton>().hoverTexture = Resources.Load("Sprites/lvlUpMenu/hoverDisabledRightlvlUpButton", typeof(Sprite)) as Sprite;
                buttonArray[i].GetComponent<MyButton>().activeTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledRightlvlUpButton", typeof(Sprite)) as Sprite;
                //buttonArray[i].GetComponent<MyButton>().disabledTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
            }

            // if not, both arrows should be disabled
            else if (statBoostInts[i] == 0)
            {
                buttonArray[i].GetComponent<MyButton>().normalTexture = Resources.Load("Sprites/lvlUpMenu/disabledlvlUpButton", typeof(Sprite)) as Sprite;
                buttonArray[i].GetComponent<MyButton>().hoverTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
                buttonArray[i].GetComponent<MyButton>().activeTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
                //buttonArray[i].GetComponent<MyButton>().disabledTexture = Resources.Load("Sprites/lvlUpMenu/activeDisabledlvlUpButton", typeof(Sprite)) as Sprite;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();

        // Update the description
        descriptionText.GetComponent<TextMesh>().text = FormatText(statDescription[selectedIndex]);

        // The last button is different than the others, so make sure the last button only responds to space
        if (selectedIndex < buttonArray.Length - 1)
        {
            PressButton(GameControl.control.rightKey);
            PressButton(GameControl.control.leftKey);
        }
        else if (selectedIndex == buttonArray.Length - 1)
        {
            PressButton(GameControl.control.selectKey);
        }
        
	}
}
