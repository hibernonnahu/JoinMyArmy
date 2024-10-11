

using System;
using System.Collections.Generic;

public class EnemyStateAttackModeController
{
    private IEnemyStateAddAttack[] typeAttack;
    List<float> weights = new List<float>();
  
    private CharacterEnemy enemy;
    public  EnemyStateAttackModeController(CharacterEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Init()
    {
        //find IEnemyStateAddAttack
        var enemyStateAttack = enemy.gameObject.GetComponents<IEnemyStateAddAttack>();
        List<IEnemyStateAddAttack> types=new List<IEnemyStateAddAttack>();
       
        if(enemyStateAttack.Length==0) throw new Exception("No EnemyStateAddAttack " +enemy.gameObject.name);
        foreach (var component in enemyStateAttack)
        {
            types.Add(component.InitStates(enemy));
        }
        typeAttack = types.ToArray();
        
    }
    public void OnRecluit()
    {
        List<IEnemyStateAddAttack> types = new List<IEnemyStateAddAttack>();
       
        foreach (var component in typeAttack)
        {
            if (!component.RemoveWhenRecluted())
            {
                types.Add(component);
               
            }
            component.ExecuteWhenRecluted();
            if (component.IsCastMainPower())
            {
                enemy.SetCastMainPower(component);
            }
        }

        typeAttack = types.ToArray();
       
    }
    internal void Attack()
    {
        float sumWeight = 0;
        int count = 0;
        weights.Clear();
        foreach (var component in typeAttack)
        {
         
            weights.Add(component.GetWeight());
            sumWeight += component.GetWeight();
        }
        if (sumWeight > 0)
        {

            float random = UnityEngine.Random.Range(0, sumWeight);
            while (random > weights[count])
            {
                random -= weights[count];
                count++;
            }
            typeAttack[count].Execute();
        }
    }
}
