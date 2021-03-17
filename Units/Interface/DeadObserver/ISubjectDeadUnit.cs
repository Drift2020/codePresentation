using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubjectDeadUnit
{
    void Attach(IObserverDeadUnit observer);
    void Detach(IObserverDeadUnit observer);
    void NotifyDead(GameObject _object, DeadCommand deadCommand);
}
