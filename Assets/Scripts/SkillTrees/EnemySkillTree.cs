using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillTree : SkillTree {

    // GOIKKO
    public Skill whip;
    public Spell poison;

    // MUDPUPPY
    public Skill bite;
    public Skill frenzy;

    // CRABGRASS
    public Skill stomp;
    public Spell heal;

    public EnemySkillTree()
    {
        ReadInfo("techniquesEnemies");

        whip = new Skill(FindTechnique("whip"));
        poison = new Spell(FindTechnique("poison"));
        bite = new Skill(FindTechnique("bite"));
        frenzy = new Skill(FindTechnique("frenzy"));
        stomp = new Skill(FindTechnique("stomp"));
        heal = new Spell(FindTechnique("heal"));
    }
}
