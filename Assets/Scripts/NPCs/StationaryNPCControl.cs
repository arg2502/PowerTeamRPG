using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryNPCControl : NPCObject {

	Animator anim;

	public enum State{
		idle, //not talking, NPC faces default direction
		talking //NPC faces player
	}

	public State currentState = State.idle; //set state to idle by default

	public enum Direction {
		up,
		left,
		down,
		right
	}

	public Direction defaultDirection = Direction.down;

    public bool isSitting;

	private BoxCollider2D boxCollider; // Variable to reference our collider

	// Use this for initialization
	new protected void Start () {

		//Find the NPC's animator
		anim = GetComponent<Animator>();

        if (anim != null)
            anim.SetFloat("isSitting", System.Convert.ToSingle(isSitting));

        //Find the NPC's collider
        boxCollider = GetComponent<BoxCollider2D> ();

		base.Start();

		//Call backtoNormal to make NPC face it's default direction and idle
		SetDefaultAnimation();
	}

    private void OnEnable()
    {
        SetDefaultAnimation();
    }

    // Update is called once per frame
    void Update () {
        base.Update();
        if (anim != null)
        {
            if (GameControl.control.currentCharacterState != characterControl.CharacterState.Normal)
            {
                anim.speed = 0;
                return;
            }
            else if (anim.speed == 0)
                anim.speed = 1;
                       
        }
	}

	public void FaceCharacter(Vector2 directionToFace)
	{
		currentState = State.talking; // set NPC to talking
		canMove = false; // Stop moving, to set to talking sprites

        // set the blend tree animator to face the proper direction
        if (anim)
        {
            anim.SetBool("canMove", canMove);
            anim.SetFloat("lastHSpeed", directionToFace.x);
            anim.SetFloat("lastVSpeed", directionToFace.y);
        }
	}
	public override void BackToNormal()
	{
        base.BackToNormal();
        SetDefaultAnimation();
	}

    void SetDefaultAnimation()
    {
        canMove = true;
        currentState = State.idle;
        if (!anim) return;

        anim.SetBool("canMove", canMove); //sets back to idle sprites

        //Set the NPC to face the default direction
        if (GameControl.AnimatorHasParameter(anim, "lastHSpeed") && GameControl.AnimatorHasParameter(anim, "lastVSpeed"))
        {
            switch (defaultDirection)
            {
                case Direction.up:
                    anim.SetFloat("lastHSpeed", 0.0f);
                    anim.SetFloat("lastVSpeed", 1.0f);
                    break;
                case Direction.left:
                    anim.SetFloat("lastHSpeed", -1.0f);
                    anim.SetFloat("lastVSpeed", 0.0f);
                    break;
                case Direction.right:
                    anim.SetFloat("lastHSpeed", 1.0f);
                    anim.SetFloat("lastVSpeed", 0.0f);
                    break;
                default: //down
                    anim.SetFloat("lastHSpeed", 0.0f);
                    anim.SetFloat("lastVSpeed", -1.0f);
                    break;
            }
        }
    }
}
