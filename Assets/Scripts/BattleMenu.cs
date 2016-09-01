using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleMenu : Menu {

    //numbers for the end of the battle
    int loss; // the amount of gold the player will lose for failing
    int gold; // the amount of gold awarded for victory
    int exp; // the amount of experience awarded for victory

    // enumeration to determine what menu is shown
    enum MenuReader { main, attack, skills, spells, summons, items, flee, targeting, battle, victory, failure };
    MenuReader state = MenuReader.main;

    // previous state for back button
    MenuReader prevState;

    // the denigen who is taking the turn
    public GameObject[] denigenGOArray;
    public List<Denigen> denigenArray;
    public Denigen currentDenigen;
    public int currentDenigenIndex = 0;

    // array of the heroes, mainly useful for enemy targeting, but everyone can use it
    public List<Denigen> heroList = new List<Denigen>() { };
    public List<Denigen> enemyList = new List<Denigen>() { };
    public List<Vector2> enemyPositions = new List<Vector2>() {new Vector2(480.0f, -90.0f), new Vector2(702.0f, 33.0f),
                                    new Vector2(361.0f, 104.0f), new Vector2(580.0f, 231.0f), new Vector2(262.0f, 286.0f)};
    public List<Vector2> heroPositions = new List<Vector2>() {new Vector2(-480.0f, -90.0f), new Vector2(-702.0f, 33.0f),
                                    new Vector2(-361.0f, 104.0f), new Vector2(-580.0f, 231.0f), new Vector2(-262.0f, 286.0f)};

    //The HUD to display stats, for now just text prefabs
    public List<GameObject> heroCards = new List<GameObject>() { };
    public List<GameObject> enemyCards = new List<GameObject>() { };

    // The names of attacks to be executed during the battle phase
    // their index in this list will correspond to the denigens' index in the denigen array
    public List<string> commands = new List<string>() { };
    string command;

    //Cursors for targeting attacks
    public GameObject[] cursors;

    //Int for tracking which denigen should act in a battle phase
    public int commandIndex = 0;

    //Holds what is written on the screen during battle
    public GameObject battleText;
    //holds a queue of the text for the battleText obj
    public List<string> battleTextList = new List<string>() { };
    int textIndex;

	// Use this for initialization
	void Start () {
        base.Start();
        numOfRow = 4;
        contentArray = new List<string> { "Attack", "Block", "Items", "Flee" };
        buttonArray = new GameObject[numOfRow];

        // set currentDenigen - temp
        //denigenGOArray = GameObject.FindGameObjectsWithTag("Denigen");
        denigenArray = new List<Denigen>() { };
        // fill Denigen array -- check for existing heroes
        if (GameControl.control.heroList != null)
        {
            List<HeroData> tempHeroes = GameControl.control.heroList;
            GameObject temp;
            for (int i = 0; i < tempHeroes.Count; i++)
            {
                switch (tempHeroes[i].name)
                {
                    case "Jethro":
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/JethroPrefab"));
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    case "Cole":
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/ColePrefab"));
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    case "Eleanor":
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/EleanorPrefab"));
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    case "Juliette":
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/JuliettePrefab"));
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    case "Selene":
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/SelenePrefab"));
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    default:
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/JethroPrefab"));
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                }
                //bring in all of the stats
                denigenArray[i].name = tempHeroes[i].name;
                denigenArray[i].Level = tempHeroes[i].level;
                denigenArray[i].hp = tempHeroes[i].hp;
                denigenArray[i].hpMax = tempHeroes[i].hpMax;
                denigenArray[i].pm = tempHeroes[i].pm;
                denigenArray[i].pmMax = tempHeroes[i].pmMax;
                denigenArray[i].atkBat = tempHeroes[i].atk;
                denigenArray[i].Atk = tempHeroes[i].atk;
                denigenArray[i].defBat = tempHeroes[i].def;
                denigenArray[i].Def = tempHeroes[i].def;
                denigenArray[i].mgkAtkBat = tempHeroes[i].mgkAtk;
                denigenArray[i].MgkAtk = tempHeroes[i].mgkAtk;
                denigenArray[i].mgkDefBat = tempHeroes[i].mgkDef;
                denigenArray[i].MgkDef = tempHeroes[i].mgkDef;
                denigenArray[i].luckBat = tempHeroes[i].luck;
                denigenArray[i].Luck = tempHeroes[i].luck;
                denigenArray[i].evasionBat = tempHeroes[i].evasion;
                denigenArray[i].Evasion = tempHeroes[i].evasion;
                denigenArray[i].spdBat = tempHeroes[i].spd;
                denigenArray[i].Spd = tempHeroes[i].spd;
                denigenArray[i].StatusState = (Denigen.Status)tempHeroes[i].statusState;
                denigenArray[i].GetComponent<Hero>().Exp = tempHeroes[i].exp;
                denigenArray[i].GetComponent<Hero>().LevelUpPts = tempHeroes[i].levelUpPts;
            }
        }
        //This code may be depricated or altered later
        /*for(int i = 0; i < denigenGOArray.Length; i++)
        {
            denigenArray.Add(denigenGOArray[i].GetComponent<Denigen>());
        }*/
        //create enemies -- sample code
        // to do - pull array of possible enemies for the area, and num of enemies
        for (int i = 0; i < GameControl.control.numOfEnemies; i++)
        {
            GameObject temp = (GameObject)Instantiate(GameControl.control.enemies[i].gameObject);
            denigenArray.Add(temp.GetComponent<Denigen>());
        }

        for (int i = 0; i < denigenArray.Count; i++)
        {
            //add the denigen to the list of heroes if it has a hero component
            if (denigenArray[i].GetComponent<Hero>() != null)
            {
                heroList.Add(denigenArray[i]);
            }
            else
            {
                enemyList.Add(denigenArray[i]);
            }
        }

        //sort the enemy denigens into position
        for (int i = enemyList.Count - 1; i > -1; i--)
        {
            enemyList[i].transform.position = enemyPositions[i];
        }

        //sort the hero denigens into position
        for (int i = heroList.Count - 1; i > -1; i--)
        {
            heroList[i].transform.position = heroPositions[i];
        }

        //sort denigens by speed
        SortDenigens();

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

            //b.Position = new Rect(Screen.width / 2 - b.width / 2, Screen.height / 2 + (i * (b.height + b.height / 2)), b.width, b.height);
            //buttonArray[i].transform.position = new Vector2((Screen.width / 2 - b.width / 2), (Screen.height / 2 + (i * (b.height + b.height / 2))));
            // if we had multiple columns
            //for (int j = 0; j < numOfCol; j++)
            //{
            //buttonArray[i] = GUILayout.Button(contentArray[j + (i * numOfCol)]); // ////print out buttons in one row based on number of columns

            //}
        }

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        //display the stats of the heroes
        for (int i = 0; i < heroList.Count; i++)
        {
            //Create a text prefab for now, we'll figure out the HUD later
            heroList[i].Card = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            heroList[i].Card.transform.position = new Vector2(-((250/heroList.Count) * (i*3) + 250), (camera.transform.position.y - 250));
            heroList[i].Card.GetComponent<TextMesh>().text = heroList[i].name + "\nLvl: " + heroList[i].Level + "\nHP: " + heroList[i].hp + " / " + heroList[i].hpMax
                + "\nPM: " + heroList[i].pm + " / " + heroList[i].pmMax;

        }

        //display the stats of the enemies
        for (int i = 0; i < enemyList.Count; i++)
        {
            //Create a text prefab for now, we'll figure out the HUD later
            enemyList[i].Card = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            enemyList[i].Card.transform.position = new Vector2(((250 / enemyList.Count) * (i * 3) + 250), (camera.transform.position.y - 250));
            enemyList[i].Card.GetComponent<TextMesh>().text = enemyList[i].name + "\nLvl: " + enemyList[i].Level + "\nHP: " + enemyList[i].hp + " / " + enemyList[i].hpMax
                + "\nPM: " + enemyList[i].pm + " / " + enemyList[i].pmMax;

        }

        //create cursors for the targeting of attacks
        cursors = new GameObject[enemyList.Count];
        for (int i = 0; i < enemyList.Count; i++)
        {
            cursors[i] = (GameObject)Instantiate(Resources.Load("Prefabs/cursorPrefab"));
            //Shut the cursors' renderers off until they are needed
            cursors[i].GetComponent<SpriteRenderer>().enabled = false;
        }
        
        //Create the text object for the battle phase
        battleText = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
        battleText.GetComponent<TextMesh>().fontSize = 250;
        battleText.GetComponent<TextMesh>().alignment = TextAlignment.Left;
        
        //highlight the current denigen
        // set the current denigen
        currentDenigen = denigenArray[currentDenigenIndex];
        currentDenigen.Card.GetComponent<TextMesh>().color = Color.yellow;
	}
    // change content array and other variables depending on the menu state
    void ChangeContentArray()
    {
        switch (state)
        {
            case MenuReader.main:
               contentArray = new List<string>{ "Attack", "Block", "Items", "Flee" };
               prevState = MenuReader.main; // default
               break;
            case MenuReader.attack:
               contentArray = new List<string> { "Strike", "Skills", "Spells", "Summons"};
               prevState = MenuReader.main;
               break;
            case MenuReader.skills:
               contentArray = currentDenigen.SkillsList;
               prevState = MenuReader.attack;
               break;
            case MenuReader.spells:
               contentArray = currentDenigen.SpellsList;
               prevState = MenuReader.attack;
               break;
            case MenuReader.summons:
               break;
            case MenuReader.items:
               break;
			default:
               break;
        }

    }

    // decide on what action to take/mode to change depending on the button pressed
    public override void ButtonAction(string label)
    {
        //base.ButtonAction(label);
        ////print("Inside BattleMenu ButtonAction");
        // perform action based on passed in string
        switch (label)
        {
                // main battle menu
            case "Strike":
                // select your target for the attack
                //currentDenigen.GetComponent<Hero>().SelectTarget(label);
                //Queue up the command to be executed later
                //commands.Add(label);
                //End of the turn for current denigen, pass to the next one
                //ChangeCurrentDenigen();

                //store the name of the command you wish to issue
                command = label;
                state = MenuReader.targeting;
                break;
            case "Block":
                //simply flip a boolean
                currentDenigen.IsBlocking = true;
                commands.Add(label);
                ChangeCurrentDenigen();
                break;
            case "Attack":
                // change state to attack menu
                state = MenuReader.attack;                           
                break;
            case "Items":
                // change state to items menu
                state = MenuReader.items;
                break;
            case "Flee":
                // nothing right now
                break;
            
                // attack menu
            case "Skills":
                state = MenuReader.skills;
                break;
            case "Spells":
                state = MenuReader.spells;
                break;
            case "Summons":
                state = MenuReader.summons;
                break;

                // back button
            case "<==":
                state = prevState;
                break;

                //if the case is none of the above, then the button selected must be an attack or an item
            default:
                if(state == MenuReader.spells || state == MenuReader.skills)
                {
                    //Passes the name of the attack to the denigen
                    //currentDenigen.GetComponent<Hero>().SelectTarget(label);
                    //Put this into the queue of commands
                    //commands.Add(label);
                    //Giving an attack command marks the end of current denigen's turn
                    //ChangeCurrentDenigen();
					command = label;
					state = MenuReader.targeting;
                }
                else if (state == MenuReader.items)
                {
                    //do the item related stuff here
                }
                break;
        }
        ChangeContentArray();
        StateChangeText();   
    }

    void ChangeCurrentDenigen()
    {
        currentDenigen.Card.GetComponent<TextMesh>().color = Color.white;
        currentDenigenIndex++;
        if (currentDenigenIndex >= denigenArray.Count)
        {
            //Begin the battle phase
            state = MenuReader.battle;
            battleText.GetComponent<Renderer>().enabled = true;
            foreach (GameObject b in buttonArray)
            {
                //hide the buttons
                b.GetComponent<Renderer>().enabled = false;
                b.GetComponent<MyButton>().textObject.GetComponent<Renderer>().enabled = false;
            }
            //reset the counter for right now
            currentDenigenIndex = 0;
            currentDenigen = denigenArray[currentDenigenIndex];
        }
        else
        {
            currentDenigen = denigenArray[currentDenigenIndex];
            currentDenigen.Card.GetComponent<TextMesh>().color = Color.yellow;
            if (currentDenigen.GetComponent<Hero>() != null)
            {
                //reset the menu state for the next denigen
                state = MenuReader.main;
            }
        }
    }

    void SortDenigens()
    {
        // temporary denigen for sorting
        Denigen temp;
        
        for (int j = 0; j < denigenArray.Count; j++)
        {
            for (int i = 0; i < denigenArray.Count - 1; i++)
            {
                if (denigenArray[i].spdBat < denigenArray[i + 1].spdBat)
                {
                    temp = denigenArray[i + 1];
                    denigenArray[i + 1] = denigenArray[i];
                    denigenArray[i] = temp;
                }
            }
        }

        //for (int i = 0; i < denigenArray.Count; i++)
        //{
        //    ////print(i + " denigen spd: " + denigenArray[i].spdBat);
        //}
    }

    string FormatText(string str)
    {
        string formattedString = null;
        int desiredLength = 30;
        string[] wordArray = str.Split(' ');
        int lineLength = 0;
        foreach (string s in wordArray)
        {
            //if the current line plus the length of the next word and SPACE is greater than the desired line length
            if (s.Length + 1 + lineLength > desiredLength)
            {
                //go to new line
                formattedString += "\n" + s;
                //starting a new line
                lineLength = s.Length;
            }
            else
            {
                formattedString += " " + s;
                lineLength += s.Length + 1;
            }
        }
        return formattedString;
    }

    void UpdateCard(Denigen d)
    {
        d.Card.GetComponent<TextMesh>().text = d.name + "\nLvl: " + d.Level + "\nHP: " + d.hp + " / " + d.hpMax
                + "\nPM: " + d.pm + " / " + d.pmMax;
    }

    void Update()
    {
        if (state == MenuReader.battle)
        {
            UpdateBattle();
        }
        //Targeting means that our WASD will not be navigating buttons
        else if (state == MenuReader.targeting && currentDenigen.StatusState != Denigen.Status.dead)
        {
            if (currentDenigen.GetComponent<Hero>() != null)
            {
                currentDenigen.GetComponent<Hero>().SelectTarget(command);
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    //Add the issued command to the queue
                    commands.Add(command);
                    //The current denigen's turn is over, move on
                    ChangeCurrentDenigen();
                    //Change the cards back to their default color
                    foreach (Denigen d in enemyList)
                    {
                        d.Card.GetComponent<TextMesh>().color = Color.white;
                    }
                    //Change state back to the default state
                    ChangeContentArray();
                    StateChangeText();
                }
            }
            else
            {
                //Add the enemy's chosen attack to the queue
                commands.Add(currentDenigen.GetComponent<Enemy>().ChooseAttack());
                //The current denigen's turn is over, move on
                ChangeCurrentDenigen();
            }
            
        }
        else if ((state == MenuReader.targeting || state == MenuReader.main) && currentDenigen.StatusState == Denigen.Status.dead) { 
            //////print(currentDenigen.name + " cannot attack");
            commands.Add(null);
            ChangeCurrentDenigen(); }
        else if (state == MenuReader.failure)
        {
            UpdateFailure();
        }
        else if (state == MenuReader.victory)
        {
            UpdateVictory();
        }
        else
        {
            // if it is an enemy's turn, go straight to targeting
            // This avoids the player gaining control of the enemy
            if (currentDenigen.GetComponent<Enemy>() != null) { state = MenuReader.targeting; } 

            foreach (Denigen d in denigenArray)
            {
                UpdateCard(d);
            }

            base.Update();

            // if back button is pressed, set state to previous state
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                state = prevState;
                ChangeContentArray();
                StateChangeText();
            }   

        }
        
       
    }

    void UpdateBattle()
    {
        //This is where we step through the issued commands and execute them one at a time
        //This should be done by calling the attack method for each denigen in order, and passing
        //the strings we previously recorded for them
        //at the end, we clear the commands list, reorder the denigens based on speed (incase there were stat changes)

		int fallenHeroes = 0;
		int fallenEnemies = 0;
       
       
        //press space to advance the battle phase
		if (commandIndex < commands.Count && (Input.GetKeyUp(KeyCode.Space) || (commandIndex == 0 && textIndex == 0))/* && !(commandIndex >= commands.Count)*/)
        {
            //this while loop bypasses any denigens who have passed away
            while (commandIndex < denigenArray.Count && denigenArray[commandIndex].StatusState == Denigen.Status.dead)
            {
                //print(denigenArray[commandIndex].name + " is dead up top");
                
                battleTextList = new List<string>() { };


				commandIndex++;
				// if the command index is at the end, there are no more live denigens in the list
				// so exit out of UpdateBattle
				if (commandIndex >= denigenArray.Count) {
					return;
				}
            }
            //if the turn hasn't already been calculated, and the denigen is not dead or blocking
			if (denigenArray[commandIndex].StatusState != Denigen.Status.dead)
            {
                if (battleTextList.Count == 0)
                {
                    if (denigenArray[commandIndex].GetComponent<Hero>() != null)
                    {
                        denigenArray[commandIndex].GetComponent<Hero>().Attack(commands[commandIndex]);
                    }
                    else
                    {
                        denigenArray[commandIndex].GetComponent<Enemy>().Attack(commands[commandIndex]);
                    }

                    //record the appropriate text to display
                    for (int i = 0; i < denigenArray[commandIndex].CalcDamageText.Count; i++)
                    {
                        battleTextList.Add(denigenArray[commandIndex].CalcDamageText[i]);
                    }
                    foreach (Denigen d in denigenArray[commandIndex].Targets)
                    {
                        //this causes an error when the target is dead. hopefully ending the battle when all heroes are dead will avoid this
						//if (d.TakeDamageText != null)
                        for (int i = 0; i < d.TakeDamageText.Count; i ++ )
                        {
                            battleTextList.Add(d.TakeDamageText[i]);
                        }
                    }
                }

                //////print(denigenArray[commandIndex]);
                //make sure there is text to display
                if (battleTextList[textIndex] != null)
                {
                    battleText.GetComponent<TextMesh>().text = FormatText(battleTextList[textIndex]);
					//print (battleText.GetComponent<TextMesh> ().text);
                }
                else { textIndex++; }

                if (battleText.GetComponent<TextMesh>().text.Contains("damage!")) //this hopefully only updates the HUD when damage is dealt
                {
					// check if heroes/enemies are dead
					// checks after attack to break out as soon as all enemies are dead
					fallenHeroes = 0;
					fallenEnemies = 0;

                    foreach (Denigen d in denigenArray)
                    {
                        UpdateCard(d);
						if (d is Hero && d.StatusState == Denigen.Status.dead) {
							fallenHeroes++;
						} else if ( d is Enemy && d.StatusState == Denigen.Status.dead) {
							fallenEnemies++;
						}

						// set to appropriate state
						if (fallenHeroes >= heroList.Count && textIndex == battleTextList.Count - 2) {
							textIndex = 0;
							state = MenuReader.failure;
							return;
							//PostBattle ();
                        } if (fallenEnemies >= enemyList.Count && textIndex == battleTextList.Count - 2)
                        {
							textIndex = 0;
							state = MenuReader.victory;
							return;
							//PostBattle ();
						}
                    }
				}


            }
            if (textIndex < (battleTextList.Count - 1))
            {
                textIndex++;
				////print ("inside if");
            }
            else
            {
				//this while loop bypasses any denigens who have passed away
				while (commandIndex < denigenArray.Count - 1 && denigenArray[commandIndex + 1].StatusState == Denigen.Status.dead)
				{
					//print(denigenArray[commandIndex + 1].name + " is dead");

					battleTextList = new List<string>() { };


					commandIndex++;
					// if the command index is at the end, there are no more live denigens in the list
					// so exit out of UpdateBattle
					if (commandIndex >= denigenArray.Count) {
						return;
					}
				}

                textIndex = 0;
                ////print("Move on to the next denigen");
                commandIndex++;
                battleTextList = new List<string>() { };

            }
        }

		else if (Input.GetKeyUp(KeyCode.Space) && commandIndex >= commands.Count)
		{			
			textIndex = 0;
			//check if all heroes have fallen
			fallenHeroes = 0;
			foreach (Denigen d in heroList)
			{
				if (d.StatusState == Denigen.Status.dead) { fallenHeroes++; }
			}
			if (heroList.Count == fallenHeroes)
			{
				//go to a gameover state
				//PostBattle ();
				textIndex = 0;
				state = MenuReader.failure;
				////print("All heroes have fallen");

				return;
			}

			//check if all enemies have fallen
			fallenEnemies = 0;
			foreach (Denigen d in enemyList)
			{
				if (d.StatusState == Denigen.Status.dead) { fallenEnemies++; }
			}
			if (enemyList.Count == fallenEnemies)
			{
				//go to a victory state
				textIndex = 0;
				//PostBattle ();
				state = MenuReader.victory;

				////print("All enemies have fallen");
				return;
			}


			PostBattle ();
		}
    }

	void PostBattle(){
		commandIndex = 0;
		commands.Clear();
		SortDenigens();
		currentDenigen = denigenArray[0];

		battleText.GetComponent<TextMesh>().text = null;
		battleText.GetComponent<Renderer>().enabled = false;
		battleTextList.Clear();
		//textIndex = 0;
		foreach (Denigen d in denigenArray)
		{
            d.CalcDamageText.Clear();
            d.TakeDamageText.Clear();
            d.IsBlocking = false;
		}
		foreach (GameObject b in buttonArray)
		{
			//show the buttons
			b.GetComponent<Renderer>().enabled = true;
			b.GetComponent<MyButton>().textObject.GetComponent<Renderer>().enabled = true;
		}
		state = MenuReader.main;
		ChangeContentArray ();
		StateChangeText ();
	}
    void UpdateFailure()
    {
		if (Input.GetKeyUp(KeyCode.Space))
        {
            if (textIndex == 0)
            { 
                //players lose 1/10th of their money for falling in battle
                loss = (int)(GameControl.control.totalGold * 0.1f); 
                battleText.GetComponent<TextMesh>().text = GameControl.control.playerName + "'s entire team has fallen!";
            }
            if (textIndex == 1) 
            { 
                battleText.GetComponent<TextMesh>().text = "The team loses " + loss + " gold!";
            }
            if (textIndex == 2) { 
                //take the player's gold
                GameControl.control.totalGold -= loss;
                //move the player back to where they last saved, restoring health and pm
                foreach (HeroData h in GameControl.control.heroList)
                {
                    h.hp = h.hpMax;
                    h.pm = h.pmMax;
                    h.statusState = HeroData.Status.normal;
                }
                // Go to the last saved location of the dungeon
                if (GameControl.control.taggedStatue)
                {
                    GameControl.control.currentPosition = GameControl.control.savedStatue;
                }
                else // if no statue is tagged, go to the entrance
                {
                    GameControl.control.currentPosition = GameControl.control.areaEntrance;
                }
                // load the scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene);
            }
            if (textIndex < 2)
            {
                battleText.GetComponent<TextMesh>().text = FormatText(battleText.GetComponent<TextMesh>().text);
				//print (battleText.GetComponent<TextMesh> ().text);
            }
            textIndex++;
        }
    }

    void UpdateVictory()
    {
		if (Input.GetKeyUp(KeyCode.Space))
        {
            if (textIndex == 0)
            {
                //the gold to award to the player
                gold = 0;
                exp = 0;
                foreach (Enemy e in enemyList) { gold += e.Gold; exp += e.Exp; }
                battleText.GetComponent<TextMesh>().text = "All of the enemies have fallen!";
            }
            if (textIndex == 1)
            {
                battleText.GetComponent<TextMesh>().text = GameControl.control.playerName + "'s team gains " + gold + " gold!";
                GameControl.control.totalGold += gold; // add to the player's total gold
            }
            if (textIndex == 2)
            {
                battleText.GetComponent<TextMesh>().text = "Each teammate receives " + exp + " experience points!";
                foreach (Hero h in heroList) { if (h.StatusState != Denigen.Status.dead) { h.Exp += exp; } }
            }
            if (textIndex == 3)
            {
                // set all of the heroData stats equal to in battle stats
                foreach (Hero h in heroList)
                {
                    foreach (HeroData hd in GameControl.control.heroList)
                    {
                        if (h.name == hd.name)
                        {
                            hd.level = h.Level;
                            hd.levelUpPts = h.LevelUpPts;
                            hd.exp = h.Exp;
                            hd.hp = h.hp;
                            hd.hpMax = h.hpMax;
                            hd.pm = h.pm;
                            hd.pmMax = h.pmMax;
                            hd.atk = h.Atk;
                            hd.def = h.Def;
                            hd.mgkAtk = h.MgkAtk;
                            hd.mgkDef = h.MgkDef;
                            hd.evasion = h.Evasion;
                            hd.luck = h.Luck;
                            hd.spd = h.Spd;
                            hd.statusState = (HeroData.Status)h.StatusState;
                        }
                    }
                }
                // exit the battle
                UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene);
            }
            if (textIndex < 3)
            {
                battleText.GetComponent<TextMesh>().text = FormatText(battleText.GetComponent<TextMesh>().text);
				//print (battleText.GetComponent<TextMesh> ().text);
            }
            textIndex++;
        }

    }

	void UpdateMain () {

        // check for selected button
        /*if (buttonArray[0])
        {
            //////print("Strike");
        }
        else if (buttonArray[1])
        {
            
        }
        else if (buttonArray[2])
        {
            //////print("ITEMS!!!!@#rJ123IO4J2IO3RIAOSFDJ");
        }
        else if (buttonArray[3])
        {
            //////print("Flee");
        }*/
	}
    void Updateattack()
    {
        /*if (buttonArray[0])
        {
            state = MenuReader.skills;
        }
        else if (buttonArray[1])
        {
            state = MenuReader.spells;
        }
        else if (buttonArray[2])
        {
            state = MenuReader.summons;
        }
        else if (buttonArray[3])
        {
            contentArray = new string[] { "Strike", "attack", "Items", "Flee" };
            buttonArray = new GameObject[numOfRow];
            numOfRow = contentArray.Length;
            state = MenuReader.main;
        }*/
    }
    void UpdateSkills() 
    {
        // here will be an array of the hero's skill set
        // that will be listed as such
        
        // ^
        // skill
        // skill
        // skill
        // skill
        // v

        // side note:
        // every possible technique name and method will be held in an attack method
        // within the specific Hero class.
        // When an attack is selected through the menu, it will be passed back to the specific hero which
        // will then search through it's list of attacks and perform the attack.
    }
    void UpdateSpells() 
    {
        // here will be an array of the hero's spells
        // that will be listed as such

        // ^
        // spell
        // spell
        // spell
        // spell
        // v
    }
    void UpdateSummons() 
    {
        // any possible character specific Guardian summons can be selected here
    }
    void UpdateItems() 
    {
        // here will be an array of the team's items
        // that will be listed as such

        // ^
        // item
        // item
        // item
        // item
        // v
    }
    void UpdateFlee() 
    {
        // here is where the player will attempt to flee from the battle
    }
   
}
