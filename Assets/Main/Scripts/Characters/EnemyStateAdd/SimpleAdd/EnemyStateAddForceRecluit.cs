using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAddForceRecluit : EnemyStateAddCanBeRecluit, IEnemySimpleAdd
{
    public int formationGrad = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    public override void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.RecluitStateType = typeof(StateCharacterEnemyFollowLeader);
        characterEnemy.CharacterMain = FindObjectOfType<CharacterMain>();
        characterEnemy.FormationGrad = formationGrad;
        characterEnemy.IdleState = characterEnemy.RecluitStateType;
        characterEnemy.NextState = characterEnemy.RecluitStateType;
        characterEnemy.VulnerableTime = 1;
        characterEnemy.StateMachine.ChangeState<StateCharacterEnemyVulnerable>();
        base.Init(characterEnemy);
    }
}
