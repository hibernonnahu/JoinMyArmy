using System;
using UnityEngine;

public class EnemyStateAddAttackMelee : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public ParticleSystem particleHit;
    public AudioSource swing;
    public AudioSource hit;
    public float forceIdleAfterSeconds = 5;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        characterEnemy.AttackState = typeof(StateCharacterEnemyMelee);
       
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyChase(characterEnemy.StateMachine, characterEnemy, GetComponent<EnemyStateAddDefaultInitAlert>().alertDistanceSqr, forceIdleAfterSeconds));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyMelee(characterEnemy.StateMachine, characterEnemy,particleHit,swing,hit));

        return this;
    }
    public override float Execute()
    {
        characterEnemy.NextState = typeof(StateCharacterEnemyMelee);
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyChase));
        return base.Execute();
    }

}
