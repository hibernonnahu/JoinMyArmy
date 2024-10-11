using System;
using UnityEngine;

public abstract class EnemyStateAddAttack : MonoBehaviour, IEnemyStateAddAttack
{
    public bool isCastMainPower = false;
    public bool redDotUITip = false;
    public float uIColdDown = 5;
    [Range(0, 10)]
    public float weight = 1;
    public bool removeWhenRecluted = false;
    private Action onUpdate = () => { };
    private float counter;
    private int weightMultiplier = 1;
    public virtual float Execute()
    {
        counter = uIColdDown;
        weightMultiplier = 0;
        onUpdate = () => {
            counter -= Time.deltaTime;
            if (counter < 0)
            {
                weightMultiplier = 1;
                onUpdate = () => { };
            }
        };
        return uIColdDown;
    }

    public virtual IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy)
    {
        throw new System.NotImplementedException();
    }

    public float GetWeight()
    {
        return weight*weightMultiplier;
    }
    public bool IsCastMainPower()
    {
        return isCastMainPower;
    }

    public bool RemoveWhenRecluted()
    {
        return removeWhenRecluted;
    }
    private void Update()
    {
        onUpdate();
    }

    public bool UseRedDotUI()
    {
        return redDotUITip;
    }

    public virtual void ExecuteWhenRecluted()
    {
        
    }
}
