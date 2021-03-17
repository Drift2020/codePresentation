using Music;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;



[Serializable]
public class SoldatNormal : MonoBehaviour, IEnemy
{

    #region varibels

    #region Lists
    
    List<IObserverDeadUnit> _observelDied = new List<IObserverDeadUnit>();

    public List<GameObject> EntryInTrigger { get { return entryInTrigger; } set { entryInTrigger = value; } }
    [Header("Lists")]
    [SerializeField]
    public List<GameObject> entryInTrigger = new List<GameObject>();
    #endregion 

    #region Spetial dates
    bool my_stairsUp;
    bool my_stairsDown;
    int conutJump = 0;
    [SerializeField]
    float timeHitPointUpdate;
    #endregion

    #region Interfase
    public IWeapone weapone { get; set; }
    public IControl control { get; set; }
    public IconPack iconOnMap { get; set; }

    IArtificialIntelligence IEnemy.artificialIntelligence { get { return _artificialIntelligence; } set { _artificialIntelligence = value; } }

    [SerializeField]
    IArtificialIntelligence _artificialIntelligence;
    #endregion

    #region Struct  
    public ref ButtonStruct buttonStruct { get { return ref _buttonStruct; }}
    [Header("Structs")]
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
    public ref DebugStruct debugStruct { get { return ref _debugStruct; } }

    [SerializeField]
    private DebugStruct _debugStruct;
    public ref MaskStruct maskStruct { get { return ref _maskStruct; } }

    [SerializeField]
    private MaskStruct _maskStruct;

    [SerializeField]
    UpdateStateFromSkill _updateStateFromSkill;
    public ref UpdateStateFromSkill updateStateFromSkill { get { return ref _updateStateFromSkill; } }
    #endregion

    #region Unity classes
    [Header("Unity classes")]
    [SerializeField]
    private Animator _animator;
    public Animator animator { get { return _animator; } set { _animator = value; } }
  
    public Rigidbody2D rigidbody2D { get { return _rigidbody2D; } set { _rigidbody2D = value; } }
    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    public SpriteRenderer spriteRenderer { get { return _spriteRenderer; } set { _spriteRenderer = value; } }
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public CapsuleCollider2D capsuleCollider2D { get { return _capsuleCollider2D; } set { _capsuleCollider2D = value; } }
    [SerializeField]
    private CapsuleCollider2D _capsuleCollider2D;
    #endregion

    #region Actions
    public Action<TypeMessage> ObjectIsAvalible { get; set; }
    public Action<TypeMessage> ObjectIsUnavalible { get; set; }
    #endregion

    #region Classes
    [Header("User Functions")]
    [SerializeField]
    private SoundPlayer _soundPlayer;
    public SoundPlayer soundPlayer { get { return _soundPlayer; } set { _soundPlayer = value; } }

    UpdateSkills _updateSkills;
    public UpdateSkills UpdateSkills { get { return _updateSkills; } set { _updateSkills = value; } }

    [SerializeField]
    TextSwitch textElement; 
    public TextSwitch TextElement { get { return textElement; } set { textElement = value; } }

    public GameObject TextBox { get { return _textBox; } set { _textBox = value; } }
    [SerializeField]
    private GameObject _textBox;
    #endregion

    #endregion

