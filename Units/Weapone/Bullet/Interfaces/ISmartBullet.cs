using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISmartBullet : IBullet
{
    Transform targetUnit { get; set; }

    float timeProsecution { get; set; }
}
