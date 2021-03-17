using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorDrone : IBehavior
{
    public void ActiveState(IUnit unit, IArtificialIntelligence AI)
    {
        float saveDirection = 1;
        // если нам нужно идти но мы не можем, делаем разворот
        if ((unit.moveStruct.isMove && !unit.buttonStruct.isMove) || !unit.buttonStruct.isMove)
        {
            unit.moveStruct.FlipX = !unit.moveStruct.FlipX;
            unit.buttonStruct.moveInput.x *= -1;
        }

        switch (AI.MyState)
        {
            case StateAI.Attacking:

                AI.TimerSwitchBox(5, 10, 1.2f);
                if (unit.buttonStruct.moveInput.x != 0 && unit.buttonStruct.isShot)
                {
                    saveDirection = unit.buttonStruct.moveInput.x;
                    unit.buttonStruct.moveInput.x = 0;
                    unit.moveStruct.isMove = false;
                }

                break;
            case StateAI.AttackingAlternative:

                AI.TimerSwitchBox(5, 10, 1.2f);
                if (unit.buttonStruct.moveInput.x != 0 && unit.buttonStruct.isPunch)
                {
                    saveDirection = unit.buttonStruct.moveInput.x;
                    unit.buttonStruct.moveInput.x = 0;
                    unit.moveStruct.isMove = false;
                }

                break;
            case StateAI.Pursue:// +
                unit.moveStruct.isMove = true;
                AI.TimerSwitchBox(5, 10, 1.2f);
                if (unit.moveStruct.speedStep != unit.moveStruct.speedRunSave)
                {
                    unit.moveStruct.speedStep = unit.moveStruct.speedRunSave;
                }
                // если мы вне зоны выстрела но в радиусе обзоа, преследовать
                if (AI.TargetUnit != null)
                {
                    if (Mathf.Abs(AI.TargetUnit.position.x) - Mathf.Abs(AI.MyUnit.position.x) >= 0.2
                    || Mathf.Abs(AI.TargetUnit.position.x) - Mathf.Abs(AI.MyUnit.position.x) <= -0.2)
                    {
                        if (unit.buttonStruct.moveInput.x == 0)
                        {
                            if (unit.moveStruct.FlipX == false)
                            {
                                unit.buttonStruct.moveInput.x = 1;
                            }
                            else
                            {
                                unit.buttonStruct.moveInput.x = -1;
                            }
                        }
                    }
                    // если мы прошли мимо бота то развернеутся 
                    else if (Mathf.Abs(AI.TargetUnit.position.x) - Mathf.Abs(AI.MyUnit.position.x) >= 0.03 ||
                                    Mathf.Abs(AI.TargetUnit.position.x) - Mathf.Abs(AI.MyUnit.position.x) <= -0.03)
                    {
                        if (unit.buttonStruct.moveInput.x == 0)
                        {
                            unit.moveStruct.FlipX = !unit.moveStruct.FlipX;
                        }
                    }
                }

                break;
            case StateAI.Searching:// +

                if (AI.TimeChengeState(1, 3, AI.speadTimeTurnOn))
                {
                    if (AI.MySearchingState == SearchingState.Patrolling)
                    {
                        AI.MySearchingState = SearchingState.Idling;
                    }
                    else
                    {
                        AI.MySearchingState = SearchingState.Patrolling;
                    }
                }
                switch (AI.MySearchingState)
                {
                    case SearchingState.Idling://
                        unit.buttonStruct.moveInput.x = 0;
                        unit.moveStruct.isMove = false;
                        break;
                    case SearchingState.Patrolling://
                        unit.moveStruct.isMove = true;
                        if (unit.buttonStruct.moveInput.x == 0)
                        {

                            unit.buttonStruct.moveInput.x = saveDirection;
                            unit.moveStruct.speedStep = unit.moveStruct.speedRunSave;
                        }

                        if (AI.TimeTurnOn(2, 4, AI.speadTimeTurnOn))
                        {
                            unit.buttonStruct.moveInput.x *= -1;
                        }
                        break;
                }
                AI.TimerSwitchBox(3, 5, 1.2f);

                break;
            case StateAI.Fleeing://

                AI.TimerSwitchBox(5, 7, 1.2f);
                break;
            case StateAI.Idling://+

                AI.TimerResetReserve(ref unit.stateStruct);

                AI.TimerSwitchBox(5, 7, 1.2f);
                if (unit.buttonStruct.moveInput.x != 0)
                {
                    saveDirection = unit.buttonStruct.moveInput.x;
                    unit.buttonStruct.moveInput.x = 0;
                    unit.moveStruct.isMove = false;
                }
                if (AI.Timer(1.2f, 5, 10) && AI.MyState == StateAI.Idling)
                {
                    AI.MyState = StateAI.Patrolling;
                    unit.moveStruct.isMove = true;
                }

                break;
            case StateAI.Patrolling://+

                AI.TimerResetReserve(ref unit.stateStruct);

                AI.TimerSwitchBox(7, 10, 1.2f);
                if (unit.buttonStruct.moveInput.x == 0)
                {
                    unit.buttonStruct.moveInput.x = saveDirection;
                    unit.moveStruct.speedStep = unit.moveStruct.speedStepSave;
                }
                if (AI.TimeTurnOn(4, 8, AI.speadTimeTurnOn))
                {
                    unit.buttonStruct.moveInput.x *= -1;
                    unit.moveStruct.FlipX = !unit.moveStruct.FlipX;
                }
                if (AI.Timer(1.2f, 8, 15) && AI.MyState == StateAI.Patrolling)
                {
                    AI.MyState = StateAI.Idling;
                    saveDirection = unit.buttonStruct.moveInput.x;
                    unit.buttonStruct.moveInput = new Vector2(0, unit.buttonStruct.moveInput.y);
                    unit.moveStruct.isMove = false;
                }

                break;
            case StateAI.FindingHelp://

                AI.TimerSwitchBox(5, 7, 1.2f);

                break;
            default:
                break;
        }
    }

    public void AnalyseFlags(IUnit unit, IArtificialIntelligence AI)
    {
        if (unit.buttonStruct.isShot || unit.buttonStruct.isPunch)
        {
            if (unit.buttonStruct.isPunch)
            {
                AI.MyState = StateAI.AttackingAlternative;
            }
            else
            {
                AI.MyState = StateAI.Attacking;

            }
            AI.TargetUnit = AI.GetUnitTransform();
        }
        else if (unit.stateStruct.isGround &&
            (AI.MyState == StateAI.Attacking || AI.MyState == StateAI.AttackingAlternative)
            && !unit.buttonStruct.isShot && !unit.buttonStruct.isPunch)
        {
            AI.MyState = StateAI.Pursue;
        }
        else if (unit.stateStruct.isGround && AI.MyState == StateAI.Pursue
            && AI.isInRadiusProsecution())
        {
            AI.MyState = StateAI.Searching;
        }
        else if (unit.stateStruct.isGround && AI.MyState == StateAI.Searching && AI.TimeSearch())
        {
            unit.stateStruct.reserveTime = unit.stateStruct.reserveTimeFull;
            unit.stateStruct.waitUpdateReserveTime = unit.stateStruct.waitUpdateReserveTimeFull;
            AI.MyState = StateAI.Patrolling;

        }
        else if (unit.stateStruct.isGround && unit.moveStruct.isMove
            && AI.MyState != StateAI.Searching && AI.MyState != StateAI.Idling
            && AI.MyState != StateAI.Pursue)
        {
            AI.MyState = StateAI.Patrolling;
        }
        else if (unit.stateStruct.isGround && !unit.moveStruct.isMove &&
            AI.MyState != StateAI.Searching && AI.MyState != StateAI.Pursue)
        {
            AI.MyState = StateAI.Idling;
        }

        ActiveState(unit, AI);
    }

  
}
