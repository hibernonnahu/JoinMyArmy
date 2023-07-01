using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAddCanBeRecluit : MonoBehaviour, IEnemySimpleAdd
{
    private SkinnedMeshRenderer meshRenderer;
    private Material materialA;
    public Material materialB;
    // Start is called before the first frame update
    void Start()
    {

    }


    public virtual void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.CanBeRecluit = true;
        characterEnemy.EnemyStateAddCanBeRecluit = this;
        meshRenderer = characterEnemy.model.GetComponentInChildren<SkinnedMeshRenderer>();
        materialA = meshRenderer.material;
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyHelpAttack(characterEnemy.StateMachine, characterEnemy));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyFollowLeader(characterEnemy.StateMachine, characterEnemy));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyKnocked(characterEnemy.StateMachine, characterEnemy));

    }
    public void SetMaterialA()
    {
        meshRenderer.material = materialA;
    }
    public void SetMaterialB()
    {
        meshRenderer.material = materialB;
    }
}
