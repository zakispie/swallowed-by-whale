using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    #region variables

    // Most Recent Facing Direction Property
    private static Vector3 FacingDirection => PlayerController.FacingDirection;

    // Most Recent Movement Direction Property
    private static Vector3 MovementDirection => PlayerController.MovementDirection;

    // Animation Controller Child Property
    private static Animator AnimationController => PlayerController.AnimationController;

    // Keyboard Property
    private static Keyboard Keyboard => PlayerController.Keyboard;

    // Crouch Detection Property
    private static bool IsCrouched => PlayerController.IsCrouched;

    // Ladder Detection Property
    // Note: Change Accessability in PlayerController (?)
    private static bool OnLadder => PlayerController._onLadder;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (FacingDirection.Equals(Vector3.up))
        {
            //Debug.Log("Facing Direction: up");
            SetAnimation("Jump");
        }
        else if (MovementDirection.Equals(Vector3.zero) && !OnLadder)
        {
            SetAnimation("Idle");
        }
        else if (FacingDirection.Equals(Vector3.left) || FacingDirection.Equals(Vector3.right))
        {
            //    Debug.Log("Facing Direction: left or right");
            SetAnimation("Run");
        }

        if (IsCrouched)
        {
        //    Debug.Log("Facing Direction: down");
            if(MovementDirection.Equals(Vector3.zero))
            {
                SetAnimation("Crouch");
            } else
            {
                SetAnimation("CrouchWalk");
            }
        }

        if(OnLadder)
        {
            Debug.Log("We should be on the ladder.");
            if(Keyboard.wKey.isPressed || Keyboard.sKey.isPressed)
            {
                SetAnimation("Climb");
            } else if (!Keyboard.wKey.isPressed && !Keyboard.sKey.isPressed)
            {
                //if not moving on ladder, pause animation
                SetAnimation("ClimbingIdle");
            }
        }

        /*Debug.Log("Idle: " + AnimationController.GetBool("Idle")
            + "\nRun: " + AnimationController.GetBool("Run")
            + "\nCrouch: " + AnimationController.GetBool("Crouch")
            + "\nJump: " + AnimationController.GetBool("Jump")
            + "\nCrouchWalk: " + AnimationController.GetBool("CrouchWalk")
            + "\nClimb: " + AnimationController.GetBool("Climb")
            + "\nClimbingIdle: " + AnimationController.GetBool("ClimbingIdle"));*/
    }


    //Takes in a string that will determine which animation to play
    void SetAnimation(string animationTitle)
    {
        AnimationController.SetBool("Idle", false);
        AnimationController.SetBool("Run", false);
        AnimationController.SetBool("Crouch", false);
        AnimationController.SetBool("Jump", false);
        AnimationController.SetBool("CrouchWalk", false);
        AnimationController.SetBool("Climb", false);
        AnimationController.SetBool("ClimbingIdle", false);

        switch (animationTitle)
        {
            case "Idle":
                AnimationController.SetBool("Idle", true);
                break;
            case "Run":
                AnimationController.SetBool("Run", true);
                break;
            case "Crouch":
                AnimationController.SetBool("Crouch", true);
                break;
            case "Jump":
                AnimationController.SetBool("Jump", true);
                break;
            case "CrouchWalk":
                AnimationController.SetBool("CrouchWalk", true);
                break;
            case "Climb":
                AnimationController.SetBool("Climb", true);
                break;
            case "ClimbingIdle":
                AnimationController.SetBool("ClimbingIdle", true);
                break;
        }
    }
}
