using UnityEngine;


public delegate void UpdateByteValue(byte value);
public delegate void UpdateFloatValue(float value);
public class UpdateSkills
{
    // Start is called before the first frame update
    public UpdateSkills(IUnit unit)
    {
        this.unit = unit;
        StartSkill(ref unit.updateStateFromSkill);
    }
    IUnit unit;

    void StartSkill(ref UpdateStateFromSkill _updateStateFromSkill)
    {
        _updateStateFromSkill.oldIsDoubleJump = false;
        _updateStateFromSkill.isDoubleJump = false;

        _updateStateFromSkill.oldPowerJump = unit.stateStruct.jumpPower;
        _updateStateFromSkill.powerJump = unit.stateStruct.jumpPower;


        _updateStateFromSkill.oldMaxEnergy = unit.stateStruct.energyStruct.energyFull;
        _updateStateFromSkill.maxEnergy =0;

        _updateStateFromSkill.oldSpeedUpdateEnergy = unit.stateStruct.energyStruct.energyUpdate;
        _updateStateFromSkill.speedUpdateEnergy = unit.stateStruct.energyStruct.energyUpdate;

        _updateStateFromSkill.oldMoreEnergyFromPoint = 0; 
        _updateStateFromSkill.moreEnergyFromPoint = 0; 


        _updateStateFromSkill.oldMaxHitPoint = unit.stateStruct.hitpointStruct.hitpointFull;
        _updateStateFromSkill.maxHitPoint = 0;

        _updateStateFromSkill.oldSpeedUpdateHitPoint = 0; 
        _updateStateFromSkill.speedUpdateHitPoint = 0; 

        _updateStateFromSkill.oldMoreHitPointFromPoint = 0; 
        _updateStateFromSkill.moreHitPointFromPoint = 0; 

        // добавить настройку в оружие для крит урона
        if (unit.weapone != null)
        {
            _updateStateFromSkill.oldMaxDamage = unit.weapone.poverShoot; 
            _updateStateFromSkill.maxDamage = unit.weapone.poverShoot; 
        }

        _updateStateFromSkill.oldMaxSpeadReload = unit.shotStruct.timeReloadFull;
        _updateStateFromSkill.maxSpeadReload = unit.shotStruct.timeReloadFull;

        _updateStateFromSkill.oldMaxCriticalDamage = 0;  
        _updateStateFromSkill.maxCriticalDamage = 0;  

        _updateStateFromSkill.oldMaxСhanceCriticalDamage = 0;  
        _updateStateFromSkill.maxСhanceCriticalDamage = 0;  

        _updateStateFromSkill.oldMegaPowerShoot = 0;  
        _updateStateFromSkill.MegaPowerShoot = 0;  

        // добавить значение уровня взлома в объекты
        _updateStateFromSkill.levelВreaking = 0;  
        _updateStateFromSkill.oldLevelВreaking = 0;  

        _updateStateFromSkill.levelCallDrone = 0;  
        _updateStateFromSkill.oldLevelCallDrone = 0;  

        _updateStateFromSkill.levelCallRobot = 0;  
        _updateStateFromSkill.oldLevelCallRobot = 0;  

        _updateStateFromSkill.levelCallSoldat = 0;  
        _updateStateFromSkill.oldLevelCallSoldat = 0;  

        _updateStateFromSkill.maxTimeForВreaking = 0;  
        _updateStateFromSkill.oldMaxTimeForВreaking = 0;  

        _updateStateFromSkill.InstantHack = false;  
        _updateStateFromSkill.oldInstantHack = false;  

        _updateStateFromSkill.oldMaxInstantHack = 0;  
        _updateStateFromSkill.maxInstantHack = 0;  

        _updateStateFromSkill.updateInstantHack = 0;  
        _updateStateFromSkill.oldUpdateInstantHack = 0;  

        _updateStateFromSkill.oldMoreAcceleration = false;
        _updateStateFromSkill.moreAcceleration = false;
 

        _updateStateFromSkill.oldLessEnergyForActivateSpeedStep = 3;  
        _updateStateFromSkill.lessEnergyForActivateSpeedStep = 3;  
    }


