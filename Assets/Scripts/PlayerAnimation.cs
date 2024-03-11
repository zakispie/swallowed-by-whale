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
            AnimationController.SetBool("Idle", false);
            AnimationController.SetBool("Run", false);
            AnimationController.SetBool("Crouch", false);
            AnimationController.SetBool("Jump", true);
            AnimationController.SetBool("CrouchWalk", false);
            AnimationController.SetBool("Climb", false);
        }
        else if (MovementDirection.Equals(Vector3.zero))
        {
            AnimationController.SetBool("Idle", true);
            AnimationController.SetBool("Run", false);
            AnimationController.SetBool("Crouch", false);
            AnimationController.SetBool("Jump", false);
            AnimationController.SetBool("CrouchWalk", false);
            AnimationController.SetBool("Climb", false);
        }
        else if (FacingDirection.Equals(Vector3.left) || FacingDirection.Equals(Vector3.right))
        {
        //    Debug.Log("Facing Direction: left or right");
            AnimationController.SetBool("Idle", false);
            AnimationController.SetBool("Run", true);
            AnimationController.SetBool("Crouch", false);
            AnimationController.SetBool("Jump", false);
            AnimationController.SetBool("CrouchWalk", false);
            AnimationController.SetBool("Climb", false);
        }

        if (IsCrouched)
        {
        //    Debug.Log("Facing Direction: down");
            if(MovementDirection.Equals(Vector3.zero))
            {
                AnimationController.SetBool("Idle", false);
                AnimationController.SetBool("Run", false);
                AnimationController.SetBool("Crouch", true);
                AnimationController.SetBool("Jump", false);
                AnimationController.SetBool("CrouchWalk", false);
                AnimationController.SetBool("Climb", false);
            } else
            {
                AnimationController.SetBool("Idle", false);
                AnimationController.SetBool("Run", false);
                AnimationController.SetBool("Crouch", false);
                AnimationController.SetBool("Jump", false);
                AnimationController.SetBool("CrouchWalk", true);
                AnimationController.SetBool("Climb", false);
            }
        }

        if(OnLadder)
        {
            if(Keyboard.wKey.isPressed || Keyboard.sKey.isPressed)
            {
                AnimationController.SetBool("Idle", false);
                AnimationController.SetBool("Run", false);
                AnimationController.SetBool("Crouch", false);
                AnimationController.SetBool("Jump", false);
                AnimationController.SetBool("CrouchWalk", false);
                AnimationController.SetBool("Climb", true);
            } else
            {
                //if not moving on ladder, pause animation
                AnimationController.SetBool("Idle", false);
                AnimationController.SetBool("Run", false);
                AnimationController.SetBool("Crouch", false);
                AnimationController.SetBool("Jump", false);
                AnimationController.SetBool("CrouchWalk", false);
                AnimationController.SetBool("Climb", true);
            }
        }


        /*else if (MovementDirection.Equals(Vector3.zero))
        {
            AnimationController.SetBool("Idle", true);
            AnimationController.SetBool("Run", false);
            AnimationController.SetBool("Crouch", false);
            AnimationController.SetBool("Jump", false);
        }
        */
    }
}
