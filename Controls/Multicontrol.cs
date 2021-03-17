using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Multicontrol : IControl
{

    // передумать управление 
    public void Move(ref ButtonStruct state_Move, ref MoveStruct moveStruct)
    {
        #region Move
        float x = CrossPlatformInputManager.GetAxis("Horizontal");
        float x1 = Input.GetAxis("Horizontal");

        float y = CrossPlatformInputManager.GetAxis("Vertical");
        float y1 = Input.GetAxis("Vertical");
        state_Move.moveInput = new Vector2((x != 0 ? x : x1), (y != 0 ? y : y1));
        state_Move.isMove = true;


        if (Input.GetButtonDown("A") || CrossPlatformInputManager.GetButtonDown("A"))
        {
            state_Move.isJump = true;
            state_Move.isJumpOff = true;
        }
        else if (!Input.GetButton("A") && !CrossPlatformInputManager.GetButton("A"))
        {
            state_Move.isJump = false;
        }


        if (Input.GetButtonDown("X") || CrossPlatformInputManager.GetButtonDown("X"))
        {
            state_Move.isAction = true;
            state_Move.isActionHold = true;
        }
        else if (!Input.GetButton("X") && !CrossPlatformInputManager.GetButton("X"))
        {
            state_Move.isAction = false;
            state_Move.isActionHold = false;
        }


        if (Input.GetButtonDown("B") || CrossPlatformInputManager.GetButtonDown("B"))
        {
            state_Move.isPunch = true;
        }
        else if (!Input.GetButton("B") && !CrossPlatformInputManager.GetButton("B"))
        {
            state_Move.isPunch = false;
        }

    
        if (Input.GetButtonDown("Y") || CrossPlatformInputManager.GetButtonDown("Y"))
        {
            state_Move.isSkill = true;
        }
        else if (!Input.GetButton("Y") && !CrossPlatformInputManager.GetButton("Y"))
        {
            state_Move.isSkill = false;
            state_Move.isSkillOff = true;
        }

        

        if (Input.GetAxis("RT")!=0 || CrossPlatformInputManager.GetAxis("RT") != 0 
            || Input.GetButtonDown("ShootAlter") || CrossPlatformInputManager.GetButtonDown("ShootAlter"))
        {
          state_Move.isShot = true;
        }
        else if (Input.GetAxis("RT") == 0 && CrossPlatformInputManager.GetAxis("RT") == 0 &&
            !Input.GetButton("ShootAlter") && !CrossPlatformInputManager.GetButton("ShootAlter"))
        {
            state_Move.isShot = false;
        }


        if (Input.GetButtonDown("Skill") || CrossPlatformInputManager.GetButtonDown("Skill") ||
            Input.GetAxis("dPadY") < 0f || CrossPlatformInputManager.GetAxis("dPadY") < 0f)
        {
            state_Move.isSkill = true;
           
        }
        else if (!Input.GetButton("Skill") && !CrossPlatformInputManager.GetButton("Skill") &&
            Input.GetAxis("dPadY") == 0f || CrossPlatformInputManager.GetAxis("dPadY") == 0f &&
            state_Move.isSkill)
        {
            state_Move.isSkill = false;
            state_Move.isSkillOff = true;
        }

        #endregion
    }

}
