using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAddCanBeRecluitDirect : EnemyStateAddCanBeRecluit
{
   public float directionMultiplier=1;
    // Start is called before the first frame update
    void Start()
    {

    }


    public override void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.CanBeRecluit = true;
        characterEnemy.EnemyStateAddCanBeRecluit = this;
        meshRenderer = characterEnemy.model.GetComponentInChildren<SkinnedMeshRenderer>();
        materialA = meshRenderer.material;

        characterEnemy.RecluitStateType = typeof(StateCharacterEnemyLead);
        characterEnemy.CharacterMain.StateMachine.AddState(new StateCharacterFollow(characterEnemy.CharacterMain.StateMachine, characterEnemy.CharacterMain));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyLead(characterEnemy.StateMachine, characterEnemy,characterEnemy.CharacterMain.floatingJoystick,characterEnemy.EnemyStateAddInit.EnemyStateAttackModeHandler,characterEnemy.attackDistanceSqr,directionMultiplier));
        if(knockToRecluit)
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyKnocked(characterEnemy.StateMachine, characterEnemy));

    }
    
}
