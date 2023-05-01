using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAddSkillsOnDead : MonoBehaviour, IEnemySimpleAdd
{
    public int skillsAmount = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    public void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.OnDeadActionList.Add(CallSkillsPopup);

        Destroy(this);
    }
    private void CallSkillsPopup()
    {
        EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.SKILLS));
        EventManager.TriggerEvent(EventName.SHUFFLE_SKILL, EventManager.Instance.GetEventData().SetInt(skillsAmount));
    }
}
