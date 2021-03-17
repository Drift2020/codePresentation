using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverMusic
{
    void UpdatePlay(string name);
    void UpdateStop();
    void UpdatePause();
    void UpdateUnPause();

}