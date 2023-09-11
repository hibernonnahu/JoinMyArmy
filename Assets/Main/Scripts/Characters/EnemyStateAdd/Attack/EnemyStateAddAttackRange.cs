using System;
using UnityEngine;

public class EnemyStateAddAttackRange : EnemyStateAddAttack
{
    public float castExpulsion = 0;
    public float expulsionSqrRange = 0;
    public ParticleSystem preCast;
    private CharacterEnemy characterEnemy;
    public float bulletSpeed = 2;
    public BulletPool bulletPool;
    public GameObject[] hideOnShot;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        characterEnemy.AttackState = typeof(StateCharacterEnemyRange);

      
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyRange(characterEnemy.StateMachine, characterEnemy, bulletPool, hideOnShot, bulletSpeed, preCast, castExpulsion, expulsionSqrRange));

        return this;
    }
    public override float Execute()
    {
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyRange));
        return base.Execute();
    }

}
