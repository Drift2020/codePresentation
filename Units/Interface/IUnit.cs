using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Music;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public struct MoveStruct
{

    public Vector2 position;
    public Vector2 moveVelocity;
    public Vector2 moveVelocityTemp;
    [Space]
    public bool FlipX;
    public bool FlipY;
    [Space]
    // you sould move
    public bool isMove;
    [Space]
    public float speedStep;
    // don't save, fix this
    public float speedStepSave;
    // don't save, fix this
    public float speedRunSave;
    [Space]
    public LayerMask platformMask;
    [Space]
    public RaycastHit2D raycastHit2D;
    public RaycastHit2D raycastHit2DForJump;
}
/// <summary>
/// Структура для получения данных из управляющего контроллера(клавиатура, тачпад и тд)
///
/// </summary>
[System.Serializable]
public struct ButtonStruct
{
    public bool isAction;
    public bool isActionHold;
    [Space]
    public bool isJump;
    public bool isJumpOff;
    [Space]
    public bool isShot;
    [Space]
    public bool isPunch;
    [Space]
    public bool isSkill;
    public bool isSkillOff;
    [Space]
    // can you move?
    public bool isMove;
    public Vector2 moveInput;
}
[System.Serializable]
public struct ShotStruct
{
    public bool shot;
    public float timeReload;
    public float timeReloadFull;
    public float speadSotTime;
    public float pointShotX;
}
/// <summary>
///-----------------------------------------
/// структура для обозначения базового сотояния юнита
/// </summary>
/// 
[System.Serializable]
public struct StateStruct
{
    public EnergyStruct energyStruct;
    public HitpointStruct hitpointStruct;
    public UseStruct useStruct;
    public string noJumpingTag;
    public int score;
    [Header("Ground")]
    public bool isGround;
    public bool isGroundTemp;
    /// <summary>
    /// если ты в колайдере платформы при падении, то true
    /// </summary>
    public bool isGroundDownJump;
    [Header("Controls and Вreaking")]
    public bool isStopAnyControlAndCheck;
    public bool isControling;
    public float costTakeControl;
    public float levelВreaking;
    [Header("Uses")]
    public bool isUseObjectCollide;
    public bool isStairs;
    public bool isDead;

   [Header("Jump")]
    public bool isJumpDown;
    public bool isDoubleJump;
    public float jumpPower;
    public float maxJump;
    public float gravityScale;

    [Header("Reserve")]
    public float reserveTimeFull;
    public float reserveTime;
    public bool  isReserveSee;
    public float waitUpdateReserveTimeFull;
    public float waitUpdateReserveTime;

}
[System.Serializable]
public struct EnergyStruct
{
    public float energy;
    public float energyFull;
    public float energyUpdate;
}
[System.Serializable]
public struct HitpointStruct
{
    public float hitpointFull;
    public float hitpoint;
    public float ValuehitpointUpdate;
}
/// <summary>
/// Структура для обозначения есть ли взаимодействие
/// </summary>
[System.Serializable]
public struct UseStruct
{
    public bool isMoveWhenUse;
    public bool isUseState;
}
/// <summary>
/// Структура для дебаг отрисовки
/// </summary>
[System.Serializable]
public struct DebugStruct
{
   
    public float sizeLine;
}
/// <summary>
/// Структура для игнорирования и рейкастов
/// </summary>
[System.Serializable]
public struct MaskStruct
{
    public int unitMaskIgnore;
    public int platformMaskIgnore;
    public int stairsMaskIgnoge;
}

