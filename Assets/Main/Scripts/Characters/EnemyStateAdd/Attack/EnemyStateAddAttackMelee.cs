using System;
using UnityEngine;

public class EnemyStateAddAttackMelee : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        characterEnemy.AttackState = typeof(StateCharacterEnemyMelee);
        characterEnemy.HelpAttack = true;
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyChase(characterEnemy.StateMachine, characterEnemy));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyMelee(characterEnemy.StateMachine, characterEnemy));

        return this;
    }
    public override float Execute()
    {
        characterEnemy.NextState = typeof(StateCharacterEnemyMelee);
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyChase));
        return base.Execute();
    }

}
