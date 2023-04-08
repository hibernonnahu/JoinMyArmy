using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAddCanBeRecluit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    internal void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.CanBeRecluit = true;
        characterEnemy.MeshRenderer = characterEnemy.model.GetComponentInChildren<SkinnedMeshRenderer>();
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyHelpAttack(characterEnemy.StateMachine, characterEnemy));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyFollowLeader(characterEnemy.StateMachine, characterEnemy));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyKnocked(characterEnemy.StateMachine, characterEnemy));

        Destroy(this);
    }
}
