using Music;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;


[Serializable]
public class DroneCivilian : MonoBehaviour, IEnemy
{
    #region Datas
    #region Interfase
    IArtificialIntelligence _artificialIntelligence;
    public IArtificialIntelligence artificialIntelligence
    {
        get { return _artificialIntelligence; }
        set { _artificialIntelligence = value; }
    }

    IWeapone _weapone;
    public IWeapone weapone
    {
        get { return _weapone; }
        set { _weapone = value; }
    }

    IControl _control;
    public IControl control
    {
        get { return _control; }
        set { _control = value; }
    }

    [Header("For mini map")]
    [SerializeField]
    IconPack _iconOnMap;
    public IconPack iconOnMap
    {
        get { return _iconOnMap; }
        set { _iconOnMap = value; }
    }
    #endregion

    #region Struct
    [Header("Structs")]
    [SerializeField]
    private ButtonStruct _buttonStruct;
    public ref ButtonStruct buttonStruct { get { return ref _buttonStruct; } }

    [SerializeField]
    private MoveStruct _moveStruct;
    public ref MoveStruct moveStruct { get { return ref _moveStruct; } }

    [SerializeField]
    private ShotStruct _shotStruct;
    public ref ShotStruct shotStruct { get { return ref _shotStruct; } }

    [SerializeField]
    private StateStruct _stateStruct;
    public ref StateStruct stateStruct { get { return ref _stateStruct; } }

    [SerializeField]
    private DebugStruct _debugStruct;
    public ref DebugStruct debugStruct { get { return ref _debugStruct; } }

    [SerializeField]
    private MaskStruct _maskStruct;
    public ref MaskStruct maskStruct { get { return ref _maskStruct; } }

    [SerializeField]
    private UpdateStateFromSkill _updateStateFromSkill;
    public ref UpdateStateFromSkill updateStateFromSkill { get { return ref _updateStateFromSkill; } }
    #endregion

    #region Actions
    Action<TypeMessage> sendMessageInMaster;
    public Action<TypeMessage> SendMessageInMaster
    {
        get { return sendMessageInMaster; }
        set { sendMessageInMaster = value; }
    }

    Action<TypeMessage> objectIsAvalible;
    public Action<TypeMessage> ObjectIsAvalible
    {
        get { return objectIsAvalible; }
        set { objectIsAvalible = value; }
    }

    Action<TypeMessage> objectIsUnavalible;
    public Action<TypeMessage> ObjectIsUnavalible
    {
        get { return objectIsUnavalible; }
        set { objectIsUnavalible = value; }
    }
    #endregion

    #region Unity classes
    [Header("Unity classes")]
    [SerializeField]
    private Animator _animator;
    public Animator animator
    {
        get { return _animator; }
        set { _animator = value; }
    }

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer spriteRenderer
    {
        get { return _spriteRenderer; }
        set { _spriteRenderer = value; }
    }

    [SerializeField]
    private CapsuleCollider2D _capsuleCollider2D;
    public CapsuleCollider2D capsuleCollider2D
    {
        get { return _capsuleCollider2D; }
        set { _capsuleCollider2D = value; }
    }
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    public Rigidbody2D rigidbody2D
    {
        get { return _rigidbody2D; }
        set { _rigidbody2D = value; }
    }
    #endregion

    #region Classes
    [Header("User Functions")]
    [SerializeField]
    private SoundPlayer _soundPlayer;
    public SoundPlayer soundPlayer
    {
        get { return _soundPlayer; }
        set { _soundPlayer = value; }
    }

    [SerializeField]
    private TextSwitch _TextElement;
    public TextSwitch TextElement
    {
        get { return _TextElement; }
        set { _TextElement = value; }
    }


    private UpdateSkills _updateSkills;
    public UpdateSkills UpdateSkills
    {
        get { return _updateSkills; }
        set { _updateSkills = value; }
    }

    [SerializeField]
    private GameObject _TextBox;
    public GameObject TextBox
    {
        get { return _TextBox; }
        set { _TextBox = value; }
    }
    #endregion

    #region Lists

    [Header("Lists")]
    private List<GameObject> _entryInTrigger;
    public List<GameObject> EntryInTrigger
    {
        get { return _entryInTrigger; }
        set { _entryInTrigger = value; }
    }


    List<IObserverDeadUnit> _observelDied = new List<IObserverDeadUnit>();
    #endregion

