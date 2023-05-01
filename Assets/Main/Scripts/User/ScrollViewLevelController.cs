using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewLevelController : MonoBehaviour
{
    public ButtonEnemyLocalLevel buttonPrefab;
    public string resourcesFolder = "";

    private void Start()
    {
        var prefabs = Resources.LoadAll<CharacterEnemy>(resourcesFolder);
        foreach (var enemy in prefabs)
        {
            if (enemy.GetComponent<EnemyStateAddCanBeRecluit>() != null)
            {
                var buttonResourcesCreator = Instantiate<ButtonEnemyLocalLevel>(buttonPrefab);
                buttonResourcesCreator.asset = enemy.name;
                buttonResourcesCreator.id = enemy.id;
                buttonResourcesCreator.baseCost = enemy.baseCost;
                buttonResourcesCreator.transform.SetParent(transform);
            }
        }
    }
}