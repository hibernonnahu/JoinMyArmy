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
        for (int i = 0; i < prefabs.Length; i++)// Melee spider goes first
        {
            if (prefabs[i].id == 12)
            {
                var temp = prefabs[i];
                prefabs[i] = prefabs[0];
                prefabs[0] = temp;
                break;
            }
        }
        foreach (var enemy in prefabs)
        {
            if (enemy.GetComponent<EnemyStateAddCanBeRecluit>() != null && SaveData.GetInstance().GetValue(SaveDataKey.RECLUIT + enemy.id) == 1)
            {
                var buttonResourcesCreator = Instantiate<ButtonEnemyLocalLevel>(buttonPrefab);
                buttonResourcesCreator.asset = enemy.name;
                buttonResourcesCreator.id = enemy.id;
                buttonResourcesCreator.baseCost = enemy.baseCost;
                buttonResourcesCreator.baseHealth = enemy.baseHealth;
                buttonResourcesCreator.defense = enemy.defense;
                buttonResourcesCreator.strength = enemy.strength;
                buttonResourcesCreator.transform.SetParent(transform);
                buttonResourcesCreator.button.interactable = false;
                list.Add(buttonResourcesCreator);
            }
        }
        LeanTween.delayedCall(0.5f, () =>
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == 12)// Melee Spider
                {
                    EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetInt(4).SetTransform(list[i].transform).SetBool(true));
                   
                }
                list[i].CalculateCost();
            }
        });
    }
}