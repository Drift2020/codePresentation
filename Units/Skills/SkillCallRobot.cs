﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCallRobot : Object, ISkill
{
    public GameObject prefab;
    public uint level { get; set; }

    public float MyTime { get; set; }
    public float FullTime { get; set; }
    public float reloadTime { get; set; }
    public float reloadFullTime { get; set; }
    public bool activate { get; set; }
    public SkillCallRobot()
    {
        prefab = Resources.Load<GameObject>("Skills/Robot");
        level = 0;
    }
    public void Use()
    {
        Debug.Log("Null relization");
        Instantiate(prefab);
    }

    public void Update()
    {
        
    }
    public bool IsAvalible()
    {
        return true;
    }
}