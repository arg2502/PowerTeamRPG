using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonSkillTree : MyButton {

    Technique technique;
    // if the Technique has a next, this next takes control of the next button's state
    List<ButtonSkillTree> listNextButton;
    SpriteRenderer contentSr;
    List<GameObject> nextLine;
    Sprite solidLine;
    Sprite dottedLine;

    public Technique Technique { get { return technique; } set { technique = value; } }
    public List<ButtonSkillTree> ListNextButton { get { return listNextButton; } set { listNextButton = value; } }
    public SpriteRenderer ContentSr { get { return contentSr; } set { contentSr = value; } }
    public List<GameObject> NextLine { get { return nextLine; } set { nextLine = value; } }
    public Sprite SolidLine { get { return solidLine; } set { solidLine = value; } }
    public Sprite DottedLine { get { return dottedLine; } set { dottedLine = value; } }

}
