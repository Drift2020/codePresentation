using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Keyboard : IControl
{
    // передумать управление 
    public void Move(ref ButtonStruct state_Move, ref MoveStruct moveStruct)
    {
        #region Move
        state_Move.moveInput = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
        state_Move.isMove = true;

        if (CrossPlatformInputManager.GetButtonDown("A"))
        {
            state_Move.isJump = true;
            state_Move.isJumpOff = true;
        }
        else if(!CrossPlatformInputManager.GetButton("A"))
        {
            state_Move.isJump = false;       
        }

        if (CrossPlatformInputManager.GetButtonDown("X"))
        {
            state_Move.isAction = true;
            state_Move.isActionHold = true;
        }
        else if(!CrossPlatformInputManager.GetButton("X"))
        {
            state_Move.isAction = false;
            state_Move.isActionHold = false;
        }

        if (CrossPlatformInputManager.GetButtonDown("B"))
        {
            state_Move.isShot = true;
        }
        else if(!CrossPlatformInputManager.GetButton("B"))
        {
            state_Move.isShot = false;
        }

        if (CrossPlatformInputManager.GetButtonDown("Y"))
        {
            state_Move.isPunch = true;
        }
        else if (!CrossPlatformInputManager.GetButton("Y"))
        {
            state_Move.isPunch = false;
        }

      
        #endregion
    }



    
 

}
