using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSkillUI : MonoBehaviour
{
    public GameObject buttonPrefab;

    private List<string> list;

    private void Awake()
    {
        list = new List<string>();
        EventManager.StartListening(EventName.HIDE_CHARACTER_UI, onHide);
    }

    private void onHide(EventData arg0)
    {
        gameObject.SetActive(!arg0.boolData);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.HIDE_CHARACTER_UI, onHide);

    }

    public void Add(string skill)
    {
        if (list.Contains(skill))
        {
            foreach (Transform item in transform)
            {
                if (item.gameObject.name == skill)
                {
                    var t = item.GetComponentInChildren<Text>();
                    if (t.text == "")
                    {
                        t.text = "2";
                    }
                    else
                    {
                        int value = int.Parse(t.text);
                        value++;
                        t.text = value.ToString();
                    }
                    break;
                }
            }
        }
        else
        {
            list.Add(skill);
            var buttonResourcesCreator = Instantiate<GameObject>(buttonPrefab);
            buttonResourcesCreator.name = skill;
            buttonResourcesCreator.GetComponent<Image>().sprite = Resources.Load<Sprite>(("Skills/" + skill));
            buttonResourcesCreator.transform.SetParent(transform);
            buttonResourcesCreator.transform.localScale = Vector3.one;
        }
    }
    private void Start()
    {
        //var prefabs = Resources.LoadAll<CharacterEnemy>(resourcesFolder);
        //List<ButtonEnemyLocalLevel> list = new List<ButtonEnemyLocalLevel>();
        //foreach (var enemy in prefabs)
        //{
        //    if (enemy.GetComponent<EnemyStateAddCanBeRecluit>() != null && SaveData.GetInstance().GetValue(SaveDataKey.RECLUIT + enemy.id) == 1)
        //    {
        //        var buttonResourcesCreator = Instantiate<ButtonEnemyLocalLevel>(buttonPrefab);
        //        buttonResourcesCreator.asset = enemy.name;
        //        buttonResourcesCreator.id = enemy.id;
        //        buttonResourcesCreator.baseCost = enemy.baseCost;
        //        buttonResourcesCreator.transform.SetParent(transform);
        //        list.Add(buttonResourcesCreator);
        //    }
        //}
        //for (int i = 0; i < list.Count; i++)
        //{
        //    if (list[i].id == 12)// Melee Spider
        //    {
        //        EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetInt(4).SetTransform(list[i].transform).SetBool(true).SetInt2((list.Count+1) % 2));
        //        break;
        //    }
        //}
    }

    internal void Refresh()
    {
        list.Clear();
    }
}