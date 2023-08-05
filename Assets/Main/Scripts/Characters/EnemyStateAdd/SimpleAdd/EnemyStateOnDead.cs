using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateOnDead : MonoBehaviour, IEnemySimpleAdd
{
  
   
    public void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.OnDeadActionList.Add(ExecuteOnDead);
    }
    protected virtual void ExecuteOnDead()
    {

    }
   
}
