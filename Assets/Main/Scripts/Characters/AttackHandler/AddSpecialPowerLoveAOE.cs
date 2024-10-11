using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpecialPowerLoveAOE : AddCharacterSpecialPower
{
    public ParticleSystem[] particles;
    public ParticleSystem[] simpleHeart;
    public AudioSource[] audioSources;
    public float sqrRadius;
    public float stuckCounter = 2;
    public string animationName = "cast";
    protected override void ExecutePower()
    {
        foreach (var item in particles)
        {
            item.Play();
        }
        foreach (var item in audioSources)
        {
            item.Play();
        }
        character.SetAnimation(animationName);
        character.VulnerableTime = stuckCounter;
        character.StateMachine.ChangeState<StateCharacterMainStuck>();
       
        Invoke("Power", 2);
    }

    private void Power()
    {
        if (!character.IsDead) {
            int miniheart = 0;
            foreach (var item in character.CharacterManager.GetEnemiesInRange(character.team, sqrRadius, character.transform.position))
            {
                if (!item.IsDead)
                    if (Utils.CanBeRecluited(character.canRecluit, item.enemyType) && item is CharacterEnemy)
                    {
                        CharacterEnemy e = (CharacterEnemy)item;
                        character.CharacterManager.RemoveCharacter(e);
                        character.CharacterManager.GoMainTeam(e, true, -1, false);
                        e.UpdateColor();
                        e.VulnerableTime = 1;
                        e.NextState = e.IdleState;
                        e.SetAnimation("idle");
                        e.StateMachine.ChangeState(typeof(StateCharacterEnemyVulnerable));
                        if (miniheart < simpleHeart.Length)
                        {
                            simpleHeart[miniheart].transform.position = Vector3.right * e.transform.position.x + Vector3.forward * e.transform.position.z + Vector3.up * 4.5f;
                            simpleHeart[miniheart].Play();
                            miniheart++;
                        }
                    }
            }
        }
    }

    public override AddCharacterSpecialPower Init(CharacterMain character)
    {
        character.StateMachine.AddState(new StateCharacterMainStuck(character.StateMachine, character));
        return base.Init(character);

    }
}
