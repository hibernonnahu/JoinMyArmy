using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExtraDamage : ISkill
{
    public void ExecuteAura()
    {
    }

    public void ExecuteOnGrab(Character character)
    {

    }

    public void ExecuteOnHit(Character characterHit, float damage)
    {

    }

    public void ExecuteOnKill(Character character, Character characterHit)
    {

    }
    public int ExtraRecluit()
    {
        return 0;
    }
    public int ExtraDamage()
    {
        return 1;
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
        return "Boost damage";
    }

    public string GetName()
    {
        return "Strength";
    }

    public bool IsAvailable()
    {
        return true;
    }
}
