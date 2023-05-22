using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExtraRecluit : ISkill
{
    public void ExecuteAura()
    {
    }

    public void ExecuteOnGrab(Character character)
    {
        ((CharacterMain)(character)).recluitController.AddMaxRecluit();
    }

    public void ExecuteOnHit(Character characterHit, float damage)
    {

    }

    public void ExecuteOnKill(Character character, Character characterHit)
    {

    }
    public int ExtraRecluit()
    {
        return 1;
    }
    public int ExtraDamage()
    {
        return 0;
    }

    public int ExtraDefense()
    {
        return 0;
    }

    public int ExtraHealth()
    {
        return 0;
    }

    public string GetDescription()
    {
        return "Gets one extra space for recluit";
    }

    public string GetName()
    {
        return "Extra Recluit";
    }

    public bool IsAvailable()
    {
        return GameObject.FindAnyObjectByType<RecluitController>().Max < 8;
    }
}
