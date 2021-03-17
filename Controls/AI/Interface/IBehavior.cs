using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehavior 
{
    void ActiveState(IUnit unit, IArtificialIntelligence AI);
    void AnalyseFlags(IUnit unit, IArtificialIntelligence AI);
}
