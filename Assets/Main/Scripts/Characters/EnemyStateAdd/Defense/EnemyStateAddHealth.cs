using System;
using UnityEngine;

public class EnemyStateAddHealth : EnemyStateAddJustAnimate
{
    public ParticleSystem particles;
    public float heal = 5;
    public float distanceSQR = 15;

    public override float Execute()
    {
        foreach (var character in characterEnemy.CharacterManager.GetTeamMatesInRange(characterEnemy.team,distanceSQR,characterEnemy.model.transform.position))
        {
            particles.Play();
            character.Heal(heal+characterEnemy.level*1.5f);
        }
        return base.Execute();
    }

}
