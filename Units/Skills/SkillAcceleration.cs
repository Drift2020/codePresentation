using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAcceleration : ISkill
{
    public Player prefab;
    public float EnergyForActivateSpeed;

    public float MyTime { get; set; }
    public float FullTime { get; set; }
    public float reloadTime { get; set; }
    public float reloadFullTime { get; set; }
    public uint level { get; set; }
    float time;

    public bool activate { get; set; }

    public SkillAcceleration(Player prefab)
    {
        this.prefab = prefab;
        activate = false;
        _switch = false;
    }
    /// <summary>
    /// false - player
    /// true - unit
    /// </summary>
    bool _switch;

    public void Use()
    {
        if (!activate)
        {
            EnergyForActivateSpeed = this.prefab.updateStateFromSkill.lessEnergyForActivateSpeedStep;
            if (!prefab.stateStruct.isControling)
            {
                _switch = false;

                if (prefab.stateStruct.energyStruct.energy >= EnergyForActivateSpeed && !activate)
                {
                    prefab.stateStruct.energyStruct.energy -= EnergyForActivateSpeed;
                    prefab.moveStruct.speedStep = prefab.moveStruct.speedStep * 2;
                    time = 0.5f;

                    activate = true;
                }
            }
            else
            {
                _switch = true;


                if (this.prefab.unit.stateStruct.energyStruct.energy >= EnergyForActivateSpeed && !activate)
                {
                    this.prefab.unit.stateStruct.energyStruct.energy -= EnergyForActivateSpeed;
                    this.prefab.unit.moveStruct.speedStep = this.prefab.unit.moveStruct.speedStep * 2;
                    time = 0.5f;

                    activate = true;
                }
            }
        }
        else
        {
            activate = false;
            if (!prefab.stateStruct.isControling)
            {
                prefab.moveStruct.speedStep = prefab.moveStruct.speedStepSave;
            }
            else
            {
                this.prefab.unit.moveStruct.speedStep = this.prefab.unit.moveStruct.speedRunSave;
            }
        }
    }

    public bool IsAvalible()
    {
        return true;
    }

    public void Update()
    {
        if(activate)
        {
            time -= Time.deltaTime;

            if (_switch && !prefab.stateStruct.isControling)
            {
                _switch = !_switch;
                                
                prefab.moveStruct.speedStep = prefab.moveStruct.speedStep * 2;              
            }
            else if (!_switch && prefab.stateStruct.isControling)
            {
                _switch = !_switch;              
                this.prefab.unit.moveStruct.speedStep = this.prefab.unit.moveStruct.speedStep * 2;
                
            }

        

            if (!prefab.stateStruct.isControling)
            {
               
                if (time <= 0)
                {
                    if (prefab.stateStruct.energyStruct.energy < EnergyForActivateSpeed)
                    {
                        activate = false;
                        prefab.moveStruct.speedStep = prefab.moveStruct.speedStepSave;
                    }
                    else
                    {
                        prefab.stateStruct.energyStruct.energy -= EnergyForActivateSpeed;
                        time = 0.5f;
                    }
                }
            }
            else
            {
               
                    

                    if (time <= 0)
                {
                    if (this.prefab.unit.stateStruct.energyStruct.energy < EnergyForActivateSpeed)
                    {
                        activate = false;
                        this.prefab.unit.moveStruct.speedStep = this.prefab.unit.moveStruct.speedRunSave;
                    }
                    else
                    {
                        this.prefab.unit.stateStruct.energyStruct.energy -= EnergyForActivateSpeed;
                        time = 0.5f;
                    }
                }
            }
        }
        else
        {
            activate = false;

            if (!prefab.stateStruct.isControling)
            {
                prefab.moveStruct.speedStep = prefab.moveStruct.speedStepSave;
            }
            else
            {
                this.prefab.unit.moveStruct.speedStep = this.prefab.unit.moveStruct.speedRunSave;
            }
        }
   
    }


}