    #region Var
    [SerializeField]
    float timeHitPointUpdate;
    #endregion

    
    #endregion

    #region Functions
    #region Dead functions
    public void Attach(IObserverDeadUnit observer)
    {
        _observelDied.Add(observer);
    }
    public void Detach(IObserverDeadUnit observer)
    {
        _observelDied.Remove(observer);
    }

    public void Dead()
    {
        if (_stateStruct.hitpointStruct.hitpoint <= 0)
        {
            _stateStruct.isDead = true;
            NotifyDead(gameObject, DeadCommand.kill);                 
        }
    }
    public void NotifyDead(GameObject _object, DeadCommand deadCommand)
    {
        for (int i = 0; i < _observelDied.Count; i++)
        {
            _observelDied[i].UpdateDead(_object, deadCommand);
        }
    }
    
    #endregion

    #region TextBox functions
    public void HideTextBox()
    {
        _TextBox.SetActive(false);
    }
    public void ShowTextBox(string text)
    {
        _TextElement.name = text;
        _TextElement.SwitchLanguage();
        _TextBox.SetActive(true);
    }
    #endregion

    #region Saves functions
    public void Load(EnemySaveData unit)
    {
        Debug.Log("Null functional");
    }
  
    public EnemySaveData Save()
    {
        EnemySaveData temp = new EnemySaveData((Vector2)transform.position, _moveStruct.moveVelocity, this.GetType().ToString(), _stateStruct.hitpointStruct.hitpoint, _stateStruct.hitpointStruct.hitpointFull);
        return temp;
    }
    #endregion

    #region Take functions
    public void TakeDamage(float damage)
    {
        _stateStruct.hitpointStruct.hitpoint -= damage;
    }

    public void TakeEnergy(float energy)
    {
        _stateStruct.energyStruct.energy += energy;
    }

    public void TakeHitpoint(float hitpoint)
    {
        _stateStruct.hitpointStruct.hitpoint += hitpoint + _updateStateFromSkill.moreHitPointFromPoint;
    }
    #endregion

    #region Use functions
    void IsUseObject()
    {
        bool isFalse = true;
        int lenght = _entryInTrigger.Count;
        for (int i = 0; i < lenght; i++)
        {
            if (_entryInTrigger[i].GetComponent<ISubjectObject>() != null || _entryInTrigger[i].tag == "Stairs")
            {
                _stateStruct.isUseObjectCollide = true;
                isFalse = false;
                break;
            }


        }
        if (isFalse)
        {
            _stateStruct.isUseObjectCollide = false;
        }
    }

    public void Use()
    {
        int lenght = _entryInTrigger.Count;
        for (int i = 0; i < lenght; i++)
        {        
            try
            {
                var temp = _entryInTrigger[i].GetComponent<ISubjectObject>();
                if (temp != null)
                {
                    temp.NotifyUse(_updateStateFromSkill.levelВreaking);
                    break;
                }
            }
            catch { }
        }
    }

    void CheckUseObject()
    {
        ISubjectObject temp = null;
        int lenght = _entryInTrigger.Count;
        for (int i = 0; i < lenght; i++)
        {

            temp = _entryInTrigger[i].GetComponent<ISubjectObject>();
            if (temp != null)
            {
                ObjectIsAvalible(TypeMessage.IsUseAvalible);
                break;
            }
        }
        if (temp == null)
        {
            ObjectIsUnavalible(TypeMessage.IsUseUnavalible);
        }
    }
    #endregion

    #region Move functions
    private void Move()
    {
        _moveStruct.moveVelocityTemp = new Vector2(_moveStruct.moveVelocity.x, _moveStruct.moveVelocity.y);
        if (_moveStruct.moveVelocityTemp.x != 0 || _moveStruct.moveVelocityTemp.y != 0 || _buttonStruct.isJump)
        {
            _stateStruct.useStruct.isMoveWhenUse = true;
        }


        //-----------------------------------------------------------
        if (_moveStruct.moveVelocityTemp.x != 0 || _moveStruct.moveVelocityTemp.y != 0)
        {

            _spriteRenderer.flipX = _moveStruct.FlipX;
            if (_stateStruct.isGround)
            {
                _animator.SetBool("Move", true);
                _soundPlayer.Play("Walk-fast", 0);
            }
        }
        else if (_stateStruct.isGround)
        {
            _animator.SetBool("Move", false);
            _spriteRenderer.flipX = _moveStruct.FlipX;
        }

        //-----------------------------------------------------------
       
        MoveWhenUse();
    }

