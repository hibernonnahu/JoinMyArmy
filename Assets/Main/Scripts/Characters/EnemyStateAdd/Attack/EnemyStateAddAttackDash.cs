using System;
using UnityEngine;

public class EnemyStateAddAttackDash : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public ParticleSystem footParticle;
    public ParticleSystem collisionParticle;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyDash(characterEnemy.StateMachine, characterEnemy,footParticle,collisionParticle));
      
        return this;
    }
    public override float Execute()
    {
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyDash));
        return base.Execute();
    }

}
