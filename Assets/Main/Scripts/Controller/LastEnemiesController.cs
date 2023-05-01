using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastEnemiesController : MonoBehaviour
{
    public Character character;

    public GameObject[] arrows;
    private Action onUpdate = () => { };
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventName.ENEMIES_REMAIN, OnEnemyAmountChange);
        EventManager.StartListening(EventName.ENEMY_KILL, OnEnemyAmountChange);
        EventManager.StartListening(EventName.SPAWN_WAVE, OnEnemyAmountChange);
    }

    private void OnEnemyAmountChange(EventData arg0)
    {
        int count = character.CharacterManager.GetTeam(1).Count;
        if (count <= arrows.Length)
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].SetActive(i < count);
            }
            onUpdate = UpdateArrows;
        }
        else
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].SetActive(false);
            }
            onUpdate = Empty;
        }
    }
   
    private void Empty()
    {     
    }
    private void UpdateArrows()
    {
        int i = 0;
        foreach (var item in character.CharacterManager.GetTeam(1))
        {
            arrows[i].transform.forward = item.transform.position - character.transform.position;
            i++;
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.ENEMIES_REMAIN, OnEnemyAmountChange);
        EventManager.StopListening(EventName.ENEMY_KILL, OnEnemyAmountChange);
        EventManager.StopListening(EventName.SPAWN_WAVE, OnEnemyAmountChange);

    }
    private void Update()
    {
        onUpdate();
    }

}
