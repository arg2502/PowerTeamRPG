using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EleanorSkillTree : SkillTree
{

    // all possible skills for Jethro

    // tree 1
    Spell purge;
    Karmaic karmaic;
    Spell tears;
    Spell peace;
    Spell gaze;
    Spell sharedBlood;
    Disciple disciple1;
    Disciple disciple2;
    Disciple disciple3;
    Spell blessing;
    Spell ressurection1;
    Spell ressurection2;
    Spell ressurection3;
    Spell rejoice;
    Spell eyesOfDarkness;
    Spell salvation;

    // tree 2
    Spell weep;
    Spell drawBlood;
    Spell borrowed;
    Spell antiheal;
    Spell distress;
    Spell passing;
    Skill staffStrike;
    Skill staffSlam;


    // Trees
    MyTree tree1;
    MyTree tree2;

    // FILL TREES DOWN BELOW, THEN TEST

    // Use this for initialization
    public EleanorSkillTree()
    {

        // set hero to jethro
        hero = GameControl.control.heroList[2];
        whichContent = new List<string> { "Tree1", "Tree2" };

        // read in info
        ReadInfo("techniquesEleanor1.tsv");

        // create all techniques
        purge = new Spell(FindTechnique("purge"));
        karmaic = new Karmaic(FindTechnique("karmaic"));
        tears = new Spell(FindTechnique("tears"));
        peace = new Spell(FindTechnique("peace"));
        gaze = new Spell(FindTechnique("gaze"));
        sharedBlood = new Spell(FindTechnique("sharedBlood"));
        disciple1 = new Disciple(FindTechnique("disciple1"));
        disciple2 = new Disciple(FindTechnique("disciple2"));
        disciple3 = new Disciple(FindTechnique("disciple3"));
        blessing = new Spell(FindTechnique("blessing"));
        ressurection1 = new Spell(FindTechnique("ressurection1"));
        ressurection2 = new Spell(FindTechnique("ressurection2"));
        ressurection3 = new Spell(FindTechnique("ressurection3"));
        rejoice = new Spell(FindTechnique("rejoice"));
        eyesOfDarkness = new Spell(FindTechnique("eyesOfDarkness"));
        salvation = new Spell(FindTechnique("salvation"));

        // next
        purge.ListNextTechnique = new List<Technique>() { karmaic, tears };
        tears.ListNextTechnique = new List<Technique>() { peace, rejoice };
        peace.ListNextTechnique = new List<Technique>() { gaze };
        gaze.ListNextTechnique = new List<Technique>() { sharedBlood, disciple1 };
        disciple1.ListNextTechnique = new List<Technique>() { disciple2, blessing };
        disciple2.ListNextTechnique = new List<Technique>() { disciple3 };
        blessing.ListNextTechnique = new List<Technique>() { ressurection1 };
        ressurection1.ListNextTechnique = new List<Technique>() { ressurection2 };
        ressurection2.ListNextTechnique = new List<Technique>() { ressurection3 };
        rejoice.ListNextTechnique = new List<Technique>() { eyesOfDarkness };        

        // prerequisites
        tears.Prerequisites = new List<Technique>() { purge };
        peace.Prerequisites = new List<Technique>() { tears };
        gaze.Prerequisites = new List<Technique>() { peace };
        sharedBlood.Prerequisites = new List<Technique>() { gaze };
        disciple1.Prerequisites = new List<Technique>() { gaze };
        disciple2.Prerequisites = new List<Technique>() { disciple1 };
        disciple3.Prerequisites = new List<Technique>() { disciple2 };
        blessing.Prerequisites = new List<Technique>() { disciple1 };
        ressurection1.Prerequisites = new List<Technique>() { blessing };
        ressurection2.Prerequisites = new List<Technique>() { ressurection1 };
        ressurection3.Prerequisites = new List<Technique>() { ressurection2 };
        rejoice.Prerequisites = new List<Technique>() { tears };
        eyesOfDarkness.Prerequisites = new List<Technique>() { rejoice };
        salvation.Prerequisites = new List<Technique>() { disciple3, ressurection3, eyesOfDarkness };

        // read in info for next tree
        ReadInfo("techniquesEleanor2.tsv");

        // TREE 2 STATS        
        weep = new Spell(FindTechnique("weep"));
        drawBlood = new Spell(FindTechnique("drawBlood"));
        borrowed = new Spell(FindTechnique("borrowed"));
        antiheal = new Spell(FindTechnique("antiheal"));
        distress = new Spell(FindTechnique("distress"));
        passing = new Spell(FindTechnique("passing"));
        staffStrike = new Skill(FindTechnique("staffStrike"));
        staffSlam = new Skill(FindTechnique("staffSlam"));

        // next
        weep.ListNextTechnique = new List<Technique>() { staffStrike, drawBlood };
        staffStrike.ListNextTechnique = new List<Technique>() { staffSlam };
        drawBlood.ListNextTechnique = new List<Technique>() { borrowed, antiheal };
        antiheal.ListNextTechnique = new List<Technique>() { distress };
        distress.ListNextTechnique = new List<Technique>() { passing };

        // prerequisites
        drawBlood.Prerequisites = new List<Technique>() { weep };
        borrowed.Prerequisites = new List<Technique>() { drawBlood };
        antiheal.Prerequisites = new List<Technique>() { drawBlood };
        distress.Prerequisites = new List<Technique>() { antiheal };
        passing.Prerequisites = new List<Technique>() { distress };
        staffStrike.Prerequisites = new List<Technique>() { weep };
        staffSlam.Prerequisites = new List<Technique>() { staffStrike };


        // trees
        tree1 = new MyTree();
        tree2 = new MyTree();

        // set content array
        tree1.listOfContent = new List<Technique>() { purge, karmaic, tears, peace, gaze, sharedBlood, disciple1, disciple2, disciple3, blessing, ressurection1, ressurection2, ressurection3, rejoice, eyesOfDarkness, salvation };
        tree2.listOfContent = new List<Technique>() { weep, drawBlood, borrowed, antiheal, distress, passing, staffStrike, staffSlam };

        // set sizes of columns and rows
        tree1.numOfColumn = 5;
        tree1.numOfRow = 9;
        tree2.numOfColumn = 5;
        tree2.numOfRow = 5;

        // set starting position
        tree1.rootCol = 2;
        tree1.rootRow = 0;
        tree2.rootCol = 3;
        tree2.rootRow = 0;
        
        listOfTrees = new List<MyTree>();
        listOfTrees.Add(tree1);
        listOfTrees.Add(tree2);
             
    }    
}
