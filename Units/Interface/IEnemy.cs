using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy : IUnit 
{
    IArtificialIntelligence artificialIntelligence { get; set; }
}
