using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    public SkillUIButton prefab;
    public Transform mainContainer;

    void Start()
    {
        EventManager.StartListening(EventName.SHUFFLE_SKILL, OnShuffleSkill);

    }

    private void OnShuffleSkill(EventData arg0)
    {
        List<string> skillNames = GetSkillNames();
        foreach (Transform item in mainContainer)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < arg0.intData; i++)
        {
            SkillUIButton skill = Instantiate<SkillUIButton>(prefab, mainContainer);
            skill.skillClassName = GetRandomSkill(skillNames);
            var rect = skill.GetComponent<RectTransform>();
            LeanTween.scale(rect, Vector3.one, 1).setDelay(0.5f).setEaseInOutElastic().setIgnoreTimeScale(true);
            rect.localScale = Vector3.zero;
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
            if (skill.IsAvailable(character.recluitController))
                list.Add(skillClassName.name);
        }
        return list;
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.SHUFFLE_SKILL, OnShuffleSkill);
    }
}