    private void MoveWhenUse()
    {
        if (_stateStruct.useStruct.isMoveWhenUse && _stateStruct.useStruct.isUseState)
        {
            _stateStruct.useStruct.isUseState = false;
            int lenght = _entryInTrigger.Count;
            for (int i = 0; i < lenght; i++)
            {
                var temp = _entryInTrigger[i].GetComponent<ISubjectObject>();
                if (temp != null)
                {
                    temp.NotifyNoUse();
                    break;
                }
            }
        }
    }
    int conutJump =0;
    private void JumpUpOrDown()
    {
        if (_buttonStruct.isJump && _buttonStruct.isJumpOff &&
          _stateStruct.isGround && _rigidbody2D.velocity.y == 0 &&
          _buttonStruct.moveInput.y < -0.35f &&
          (_moveStruct.raycastHit2D.collider != null &&
          _moveStruct.raycastHit2D.collider.tag != stateStruct.noJumpingTag
          ||
          _moveStruct.raycastHit2DForJump.collider != null &&
          _moveStruct.raycastHit2DForJump.collider.tag != stateStruct.noJumpingTag))
        {
            _buttonStruct.isJumpOff = false;
            animator.SetBool("Jump", true);
            _soundPlayer.Stop(0);
            _soundPlayer.Play("Jump-start", 0); 
            _stateStruct.isJumpDown = true;
            _capsuleCollider2D.isTrigger = true;
            _animator.SetBool("Jump", false);
        }
        else if ((_buttonStruct.isJump && _buttonStruct.isJumpOff) &&


            (((_stateStruct.isDoubleJump && conutJump < _stateStruct.maxJump) || _stateStruct.isGround)
            && (_rigidbody2D.velocity.y == 0 || conutJump == 1))


            && (!_stateStruct.isJumpDown))
        {
            _rigidbody2D.velocity = new Vector2();
            conutJump++;
            _buttonStruct.isJumpOff = false;
            _animator.SetBool("Jump", true);
            _soundPlayer.Stop(0);
            _soundPlayer.Play("Jump-start", 0);
            _rigidbody2D.AddForce(Vector2.up * _stateStruct.jumpPower, ForceMode2D.Impulse);
            _animator.SetBool("Jump", false);

        }
    }
    private void SwitchJumpDown()
    {
        //включает колайдер если не нажат прыжок и есть земля
        if (!_stateStruct.isGroundDownJump)
        {
            capsuleCollider2D.isTrigger = false;
            _stateStruct.isJumpDown = false;
        }
    }
    private bool Is_Ground()
    {
        Vector3 temp = new Vector3(capsuleCollider2D.bounds.center.x,
            capsuleCollider2D.bounds.center.y - 0.01f - capsuleCollider2D.size.y / 2,
            capsuleCollider2D.bounds.center.z);

        _moveStruct.raycastHit2D = Physics2D.Raycast(temp, Vector2.down,
            _debugStruct.sizeLine, _moveStruct.platformMask);
        Color t = Color.yellow;
        Debug.DrawRay(temp, Vector2.down * _debugStruct.sizeLine, t);

        return (_moveStruct.raycastHit2D.collider != null);
    }
    private bool IsGroundJumpDown()
    {
        Vector3 temp = new Vector3(capsuleCollider2D.bounds.center.x,
            capsuleCollider2D.bounds.center.y + capsuleCollider2D.size.y / 2,
            capsuleCollider2D.bounds.center.z);

        _moveStruct.raycastHit2DForJump = Physics2D.Raycast(temp, Vector2.down,
             0.5f, _moveStruct.platformMask);
        Color t = Color.magenta;
        Debug.DrawRay(temp, Vector2.down * 0.5f, t);

        return (_moveStruct.raycastHit2DForJump.collider != null && _moveStruct.raycastHit2DForJump.collider.tag != _stateStruct.noJumpingTag);
    }
    private void CollisionPlatformDisable()
    {
        if (_rigidbody2D.velocity.y > 0 || (_moveStruct.raycastHit2DForJump.collider != null && _moveStruct.raycastHit2DForJump.collider.tag != _stateStruct.noJumpingTag))
        {
            capsuleCollider2D.isTrigger = true;
        }
        //а нужен ли этот код?
        else if (_stateStruct.isJumpDown &&
                ((_moveStruct.raycastHit2D.collider != null &&
                _moveStruct.raycastHit2D.collider.tag == stateStruct.noJumpingTag)
                ||
                (_moveStruct.raycastHit2DForJump.collider != null &&
                _moveStruct.raycastHit2DForJump.collider.tag == stateStruct.noJumpingTag)
                ||
                !_buttonStruct.isJump)
                )
        {
            SwitchJumpDown();

            //if (_stateStruct.isGroundTemp)
            //{
            //    capsuleCollider2D.isTrigger = false;
            //    _stateStruct.isJumpDown = false;
            //}
        }
        else
        {
            if (!_stateStruct.isStairs)
            {
                if (_stateStruct.isJumpDown)
                {
                    if (_moveStruct.raycastHit2D.collider != null && _moveStruct.raycastHit2D.collider.tag == stateStruct.noJumpingTag)
                    {
                        capsuleCollider2D.isTrigger = false;
                        _stateStruct.isJumpDown = false;
                    }
                }
                else
                {
                    capsuleCollider2D.isTrigger = false;
                    _stateStruct.isJumpDown = false;
                }
            }
        }
    }

