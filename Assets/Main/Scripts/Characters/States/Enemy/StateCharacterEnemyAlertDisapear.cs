using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyAlertDisapear : StateCharacterEnemyAlert
{
    int disapearCounter=0;
    int disapearIterations;
    float counter;
    public StateCharacterEnemyAlertDisapear(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy,EnemyStateAttackModeController enemyStateAttackModeHandler,float alertDistanceSqr,int disapearIterations) : base(stateMachine, characterEnemy, enemyStateAttackModeHandler,alertDistanceSqr)
    {
        this.disapearIterations = disapearIterations;
    }
    public override void Awake()
    {
        base.Awake();
        disapearCounter++;
        if (disapearCounter > disapearIterations) {
            EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("pop"));
            ChangeState(typeof(StateCharacterEnemyIdle));
            enemy.GeneralParticleHandler.wallHit.transform.SetParent(null);
            enemy.GeneralParticleHandler.wallHit.Play();

            enemy.transform.position = Vector3.right * 99999;
            enemy.Rigidbody.isKinematic = true;
            
        }
    }

    public override void Sleep()
    {
        
    }

    public override bool GetHit(float damage, Character attacker)
    {
        return false;
    }
    public override void Update()
    {
        base.Update();
    }
}
