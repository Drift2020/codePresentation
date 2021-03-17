using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Music;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour, IUnit
{
    #region varibel's interface

    // камера для слежения за активной единицей
    public MasterCamera _my_camera;
    [SerializeField]
    private List<GameObject> GOinTrigger;
    public List<GameObject> EntryInTrigger { get { return GOinTrigger; } set { GOinTrigger = value; } }

    List<IObserverDeadUnit> _observelDied = new List<IObserverDeadUnit>();
    UpdateSkills _updateSkills;
    public UpdateSkills UpdateSkills { get { return _updateSkills; } set { _updateSkills = value; } }


    //должно работать при нажатии на кнопку на клавиатуре
    [SerializeField]
    SpecialSkills _specialSkill;
    public SpecialSkills SpecialSkill { get { return _specialSkill; } set { _specialSkill = value; } }


    public IUnit unit;
    public IWeapone weapone { get; set; }
    public IControl control { get; set; }

   
    #region struct
    
    public IconPack iconOnMap { get; set; }
    public ref ButtonStruct buttonStruct { get { return ref _buttonStruct; } }
    [Header("Struct")]
    [SerializeField]
    private ButtonStruct _buttonStruct;
    public ref MoveStruct moveStruct { get { return ref _moveStruct; } }
    [SerializeField]
    private MoveStruct _moveStruct;
    public ref ShotStruct shotStruct { get { return ref _shotStruct; }  }
    [SerializeField]
    private ShotStruct _shotStruct;
    public ref StateStruct stateStruct { get { return ref _stateStruct; } }
    [SerializeField]
    private StateStruct _stateStruct;
    public ref DebugStruct debugStruct { get { return ref _debugStruct; }}
    [SerializeField]
    private DebugStruct _debugStruct;
    public ref MaskStruct maskStruct { get { return ref _maskStruct; } }
    [SerializeField]
    private MaskStruct _maskStruct;
    [SerializeField]
    UpdateStateFromSkill _updateStateFromSkill;
    public ref UpdateStateFromSkill updateStateFromSkill { get { return ref _updateStateFromSkill; } }
    #endregion

    #region Class
  
    public Animator animator { get { return _animator; } set { _animator = value; } }
    [Header("Class")]
    [SerializeField]
    private Animator _animator;
    public Rigidbody2D rigidbody2D { get { return _rigidbody2D; } set { _rigidbody2D = value; } }
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    public SpriteRenderer spriteRenderer { get { return _spriteRenderer; } set { _spriteRenderer = value; } }
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public SoundPlayer soundPlayer { get { return _soundPlayer; } set { _soundPlayer = value; } }
    [SerializeField]
    private SoundPlayer _soundPlayer;
    public CapsuleCollider2D capsuleCollider2D { get { return _capsuleCollider2D; } set { _capsuleCollider2D = value; } }
    [SerializeField]
    private CapsuleCollider2D _capsuleCollider2D;
    [SerializeField]
    TextSwitch textElement;
    public TextSwitch TextElement { get { return textElement; } set { textElement = value; } }


    [SerializeField]
    GameObject InstantHackButton;

    [SerializeField]
    InstantHackPanel instantHackPanel;

    #endregion
    [Header("Varibel")]
    [SerializeField]
    bool inEnergyZone;
    [SerializeField]
    bool inNoEnergyZone;
    [SerializeField]
    float timeHitPointUpdate;

    #endregion

    #region Save/load
    public void Load(EnemySaveData unit)
    {
        // применять position в коде игрока
        transform.position = _moveStruct.position = unit.Position;
        _moveStruct.moveVelocity = unit.Direction;
        _stateStruct.hitpointStruct.hitpoint = unit.hitpoint;
        _stateStruct.hitpointStruct.hitpointFull = unit.hitpointFull;
        _spriteRenderer.enabled = true;
    }

    public EnemySaveData Save()
    {
        return new EnemySaveData(new Vector2(transform.position.x, transform.position.y), _moveStruct.moveVelocity, this.GetType().ToString(), _stateStruct.hitpointStruct.hitpoint, _stateStruct.hitpointStruct.hitpointFull);
    }
    public void UpdateSave(FileStream fs)
    {
        BinaryFormatter bf = new BinaryFormatter();

        EnemySaveData temp = new EnemySaveData((Vector2)transform.position, _moveStruct.moveVelocity, this.GetType().ToString(), _stateStruct.hitpointStruct.hitpoint, _stateStruct.hitpointStruct.hitpointFull);
        bf.Serialize(fs, temp);
    }
    #endregion

    #region Unity
    void Start()
    {
        iconOnMap = IconPack.Player;
        _stateStruct.isControling = false;
        GOinTrigger = new List<GameObject>();
        control = new Multicontrol();
        rigidbody2D = GetComponent<Rigidbody2D>();
        _stateStruct.hitpointStruct.hitpoint = 3f;
        _stateStruct.hitpointStruct.hitpointFull = 3f;

        SwitchGUI(gameObject);
        _stateStruct.energyStruct.energyFull = 4f;
        _stateStruct.energyStruct.energy = 4f;
      


    }
    void Awake()
    {
        iconOnMap = IconPack.Player;
        _stateStruct.hitpointStruct.hitpoint = 3f;
        _stateStruct.hitpointStruct.hitpointFull = 3f;
        _stateStruct.energyStruct.energyFull = 4f;
        _stateStruct.energyStruct.energy = 4f;
        _stateStruct.isControling = false;

        _updateSkills = new UpdateSkills(this);
        _specialSkill.unit = this;
    }

    void Update()
    {
        UnitIsDead();
        if (_stateStruct.hitpointStruct.hitpoint > 0)
        {
            if (!_stateStruct.isStopAnyControlAndCheck)
            {
                //управление выбраной единицей
                if (unit != null)
                {
                    control.Move(ref unit.buttonStruct, ref unit.moveStruct);
                    ActionWhenUnitControl();
                }
                else
                {
                    UpdateEnergy();
                    UpdateHitpoint();
                    control.Move(ref _buttonStruct, ref _moveStruct);

                    if (_buttonStruct.moveInput.x != 0 || _buttonStruct.moveInput.y != 0)
                    {
                        _stateStruct.useStruct.isMoveWhenUse = true;
                    }
                    _moveStruct.moveVelocity = _buttonStruct.moveInput * _moveStruct.speedStep;
                    Actions();
                }
            }
            MoveWhenUse();
        }
      
    }
    void FixedUpdate()
    {

        //изменение игрока когда он не в юните
        if (_stateStruct.hitpointStruct.hitpoint > 0 && unit == null)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + _moveStruct.moveVelocity * Time.fixedDeltaTime);
        }

    }
    #endregion

    void CheckUseObject()
    {
        int lenght = GOinTrigger.Count;
        for (int i = 0; i < lenght; i++)
        {
            if (GOinTrigger[i].GetComponent<ISubjectObject>() != null || GOinTrigger[i].GetComponent<IUnit>() != null)
            {
                ObjectIsAvalible(TypeMessage.IsUseAvalible);
                return;
            }
        }
        ObjectIsUnavalible(TypeMessage.IsUseUnavalible);
    }
    public Action<GameObject> SwitchGUI;
    public Action<TypeMessage> ObjectIsAvalible { get; set; }
    public Action<TypeMessage> ObjectIsUnavalible { get; set; }


    #region dialoge with master

    public Action<TypeMessage> SendMessageInMaster { get; set; }

    #endregion

    #region use
    private void MoveWhenUse()
    {
        if (_stateStruct.useStruct.isMoveWhenUse && _stateStruct.useStruct.isUseState)
        {
            _stateStruct.useStruct.isUseState = false;
            int lenght = GOinTrigger.Count;
            for(int i = 0; i < lenght; i++)
            {
                var temp = GOinTrigger[i].GetComponent<ISubjectObject>();
                if (temp != null)
                {
                    temp.NotifyNoUse();
                    break;
                }
            }
        }
    }
    void Actions()
    {
        if (_buttonStruct.isAction)
        {
            _buttonStruct.isAction = false;
            if (unit == null)
            {
                int lenght1 = GOinTrigger.Count;
                for(int i = 0; i < lenght1; i++)
                {
                    try

                    {
                        if (GOinTrigger[i].GetComponent<IUnit>() != null && _stateStruct.energyStruct.energy > GOinTrigger[i].GetComponent<IUnit>().stateStruct.costTakeControl)
                        {
                            TakeControlUints(GOinTrigger[i]);
                            return;

                        }
                    }
                    catch { };
                }
            }
            int lenght = GOinTrigger.Count;
            for (int i = 0; i < lenght; i++)
            {

                var temp = GOinTrigger[i].GetComponent<ISubjectObject>();
                if (temp != null)
                {
                    _stateStruct.useStruct.isUseState = true;

                    if (_buttonStruct.moveInput.x == 0 && _buttonStruct.moveInput.y == 0)
                    {
                        _stateStruct.useStruct.isMoveWhenUse = false;
                    }
                    temp.NotifyUse(_updateStateFromSkill.levelВreaking);
                    return;
                }
            }

        }
        ActivateSkill();
    }
    void ActivateSkill()
    {
        if((_buttonStruct.isSkill && _buttonStruct.isSkillOff )
            || (unit!=null && unit.buttonStruct.isSkill && unit.buttonStruct.isSkillOff))
        {
            if (!InstantHackButton.active)
            {
                _specialSkill.Use();
            }
            else
            {
                instantHackPanel.UseInstantHack();
            }

            if(unit!=null)
            {
                unit.buttonStruct.isSkillOff = false;
            }
            _buttonStruct.isSkillOff = false;
        }
    }

    [SerializeField]
    float timeForControl;
    [SerializeField]
    float FulltimeForControl;
    [SerializeField]
    float StefForControl;
    [SerializeField]
    float isWorkTimeForControl;

    bool TimerGiveControlUints()
    {      
        timeForControl -= Time.deltaTime * StefForControl;
        if (timeForControl > 0)
        {
            return false;
        }
        else
        {
            timeForControl = FulltimeForControl;
            return true;
        }
    }

    bool NullActiveObjects()
    {
        return (!unit.stateStruct.isUseObjectCollide && unit.buttonStruct.isActionHold && !unit.stateStruct.isStairs);
    }
    void ActionWhenUnitControl()
    {
        ActivateSkill();
        if (NullActiveObjects() && TimerGiveControlUints())
        {         
            GiveControlUints();
        }
        else if (!NullActiveObjects())
        {
                timeForControl = FulltimeForControl;         
        }
       
    }



    #endregion

    #region functions

    #region TextBox
    public GameObject TextBox { get { return _textBox; } set { _textBox = value; } }
    [SerializeField]
    private GameObject _textBox;
    public void ShowTextBox(string text)
    {

    }
    public void HideTextBox()
    {

    }
    #endregion

    #region Control
    public void SetControling(bool controling)
    {
        _stateStruct.isControling = controling;

    }
    void TakeControlUints(GameObject item)
    {
       
        _stateStruct.energyStruct.energy -= item.GetComponent<IUnit>().stateStruct.costTakeControl;

        unit = item.GetComponent<IUnit>();
        unit.SetControling(true);
        unit.stateStruct.isStopAnyControlAndCheck= false;
        unit.UpdateSkills.UpdateAllData(ref _updateStateFromSkill);


        _stateStruct.isControling = true;
        _my_camera.target = item.transform;
        _spriteRenderer.enabled = false;
        SwitchGUI(item);

    }
    void GiveControlUints()
    {
        unit.SetControling(false);
        unit.UpdateSkills.SetOldAllData();
        unit = null;

        gameObject.transform.position = _my_camera.target.position;
        _stateStruct.isControling  = false;
        _my_camera.target = gameObject.transform;
        _spriteRenderer.enabled = true;
        SwitchGUI(gameObject);

    }
    #endregion

    #region update date
    public void UpdateEnergy()
    {
        if(inEnergyZone)
        {
            if (_stateStruct.energyStruct.energy < stateStruct.energyStruct.energyFull)
            {
                _stateStruct.energyStruct.energy += Time.deltaTime * _stateStruct.energyStruct.energyUpdate;
            }
            else if(_stateStruct.energyStruct.energy > stateStruct.energyStruct.energyFull)
            {
                _stateStruct.energyStruct.energy = stateStruct.energyStruct.energyFull;
            }
        }
        else if(inNoEnergyZone)
        {
            if (_stateStruct.energyStruct.energy > 0)
            {
                _stateStruct.energyStruct.energy -= Time.deltaTime * _stateStruct.energyStruct.energyUpdate;
                if(_stateStruct.energyStruct.energy < 0)
                {
                    _stateStruct.energyStruct.energy = 0;
                }
            }

        }
    }
    public void UpdateHitpoint()
    {

        if (_updateStateFromSkill.speedUpdateHitPoint > 0 &&
            _stateStruct.hitpointStruct.hitpoint < stateStruct.hitpointStruct.hitpointFull)
        {
            if (timeHitPointUpdate <= 0)
            {
                _stateStruct.hitpointStruct.hitpoint += _stateStruct.hitpointStruct.ValuehitpointUpdate;
                timeHitPointUpdate = _updateStateFromSkill.speedUpdateHitPoint;
            }
            timeHitPointUpdate -= Time.deltaTime;

        }
        else if (_stateStruct.hitpointStruct.hitpoint > stateStruct.hitpointStruct.hitpointFull)
        {
            _stateStruct.hitpointStruct.hitpoint = _stateStruct.hitpointStruct.hitpointFull;
        }
    }
    #endregion

    #region Take funk
    public void TakeDamage(float damage)
    {
        _stateStruct.hitpointStruct.hitpoint -= damage;
    }
    public void TakeEnergy(float energy)
    {
        _stateStruct.energyStruct.energy += energy;
    }
    /// <summary>
    /// Получение жизней от внешних источнеков
    /// улучшается навыком skillNameMoreHitPointsFromPoint
    /// </summary>
    /// <param name="hitpoint"></param>
    public void TakeHitpoint(float hitpoint)
    {
        _stateStruct.hitpointStruct.hitpoint += hitpoint + _updateStateFromSkill.moreHitPointFromPoint;
    }
    #endregion

    #endregion

    #region triggers
    void OnTriggerEnter2D(Collider2D other)
    {
        GOinTrigger.Add(other.gameObject);
        CheckUseObject();
        if (other.gameObject.tag == "EnergyZone")
        {
            inEnergyZone = true;
            _stateStruct.energyStruct.energyUpdate = other.gameObject.GetComponent<EnergyZone>().EnegryPower;
        }
        else if(other.gameObject.tag == "noEnergyZone" && !inEnergyZone)
        {
            inNoEnergyZone = true;
            _stateStruct.energyStruct.energyUpdate = other.gameObject.GetComponent<EnergyZone>().EnegryPower;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        var temp = other.GetComponent<ISubjectObject>();
        if (temp != null)
        {
            temp.NotifyNoUse();
        }

        GOinTrigger.Remove(other.gameObject);
        CheckUseObject();
        if (other.gameObject.tag == "EnergyZone")
        {
            inEnergyZone = false;          
        }
        if (other.gameObject.tag == "noEnergyZone")
        {
            inEnergyZone = false;
        }
    }

    #endregion
   
    #region Dead
    void UnitIsDead()
    {
        if (_stateStruct.isControling && (unit == null || unit.stateStruct.hitpointStruct.hitpoint <= 0))
        {
            GiveControlUints();
        }
        else if (_stateStruct.hitpointStruct.hitpoint <= 0)
        {
            Dead();
        }
    }
    public void Dead()
    {
        if (_spriteRenderer.enabled && _stateStruct.hitpointStruct.hitpoint <= 0)
        {
            for (int i = 0; i < _observelDied.Count; i++)
            {
                _observelDied[i].UpdateDead(gameObject, DeadCommand.playerDied);
            }
            _spriteRenderer.enabled = false;
        }
    }
    public void Attach(IObserverDeadUnit observer)
    {
        _observelDied.Add(observer);
    }
    public void Detach(IObserverDeadUnit observer)
    {
        _observelDied.Remove(observer);
    }

    /// <summary>
    /// изменить класс, добавить параметр типа, чтобы понимать кто умер
    /// </summary>
    public void NotifyDead(GameObject _object, DeadCommand deadCommand)
    {
        throw new NotImplementedException();
    }




    #endregion

    #region skill
    public void UpgrateSkillDate(string name, float value)
    {
        _updateSkills.UpdateDataSkills(name, value);
        if (unit != null)
        {
            unit.UpgrateSkillDate(name, value);            
        }
       
    }
    #endregion
}

