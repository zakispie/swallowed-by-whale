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
        }
        else if (MovementDirection.Equals(Vector3.zero))
        {
            AnimationController.SetBool("Idle", true);
            AnimationController.SetBool("Run", false);
            AnimationController.SetBool("Crouch", false);
            AnimationController.SetBool("Jump", false);
        }
        else if (FacingDirection.Equals(Vector3.left) || FacingDirection.Equals(Vector3.right))
        {
        //    Debug.Log("Facing Direction: left or right");
            AnimationController.SetBool("Idle", false);
            AnimationController.SetBool("Run", true);
            AnimationController.SetBool("Crouch", false);
            AnimationController.SetBool("Jump", false);
        }

        if (Keyboard.sKey.isPressed)
        {
        //    Debug.Log("Facing Direction: down");
            AnimationController.SetBool("Idle", false);
            AnimationController.SetBool("Run", false);
            AnimationController.SetBool("Crouch", true);
            AnimationController.SetBool("Jump", false);
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
