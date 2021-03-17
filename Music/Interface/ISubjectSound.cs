using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubjectSound
{
    void Attach(IObserverMusic observer);

    void Detach(IObserverMusic observer);
    void NotifyPlay();

    void NotifyStop();

    void NotifyPause();

    void NotifyUpPause();
}