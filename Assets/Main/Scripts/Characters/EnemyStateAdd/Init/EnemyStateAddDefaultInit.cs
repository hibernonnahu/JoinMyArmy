using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateAddDefaultInit : MonoBehaviour
{
    protected EnemyStateAttackModeHandler enemyStateAttackModeHandler;
    virtual internal void Init(CharacterEnemy characterEnemy)
    {
       
    }
    protected void InitAttackHandler(CharacterEnemy characterEnemy)
    {
        enemyStateAttackModeHandler = new EnemyStateAttackModeHandler(characterEnemy);
    }
    public void OnRecluit()
    {
        enemyStateAttackModeHandler.OnRecluit();
    }
}
