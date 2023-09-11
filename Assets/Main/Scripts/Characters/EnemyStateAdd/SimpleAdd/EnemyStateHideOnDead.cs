using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateHideOnDead : EnemyStateOnDead
{
    public GameObject[] toHide;
  
    protected override void ExecuteOnDead()
    {
        foreach (var item in toHide)
        {
            item.SetActive(false);
        }

    }

}
