using UnityEngine;
using System.Collections;

public class SkillTreeManager {

    //HeroData hero;
    //bool skillTree;
    internal SkillTree currentSkillTree;

    JethroSkillTree jethro;
    ColeSkillTree cole;
    EleanorSkillTree eleanor;
    JulietteSkillTree juliette;

    public TechniqueImageDatabase imageDatabase;

	// Use this for initialization
	public SkillTreeManager () {
        jethro = new JethroSkillTree();
        cole = new ColeSkillTree();
        eleanor = new EleanorSkillTree();
        juliette = new JulietteSkillTree();
        imageDatabase = Resources.Load<TechniqueImageDatabase>("Databases/TechniqueImages");
        //skillTree = false;

        //Set the hero
        //for (int i = 0; i < GameControl.control.heroList.Count; i++)
        //{
        //    if (GameControl.control.heroList[i].skillTree)
        //    {
        //        GameControl.control.heroList[i].skillTree = false;
        //        hero = GameControl.control.heroList[i];
        //        // set specific skill tree
        //        switch (hero.identity)
        //        {
        //            case 0:
        //                gameObject.AddComponent<JethroSkillTree>();
        //                break;
        //            case 1:
        //                gameObject.AddComponent<ColeSkillTree>();
        //                break;
        //            case 2:
        //                gameObject.AddComponent<EleanorSkillTree>();
        //                break;
        //            case 3:
        //                gameObject.AddComponent<JulietteSkillTree>();
        //                break;
        //            default:
        //                break;
        //        }

        //        break;
        //    }
        //}

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
                currentSkillTree = juliette;
                break;
        }
    }

    public void AddTechnique(HeroData hero, Technique tech)
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

    public bool HasTechnique(HeroData hero, Technique tech)
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

    bool HasSkill(HeroData hero, Skill skill)
    {
        if (hero.skillsList.Contains(skill))
            return true;
        else
            return false;
    }
    bool HasSpell(HeroData hero, Spell spell)
    {
        if (hero.spellsList.Contains(spell))
            return true;
        else
            return false;
    }
    bool HasPassive(HeroData hero, Passive passive)
    {
        if (hero.passiveList.Contains(passive))
            return true;
        else
            return false;
    }
}
