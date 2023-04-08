using UnityEngine;

public abstract class EnemyStateAddAttack : MonoBehaviour, IEnemyStateAddAttack
{
    public bool isCastMainPower = false;
    public float uIColdDown = 5;
    [Range(0, 10)]
    public float weight = 1;
    public bool removeWhenRecluted = false;
    public virtual float Execute()
    {
        throw new System.NotImplementedException();
    }

    public virtual IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy)
    {
        throw new System.NotImplementedException();
    }

    public float GetWeight()
    {
        return weight;
    }
    public bool IsCastMainPower()
    {
        return isCastMainPower;
    }

    public bool RemoveWhenRecluted()
    {
        return removeWhenRecluted;
    }
}
