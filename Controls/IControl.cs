using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControl 
{
    void Move(ref ButtonStruct state_Move, ref MoveStruct moveStruct);
}


