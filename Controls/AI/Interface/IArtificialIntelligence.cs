using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeDialoge
{
    FoundEnemy,
    LostEnemy,
    Attact,
    Search,
    Patrule,
    Idle,
    Living
}
public delegate void ShowTextBox(string name);
public delegate void HideTextBox();

public delegate Transform GetUnitTransform();
public interface IArtificialIntelligence
{

    StateAI MyState { get; set; }
    SearchingState MySearchingState { get; set; }

    void AnalyseFlags(IUnit unit);

    GetUnitTransform GetUnitTransform { get; set; }
    ShowTextBox textShow { get; set; }
    HideTextBox textHide { get; set; }

    UnitTexts My_text { get; set; }
    Transform TargetUnit { get; set; }
    Transform MyUnit { get; set; }

    Dictionary<TypeDialoge,  List<string>> DictionaryNameKeys { get; }

    float minDst { get; set; }

    float timeShowBox { get; set; }
    bool isTimerShowBox { get; set; }

    float timeTurnOn { get; set; }
    float speadTimeTurnOn { get; set; }

    float timeChenge { get; set; }

    float saveTimeSearchStart { get; set; }
    float saveTimeSearchEnd { get; set; }
    float timeSearch { get; set; }
    float speadTimeSearch { get; set; }

    float timeSwitchBox { get; set; }
    float speadTimeSwitchBox { get; set; }
    bool isTimerSwitchBox { get; set; }
    float timer { get; set; }


    bool TimeTurnOn(float max, float min, float speadTimeTurnOn);
    bool TimeChengeState(float max, float min, float speadTime); 
    bool TimeSearch();
    void TimerSwitchBox(float max, float min, float speadTime);
    void TimerShowBox(float speadTime);
    void TimerResetReserve(ref StateStruct stateStruct);
    bool Timer(float spead, float min, float max);
    bool isInRadiusProsecution();
    void ResetBoxAndTimer();



}
public enum StateAI
{
    Attacking,
    Searching,
    Fleeing,
    Idling,
    Patrolling,
    FindingHelp,
    Pursue,
    AttackingAlternative
}


public enum SearchingState
{ 
    Idling,
    Patrolling
}

