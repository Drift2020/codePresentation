using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AISoldat : IArtificialIntelligence
{
    

    #region Dates
    public UnitTexts My_text { get { return my_text; } set { my_text = value; } }
    UnitTexts my_text;

    public Dictionary<TypeDialoge, List<string>> DictionaryNameKeys 
    { get { return ((Dictionary<TypeDialoge, List<string>>)((object)_dictionaryNameKeys)); } 
     private set { _dictionaryNameKeys = value; } }

    Dictionary<TypeDialoge, List<string>> _dictionaryNameKeys;

    IBehavior behavior;

    #region Data delegations
    public ShowTextBox textShow { get { return _textShow; } set { _textShow = value; } }
    private ShowTextBox _textShow;
    public HideTextBox textHide { get { return _textHide; } set { _textHide = value; } }
    private HideTextBox _textHide; 
    public GetUnitTransform GetUnitTransform { get { return getUnitTransform; } set { getUnitTransform = value; } }
    private GetUnitTransform getUnitTransform;
    #endregion

    #region Data unity calasses
    public Transform TargetUnit { get { return targetUnit; } set { targetUnit = value; } }
    private Transform targetUnit;
    public Transform MyUnit { get { return myUnit; } set { myUnit = value; } }
    private Transform myUnit;
    #endregion

    #region Data Timer for text box
    public float timeSwitchBox { get { return _timeSwitchBox; } set { _timeSwitchBox = value; } }
    private float _timeSwitchBox;
    public float speadTimeSwitchBox { get { return _speadTimeSwitchBox; } set { _speadTimeSwitchBox = value; } }
    private float _speadTimeSwitchBox;
    public bool isTimerSwitchBox { get { return _isTimerSwitchBox; } set { _isTimerSwitchBox = value; } }
    private bool _isTimerSwitchBox;
    public float timeShowBox { get; set; }
    public bool isTimerShowBox { get; set; }
    #endregion

    #region Data timer for turn side
    public float timeTurnOn { get { return _timeTurnOn; } set { _timeTurnOn = value; } }
    float _timeTurnOn = 0;
    public float speadTimeTurnOn { get { return _speadTimeTurnOn; } set { _speadTimeTurnOn = value; } }
    float _speadTimeTurnOn = 1f;
    #endregion

    #region Data timer for search 
    public float saveTimeSearchStart { get { return _saveTimeSearchStart; } set { _saveTimeSearchStart = 0; } }
    float _saveTimeSearchStart = 5;
    public float saveTimeSearchEnd { get { return _saveTimeSearchEnd; } set { _saveTimeSearchEnd = value; } }
    float _saveTimeSearchEnd = 8;
    public float timeSearch { get { return _timeSearch; } set { _timeSearch = value; } }
    float _timeSearch = 0;
    public float speadTimeSearch { get { return _speadTimeSearch; } set { _speadTimeSearch = value; } }
    float _speadTimeSearch = 1f;
    #endregion 

    public float timeChenge { get { return _timeChenge; } set { _timeChenge = value; } }
    float _timeChenge = 0;
    public float minDst { get; set; }
    float _timer = 0;
    public float timer { get { return _timer; } set { _timer = value; } }

    #region Enum state;
    public StateAI MyState { get; set; }
    public SearchingState MySearchingState { get; set; }
    #endregion

    #endregion Dates
    #region Functions
    public AISoldat(Transform unit, float minDst, IBehavior behavior,string Name)
    {
        
        MyState = StateAI.Patrolling;
        MySearchingState = SearchingState.Idling;
        timeShowBox = 2;
        myUnit = unit;
        my_text = new UnitTexts(Name);
        this.minDst = minDst;
        this.behavior = behavior;

        _dictionaryNameKeys = new Dictionary<TypeDialoge, List<string>>();

        _dictionaryNameKeys.Add(TypeDialoge.FoundEnemy, my_text.GetNamesKey(TypeDialoge.FoundEnemy.ToString()));
        _dictionaryNameKeys.Add(TypeDialoge.LostEnemy, my_text.GetNamesKey(TypeDialoge.LostEnemy.ToString()));
        _dictionaryNameKeys.Add(TypeDialoge.Attact, my_text.GetNamesKey(TypeDialoge.Attact.ToString()));
        _dictionaryNameKeys.Add(TypeDialoge.Search, my_text.GetNamesKey(TypeDialoge.Search.ToString()));
        _dictionaryNameKeys.Add(TypeDialoge.Patrule, my_text.GetNamesKey(TypeDialoge.Patrule.ToString()));
        _dictionaryNameKeys.Add(TypeDialoge.Idle, my_text.GetNamesKey(TypeDialoge.Idle.ToString()));
    }
    private string GetText(List<string> items)
    {
        return items[Random.Range(0, items.Count)];
    }

    #region TextBox
    public void ResetBoxAndTimer()
    {
        _timer = 0;
        textHide();
    }
    public void TimerSwitchBox(float max, float min, float speadTime)
    {

      
        if (_timeSwitchBox <= 0 && !isTimerSwitchBox)
        {

            isTimerSwitchBox = true;
            switch (MyState)
            {
                case StateAI.Attacking:
                    _textShow(GetText(_dictionaryNameKeys[TypeDialoge.Attact]));
                    break;
                case StateAI.Patrolling:
                    _textShow(GetText(_dictionaryNameKeys[TypeDialoge.Patrule]));
                    break;
                case StateAI.Idling:
                    _textShow(GetText(_dictionaryNameKeys[TypeDialoge.Idle]));
                    break;
                case StateAI.Searching:
                    _textShow(GetText(_dictionaryNameKeys[TypeDialoge.Search]));
                    break;
            }
        }
        else if (_timeSwitchBox > 0)
        {
          
            _timeSwitchBox -= Time.deltaTime * speadTime;
        }
        else if (isTimerSwitchBox && isTimerShowBox)
        {
           
            _timeSwitchBox = Random.Range(min, max);
            isTimerSwitchBox = false;
            isTimerShowBox = false;
        }
        else
        {
           
            TimerShowBox(1.2f);
        }
    }
    public  void TimerShowBox(float speadTime)
    {
        if(timeShowBox>0)
        {
            timeShowBox -= Time.deltaTime * speadTime;
        }
        else
        {
            timeShowBox = 4;
            isTimerShowBox = true;
            textHide();
        }
    }
    #endregion

    #region Timers
    public bool TimeTurnOn(float max, float min, float speadTimeTurnOn)
    {
        if(timeTurnOn <= 0)
        {
            timeTurnOn = Random.Range(min, max);
            return true;
        }
        timeTurnOn -= Time.deltaTime * speadTimeTurnOn;


        return false;
    }
    public bool TimeChengeState(float max, float min, float speadTime)
    {
        if (timeChenge <= 0)
        {
            timeChenge = Random.Range(min, max);
            return true;
        }
        timeChenge -= Time.deltaTime * speadTime;


        return false;
    }
    public bool Timer(float spead, float min, float max)
    {

        if (_timer <= 0)
        {

            _timer = Random.Range(min, max);
            return true;
        }
        _timer -= Time.deltaTime * spead;
        return false;
    }
    public bool TimeSearch()
    {
        if (timeSearch <= 0)
        {
            timeSearch = Random.Range(saveTimeSearchStart, saveTimeSearchEnd);
            return true;
        }
        timeSearch -= Time.deltaTime * speadTimeSearch;


        return false;
    }
    bool TimerWaitUpdateReserveTime(ref StateStruct stateStruct)
    {
        if (stateStruct.waitUpdateReserveTime <= 0)
        {
            stateStruct.waitUpdateReserveTime = stateStruct.waitUpdateReserveTimeFull;
            return true;
        }
        stateStruct.waitUpdateReserveTime -= Time.deltaTime;

        return false;
    }
    public void TimerResetReserve(ref StateStruct stateStruct)
    {
        if (stateStruct.reserveTime != stateStruct.reserveTimeFull && !stateStruct.isReserveSee && TimerWaitUpdateReserveTime(ref stateStruct))
        {
            stateStruct.reserveTime = stateStruct.reserveTimeFull;
        }
    }
    #endregion

    public bool isInRadiusProsecution()
    {
        if (targetUnit == null ||Vector2.Distance(myUnit.position, targetUnit.position) > minDst ||
           (Mathf.Abs(myUnit.position.y) - Mathf.Abs(targetUnit.position.y) <= -0.7 &&
           Mathf.Abs(myUnit.position.y) - Mathf.Abs(targetUnit.position.y) >= 0.7))
        {
            return true;
        }

     
        return false;
    }

    /// <summary>
    /// выбор состояния поведения бота без изменения направлений
    /// </summary>
    /// <param name="stateStruct"></param>
    /// <param name="buttonStruct"></param>
    /// <param name="moveStruct"></param>
  
    public void AnalyseFlags(IUnit unit)
    {
        behavior.AnalyseFlags(unit, this);
    }

    /// <summary>
    /// Активация состояния и изменение переменных
    /// </summary>
    /// <param name="buttonStruct"></param>
    /// <param name="moveStruct"></param>
 
   


   
    #endregion
}
