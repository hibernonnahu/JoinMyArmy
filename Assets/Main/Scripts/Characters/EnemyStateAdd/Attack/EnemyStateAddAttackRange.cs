using System;
using UnityEngine;

public class EnemyStateAddAttackRange : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public float bulletSpeed = 2;
    public BulletPool bulletPool;
    public GameObject[] hideOnShot;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        characterEnemy.HelpAttack = true;
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyRange(characterEnemy.StateMachine, characterEnemy, bulletPool, hideOnShot, bulletSpeed));

        return this;
    }
    public override float Execute()
    {
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyRange));
        return base.Execute();
    }

}
