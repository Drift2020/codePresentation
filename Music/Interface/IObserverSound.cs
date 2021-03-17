using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverSound
{
    void UpdatePlay(string name, int number);
    void UpdateStop(int number);
    void UpdatePause(int number);
    void UpdateUnPause(int number);

}