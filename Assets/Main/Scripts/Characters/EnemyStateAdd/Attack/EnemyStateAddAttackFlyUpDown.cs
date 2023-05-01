using System;
using UnityEngine;

public class EnemyStateAddAttackFlyUpDown : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public ParticleSystem jumpUpParticle;
    public ParticleSystem jumpDownParticle;
    public GameObject groundShadow;
    public AudioSource audioUp;
    public AudioSource audioDown;
    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        groundShadow.SetActive(false);
        groundShadow.transform.SetParent(null);
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyFlyUp(characterEnemy.StateMachine, characterEnemy,jumpUpParticle,groundShadow,audioUp));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyFlyDown(characterEnemy.StateMachine, characterEnemy,jumpDownParticle,groundShadow,audioDown));
      
        return this;
    }
    public override float Execute()
    {
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyFlyUp));
        return base.Execute();
    }

}
