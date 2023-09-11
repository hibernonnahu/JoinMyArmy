using System;
using UnityEngine;

public class EnemyStateAddAttackGoBack : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
       
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyGoBack(characterEnemy.StateMachine, characterEnemy));

        return this;
    }
    public override float Execute()
    {
        characterEnemy.NextState = characterEnemy.IdleState;
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyGoBack));
        return base.Execute();
    }

}