    #region Functions
    #region UnityFunctions
    void Start()
    {
      
        my_stairsUp = false;
        my_stairsDown = false;
        iconOnMap = IconPack.SoldatNormal;
        _maskStruct.unitMaskIgnore = gameObject.layer;
        _maskStruct.platformMaskIgnore = LayerMask.NameToLayer("FloorCollision");
        _maskStruct.platformMaskIgnore = LayerMask.NameToLayer("FloorCollision");
        _stateStruct.isControling = false;
        weapone = GetComponent<Gun>();
        weapone.unit = this;
        _stateStruct.score = 1;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _moveStruct.moveVelocity = new Vector2();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _soundPlayer = GetComponent<SoundPlayer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _moveStruct.speedStep = _moveStruct.speedStepSave = 0.8f;
        _moveStruct.speedRunSave = 1f;
        control = new ControlEnemyGraund(gameObject);
        _artificialIntelligence = new AISoldat(gameObject.transform,3f, 
            new BehaviorSoldat(), this.GetType().ToString());
        _artificialIntelligence.textShow = ShowTextBox;
        _artificialIntelligence.textHide = HideTextBox;
        _artificialIntelligence.GetUnitTransform = (control as ControlEnemyGraund).GetEnemyTransform;
        _updateSkills = new UpdateSkills(this);
    }
    void Awake()
    {

        _maskStruct.unitMaskIgnore = gameObject.layer;
        _maskStruct.platformMaskIgnore = LayerMask.NameToLayer("FloorCollision");
        _maskStruct.platformMaskIgnore = LayerMask.NameToLayer("FloorCollision");
        _stateStruct.isControling = false;
        weapone = GetComponent<Gun>();
        weapone.unit = this;
        _stateStruct.score = 1;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _moveStruct.moveVelocity = new Vector2();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _soundPlayer = GetComponent<SoundPlayer>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _moveStruct.speedStep = _moveStruct.speedStepSave = 0.8f;
        _moveStruct.speedRunSave = 1.3f;
        control = new ControlEnemyGraund(gameObject);
        _artificialIntelligence = new AISoldat(gameObject.transform,2f, 
            new BehaviorSoldat(), this.GetType().ToString());
        _updateSkills = new UpdateSkills(this);
    }
    void Update()
    {
        if (_rigidbody2D.velocity.y < -2)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -2);
        }
        //проверки 
        Dead();
        IsUseObject();
        UpdateEnergy();
        UpdateHitpoint();
        
        if ((_stateStruct.isStairs && !IsStairsNow()) || (!_stateStruct.isControling && _stateStruct.isStairs))
        {
            UnCheckStair();
        }


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

       
        _animator.SetBool("Graund", _stateStruct.isGround);

        //различные действия
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

        JumpUpOrDown();

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
                //  _moveStruct.speedStep = 1.3f;
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
        _moveStruct.moveVelocity = _buttonStruct.moveInput * _moveStruct.speedStep;
        Move();
    
        UseStairs();      

        CollisionPlatformDisable();

        if ((_moveStruct.moveVelocity.y == 0 && !_stateStruct.isStairs &&
            _moveStruct.moveVelocity.x == 0 &&  _stateStruct.isGround && !_buttonStruct.isJump) || 
            (_soundPlayer.Name == "Walk-fast" && !_stateStruct.isGround))
        {
            _soundPlayer.Stop(0);
        }

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
       
        transform.position = new Vector2(transform.position.x + _moveStruct.moveVelocityTemp.x * 
            _moveStruct.speedStep * Time.fixedDeltaTime, transform.position.y + 
            _moveStruct.moveVelocityTemp.y * _moveStruct.speedStep * Time.fixedDeltaTime);
        _moveStruct.position = transform.position;
    }
    #endregion

    #region save/laod
    public void Load(EnemySaveData unit)
    {
        Debug.Log("Null functional");
    }

    public EnemySaveData Save()
    {
        EnemySaveData temp = new EnemySaveData((Vector2)transform.position, _moveStruct.moveVelocity, this.GetType().ToString(), _stateStruct.hitpointStruct.hitpoint, _stateStruct.hitpointStruct.hitpointFull);
        return temp;
    }

    //сделать ссылки
    public void UpdateSave(FileStream fs)
    {
        Save();
    }
    #endregion

    #region TextBox
    

    public void ShowTextBox(string text)
    {
        textElement.name = text;
        textElement.SwitchLanguage();
        _textBox.SetActive(true);
    }
    public void HideTextBox()
    {
        _textBox.SetActive(false);
    }
    #endregion

    #region use
    /// <summary>
    /// для игрока необнодимые данные устанавливаются когда тот взаимодействует с объектом
    /// </summary>
    void IsUseObject()
    {
        bool isFalse = true;
        int lenght = entryInTrigger.Count;
        for (int i = 0; i < lenght; i++)
        {
            if (entryInTrigger[i].GetComponent<ISubjectObject>() != null || entryInTrigger[i].tag == "Stairs")
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
    void CheckUseObject()
    {
        ISubjectObject temp = null;
        int lenght = entryInTrigger.Count;
        for (int i = 0; i < lenght; i++)
        {

            temp = entryInTrigger[i].GetComponent<ISubjectObject>();
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
    /// <summary>
    /// для взаимодействия с лестницей по координатам Х и Y
    /// по X отцепится от лесници, он главнее
    /// </summary>
    public void UseStairs()
    {
        if (_moveStruct.moveVelocity.x != 0 || _moveStruct.moveVelocity.y != 0)
        {
            int lenght = entryInTrigger.Count;
            for (int i = 0; i < lenght; i++)
            {
                if (_moveStruct.moveVelocity.x == 0 && _moveStruct.moveVelocity.y != 0 &&
                    ((_moveStruct.moveVelocity.y > 0 && !my_stairsUp) || (_moveStruct.moveVelocity.y < 0 && !my_stairsDown)) &&
                    (entryInTrigger[i].tag == "Stairs" || entryInTrigger[i].tag == "StairsUp") && !_stateStruct.isStairs)
                {
                    CheckStair(entryInTrigger[i]);
                    break;
                }
                else if ((_moveStruct.moveVelocity.x != 0 && entryInTrigger[i].tag == "Stairs" && _stateStruct.isStairs))
                {
                    UnCheckStair();
                    break;
                }
            }
        }
    }
    public void Use()
    {
        int lenght = entryInTrigger.Count;
        for (int i = 0; i < lenght; i++)
        {
            if ((entryInTrigger[i].tag == "Stairs" || entryInTrigger[i].tag == "StairsUp") && !_stateStruct.isStairs)
            {
                CheckStair(entryInTrigger[i]);
                break;
            }
            else if (entryInTrigger[i].tag == "Stairs" && _stateStruct.isStairs)
            {
                UnCheckStair();
                break;
            }


            try
            {
                var temp = entryInTrigger[i].GetComponent<ISubjectObject>();
                if (temp != null)
                {
                    temp.NotifyUse(_updateStateFromSkill.levelВreaking);
                    break;
                }
            }
            catch { }
        }
    }
    #endregion 

    public void SetControling(bool controling)
    {
        _stateStruct.isControling = controling;
       if(!controling)
        {
            _moveStruct.speedStep = _moveStruct.speedStepSave;
        }
       else if (controling)
        {
            _moveStruct.speedStep = _moveStruct.speedRunSave;
        }
    }

    #region Update dates
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
    #endregion

    #region dialoge with master

    public Action<TypeMessage> SendMessageInMaster { get; set; }

    #endregion

    #region ground / move
    private void MoveWhenUse()
    {
        if ( _stateStruct.useStruct.isMoveWhenUse && _stateStruct.useStruct.isUseState)
        {
            _stateStruct.useStruct.isUseState = false;
            int lenght = entryInTrigger.Count;
            for (int i = 0; i < lenght; i++)
            {
                var temp = entryInTrigger[i].GetComponent<ISubjectObject>();
                if (temp != null)
                {
                    temp.NotifyNoUse();
                    break;
                }
            }
        }
    }
    private void Move()
    {

        if (_stateStruct.isStairs)
        {
            _moveStruct.moveVelocityTemp = new Vector2(0, _moveStruct.moveVelocity.y);
        }
        else
        {
            _moveStruct.moveVelocityTemp = new Vector2(_moveStruct.moveVelocity.x, 0);
        }


        if (_moveStruct.moveVelocityTemp.x != 0 || _moveStruct.moveVelocityTemp.y != 0 || _buttonStruct.isJump)
        {
             _stateStruct.useStruct.isMoveWhenUse = true;
        }


        //-----------------------------------------------------------
        if (_moveStruct.moveVelocityTemp.x > 0|| _moveStruct.moveVelocityTemp.x < 0)
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
        if (_moveStruct.moveVelocityTemp.y > 0)
        {

            if (_stateStruct.isStairs)
            {
                _animator.SetBool("Stairs", true);
                _soundPlayer.Play("Stairs", 0);
            }
        }
        else if (_moveStruct.moveVelocityTemp.y < 0)
        {


            if (_stateStruct.isStairs)
            {
                _animator.SetBool("Stairs", true);
                _soundPlayer.Play("Stairs", 0);
            }
        }
        else if (_stateStruct.isGround)
        {
            _animator.SetBool("Stairs", false);
        }

        MoveWhenUse();
    } 
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
        else if ( (_buttonStruct.isJump && _buttonStruct.isJumpOff) &&

           
            ( ( (_stateStruct.isDoubleJump && conutJump < _stateStruct.maxJump) || (_stateStruct.isGround || _stateStruct.isStairs)) 
            &&  (_rigidbody2D.velocity.y == 0 || conutJump == 1))


            && (!_stateStruct.isJumpDown && _buttonStruct.moveInput.y >= 0) )
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
      if (  !_stateStruct.isGroundDownJump)
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
            _debugStruct.sizeLine,_moveStruct.platformMask);
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
            0.35f, _moveStruct.platformMask);
        Color t = Color.magenta;
        Debug.DrawRay(temp, Vector2.down * 0.35f, t);

        return (_moveStruct.raycastHit2DForJump.collider != null && _moveStruct.raycastHit2DForJump.collider.tag != _stateStruct.noJumpingTag );
    }
    private void CollisionPlatformDisable()
    {      
        if(_rigidbody2D.velocity.y > 0 ||
            (_moveStruct.raycastHit2DForJump.collider != null 
            && _moveStruct.raycastHit2DForJump.collider.tag != _stateStruct.noJumpingTag))
        {
            capsuleCollider2D.isTrigger = true;
        }
        //а нужен ли этот код?
        else if(_stateStruct.isJumpDown && 
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
            if (!_stateStruct.isStairs )              
            {
                if(_stateStruct.isJumpDown)
                {
                    if(_moveStruct.raycastHit2D.collider != null && _moveStruct.raycastHit2D.collider.tag == stateStruct.noJumpingTag)
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

    #region Stairs
    private bool IsStairsNow()
    {
        bool temp = false;
        for (int i =0; i < entryInTrigger.Count;i++)
        {
            if (entryInTrigger[i].tag == "StairsDown")
            {
                return false;
            }
            if(entryInTrigger[i].tag == "Stairs")
            {
                temp = true;
            }
              
        }

        return temp;
    }
    private void CheckStair(GameObject elem)
    {
        //_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _stateStruct.gravityScale = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;
        _capsuleCollider2D.isTrigger = true;
        //CollisionStairsDisable(true);
        _rigidbody2D.velocity = new Vector2(0,0);

        

        float tempY;

        // придумать как подниматся по лестнеце
        if (_stateStruct.isGround && elem.tag == "StairsUp")
        {
            tempY = gameObject.transform.position.y - _capsuleCollider2D.size.y;
        }
        else if (_stateStruct.isGround )
        {
            tempY = gameObject.transform.position.y + 0.05f;
        }
        else
        {
            tempY = gameObject.transform.position.y;
        }

        gameObject.transform.position = new Vector2(elem.transform.position.x, tempY);
        _moveStruct.moveVelocity = new Vector2(0,0);
        _moveStruct.moveVelocityTemp = new Vector2(0,0);
        _stateStruct.isStairs = true;
    }
    private void UnCheckStair()
    {
        _stateStruct.isStairs = false;
        _rigidbody2D.velocity = new Vector2(0, 0);
        _moveStruct.moveVelocity = new Vector2(0, 0);
        _moveStruct.moveVelocityTemp = new Vector2(0, 0);
        _rigidbody2D.gravityScale = _stateStruct.gravityScale;
        capsuleCollider2D.isTrigger = false;
        //CollisionStairsDisable(false);

    }
    #endregion Stairs

    #region Weapone
    void TimerShoot()
    {
        if (_shotStruct.timeReload <= 0 && _stateStruct.energyStruct.energy - weapone.energyCost >= 0 && _buttonStruct.isShot)
        {
            _stateStruct.energyStruct.energy -= weapone.energyCost;
            weapone.Shot(new Vector2(_spriteRenderer.flipX == true ? -1 : 1, 0), new Vector2(_spriteRenderer.flipX == true ? (-1f * _shotStruct.pointShotX) : (1f * _shotStruct.pointShotX), 0));
            _soundPlayer.PlayOneShoot("Shoot-gun", 1);
            _shotStruct.timeReload = _shotStruct.timeReloadFull;
        }
        if (_shotStruct.timeReload >= 0)
        {
            _shotStruct.timeReload -= Time.deltaTime * _shotStruct.speadSotTime;
        }
    }
    void Reload()
    {
        _shotStruct.timeReload = 0;
    }
    #endregion

    #region Trigers

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag != "Graund")
        {
            entryInTrigger.Remove(other.gameObject);
            if (other.tag == "Stairs" || other.tag == "StairsUp" || other.tag == "StairsDown")
            {
                 if (other.tag == "StairsUp")
                {
                    my_stairsUp = false;
                }
                else if (other.tag == "StairsDown")
                {
                    my_stairsDown = false;
                }

            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Graund")
        {
            entryInTrigger.Add(other.gameObject);

            if (other.tag == "Stairs" || other.tag == "StairsUp"|| other.tag == "StairsDown")
            {
                  if(other.tag == "StairsUp")
                {
                     my_stairsUp = true;
                }
                else if (other.tag == "StairsDown")
                {
                     my_stairsDown = true;
                }

            }
           
        }
    }



    #endregion

    #region UnitDie
    public void Dead()
    {
        if (_stateStruct.hitpointStruct.hitpoint <= 0)
        {
            _stateStruct.isDead = true;
            NotifyDead(gameObject, DeadCommand.kill);
            //Destroy(gameObject);          
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

    public void NotifyDead(GameObject _object,DeadCommand deadCommand)
    {
        for (int i = 0; i < _observelDied.Count; i++)
        {
            _observelDied[i].UpdateDead(_object, deadCommand);
        }
    }


    #endregion

    #region skill
    public void UpgrateSkillDate(string name, float value)
    {
        _updateSkills.UpdateDataSkills(name, value);
    }

    #endregion

    #endregion

}
