using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialSkills : MonoBehaviour
{
    [SerializeField]
    Dictionary<SpetialSkillsType, ISkill> skillCallUnits;
    public Player unit;

    [SerializeField]
    LongClickButton spetialButton;
    [SerializeField]
    IconTimer spetialButtonImage;

    SpetialSkillsType memberType;

    SpetialSkillsType selectSkill;

    // Start is called before the first frame update
    void Start()
    {

        skillCallUnits = new Dictionary<SpetialSkillsType, ISkill>();
        skillCallUnits.Add(SpetialSkillsType.Drone,new SkillCallDrone());
        skillCallUnits.Add(SpetialSkillsType.Soldat, new SkillCallSoldat());
        skillCallUnits.Add(SpetialSkillsType.Robot, new SkillCallRobot());
        skillCallUnits.Add(SpetialSkillsType.Acceleration, new SkillAcceleration(unit));
    }

    public void Use()
    {
        if (skillCallUnits.Count != 0 && !spetialButtonImage.IsWork)
        {
            if (skillCallUnits[selectSkill].level>0 && 
                skillCallUnits[selectSkill].IsAvalible())
            {
                memberType = selectSkill;
                skillCallUnits[selectSkill].Use();
                if (!skillCallUnits[selectSkill].IsAvalible())
                {
                    spetialButton.interactable = false;
                    spetialButtonImage.StarTimer();
                }
            }

            
        }
    }

    public void ActivateAcceleration()
    {
        skillCallUnits[SpetialSkillsType.Acceleration].level = 1;
    }

    public void UpdateLevelCallDrone (uint value)
    {
        skillCallUnits[SpetialSkillsType.Drone].level = value;
    }
    public void UpdateLevelCallRobot(uint value)
    {
        skillCallUnits[SpetialSkillsType.Robot].level = value;
    }
    public void UpdateLevelCallSoldat(uint value)
    {
        skillCallUnits[SpetialSkillsType.Soldat].level = value;
    }


    public void SetSkill(SpetialSkillsType value)
    {
        selectSkill = value;
    }
   

    void Timer()
    {
        if (skillCallUnits[memberType].MyTime > 0)
        {
            spetialButtonImage.UpdateTimer(skillCallUnits[memberType].FullTime, skillCallUnits[memberType].MyTime);
        }
        else
        {
            spetialButton.interactable = true;
        }
        
    }

    public bool IsActiveNow()
    {
        return skillCallUnits[memberType].activate;
    }

    public void Update()
    {
        
        for(int i = 0; i < skillCallUnits.Count; i++)
        {     
            skillCallUnits[(SpetialSkillsType)i].Update();
            if((SpetialSkillsType)i == memberType)
            {
               if(!skillCallUnits[memberType].IsAvalible())
                {
                    Timer();
                }
            }
        }
    }
}