    // Update is called once per frame
    public void UpdateDataSkills(string name, float value)
    {

        if (name == "maxHitPoint")
        {        
            SetMaxHitPoint(value);
        }
        else if (name == "speedUpdateHitPoint")
        {
            SetSpeedUpdateHitPoint(value);
        }
        else if (name == "moreHitPointFromPoint")
        {
            SetMoreHitPointFromPoint(value);
        }
        else if (name == "maxDamage")
        {
            SetMaxDamage(value);
        }
        else if (name == "maxSpeadReload")
        {
            SetMaxSpeadReload(value);
        }
        else if (name == "maxCriticalDamage")
        {
            SetMaxCriticalDamage(value);

        }
        else if (name == "maxСhanceCriticalDamage")
        {
            SetMaxСhanceCriticalDamage(value);
        }
        else if (name == "MegaPowerShoot")
        {
            SetMegaPowerShoot(value);
        }
        else if (name == "powerJump")
        {
            SetPowerJump(value);
        }
        else if (name == "isDoubleJump")
        {
            SetIsDoubleJump(value > 0);
        }
        else if (name == "levelВreaking")
        {
            SetLevelВreaking(value);
        }
        else if (name == "maxTimeForВreaking")
        {
            SetMaxTimeForВreaking(value);
        }
        else if (name == "levelCallDrone")
        {
            SetLevelCallDrone(value);
        }
        else if (name == "InstantHack")
        {
            SetInstantHack(value > 0);
          
        }
        else if (name == "levelCallRobot")
        {
            SetLevelCallRobot(value);
           
        }
        else if (name == "maxInstantHack")
        {
           
            SetMaxInstantHack((byte)value);
        }
        else if (name == "updateInstantHack")
        {
            SetUpdateInstantHack(value);
            
        }
        else if (name == "levelCallSoldat")
        {
            SetLevelCallSoldat(value);
           
        }
        else if (name == "moreAcceleration")
        {
            SetMoreAcceleration(value > 0);
        }
        else if (name == "lessEnergyForActivateSpeedStep")
        {
            SetLessEnergyForActivateSpeedStep(value);
        }
        else if (name == "maxEnergy")
        {
            SetMaxEnergy(value);
        }
        else if (name == "speedUpdateEnergy")
        {
            SetSpeedUpdateEnergy(value);
        }
        else if (name == "moreEnergyFromPoint")
        {
            SetMoreEnergyFromPoint(value);
        }
    }