    void FlightEnegry()
    {

        if (_moveStruct.moveVelocityTemp.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(); 
            _rigidbody2D.gravityScale = 0;
            capsuleCollider2D.isTrigger = true;
        }
        else
        {
            _rigidbody2D.gravityScale = 1;
        }
    }
    #endregion

    #region Weapone functions

    void TimerShoot()
    {
        if (_weapone != null)
        {
            if (_shotStruct.timeReload <= 0 && _stateStruct.energyStruct.energy - _weapone.energyCost >= 0 && _buttonStruct.isShot)
            {
                _stateStruct.energyStruct.energy -= _weapone.energyCost;

                if(_stateStruct.isControling)
                {
                    _weapone.poverShoot = 1;
                }
                else
                {
                    _weapone.poverShoot = 0;
                }

                _weapone.Shot(new Vector2(_spriteRenderer.flipX == true ? -1 : 1, 0),
                new Vector2(_spriteRenderer.flipX == true ? (-1f * _shotStruct.pointShotX) : (1f * _shotStruct.pointShotX), 0));
                _soundPlayer.PlayOneShoot("Shoot-gun", 1);
                _shotStruct.timeReload = _shotStruct.timeReloadFull;
            }
            if (_shotStruct.timeReload >= 0)
            {
                _shotStruct.timeReload -= Time.deltaTime * _shotStruct.speadSotTime;
            }
        }
    }

    #endregion

