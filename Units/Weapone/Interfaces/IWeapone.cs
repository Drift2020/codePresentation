using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapone
{
    float energyCost { get; set; }   
    IUnit unit { get; set; }
    bool reload { get; set; }
    bool is_active_shot { get; set; }
    bool shot { get; set; }
    float poverShoot { get; set; }
    string pathToAmmo { get; set; }
    
    float MegaPowerShoot { get; set; }
    float maxCriticalDamage { get; set; }
    float maxСhanceCriticalDamage { get; set; }

    void UpdateDamageBulet(float value);
    void Shot(Vector2 vector, Vector2 poitShoot);
}
