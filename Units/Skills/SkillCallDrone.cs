using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCallDrone : Object, ISkill
{
    public GameObject prefab;
    public uint level { get; set; }
    bool isAvalible;
    public float MyTime { get; set; }
    public float FullTime { get; set; }
    public float reloadTime { get; set; }
    public float reloadFullTime { get; set; }
    public bool activate { get; set; }
    public SkillCallDrone()
    {
        prefab = Resources.Load<GameObject>("Units/City/Drone");
        level = 0;
        MyTime = FullTime = 60;
        isAvalible = true;
    }

    public void Use()
    {
        Debug.Log("Null relization");
        Instantiate(prefab);
        isAvalible = false;
    }
    void Timer()
    {
        if (MyTime > 0)
        {
            MyTime -= Time.deltaTime;
        
        }
        else
        {
            MyTime = FullTime;
            isAvalible = true;
        }

    }
    public void Update()
    {
        if (!isAvalible)
        {
            Timer();
        }
    }

    public bool IsAvalible()
    {
        return isAvalible;
    }
}