    public UpdateByteValue UpdateInstantHackMaxCount;
    public UpdateByteValue UpdateInstantHackCount;
    public UpdateFloatValue UpdateInstantHackSpeed;
    /// <summary>
    /// Copy normal dates
    /// </summary>
    /// <param name="_updateStateFromSkill"> In </param>
    /// <param name="_updateStateFromSkill2"> From </param>
    #region set 
    //
    private void SetMaxHitPoint(float value)
    {
        unit.updateStateFromSkill.maxHitPoint = value;
        unit.stateStruct.hitpointStruct.hitpointFull = unit.updateStateFromSkill.oldMaxHitPoint + unit.updateStateFromSkill.maxHitPoint;
    }
    private void SetSpeedUpdateHitPoint(float value)
    {
        unit.updateStateFromSkill.speedUpdateHitPoint = value;
    }
    private void SetMaxDamage(float value)
    {
        if (unit.weapone != null)
        {
            unit.weapone.UpdateDamageBulet(value);

        }
    }
    private void SetMaxSpeadReload(float value)
    {
        unit.updateStateFromSkill.maxSpeadReload = value;
        if (unit.updateStateFromSkill.maxSpeadReload != unit.updateStateFromSkill.oldMaxSpeadReload)
        {
            unit.shotStruct.timeReloadFull = unit.updateStateFromSkill.oldMaxSpeadReload +
                unit.updateStateFromSkill.oldMaxSpeadReload / 100 * value;
        }
        else
        {
            unit.shotStruct.timeReloadFull = value;
        }
    }
    private void SetMaxCriticalDamage(float value)
    {
        unit.updateStateFromSkill.maxCriticalDamage = value;
        if (unit.weapone != null)
        {
            unit.weapone.maxCriticalDamage = value; 
        }
    }
    private void SetMaxСhanceCriticalDamage(float value)
    {
        unit.updateStateFromSkill.maxСhanceCriticalDamage = value;
        if (unit.weapone != null)
        {
            unit.weapone.maxСhanceCriticalDamage = value;
        }
    }
    private void SetMegaPowerShoot(float value)
    {
        unit.updateStateFromSkill.MegaPowerShoot = value;
        if (unit.weapone != null)
        {
            unit.weapone.MegaPowerShoot = value;
        }
    }
    private void SetPowerJump(float value)
    {
        unit.updateStateFromSkill.powerJump = value;
        if (unit.updateStateFromSkill.powerJump != unit.updateStateFromSkill.oldPowerJump)
        {

            unit.stateStruct.jumpPower = unit.updateStateFromSkill.oldPowerJump +
                unit.updateStateFromSkill.oldPowerJump / 100 * value;
        }
        else
        {
            unit.stateStruct.jumpPower = value;
        }
    }
    private void SetIsDoubleJump(bool value)
    {
        unit.updateStateFromSkill.isDoubleJump = value;
        unit.stateStruct.isDoubleJump = value ;
    }
    private void SetMoreHitPointFromPoint(float value)
    {
        unit.updateStateFromSkill.moreHitPointFromPoint = value;
    }
    private void SetLevelВreaking(float value)
    {
       unit.updateStateFromSkill.levelВreaking = value;
    }
    private void SetMaxTimeForВreaking(float value)
    {
        unit.updateStateFromSkill.maxTimeForВreaking = value;
        if(unit.iconOnMap == IconPack.Player)
        {
            unit.SendMessageInMaster(TypeMessage.BreakingUpdateTime);
        }
    }
    private void SetLevelCallDrone(float value)
    {
        unit.updateStateFromSkill.levelCallDrone = value;
        if (unit.iconOnMap == IconPack.Player)
        {
            unit.SendMessageInMaster(TypeMessage.UpdateLevelCallDrone);
        }
    }
    private void SetMoreAcceleration(bool value)
    {
       unit.updateStateFromSkill.moreAcceleration = value;
        if (unit.iconOnMap == IconPack.Player)
        {
            unit.SendMessageInMaster(TypeMessage.ActivateAcceleration);
        }
    }
    private void SetLessEnergyForActivateSpeedStep(float value)
    {
       unit.updateStateFromSkill.lessEnergyForActivateSpeedStep = value;
    }
    private void SetMaxEnergy(float value)
    {
        unit.updateStateFromSkill.maxEnergy = value;
        unit.stateStruct.energyStruct.energyFull = unit.updateStateFromSkill.oldMaxEnergy + unit.updateStateFromSkill.maxEnergy;
    }
    private void SetSpeedUpdateEnergy(float value)
    {
        unit.updateStateFromSkill.speedUpdateEnergy = value;
    }
    private void SetMoreEnergyFromPoint(float value)
    {
        unit.updateStateFromSkill.moreEnergyFromPoint = value;
    }

    private void SetLevelCallRobot(float value)
    {
        unit.updateStateFromSkill.levelCallRobot = value;
        if (unit.iconOnMap == IconPack.Player)
        {
             unit.SendMessageInMaster(TypeMessage.UpdateLevelCallRobot);
        }
    }
    private void SetLevelCallSoldat(float value)
    {
        unit.updateStateFromSkill.levelCallSoldat = value;
        if (unit.iconOnMap == IconPack.Player)
        {
            unit.SendMessageInMaster(TypeMessage.UpdateLevelCallSoldat);
        }
    }
    private void SetInstantHack(bool value)
    {
        unit.updateStateFromSkill.InstantHack = value;
        if (unit.iconOnMap == IconPack.Player)
        {
            if (value)
            {
                unit.SendMessageInMaster(TypeMessage.ActivateInstantHack);
            }
            else
            {
                unit.SendMessageInMaster(TypeMessage.DisableInstantHack);
            }
        }
    }
    private void SetMaxInstantHack(byte value)
    {
        unit.updateStateFromSkill.maxInstantHack = value;
        if (unit.iconOnMap == IconPack.Player)
        {
            UpdateInstantHackMaxCount(value);
            unit.SendMessageInMaster(TypeMessage.UpdateCountInstantHack);         
        }
    }
    private void SetUpdateInstantHack(float value)
    {
        unit.updateStateFromSkill.updateInstantHack = value;
        if (unit.iconOnMap == IconPack.Player)
        {
            UpdateInstantHackSpeed(value);
            unit.SendMessageInMaster(TypeMessage.UpdateSpesdUpdateInstantHack);
        }
    }
    #endregion 

