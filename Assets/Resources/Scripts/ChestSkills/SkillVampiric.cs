using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillVampiric : ISkill
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
        character.Heal((int)(characterHit.Health * 0.25f));
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
        return 0;
    }

    public int ExtraRecluit()
    {
        return 0;
    }

    public string GetDescription()
    {
        return "Gains health per kill";
    }

    public string GetName()
    {
        return "Vampiric";
    }

    public bool IsAvailable(RecluitController recluitController)
    {
        return true;
    }
}