    #region Update dates functions
    public void UpdateEnergy()
    {
        if (_stateStruct.energyStruct.energy != _stateStruct.energyStruct.energyFull)
        {
            _stateStruct.energyStruct.energy += Time.deltaTime * _stateStruct.energyStruct.energyUpdate;

            if (_stateStruct.energyStruct.energy > _stateStruct.energyStruct.energyFull)
            {
                _stateStruct.energyStruct.energy = _stateStruct.energyStruct.energyFull;
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

    public void UpdateSave(FileStream fs)
    {
        Save();
    }
    #endregion

    #region Skill functions
    public void UpgrateSkillDate(string name, float value)
    {
        _updateSkills.UpdateDataSkills(name, value);
    }

    /// <summary>
    /// Teleport in direction X and Y
    /// in ground units it is Jump function
    /// </summary>
    private void Teleport()
    {

    }



    #endregion

    public void SetControling(bool controling)
    {
        _stateStruct.isControling = controling;
        if (!controling)
        {
            _moveStruct.speedStep = _moveStruct.speedStepSave;
        }
        else if (controling)
        {
            _moveStruct.speedStep = _moveStruct.speedRunSave;
        }
    }

    #region Unity functions
    void Start()
    {
        _entryInTrigger = new List<GameObject>();
        iconOnMap = IconPack.Drone;
        _maskStruct.unitMaskIgnore = gameObject.layer;
        _maskStruct.platformMaskIgnore = LayerMask.NameToLayer("FloorCollision");
        _maskStruct.platformMaskIgnore = LayerMask.NameToLayer("FloorCollision");
        _stateStruct.isControling = false;
      
        _stateStruct.score = 1;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _moveStruct.moveVelocity = new Vector2();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _soundPlayer = GetComponent<SoundPlayer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _moveStruct.speedStep = _moveStruct.speedStepSave = 0.8f;
        _moveStruct.speedRunSave = 1f;
        control = new ControlEnemyFlight(gameObject);
        _artificialIntelligence = new AISoldat(gameObject.transform, 3f,new BehaviorDrone(), this.GetType().ToString());
        _artificialIntelligence.textShow = ShowTextBox;
        _artificialIntelligence.textHide = HideTextBox;
        _artificialIntelligence.GetUnitTransform = (control as ControlEnemyFlight).GetEnemyTransform;
        _updateSkills = new UpdateSkills(this);

        weapone = GetComponent<SmartGun>();
        if (weapone != null)
        {
            weapone.unit = this;
            (weapone as SmartGun).GetEnemyTransform = (control as ControlEnemyFlight).GetEnemyTransform;
        }
    }
  
    void Update()
    {
        #region Is
        Dead();
        IsUseObject();
        UpdateEnergy();
        UpdateHitpoint();
        _stateStruct.isGroundDownJump = IsGroundJumpDown();
        _stateStruct.isGroundTemp = Is_Ground();

        if (_stateStruct.isJumpDown && _stateStruct.isGroundTemp)
        {
            _stateStruct.isGround = false;
        }
        else
        {
            if (!_stateStruct.isGround && _stateStruct.isGroundTemp)
            {
                conutJump = 0;
            }

            _stateStruct.isGround = _stateStruct.isGroundTemp;
        }
        #endregion

        #region   Use
        _animator.SetBool("Graund", _stateStruct.isGround);



        if (_buttonStruct.isPunch)
        {
            //добавить скилл
            _animator.SetBool("Skill", true);
            //  TimerShoot();
        }
        else if (!_buttonStruct.isPunch)
        {
            _animator.SetBool("Skill", false);

            if (_buttonStruct.isShot)
            {
                _animator.SetBool("Shoot", true);

            }
            else if (!_buttonStruct.isShot)
            {
                _animator.SetBool("Shoot", false);
                //  Reload();
            }
            TimerShoot();
        }

        Teleport();

        if (_buttonStruct.isAction)
        {
            Use();
            _buttonStruct.isAction = false;
            _stateStruct.useStruct.isUseState = true;
            _stateStruct.useStruct.isMoveWhenUse = false;
        }

        if (!_stateStruct.isStopAnyControlAndCheck)
        {
            if (!_stateStruct.isControling)
            {
                control.Move(ref _buttonStruct, ref _moveStruct);
                _artificialIntelligence.AnalyseFlags(this);
            }
            else
            {

                if (_buttonStruct.moveInput.x > 0)
                {
                    _moveStruct.FlipX = false;
                }
                else if (_buttonStruct.moveInput.x < 0)
                {
                    _moveStruct.FlipX = true;
                }
                CheckUseObject();
                _artificialIntelligence.ResetBoxAndTimer();
            }
        }
        #endregion

        #region Move


        _moveStruct.moveVelocity = _buttonStruct.moveInput * _moveStruct.speedStep;
        Move();
        JumpUpOrDown();
        CollisionPlatformDisable();

        if ((_moveStruct.moveVelocity.y == 0 && !_stateStruct.isStairs &&
           _moveStruct.moveVelocity.x == 0 && _stateStruct.isGround && !_buttonStruct.isJump) ||
           (_soundPlayer.Name == "Walk-fast" && !_stateStruct.isGround))
        {
            _soundPlayer.Stop(0);
        }


      
        #endregion

    }
    void FixedUpdate()
    {

        if (_stateStruct.isGround && _moveStruct.raycastHit2D.collider != null
            && _moveStruct.raycastHit2D.collider.tag == "Elevator")
        {
            transform.parent = _moveStruct.raycastHit2D.collider.transform;
        }
        else
        {
            transform.parent = null;
        }


        FlightEnegry();

        transform.position = new Vector2(transform.position.x + _moveStruct.moveVelocityTemp.x *
          _moveStruct.speedStep * Time.fixedDeltaTime, 
          transform.position.y + _moveStruct.moveVelocityTemp.y *
          _moveStruct.speedStep * Time.fixedDeltaTime);
        _moveStruct.position = transform.position;

    }
    #endregion
    #region Trigers
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Graund")
        {
            _entryInTrigger.Remove(other.gameObject);         
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {    
        if (other.tag != "Graund")
        {
            _entryInTrigger.Add(other.gameObject);         
        }
    }
    #endregion


    #endregion
}
