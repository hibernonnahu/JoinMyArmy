using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAddSkillsOnDead : MonoBehaviour, IEnemySimpleAdd
{
    private const float OFFSET_PER_UNIT = 3;
    private const float OFFSET = 1f;
    public SkillIconController skillIconController;
    public int skillsAmount = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    public void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.OnDeadActionList.Add(CallSkillsPopup);
    }
    private void CallSkillsPopup()
    {
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("chestopen"));

        OnShuffleSkill();
        //EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.SKILLS));
        //EventManager.TriggerEvent(EventName.SHUFFLE_SKILL, EventManager.Instance.GetEventData().SetInt(skillsAmount));
    }
    private void OnShuffleSkill()
    {
        List<string> skillNames = GetSkillNames();
        SkillIconController[] skillIconControllerArray = new SkillIconController[skillsAmount];
        float offset = -OFFSET_PER_UNIT * 0.5f * skillsAmount+OFFSET;
        for (int i = 0; i < skillsAmount; i++)
        {
           
            skillIconControllerArray[i] = Instantiate<SkillIconController>(skillIconController);
            skillIconControllerArray[i].Init(skillIconControllerArray, GetRandomSkill(skillNames),
                (transform.position.x+offset+OFFSET_PER_UNIT*i) * Vector3.right + skillIconControllerArray[i].transform.position.y * Vector3.up + (transform.position.z+ skillIconControllerArray[i].transform.position.z) * Vector3.forward
                ); ;       
        }
    }

    private string GetRandomSkill(List<string> skillNames)
    {
        int index = UnityEngine.Random.Range(0, skillNames.Count);
        string name = skillNames[index];
        skillNames.RemoveAt(index);
        return name;
    }

    private List<string> GetSkillNames()
    {
        var list = new List<string>();
        //var info = new DirectoryInfo(Application.dataPath + "/Main/Scripts/SkillControl/Skill");
        var info = Resources.LoadAll<TextAsset>("Scripts/ChestSkills");
        CharacterMain character = FindAnyObjectByType<CharacterMain>();
        //var fileInfo = info.GetFiles();
        foreach (var skillClassName in info)
        {
            var type = Type.GetType(skillClassName.name);

            ISkill skill = (ISkill)Activator.CreateInstance(type);
            if (skill.IsAvailable())
                list.Add(skillClassName.name);
        }
        return list;
    }
}
