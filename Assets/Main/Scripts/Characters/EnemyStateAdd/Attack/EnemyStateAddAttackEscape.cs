using System;
using UnityEngine;

public class EnemyStateAddAttackEscape : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        
        characterEnemy.AttackState = characterEnemy.IdleState;

        characterEnemy.StateMachine.AddState(new StateCharacterEnemyEscape(characterEnemy.StateMachine, characterEnemy));

        return this;
    }
    public override float Execute()
    {
        characterEnemy.NextState = characterEnemy.IdleState;
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyEscape));
        return base.Execute();
    }

}
