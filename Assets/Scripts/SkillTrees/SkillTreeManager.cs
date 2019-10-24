using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTreeManager {
    
    //HeroData hero;
    //bool skillTree;
    internal SkillTree currentSkillTree;

    JethroSkillTree jethro;
    ColeSkillTree cole;
    EleanorSkillTree eleanor;
    JoulietteSkillTree jouliette;

    public EnemySkillTree enemySkillTree;

    public TechniqueImageDatabase imageDatabase;

    List<HeroData> heroList; // reference to GameControl's list


    Passive iceArmorPassive;
    Passive iceBarrierPassive;
    Passive fogPassive;
    List<Technique> tempTechniques;

	// Use this for initialization
	public SkillTreeManager () {
        
        jethro = new JethroSkillTree();
        cole = new ColeSkillTree();
        eleanor = new EleanorSkillTree();
        jouliette = new JoulietteSkillTree();
        enemySkillTree = new EnemySkillTree();
        imageDatabase = Resources.Load<TechniqueImageDatabase>("Databases/TechniqueImages");
        heroList = GameControl.control.heroList;
        AddTempTechniques();
        AddStartingTechniques();
    }

    public void SetHero(int heroIndex)
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
    public void AddTechnique(DenigenData hero, string techName)
    {
        AddTechnique(hero, FindTechnique(hero, techName));
    }

    public void AddTechnique(DenigenData hero, Technique tech)
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

    public void RemoveTechnique(DenigenData hero, string techName)
    {
        RemoveTechnique(hero, FindTechnique(hero, techName));
    }

    public void RemoveTechnique(DenigenData hero, Technique tech)
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

    public Technique FindTempTechnique(string techName)
    {
        return tempTechniques.Find(t => t.Name == techName);
    }

    public bool HasTechnique(DenigenData hero, Technique tech)
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

    bool HasSkill(DenigenData hero, Skill skill)
    {
        if (hero.skillsList.Contains(skill))
            return true;
        else
            return false;
    }
    bool HasSpell(DenigenData hero, Spell spell)
    {
        if (hero.spellsList.Contains(spell))
            return true;
        else
            return false;
    }
    bool HasPassive(DenigenData hero, Passive passive)
    {
        if (hero.passiveList.Contains(passive))
            return true;
        else
            return false;
    }

    void AddTempTechniques()
    {
        iceArmorPassive = new IceArmorPassive();
        iceBarrierPassive = new IceBarrierPassive();
        fogPassive = new FogPassive();        
        
        tempTechniques = new List<Technique>() { iceArmorPassive, iceBarrierPassive, fogPassive };
    }

    void AddStartingTechniques()
    {
        AddJethroStartingTechniques();
        if(heroList.Count > 1) AddColeStartingTechniques();
        if(heroList.Count > 2) AddEleanorStartingTechniques();
        if(heroList.Count > 3) AddJoulietteStartingTechniques();
    }

    void AddJethroStartingTechniques()
    {
        foreach (var tech in jethro.startingTechs)
            AddTechnique(GameControl.control.heroList[0], tech);
    }

    void AddColeStartingTechniques()
    {
        foreach (var tech in cole.startingTechs)
            AddTechnique(GameControl.control.heroList[1], tech);
    }

    void AddEleanorStartingTechniques()
    {
        foreach (var tech in eleanor.startingTechs)
            AddTechnique(GameControl.control.heroList[2], tech);
    }

    void AddJoulietteStartingTechniques()
    {
        foreach (var tech in jouliette.startingTechs)
            AddTechnique(GameControl.control.heroList[3], tech);
    }

    public Technique FindTechnique(DenigenData data, string techName)
    {
        // try checking the general temp techniques list
        foreach (var tech in tempTechniques)
        {
            if (string.Compare(techName, tech.Name) == 0)
                return tech;
        }

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
