using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    float damage { get; set; }
    float speed { get; set; }
    Vector2 vector { get; set; }
    IUnit master { get; set; }
}
