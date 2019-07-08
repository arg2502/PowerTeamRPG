using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "Database/Sprites", order = 3)]
public class SpriteDatabase : ScriptableObject {
    [Header("Jethro")]
    public Sprite jethroNeutralPortrait;
    public Sprite jethroHappyPortrait;
    public Sprite jethroSadPortrait;
    public Sprite jethroAngryPortrait;
    [Header("Cole")]
    public Sprite coleNeutralPortrait;
    public Sprite coleHappyPortrait;
    public Sprite coleSadPortrait;
    public Sprite coleAngryPortrait;
    [Header("Eleanor")]
    public Sprite eleanorNeutralPortrait;
    public Sprite eleanorHappyPortrait;
    public Sprite eleanorSadPortrait;
    public Sprite eleanorAngryPortrait;
    [Header("Jouliette")]
    public Sprite joulietteNeutralPortrait;
    public Sprite joulietteHappyPortrait;
    public Sprite joulietteSadPortrait;
    public Sprite joulietteAngryPortrait;
}
