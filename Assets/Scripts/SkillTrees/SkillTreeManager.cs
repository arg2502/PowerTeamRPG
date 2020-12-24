using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTreeManager {
    
    //HeroData hero;
    //bool skillTree;
    public static SkillTree currentSkillTree;

    private static JethroSkillTree jethro;
    private static ColeSkillTree cole;
    private static EleanorSkillTree eleanor;
    private static JoulietteSkillTree jouliette;

    public static EnemySkillTree enemySkillTree;

    public static TechniqueImageDatabase imageDatabase;

    private static List<HeroData> heroList; // reference to GameControl's list


    private static Passive iceArmorPassive;
    private static Passive iceBarrierPassive;
    private static Passive fogPassive;
    private static Passive bonecrushPassive;
	private static Passive bucketSplashPassive;
	private static Passive flaskSplashPassive;
	private static Passive coldShoulderPassive;
	private static Passive slowBurnPassive;
    private static List<Technique> tempTechniques;
    
	public static void Init () {

        imageDatabase = Resources.Load<TechniqueImageDatabase>("Databases/TechniqueImages");

        CreateTrees();     

        heroList = GameControl.control.heroList;
        AddTempTechniques();
        AddStartingTechniques();
    }

    private static void CreateTrees()
    {
        jethro = new JethroSkillTree();
        cole = new ColeSkillTree();
        eleanor = new EleanorSkillTree();
        jouliette = new JoulietteSkillTree();
        enemySkillTree = new EnemySkillTree();
    }

    public static void SetHero(int heroIndex)
    {
        switch(heroIndex)
        {
            case 0:
                currentSkillTree = jethro;
                break;
            case 1:
                currentSkillTree = cole;
                break;
            case 2:
                currentSkillTree = eleanor;
                break;
            case 3:
                currentSkillTree = jouliette;
                break;
        }
    }

    public static void AddPassiveTechnique(DenigenData hero, string techName, bool onlyOne)
    {
        if (!onlyOne || (FindTechnique(hero, techName) == null))
            AddTechnique(hero, FindTempTechnique(techName));
    }

    public static void AddTechnique(DenigenData hero, string techName)
    {
        AddTechnique(hero, FindTechnique(hero, techName));
    }

    public static void AddTechnique(DenigenData hero, Technique tech)
    {
        // check what kind of technique
        if (tech is Skill)
        {
            hero.skillsList.Add((Skill)tech);
        }
        else if (tech is Spell)
        {
            hero.spellsList.Add((Spell)tech);
        }
        else if (tech is Passive)
        {
            hero.passiveList.Add((Passive)tech);
        }
        else
        {
            Debug.Log("Technique not added.");
            return;
        }
    }

    public static void RemoveTechnique(DenigenData hero, string techName)
    {
        RemoveTechnique(hero, FindTechnique(hero, techName));
    }

    public static void RemoveTechnique(DenigenData hero, Technique tech)
    {
        // check what kind of technique
        if (tech is Skill)
        {
            hero.skillsList.Remove((Skill)tech);
        }
        else if (tech is Spell)
        {
            hero.spellsList.Remove((Spell)tech);
        }
        else if (tech is Passive)
        {
            hero.passiveList.Remove((Passive)tech);
        }
        else
        {
            Debug.Log("Technique could not be removed -- not a Skill, Spell, or Passive");
            return;
        }
    }

    public static Technique FindTempTechnique(string techName)
    {
        return tempTechniques.Find(t => t.Name == techName);
    }

    public static bool HasTechnique(DenigenData hero, Technique tech)
    {
        if (tech is Skill)
            return HasSkill(hero, tech as Skill);
        else if (tech is Spell)
            return HasSpell(hero, tech as Spell);
        else if (tech is Passive)
            return HasPassive(hero, tech as Passive);
        else
        {
            Debug.LogError("Unable to define technique");
            return false;
        }
    }

    private static bool HasSkill(DenigenData hero, Skill skill)
    {
        if (hero.skillsList.Contains(skill))
            return true;
        else
            return false;
    }
    private static bool HasSpell(DenigenData hero, Spell spell)
    {
        if (hero.spellsList.Contains(spell))
            return true;
        else
            return false;
    }
    private static bool HasPassive(DenigenData hero, Passive passive)
    {
        if (hero.passiveList.Contains(passive))
            return true;
        else
            return false;
    }

    private static void AddTempTechniques()
    {
        iceArmorPassive = new IceArmorPassive();
        iceBarrierPassive = new IceBarrierPassive();
        fogPassive = new FogPassive();
        bonecrushPassive = new BonecrushPassive();
		bucketSplashPassive = new BucketSplashPassive ();
		flaskSplashPassive = new FlaskSplashPassive ();
		coldShoulderPassive = new ColdShoulderPassive ();
		slowBurnPassive = new SlowBurnPassive ();

		tempTechniques = new List<Technique>() { iceArmorPassive, iceBarrierPassive, fogPassive, bonecrushPassive, bucketSplashPassive, flaskSplashPassive,
			coldShoulderPassive, slowBurnPassive};
    }

    private static void AddStartingTechniques()
    {
        AddJethroStartingTechniques();
        if(heroList.Count > 1) AddColeStartingTechniques();
        if(heroList.Count > 2) AddEleanorStartingTechniques();
        if(heroList.Count > 3) AddJoulietteStartingTechniques();
    }

    private static void AddJethroStartingTechniques()
    {
        foreach (var tech in jethro.startingTechs)
            AddTechnique(GameControl.control.heroList[0], tech);
    }

    private static void AddColeStartingTechniques()
    {
        foreach (var tech in cole.startingTechs)
            AddTechnique(GameControl.control.heroList[1], tech);
    }

    private static void AddEleanorStartingTechniques()
    {
        foreach (var tech in eleanor.startingTechs)
            AddTechnique(GameControl.control.heroList[2], tech);
    }

    private static void AddJoulietteStartingTechniques()
    {
        foreach (var tech in jouliette.startingTechs)
            AddTechnique(GameControl.control.heroList[3], tech);
    }

    public static Technique FindTechnique(DenigenData data, string techName)
    {
        //// try checking the general temp techniques list
        //foreach (var tech in tempTechniques)
        //{
        //    if (string.Compare(techName, tech.Name) == 0)
        //        return tech;
        //}

        // check skills list first
        foreach (var tech in data.skillsList)
        {
            if (string.Compare(techName, tech.Name) == 0)
                return tech;
        }

        // if no luck yet, check spells
        foreach(var tech in data.spellsList)
        {
            if (string.Compare(techName, tech.Name) == 0)
                return tech;
        }

        // if we're trying to find a passive, try that
        foreach(var tech in data.passiveList)
        {
            if (string.Compare(techName, tech.Name) == 0)
                return tech;
        }


        // at this point, we don't have the technique we're looking for, return null
        return null;
    }
}
