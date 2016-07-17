using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

    //This class is sort of a singleton, but not really.
    //This will hold our persistent data between rooms, like the heroes, and equipment

    //attributes
    public static GameControl control;
    public Hero JethroPrefab;
    public Hero ColePrefab;
    public Hero EleanorPrefab;
    public Hero JuliettePrefab;
    public Hero SelenePrefab;

    //float health;
    //string currentScene;
    //public List<Denigen> heroList = new List<Denigen>() { };
    public List<Hero> heroList = new List<Hero>() { };

    //awake gets called before start
    void Awake () {
        if (control == null)
        {
            //this keeps the game object from being destroyed between scenes
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            //If a gameControl already exists, we don't want 2 of them
            Destroy(gameObject);
        }
        
	}

    //this will save our game data to an external, persistent file
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        //data.health = health;
        //save all of the heroes
        for (int i = 0; i < heroList.Count; i++)
        {
            data.heroList.Add(new HeroData());
            data.heroList[i].name = heroList[i].name;
            data.heroList[i].level = heroList[i].Level;
            data.heroList[i].exp = heroList[i].Exp;
            data.heroList[i].levelUpPts = heroList[i].LevelUpPts;
            data.heroList[i].techPts = heroList[i].TechPts;
            data.heroList[i].hp = heroList[i].hp;
            data.heroList[i].hpMax = heroList[i].hpMax;
            data.heroList[i].pm = heroList[i].pm;
            data.heroList[i].pmMax = heroList[i].pmMax;
            data.heroList[i].atk = heroList[i].Atk;
            data.heroList[i].def = heroList[i].Def;
            data.heroList[i].mgkAtk = heroList[i].MgkAtk;
            data.heroList[i].mgkDef = heroList[i].MgkDef;
            data.heroList[i].luck = heroList[i].Luck;
            data.heroList[i].evasion = heroList[i].Evasion;
            data.heroList[i].spd = heroList[i].Spd;
        }
        //Save which scene the player is in
        data.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        //Writes our player class to a file
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //sets local data to the stored data
            //health = data.health;
            //load all of the heroes
            for (int i = 0; i < data.heroList.Count; i++)
            {
                switch (data.heroList[i].name)
                {
                    case "Jethro":
                        heroList.Add(GameObject.Instantiate(JethroPrefab));
                        break;
                    case "Cole":
                        heroList.Add(GameObject.Instantiate(ColePrefab));
                        break;
                    case "Eleanor":
                        heroList.Add(GameObject.Instantiate(EleanorPrefab));
                        break;
                    case "Juliette":
                        heroList.Add(GameObject.Instantiate(JuliettePrefab));
                        break;
                    case "Selene":
                        heroList.Add(GameObject.Instantiate(SelenePrefab));
                        break;
                    default:
                        heroList.Add(GameObject.Instantiate(JethroPrefab));
                        break;

                }
                heroList[i].name = data.heroList[i].name;
                heroList[i].Level = data.heroList[i].level;
                heroList[i].Exp = data.heroList[i].exp;
                heroList[i].LevelUpPts = data.heroList[i].levelUpPts;
                heroList[i].TechPts = data.heroList[i].techPts;
                heroList[i].hp = data.heroList[i].hp;
                heroList[i].hpMax = data.heroList[i].hpMax;
                heroList[i].pm = data.heroList[i].pm;
                heroList[i].pmMax = data.heroList[i].pmMax;
                heroList[i].Atk = data.heroList[i].atk;
                heroList[i].Def = data.heroList[i].def;
                heroList[i].MgkAtk = data.heroList[i].mgkAtk;
                heroList[i].MgkDef = data.heroList[i].mgkDef;
                heroList[i].Luck = data.heroList[i].luck;
                heroList[i].Evasion = data.heroList[i].evasion;
                heroList[i].Spd = data.heroList[i].spd;
            }
            //put the player back where they were
            UnityEngine.SceneManagement.SceneManager.LoadScene(data.currentScene);
            // Put their position vector here if we choose
        }
    }
}

//this class is where all of our data will be sent in order to be saved
[Serializable]
class PlayerData
{
    public float health;
    public string currentScene;
    public List<HeroData> heroList = new List<HeroData>() { };
}

//this class should hold all of the stuff necessary for a hero object
[Serializable]
class HeroData
{
    public string name;
    public int level, exp, levelUpPts, techPts;
    public int hp, hpMax, pm, pmMax, atk, def, mgkAtk, mgkDef, luck, evasion, spd;
    public List<string> skillsList, skillsDescription, spellsList, spellsDescription;
    //Also need passives, equipment
}
