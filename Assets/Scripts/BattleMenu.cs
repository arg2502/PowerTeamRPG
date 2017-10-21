using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleMenu : Menu {

    //numbers for the end of the battle
    int loss; // the amount of gold the player will lose for failing
    int gold; // the amount of gold awarded for victory
    int exp; // the amount of experience awarded for victory

    bool failedFlee; // this will give the enemy a free turn if you fail to flee
    bool levelUp; // handles whether to transition to the level up scene or the overworld

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
    public List<Vector2> enemyPositions = new List<Vector2>() {new Vector2(7.5f, -1.4f), new Vector2(11.0f, 0.5f),
                                    new Vector2(5.65f, 1.625f), new Vector2(9f, 3.6f), new Vector2(4.1f, 4.5f)};
    public List<Vector2> heroPositions = new List<Vector2>() {new Vector2(-7.5f, -1.4f), new Vector2(-11.0f, 0.5f),
                                    new Vector2(-5.65f, 1.625f), new Vector2(-9f, 3.6f), new Vector2(-4.1f, 4.5f)};

    //The HUD to display stats, for now just text prefabs
    public List<GameObject> heroCards = new List<GameObject>() { };
    public List<GameObject> enemyCards = new List<GameObject>() { };

    // The names of attacks to be executed during the battle phase
    // their index in this list will correspond to the denigens' index in the denigen array
    public List<string> commands = new List<string>() { };
    string command;

    //Cursors for targeting attacks
    //public GameObject[] cursors;

    //Int for tracking which denigen should act in a battle phase
    public int commandIndex = 0;

    //Holds what is written on the screen during battle
    public GameObject battleText;
    //holds a queue of the text for the battleText obj
    public List<string> battleTextList = new List<string>() { };
    int textIndex;

    // A textObject that will show the decriptions of attacks
    //public GameObject descriptionText;
    // an object to contain the descriptions
    public GameObject descriptionBox;

    // A textObject that will say whose turn it is
    public GameObject menuLabel;

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
                switch (tempHeroes[i].identity)
                {
                    case 0:
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/JethroPrefab"));
                        temp.name = "Jethro";
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    case 1:
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/ColePrefab"));
                        temp.name = "Cole";
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    case 2:
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/EleanorPrefab"));
                        temp.name = "Eleanor";
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    case 3:
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/JuliettePrefab"));
                        temp.name = "Juliette";
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                    //case 4:
                    //    temp = (GameObject)Instantiate(Resources.Load("Prefabs/SelenePrefab"));
                    //    denigenArray.Add(temp.GetComponent<Denigen>());
                    //    break;
                    default:
                        temp = (GameObject)Instantiate(Resources.Load("Prefabs/JethroPrefab"));
                        temp.name = "Jethro";
                        denigenArray.Add(temp.GetComponent<Denigen>());
                        break;
                }
                //bring in all of the stats
                denigenArray[i].GetComponent<Hero>().identity = tempHeroes[i].identity;
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
                denigenArray[i].GetComponent<Hero>().ExpToLevelUp = tempHeroes[i].expToLvlUp;
                denigenArray[i].GetComponent<Hero>().SkillsList = tempHeroes[i].skillsList;
                denigenArray[i].GetComponent<Hero>().SpellsList = tempHeroes[i].spellsList;
                denigenArray[i].GetComponent<Hero>().TechPts = tempHeroes[i].techPts;
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
            temp.name = "Enemy" + i.ToString();
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
            buttonArray[i].name = "BattleMenuButton" + i.ToString();
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(transform.position.x, transform.position.y + (b.height*2) + (i * -(b.height + b.height / 2)));

            // assign text
			b.textObject = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            b.textObject.name = "BattleMenuText" + i.ToString();
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);

        }

        // Create the text object for the top of the menu which says whose turn it is
        menuLabel = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
        menuLabel.name = "MenuLabel";
        menuLabel.GetComponent<TextMesh>().color = Color.black;
        menuLabel.transform.position = buttonArray[0].transform.position + new Vector3(0.0f, 0.8f, 0.0f);

        // Create the description box and its text
        descriptionBox = (GameObject)Instantiate(Resources.Load("Prefabs/DescriptionBoxPrefab"));
        descriptionBox.name = "DescriptionBox";
        descriptionBox.transform.position = transform.position + new Vector3(0.0f, -3.0f, 0.0f);
        descriptionText = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
        descriptionText.name = "DescriptionText";
        descriptionText.GetComponent<TextMesh>().color = Color.black;
        descriptionText.transform.position = descriptionBox.transform.position;
        GetComponent<SpriteRenderer>().sortingOrder = 9800;

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;

        //display the stats of the heroes
        for (int i = 0; i < heroList.Count; i++)
        {
            //Create a text prefab for now, we'll figure out the HUD later
            heroList[i].Card = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            heroList[i].Card.name = "HeroCard" + i.ToString();
            //heroList[i].Card.GetComponent<TextMesh>().fontSize = 40;
            if (heroList.Count == 1) { heroList[i].Card.transform.position = new Vector2(-(camera.transform.position.x + 15f), (0)); }
            else { heroList[i].Card.transform.position = new Vector2(-(camera.transform.position.y + 15f), ((3.5f / heroList.Count) * (i * 3) - 3.5f)); }
            heroList[i].Card.GetComponent<TextMesh>().text = heroList[i].name + "\nLvl: " + heroList[i].Level + "\nHP: " + heroList[i].hp + " / " + heroList[i].hpMax
                + "\nPM: " + heroList[i].pm + " / " + heroList[i].pmMax;

        }

        //display the stats of the enemies
        for (int i = 0; i < enemyList.Count; i++)
        {
            //Create a text prefab for now, we'll figure out the HUD later
            enemyList[i].Card = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
            enemyList[i].name = "EnemyCard" + i.ToString();
            //enemyList[i].Card.GetComponent<TextMesh>().fontSize = 40;
            if (enemyList.Count == 1) { enemyList[i].Card.transform.position = new Vector2((camera.transform.position.x + 15f), ( 0 )); }
            else { enemyList[i].Card.transform.position = new Vector2((camera.transform.position.x + 15f), ((3.5f / enemyList.Count) * (i * 3) - 3.5f)); }
            enemyList[i].Card.GetComponent<TextMesh>().text = enemyList[i].name + "\nLvl: " + enemyList[i].Level + "\nHP: " + enemyList[i].hp + " / " + enemyList[i].hpMax
                + "\nPM: " + enemyList[i].pm + " / " + enemyList[i].pmMax;

        }

        //create cursors for the targeting of attacks
        /*cursors = new GameObject[enemyList.Count];
        for (int i = 0; i < enemyList.Count; i++)
        {
            cursors[i] = (GameObject)Instantiate(Resources.Load("Prefabs/cursorPrefab"));
            //Shut the cursors' renderers off until they are needed
            cursors[i].GetComponent<SpriteRenderer>().enabled = false;
        }*/
        
        //Create the text object for the battle phase
        battleText = (GameObject)Instantiate(Resources.Load("Prefabs/CenterTextPrefab"));
        battleText.name = "BattleText";
        //battleText.GetComponent<TextMesh>().fontSize = 60;
        battleText.GetComponent<TextMesh>().alignment = TextAlignment.Left;
        battleText.transform.position = transform.position + new Vector3(0.0f, -3.0f, 0.0f);
        battleText.GetComponent<TextMesh>().color = Color.black;
        
        
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
               contentArray = new List<string> { };
               for (int i = 0; i < currentDenigen.SkillsList.Count; i++)
               {
                   contentArray.Add(currentDenigen.SkillsList[i].Name);
               }
               prevState = MenuReader.attack;
               break;
            case MenuReader.spells:
               contentArray = new List<string> { };
               for (int i = 0; i < currentDenigen.SpellsList.Count; i++)
               {
                   contentArray.Add(currentDenigen.SpellsList[i].Name);
               }
               prevState = MenuReader.attack;
               break;
            case MenuReader.summons:
               break;
            case MenuReader.items:
               contentArray = new List<string> { };
               for (int i = 0; i < GameControl.control.consumables.Count; i++ )
               {
                   contentArray.Add(GameControl.control.consumables[i].GetComponent<ConsumableItem>().name);
               }
               prevState = MenuReader.main;
               break;
			default:
               break;
        }

    }

    // decide on what action to take/mode to change depending on the button pressed
    public override void ButtonAction(string label)
    {
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
                prevState = state;
                DisableMenu();
                GetComponent<Renderer>().enabled = false;
                descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
                state = MenuReader.targeting;
                break;
            case "Block":
                //simply flip a boolean
                //currentDenigen.IsBlocking = true;
                //commands.Add(label);
                //ChangeCurrentDenigen();
                command = label;
                prevState = state;
                DisableMenu();
                GetComponent<Renderer>().enabled = false;
                descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
                state = MenuReader.targeting;
                break;
            case "Attack":
                // change state to attack menu
                prevState = state;
                state = MenuReader.attack;                           
                break;
            case "Items":
                // change state to items menu
                prevState = state;
                state = MenuReader.items;
                break;
            case "Flee":
                prevState = state;
                state = MenuReader.flee;
                CalcFlee();
                break;
            
                // attack menu
            case "Skills":
                prevState = state;
                state = MenuReader.skills;
                break;
            case "Spells":
                prevState = state;
                state = MenuReader.spells;
                break;
            case "Summons":
                prevState = state;
                state = MenuReader.summons;
                break;

                // back button
            case "<==":
                state = prevState;
                break;

                //if the case is none of the above, then the button selected must be an attack or an item
            default:
                if (state == MenuReader.spells || state == MenuReader.skills || state == MenuReader.items)
                {
                    //Passes the name of the attack to the denigen
                    //currentDenigen.GetComponent<Hero>().SelectTarget(label);
                    //Put this into the queue of commands
                    //commands.Add(label);
                    //Giving an attack command marks the end of current denigen's turn
                    //ChangeCurrentDenigen();
					command = label;
                    prevState = state;
                    DisableMenu();
                    GetComponent<Renderer>().enabled = false;
                    descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;
					state = MenuReader.targeting;
                }
                //else if (state == MenuReader.items)
                //{
                //    //do the item related stuff here
                //    command = label;
                //    prevState = state;
                //    DisableMenu();
                //    state = MenuReader.targeting;
                //}
                break;
        }
        ChangeContentArray();
		StateChangeText();   
		UpdateDescription ();
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
            GetComponent<Renderer>().enabled = false;
            descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;

            //reset the counter for right now
            currentDenigenIndex = 0;
            currentDenigen = denigenArray[currentDenigenIndex];
        }
        else
        {
            currentDenigen = denigenArray[currentDenigenIndex];
            if (currentDenigen is Hero
                && (currentDenigen.statusState != Denigen.Status.dead
                && currentDenigen.statusState != Denigen.Status.overkill))
            {               
                if (currentDenigen.GetComponent<Hero>() != null)
                {
                    //reset the menu state for the next denigen
                    state = MenuReader.main;
                    ChangeContentArray();
					StateChangeText();
					UpdateDescription ();
                }
				EnableMenu();
				GetComponent<Renderer>().enabled = true;
				descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
				currentDenigen.Card.GetComponent<TextMesh>().color = Color.yellow;
            }
        }
    }

    public void SortDenigens()
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
    }

    string FormatText(string str)
    {
        string formattedString = null;
        int desiredLength = 50;
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

    void UpdateCard(Denigen d)
    {
        d.Card.GetComponent<TextMesh>().text = d.name + "\nLvl: " + d.Level + "\nHP: " + d.hp + " / " + d.hpMax
                + "\nPM: " + d.pm + " / " + d.pmMax;
        
        // display nothing if denigen is dead
        if(d.statusState == Denigen.Status.dead || d.statusState == Denigen.Status.overkill)
        {
            d.Card.GetComponent<TextMesh>().text = "";
        }
    }
    void CheckForInactive()
    {
        // search through all the buttons for their text
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactive &&
                buttonArray[i].GetComponent<MyButton>().state != MyButton.MyButtonTextureState.inactiveHover)
            {
                if (state == MenuReader.main)
                {
                    if (buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text == "Items"
                        && GameControl.control.consumables.Count <= 0)
                    {
                        buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
                    }
                }
                // inside the attack menu
                else if (state == MenuReader.attack)
                {
                    // disable skills/spells if that denigen's list are empty                    
                    if ((buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text == "Skills"
                        && currentDenigen.SkillsList.Count <= 0)
                        || (buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text == "Spells"
                        && currentDenigen.SpellsList.Count <= 0))
                    {
                        buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive;
                    }

                }
                else if (state == MenuReader.skills)
                {
                    // search through all the skills
                    for (int j = 0; j < currentDenigen.SkillsList.Count; j++)
                    {
                        // compare the text of the button with the skill name
                        // if the hero's pm is not enough to use the skill, deactivate the button
                        if (buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text == currentDenigen.SkillsList[j].Name
                            && currentDenigen.pm < currentDenigen.SkillsList[j].Pm)
                        {
                            if (i == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover; }
                            else { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                        }
                    }
                }
                else if (state == MenuReader.spells)
                {
                    // search through all the spells
                    for (int j = 0; j < currentDenigen.SpellsList.Count; j++)
                    {
                        // compare the text of the button with the spell name
                        // if the hero's pm is not enough to use the spell, deactivate the button
                        if (buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text == currentDenigen.SpellsList[j].Name
                            && currentDenigen.pm < currentDenigen.SpellsList[j].Pm)
                        {
                            if (i == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover; }
                            else { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                        }
                    }
                }

                else if (state == MenuReader.items)
                {
                    //search through all of the items
                    for (int j = 0; j < GameControl.control.consumables.Count; j++)
                    {
                        // Disable an item if the number of denigens commanded to use said item is >= its quantity
                        if (buttonArray[i].GetComponent<MyButton>().textObject.GetComponent<TextMesh>().text == GameControl.control.consumables[j].GetComponent<ConsumableItem>().name
                            && GameControl.control.consumables[j].GetComponent<ConsumableItem>().uses >= GameControl.control.consumables[j].GetComponent<ConsumableItem>().quantity)
                        {
                            if (i == 0) { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactiveHover; }
                            else { buttonArray[i].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.inactive; }
                        }
                    }
                }
            }
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale -= 0.1f;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale += 0.1f;
        }


        // don't update if a denigen is performing a battle animation
        if (GameControl.control.isAnimating || GameControl.control.isDying)
            return;

        // check for inactive buttons
        CheckForInactive();

        // make the menu label object reflect the current denigen
        if ((state != MenuReader.battle && state != MenuReader.failure && state != MenuReader.flee && state != MenuReader.targeting && state != MenuReader.victory) && currentDenigen.GetComponent<Hero>() != null)
        {
            menuLabel.GetComponent<TextMesh>().text = currentDenigen.name + "'s turn";
        }
        else
        {
            menuLabel.GetComponent<TextMesh>().text = "";
        }
        

        if (state == MenuReader.flee)
        {
            UpdateFlee();
        }
        else if (state == MenuReader.battle)
        {
            UpdateBattle();
        }
        //Targeting means that our WASD will not be navigating buttons
        else if (state == MenuReader.targeting && currentDenigen.StatusState != Denigen.Status.dead)
        {
            if (currentDenigen.GetComponent<Hero>() != null)
            {
                if (!failedFlee)
                {
                    currentDenigen.GetComponent<Hero>().SelectTarget(command);
					if (Input.GetKeyUp(GameControl.control.selectKey))
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
						UpdateDescription();
                    }
                    // if back button is pressed, set state to previous state
					if (Input.GetKeyDown(GameControl.control.backKey))
                    {
						state = prevState;
						ChangeContentArray();
						StateChangeText();
						UpdateDescription ();
                        EnableMenu();
                        //descriptionBox.GetComponent<Renderer>().enabled = true;
                        GetComponent<Renderer>().enabled = true;
                        descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
                        
                        // make sure all of the enemies are back to their normal color
                        for (int i = 0; i < enemyList.Count; i++)
                        {
                            if (enemyList[i].statusState != Denigen.Status.dead && enemyList[i].statusState != Denigen.Status.overkill)
                            {
                                enemyList[i].Sr.material.shader = enemyList[i].normalShader;
                                enemyList[i].Sr.color = Color.white;
                            }
                        }
                        // make sure all of the heroes turn back as well
                        for (int i = 0; i < heroList.Count; i++)
                        {
                            if (heroList[i].statusState != Denigen.Status.dead && heroList[i].statusState != Denigen.Status.overkill)
                            {
                                heroList[i].Sr.material.shader = heroList[i].normalShader;
                                heroList[i].Sr.color = Color.white;
                            }
                        }
                        //set the target index back to 0
                        currentDenigen.GetComponent<Hero>().TargetIndex = 0;
                    }   
                }
                else
                { 
                    //Add the issued command to the queue
                    commands.Add(null);
                    //The current denigen's turn is over, move on
                    ChangeCurrentDenigen();
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
            else if (failedFlee) { state = MenuReader.targeting; }

            foreach (Denigen d in denigenArray)
            {
                UpdateCard(d);
            }

            base.Update();
			PressButton(GameControl.control.selectKey);

            //update the description text
			UpdateDescription();

            // if back button is pressed, set state to previous state
			if (Input.GetKeyDown(GameControl.control.backKey))
            {
                
                state = prevState;
                ChangeContentArray();
                StateChangeText();
				UpdateDescription ();
				EnableMenu();
				GetComponent<Renderer>().enabled = true;
				descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;
            }   

        }

        // diable cursors if the player is not targeting
        if(state != MenuReader.targeting)
        {
            for(int i = 0; i < enemyList.Count; i++)
            {
                //cursors[i].GetComponent<SpriteRenderer>().enabled = false;
                enemyList[i].Card.GetComponent<TextMesh>().color = Color.white;
                //enemyList[i].Sr.color = Color.white;
            }
            for (int i = 0; i < heroList.Count; i++)
            {
                heroList[i].Card.GetComponent<TextMesh>().color = Color.white;
            }
        }


        // check for button press
        //PressButton(GameControl.control.selectKey);
    }
	void UpdateDescription()
	{
		if (state == MenuReader.main) { descriptionText.GetComponent<TextMesh>().text = descriptionBox.GetComponent<DescriptionText>().mainDesc[selectedIndex]; }
		else if (state == MenuReader.attack) { descriptionText.GetComponent<TextMesh>().text = descriptionBox.GetComponent<DescriptionText>().attackDesc[selectedIndex]; }
		else if (state == MenuReader.skills) { descriptionText.GetComponent<TextMesh>().text = currentDenigen.SkillsList[selectedIndex + scrollIndex].Description; }
		else if (state == MenuReader.spells) { descriptionText.GetComponent<TextMesh>().text = currentDenigen.SpellsList[selectedIndex + scrollIndex].Description; }
		else if (state == MenuReader.items) { descriptionText.GetComponent<TextMesh>().text = GameControl.control.consumables[selectedIndex + scrollIndex].GetComponent<Item>().description; }

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
		if (commandIndex < commands.Count 
            && (Input.GetKeyUp(GameControl.control.selectKey) 
            || (commandIndex == 0 && textIndex == 0)))
        {
            while (commandIndex < denigenArray.Count && (failedFlee && denigenArray[commandIndex].GetComponent<Hero>() != null))
            {
                battleTextList = new List<string>() { };

                commandIndex++;
                // if the command index is at the end, there are no more live denigens in the list
                // so exit out of UpdateBattle
                if (commandIndex >= denigenArray.Count)
                {
                    return;
                }
            }
            //this while loop bypasses any denigens who have passed away
            while (commandIndex < denigenArray.Count && denigenArray[commandIndex].StatusState == Denigen.Status.dead)
            {
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
                }

                //make sure there is text to display
                if ( battleTextList[textIndex] != null)
                {
                    battleText.GetComponent<TextMesh>().text = FormatText(battleTextList[textIndex]);
                }
                else { textIndex++; }
                
				fallenHeroes = 0;
				fallenEnemies = 0;
                if (CheckForDead()) { return; }

            }
            if (textIndex < (battleTextList.Count - 1))
            {
                textIndex++;
            }
            else
            {
				//this while loop bypasses any denigens who have passed away
				while (commandIndex < denigenArray.Count - 1 && denigenArray[commandIndex + 1].StatusState == Denigen.Status.dead)
				{
					battleTextList = new List<string>() { };
					commandIndex++;
					// if the command index is at the end, there are no more live denigens in the list
					// so exit out of UpdateBattle
					if (commandIndex >= denigenArray.Count) {
						return;
					}
				}
                textIndex = 0;
                if (CheckForDead()) { return; }
                commandIndex++;
                battleTextList = new List<string>() { };
            }
        }

		else if (Input.GetKeyUp(GameControl.control.selectKey) && commandIndex >= commands.Count)
		{
			textIndex = 0;
			//check if all heroes have fallen
            //check if all enemies have fallen
            if (CheckForDead()) { return; }
			PostBattle ();
		}

        // check if heroes/enemies are dead
        // checks after attack to break out as soon as all enemies are dead
        if (CheckForDead()) { return;}
    }

    bool CheckForDead()
    {
        int fallenHeroes = 0;
        int fallenEnemies = 0;

        foreach (Denigen d in denigenArray)
        {
            UpdateCard(d);
            if (d is Hero && d.StatusState == Denigen.Status.dead)
            {
                fallenHeroes++;
            }
            else if (d is Enemy && d.StatusState == Denigen.Status.dead)
            {
                fallenEnemies++;
            }

            // set to appropriate state
            if (fallenHeroes >= heroList.Count && textIndex == battleTextList.Count - 1)
            {
                textIndex = 0;
                state = MenuReader.failure;
                return true;
            } if (fallenEnemies >= enemyList.Count && textIndex == battleTextList.Count - 1)
            {
                textIndex = 0;
                state = MenuReader.victory;
                return true;
            }
        }
        return false;
    }
	void PostBattle(){

		commandIndex = 0;
		commands.Clear();
		SortDenigens();
		currentDenigen = denigenArray[0];

		battleText.GetComponent<TextMesh>().text = null;
		battleText.GetComponent<Renderer>().enabled = false;
		battleTextList.Clear();

		foreach (Denigen d in denigenArray)
		{
            // Passive check here for now... maybe change it later?
            if (d.statusState != Denigen.Status.dead)
            {
                foreach (Passive ptp in d.PassivesList)
                {
                    if (ptp is PerTurnPassive) { ptp.Use(d, null); }
                }
            }
            d.CalcDamageText.Clear();
            d.TakeDamageText.Clear();
            d.IsBlocking = false;
		}

		// change the state and text before showing the buttons and text again
		state = MenuReader.main;
		ChangeContentArray ();
		StateChangeText ();
		UpdateDescription ();

		foreach (GameObject b in buttonArray)
		{
			//show the buttons
			b.GetComponent<Renderer>().enabled = true;
			b.GetComponent<MyButton>().textObject.GetComponent<Renderer>().enabled = true;
        }

		// bring back background and description text
		GetComponent<Renderer>().enabled = true;
		descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = true;

        foreach (GameObject i in GameControl.control.consumables)
        {
            i.GetComponent<ConsumableItem>().uses = 0;
        }

        if (failedFlee) { failedFlee = false; }

	}
    void UpdateFailure()
    {
		if (Input.GetKeyUp(GameControl.control.selectKey))
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
                // reset the "uses" variable for any items that didn't get used by the battle's end
                foreach (GameObject i in GameControl.control.consumables) { i.GetComponent<ConsumableItem>().uses = 0; }

                GameControl.control.currentCharacterState = characterControl.CharacterState.Defeat;
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
            }
            textIndex++;
        }
    }

    void UpdateVictory()
    {
		if (Input.GetKeyUp(GameControl.control.selectKey))
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
                foreach (Hero h in heroList) {
                    if (h.StatusState != Denigen.Status.dead) { 
                        h.Exp += exp;
                        h.ExpToLevelUp -= exp; 
                    } 

                    //Level up the hero, if necessary
                    
                    if (h.ExpToLevelUp <= 0)
                    {
                        // keep the rollover experience
                        int extraExp = Mathf.Abs(h.ExpToLevelUp);
                        h.LevelUp(extraExp);
                        UpdateCard(h);
                        h.statBoost = true;
                        h.skillTree = true;
                        levelUp = true; // tells the game to go to the level up scene
                    }
                }
            }
            if (textIndex == 3)
            {
                // set all of the heroData stats equal to in battle stats
                foreach (Hero h in heroList)
                {
                    foreach (HeroData hd in GameControl.control.heroList)
                    {
                        if (h.identity == hd.identity)
                        {
                            hd.level = h.Level;
                            hd.levelUpPts = h.LevelUpPts;
                            hd.techPts = h.TechPts;
                            hd.exp = h.Exp;
                            hd.expToLvlUp = h.ExpToLevelUp;
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
                            hd.statBoost = h.statBoost;
                            hd.skillTree = h.skillTree;
                            hd.skillsList = h.SkillsList;
                            hd.spellsList = h.SpellsList;
                            hd.passiveList = h.PassivesList;
                        }
                    }
                }
                // reset the "uses" variable for any items that didn't get used by the battle's end
                foreach (GameObject i in GameControl.control.consumables) { i.GetComponent<ConsumableItem>().uses = 0; }
                // exit the battle
                // if a hero leveled up, go to level up screen
                if (levelUp == true) { UnityEngine.SceneManagement.SceneManager.LoadScene("LevelUpMenu"); }
                // otherwise, just go to current room
                else { UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene); }
            }
            if (textIndex < 3)
            {
                battleText.GetComponent<TextMesh>().text = FormatText(battleText.GetComponent<TextMesh>().text);
            }
            textIndex++;
        }

    }

    // here is where the player will attempt to flee from the battle
    void UpdateFlee() 
    {
        battleText.GetComponent<Renderer>().enabled = true;

        foreach (GameObject b in buttonArray)
        {
            //hide the buttons
            b.GetComponent<Renderer>().enabled = false;
            b.GetComponent<MyButton>().textObject.GetComponent<Renderer>().enabled = false;
        }
        GetComponent<Renderer>().enabled = false;
        descriptionText.GetComponent<TextMesh>().GetComponent<Renderer>().enabled = false;

        if (Input.GetKeyUp(GameControl.control.selectKey) || textIndex == 0)
        {
            if (failedFlee)
            {
                if (textIndex == 0) { battleText.GetComponent<TextMesh>().text = GameControl.control.playerName + "'s team failed to escape!"; }
                if (textIndex == 1) { battleText.GetComponent<TextMesh>().text = "The enemy seizes the opportunity!"; }
                if (textIndex == 2)
                {
                    textIndex = 0;
                    state = MenuReader.main;
                    return;
                }
            }
            else
            {
                if (textIndex == 0) { battleText.GetComponent<TextMesh>().text = GameControl.control.playerName + "'s team successfully escapes!"; }
                if (textIndex == 1)
                {
                    // set all of the heroData stats equal to in battle stats
                    foreach (Hero h in heroList)
                    {
                        foreach (HeroData hd in GameControl.control.heroList)
                        {
                            if (h.identity == hd.identity)
                            {
                                hd.level = h.Level;
                                hd.levelUpPts = h.LevelUpPts;
                                hd.exp = h.Exp;
                                hd.expToLvlUp = h.ExpToLevelUp;
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
                                hd.skillsList = h.SkillsList;
                                hd.spellsList = h.SpellsList;
                            }
                        }
                    }
                    // reset the "uses" variable for any items that didn't get used by the battle's end
                    foreach (GameObject i in GameControl.control.consumables) { i.GetComponent<ConsumableItem>().uses = 0; }

                    // exit the battle
                    UnityEngine.SceneManagement.SceneManager.LoadScene(GameControl.control.currentScene);
                }
            }
            if (textIndex <= 1)
            {
                battleText.GetComponent<TextMesh>().text = FormatText(battleText.GetComponent<TextMesh>().text);
            }
            textIndex++;
        }
    }

    void CalcFlee()
    {
        // calculate the likelihood of flight
        int enemyMight = 0;
        int heroMight = 0;
        foreach (Enemy e in enemyList)
        {
            if( e.statusState != Denigen.Status.dead )enemyMight += e.Level * e.Stars;
        }
        foreach (Hero h in heroList)
        {
           if( h.statusState != Denigen.Status.dead ) heroMight += h.Level * h.Stars;
        }
        float likelihood = (60 + heroMight - enemyMight) / 100.0f;

        //see if the player succeeds or fails
        float num = Random.Range(0.0f, 1.0f);
        if (num > likelihood) { failedFlee = true; }
        else {failedFlee = false;}
    }
   
}
