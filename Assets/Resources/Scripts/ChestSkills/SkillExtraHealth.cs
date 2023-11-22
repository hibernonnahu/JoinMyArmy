using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExtraHealth : ISkill
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
        return 0;
    }
    public float ExtraSpeed()
    {
        return 0;
    }
    public int ExtraDefense()
    {
        return 0;
    }

    public int ExtraHealth()
    {
        return 25;
    }

    public string GetDescription()
    {
        return "More health";
    }

    public string GetName()
    {
        return "Health";
    }

    public bool IsAvailable(RecluitController recluitController)
    {
        return true;
    }
}
