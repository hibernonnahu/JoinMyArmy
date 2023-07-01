using System;
using UnityEngine;

public class EnemyStateAddAttackStomp : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public ParticleSystem particle;
    public float animationTime = 1;
    public float vulnerableTime = 0.2f;
    public AudioSource audioDown;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyStomp(characterEnemy.StateMachine, characterEnemy, particle, audioDown, animationTime, vulnerableTime));
        return this;
    }
    public override float Execute()
    {
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyStomp));
        return base.Execute();
    }

}