//распределить для улучшения
public struct UpdateStateFromSkill
{
    public bool oldIsDoubleJump;
    public bool isDoubleJump;
    [Space]
    public float oldPowerJump;
    public float powerJump;
    [Space]
    public float SkillUpdateoldPowerJump;
    public float oldSkillUpdatepowerJump;
    [Space]
    public float oldMaxEnergy;
    public float maxEnergy;
    [Space]
    public float oldSpeedUpdateEnergy;
    public float speedUpdateEnergy;
    [Space]
    public float oldMoreEnergyFromPoint;
    public float moreEnergyFromPoint;
    [Space]
    public float oldMaxHitPoint;
    public float maxHitPoint;
    [Space]
    public float oldSpeedUpdateHitPoint;
    public float speedUpdateHitPoint;
    [Space]
    public float oldMoreHitPointFromPoint;
    public float moreHitPointFromPoint;
    [Space]
    // добавить настройку в орижие для крит урона
    public float oldMaxDamage;
    public float maxDamage;
    [Space]
    public float oldMaxSpeadReload;
    public float maxSpeadReload;
    [Space]
    public float oldMaxCriticalDamage;
    public float maxCriticalDamage;
    [Space]
    public float oldMaxСhanceCriticalDamage;
    public float maxСhanceCriticalDamage;
    [Space]
    public float oldMegaPowerShoot;
    public float MegaPowerShoot;
    [Space]
    // добавить значение уровня взлома в объекты
    public float levelВreaking;
    public float oldLevelВreaking;
    [Space]
    public float levelCallDrone;
    public float oldLevelCallDrone;
    [Space]
    public float levelCallRobot;
    public float oldLevelCallRobot;
    [Space]
    public float levelCallSoldat;
    public float oldLevelCallSoldat;
    [Space]
    public float maxTimeForВreaking;
    public float oldMaxTimeForВreaking;
    [Space]
    public bool InstantHack;
    public bool oldInstantHack;
    [Space]
    public byte oldMaxInstantHack;
    public byte maxInstantHack;
    [Space]
    public float updateInstantHack;
    public float oldUpdateInstantHack;
    [Space]
    public bool oldMoreAcceleration;
    public bool moreAcceleration;
    [Space]
    public float oldLessEnergyForActivateSpeedStep;
    public float lessEnergyForActivateSpeedStep;
}
  
public interface IUnityVaribels
{
    Animator animator { get; set; }
    Rigidbody2D rigidbody2D { get; set; }
    SpriteRenderer spriteRenderer { get; set; }
    SoundPlayer soundPlayer { get; set; }
    CapsuleCollider2D capsuleCollider2D { get; set; }
    TextSwitch TextElement { get; set; }

}
/// <summary>
/// Базовый интерфейс для юнитов
/// </summary>
public interface IUnit : IObserverSave, ISubjectDeadUnit, ITrigers, IUnityVaribels, IIconOnMiniMap
{

    IWeapone weapone { get; set; }
    IControl control { get; set; }
    ref ButtonStruct buttonStruct { get; }
    ref MoveStruct moveStruct { get;  }
    ref ShotStruct shotStruct { get; }
    ref StateStruct  stateStruct { get; }
    ref DebugStruct debugStruct { get; }
    ref MaskStruct maskStruct { get;  }

    ref UpdateStateFromSkill updateStateFromSkill { get; }
    UpdateSkills UpdateSkills { get; set; }
    void Dead();

    void Load(EnemySaveData unit);
    EnemySaveData Save();
  
    void TakeDamage(float damage);
    void TakeEnergy(float energy);
    void TakeHitpoint(float hitpoint);

    void UpdateEnergy();
    void UpdateHitpoint();


    /// <summary>
    /// клас хранения окна для разговора
    /// </summary>
    GameObject TextBox { get; set; }
    void ShowTextBox(string text);
    void HideTextBox();

    void SetControling(bool controling);

    void UpgrateSkillDate(string name, float value);


    Action<TypeMessage> SendMessageInMaster { get; set; }
    /// <summary>
    /// вызывает иконку взаимодействия InfoUseBox
    /// </summary>
    Action <TypeMessage> ObjectIsAvalible{ get; set; }
    /// <summary>
    /// прячет иконку взаимодействия InfoUseBox
    /// </summary>
    Action<TypeMessage>  ObjectIsUnavalible { get; set; }

    // ISoundControl

    #region skills

    #endregion
}
