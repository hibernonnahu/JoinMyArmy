using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateAddDefaultInit : MonoBehaviour
{
    protected EnemyStateAttackModeController enemyStateAttackModeHandler;
    virtual internal void Init(CharacterEnemy characterEnemy)
    {
       
    }
    protected void InitAttackHandler(CharacterEnemy characterEnemy)
    {
        enemyStateAttackModeHandler = new EnemyStateAttackModeController(characterEnemy);
    }
    public void OnRecluit()
    {
        enemyStateAttackModeHandler.OnRecluit();
    }
    public virtual void SetDefaultInit()
    {
        
    }
}
