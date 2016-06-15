using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleMenu : Menu {

    // enumeration to determine what menu is shown
    enum MenuReader { main, techniques, skills, spells, summons, items, flee };
    MenuReader state = MenuReader.main;

    // previous state for back button
    MenuReader prevState;

    // the denigen who is taking the turn
    public GameObject[] denigenGOArray;
    public Denigen[] denigenArray;
    public Denigen currentDenigen;

	// Use this for initialization
	void Start () {
        base.Start();
        numOfRow = 4;
        contentArray = new List<string> { "Strike", "Techniques", "Items", "Flee" };
        buttonArray = new GameObject[numOfRow];

        // set currentDenigen - temp
        denigenGOArray = GameObject.FindGameObjectsWithTag("Denigen");
        denigenArray = new Denigen[denigenGOArray.Length];
        // fill Denigen array
        for(int i = 0; i < denigenGOArray.Length; i++)
        {
            denigenArray[i] = denigenGOArray[i].GetComponent<Denigen>();
        }

        // temporary denigen for sorting
        Denigen temp;
		for(int i = 0; i < denigenArray.Length - 1; i++){
            for (int j = 1; j < denigenArray.Length - i; j++)
            {
                if (denigenArray[j - 1].Spd > denigenArray[j].Spd)
                {
                    temp = denigenArray[j - 1];
                    denigenArray[j - 1] = denigenArray[j];
                    denigenArray[j] = temp;
                }
            }
		}

        // set the current denigen
        currentDenigen = denigenArray[0];

        for (int i = 0; i < numOfRow; i++)
        {
            // create a button
            buttonArray[i] = GameObject.Instantiate(buttonPrefab); ;
            MyButton b = buttonArray[i].GetComponent<MyButton>();
            buttonArray[i].transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y - (250 - b.height) + (i * -(b.height + b.height / 2)));

            // assign text
            b.textObject = GameObject.Instantiate(b.textPrefab);
            b.labelMesh = b.textObject.GetComponent<TextMesh>();
            b.labelMesh.text = contentArray[i];
            b.labelMesh.transform.position = new Vector3(buttonArray[i].transform.position.x, buttonArray[i].transform.position.y, -1);

            //b.Position = new Rect(Screen.width / 2 - b.width / 2, Screen.height / 2 + (i * (b.height + b.height / 2)), b.width, b.height);
            //buttonArray[i].transform.position = new Vector2((Screen.width / 2 - b.width / 2), (Screen.height / 2 + (i * (b.height + b.height / 2))));
            // if we had multiple columns
            //for (int j = 0; j < numOfCol; j++)
            //{
            //buttonArray[i] = GUILayout.Button(contentArray[j + (i * numOfCol)]); // print out buttons in one row based on number of columns

            //}
        }

        // set selected button
        buttonArray[selectedIndex].GetComponent<MyButton>().state = MyButton.MyButtonTextureState.hover;
        
	}
    // change content array and other variables depending on the menu state
    void ChangeContentArray()
    {
        switch (state)
        {
            case MenuReader.main:
               contentArray = new List<string>{ "Strike", "Techniques", "Items", "Flee" };
               prevState = MenuReader.main; // default
               break;
            case MenuReader.techniques:
               contentArray = new List<string> { "<==", "Skills", "Spells", "Summons"};
               prevState = MenuReader.main;
               break;
            case MenuReader.skills:
               contentArray = currentDenigen.SkillsList;
               prevState = MenuReader.techniques;
               break;
            case MenuReader.spells:
               contentArray = currentDenigen.SpellsList;
               prevState = MenuReader.techniques;
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
        //print("Inside BattleMenu ButtonAction");
        // perform action based on passed in string
        switch (label)
        {
                // main battle menu
            case "Strike":
                // nothing right now
                break;
            case "Techniques":
                // change state to techniques menu
                state = MenuReader.techniques;                           
                break;
            case "Items":
                // change state to items menu
                state = MenuReader.items;
                break;
            case "Flee":
                // nothing right now
                break;
            
                // techniques menu
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
            default:
                break;
        }
        ChangeContentArray();
        StateChangeText();   
    }

    void Update()
    {
        base.Update();

        // if back button is pressed, set state to previous state
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            state = prevState;
            ChangeContentArray();
            StateChangeText();
        }   

       
    }
	void UpdateMain () {

        // check for selected button
        /*if (buttonArray[0])
        {
            //print("Strike");
        }
        else if (buttonArray[1])
        {
            
        }
        else if (buttonArray[2])
        {
            //print("ITEMS!!!!@#rJ123IO4J2IO3RIAOSFDJ");
        }
        else if (buttonArray[3])
        {
            //print("Flee");
        }*/
	}
    void UpdateTechniques()
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
            contentArray = new string[] { "Strike", "Techniques", "Items", "Flee" };
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