    /// <summary>
    ///  когда вселяешся в нового юнита , то использовать из _updateStateFromSkill обычные переменные.
    /// </summary>
    /// <param name="_updateStateFromSkill"></param>
    public void UpdateAllData(ref UpdateStateFromSkill _updateStateFromSkill)
    {
        SetMaxHitPoint(_updateStateFromSkill.maxHitPoint);
        SetMoreHitPointFromPoint(_updateStateFromSkill.moreHitPointFromPoint);
        SetSpeedUpdateHitPoint(_updateStateFromSkill.speedUpdateHitPoint);
        SetMaxDamage(_updateStateFromSkill.maxDamage);
        SetMaxSpeadReload(_updateStateFromSkill.maxSpeadReload);
        SetMaxCriticalDamage(_updateStateFromSkill.maxCriticalDamage);
        SetMaxСhanceCriticalDamage(_updateStateFromSkill.maxСhanceCriticalDamage);
        SetMegaPowerShoot(_updateStateFromSkill.MegaPowerShoot);
        SetPowerJump(_updateStateFromSkill.powerJump);
        SetIsDoubleJump(_updateStateFromSkill.isDoubleJump);
        SetLevelВreaking(_updateStateFromSkill.levelВreaking);
        SetMaxTimeForВreaking(_updateStateFromSkill.maxTimeForВreaking);
        SetMoreAcceleration(_updateStateFromSkill.moreAcceleration);
        SetLessEnergyForActivateSpeedStep(_updateStateFromSkill.lessEnergyForActivateSpeedStep);
        SetMaxEnergy(_updateStateFromSkill.maxEnergy);
        SetSpeedUpdateEnergy(_updateStateFromSkill.speedUpdateEnergy);
        SetMoreEnergyFromPoint(_updateStateFromSkill.moreEnergyFromPoint);
        SetLevelCallDrone(_updateStateFromSkill.levelCallDrone);
        SetLevelCallRobot(_updateStateFromSkill.levelCallRobot);
        SetLevelCallSoldat(_updateStateFromSkill.levelCallSoldat);
        SetInstantHack(_updateStateFromSkill.InstantHack);
        SetMaxInstantHack(_updateStateFromSkill.maxInstantHack);
        SetUpdateInstantHack(_updateStateFromSkill.updateInstantHack);
    }
    public void SetOldAllData()
    {
        SetMaxHitPoint(0);
        SetMoreHitPointFromPoint(unit.updateStateFromSkill.oldMoreHitPointFromPoint);
        SetSpeedUpdateHitPoint(unit.updateStateFromSkill.oldSpeedUpdateHitPoint);
        SetMaxDamage(unit.updateStateFromSkill.oldMaxDamage);
        SetMaxSpeadReload(unit.updateStateFromSkill.oldMaxSpeadReload);
        SetMaxCriticalDamage(unit.updateStateFromSkill.oldMaxCriticalDamage);
        SetMaxСhanceCriticalDamage(unit.updateStateFromSkill.oldMaxСhanceCriticalDamage);
        SetMegaPowerShoot(unit.updateStateFromSkill.oldMegaPowerShoot);
        SetPowerJump(unit.updateStateFromSkill.oldPowerJump);
        SetIsDoubleJump(unit.updateStateFromSkill.oldIsDoubleJump);
        SetLevelВreaking(unit.updateStateFromSkill.oldLevelВreaking);
        SetMaxTimeForВreaking(unit.updateStateFromSkill.oldMaxTimeForВreaking);
        SetMoreAcceleration(unit.updateStateFromSkill.oldMoreAcceleration);
        SetLessEnergyForActivateSpeedStep(unit.updateStateFromSkill.oldLessEnergyForActivateSpeedStep);
        SetMaxEnergy(0);
        SetSpeedUpdateEnergy(unit.updateStateFromSkill.oldSpeedUpdateEnergy);
        SetMoreEnergyFromPoint(unit.updateStateFromSkill.oldMoreEnergyFromPoint);
        SetLevelCallDrone(unit.updateStateFromSkill.oldLevelCallDrone);
        SetLevelCallRobot(unit.updateStateFromSkill.oldLevelCallRobot);
        SetLevelCallSoldat(unit.updateStateFromSkill.oldLevelCallSoldat);
        SetInstantHack(unit.updateStateFromSkill.oldInstantHack);
        SetMaxInstantHack(unit.updateStateFromSkill.oldMaxInstantHack);
        SetUpdateInstantHack(unit.updateStateFromSkill.oldUpdateInstantHack);
    }
}
