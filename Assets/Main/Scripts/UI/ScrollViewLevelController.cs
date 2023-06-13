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
        List<ButtonEnemyLocalLevel> list = new List<ButtonEnemyLocalLevel>();
        foreach (var enemy in prefabs)
        {
            if (enemy.GetComponent<EnemyStateAddCanBeRecluit>() != null && SaveData.GetInstance().GetValue(SaveDataKey.RECLUIT + enemy.id) == 1)
            {
                var buttonResourcesCreator = Instantiate<ButtonEnemyLocalLevel>(buttonPrefab);
                buttonResourcesCreator.asset = enemy.name;
                buttonResourcesCreator.id = enemy.id;
                buttonResourcesCreator.baseCost = enemy.baseCost;
                buttonResourcesCreator.transform.SetParent(transform);
                list.Add(buttonResourcesCreator);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].id == 12)// Melee Spider
            {
                EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetInt(4).SetTransform(list[i].transform).SetBool(true).SetInt2((list.Count+1) % 2));
                break;
            }
        }
    }
}