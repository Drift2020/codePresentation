using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubjectMusic
{
    void Attach(IObserverMusic observer);

    void Detach(IObserverMusic observer);

    void NotifyPlayMusic();

    void NotifyStopMusic();

    void NotifyPauseMusic();

    void NotifyUpPauseMusic();
}