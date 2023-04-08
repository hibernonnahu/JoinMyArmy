

using System;
using System.Collections.Generic;

public class EnemyStateAttackModeHandler 
{
    private IEnemyStateAddAttack[] typeAttack;
    private float[] weight;
    float sumWeight = 0;
    private CharacterEnemy enemy;
    public  EnemyStateAttackModeHandler(CharacterEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Init()
    {
        //find IEnemyStateAddAttack
        var enemyStateAttack = enemy.gameObject.GetComponents<IEnemyStateAddAttack>();
        List<IEnemyStateAddAttack> types=new List<IEnemyStateAddAttack>();
        List<float> weights = new List<float>();
        if(enemyStateAttack.Length==0) throw new Exception("No EnemyStateAddAttack " +enemy.gameObject.name);
        foreach (var component in enemyStateAttack)
        {
            types.Add(component.InitStates(enemy));
            weights.Add(component.GetWeight());
            sumWeight += component.GetWeight();
        }
        typeAttack = types.ToArray();
        weight = weights.ToArray();
    }
    public void OnRecluit()
    {
        List<IEnemyStateAddAttack> types = new List<IEnemyStateAddAttack>();
        List<float> weights = new List<float>();
        sumWeight = 0;
        foreach (var component in typeAttack)
        {
            if (!component.RemoveWhenRecluted())
            {
                types.Add(component);
                weights.Add(component.GetWeight());
                sumWeight += component.GetWeight();
            }
            if (component.IsCastMainPower())
            {
                enemy.SetCastMainPower(component);
            }
        }

        typeAttack = types.ToArray();
        weight = weights.ToArray();
    }
    internal void Attack()
    {
        int count = 0;
        float random = UnityEngine.Random.Range(0, sumWeight);
        while (random > weight[count])
        {
            random -= weight[count];
            count++;
        }
        typeAttack[count].Execute();
    }
}
