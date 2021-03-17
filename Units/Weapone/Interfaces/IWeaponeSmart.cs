using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponeSmart : IWeapone
{
    void SetTarget(Transform targetUnit);
}
