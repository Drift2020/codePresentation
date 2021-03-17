using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum DeadCommand 
{ 
    kill,
    destroy,
    playerDied
}
public interface IObserverDeadUnit
{
    void UpdateDead(GameObject _object, DeadCommand command);
}
