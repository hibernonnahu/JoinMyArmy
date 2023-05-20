using System;
using UnityEngine;

public class EnemyStateAddAttackTeleport : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
  
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyTeleport(characterEnemy.StateMachine,characterEnemy));
      
        return this;
    }
    public override float Execute()
    {
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyTeleport));
        return base.Execute();
    }

}
