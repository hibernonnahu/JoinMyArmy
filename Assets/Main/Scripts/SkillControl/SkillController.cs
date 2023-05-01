using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController 
{
    private List<ISkill> skills=new List<ISkill>();

    private int extraRecluit;
    public int ExtraRecluit { get => extraRecluit; }
    private int extraHealth;
    public int ExtraHealth { get => extraHealth;}
    private int extraDamage;
    public int ExtraDamage { get => extraDamage; }
    private int extraDefense;
    public int ExtraDefense { get => extraDefense; }
    private Character character;
    public Character Character { set { character = value; } }

    public SkillController(Character character)
    {
        this.character = character;
    }
    public void CalculateSkills()
    {
        extraDamage = 0;
        extraHealth = 0;
        extraDefense = 0;
        foreach (var skill in skills)
        {
            UpdateSkill(skill);
        }
        character.UpdateStatsOnLevel(character.level);
    }

    private void UpdateSkill(ISkill skill)
    {
        extraDamage += skill.ExtraDamage();
        extraHealth += skill.ExtraHealth();
        extraDefense += skill.ExtraDefense();
        extraRecluit += skill.ExtraRecluit();
        
        character.UpdateStatsOnLevel(character.level);
    }

    public void AddSkill(ISkill skill) 
{
        skills.Add(skill);
        UpdateSkill(skill);
        skill.ExecuteOnGrab(character);
        EventManager.TriggerEvent(EventName.MAIN_TEXT, EventManager.Instance.GetEventData().SetString(skill.GetDescription()));
    }

    public void OnKill(Character character,Character characterKill)
    {
        foreach (var item in skills)
        {
            item.ExecuteOnKill(character, characterKill);
        }
    }
}
