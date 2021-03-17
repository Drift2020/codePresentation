using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill 
{

    void Use();

    void Update();

    bool IsAvalible();

    float MyTime { get; set; }
    float FullTime { get; set; }
    bool activate { get; set; }
    float reloadTime { get; set; }
    float reloadFullTime { get; set; }
    uint level { get; set; }
}
