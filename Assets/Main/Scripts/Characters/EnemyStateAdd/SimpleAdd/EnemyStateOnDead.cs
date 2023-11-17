using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateOnDead : MonoBehaviour, IEnemySimpleAdd
{
    private Action onDead;
   
    public void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.OnDeadActionList.Add(ExecuteOnDead);
    }
    protected virtual void ExecuteOnDead()
    {
        onDead();
    }
    public void SetAction(Action action)
    {
        onDead = action;
    }
   
}
