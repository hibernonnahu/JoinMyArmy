using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlaySingleton 
{
    public int book = 1;
    public int chapter = 1;
    public int level = 1;
    private List<int> party;
    static CurrentPlaySingleton instance;
    public static CurrentPlaySingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new CurrentPlaySingleton();
            var loader = GameObject.FindAnyObjectByType<LevelJsonLoader>();
            instance.book = loader.book;
            instance.chapter = loader.chapter;
            instance.level = loader.level;
        }
        return instance;
    }

    internal void SaveParty()
    {
        party = new List<int>();
        var currentTeam = GameObject.FindObjectOfType<RecluitController>().Enemies;
        for (int i = 0; i < currentTeam.Length; i++)
        {
            if (currentTeam[i] != null)
            {
                party.Add(currentTeam[i].id);
                party.Add(Mathf.RoundToInt(currentTeam[i].CurrentHealth));
                party.Add(i);
            }    
        }
    }

    public void LoadParty(CharacterMain characterMain)
    {
        var loader = GameObject.FindObjectOfType<LevelJsonLoader>();
       
        if (party != null)
        {
           
            for (int i = 0; i < party.Count; i+=3)
            {
                CharacterEnemy prefab = loader.GetCharacter(party[i]);
                LoadEnemy(prefab,characterMain,i);
                
            }
        }
    }

    private void LoadEnemy(CharacterEnemy prefab,CharacterMain characterMain,int i)//0-id 1-currentHealth 2-UIposition 
    {
        var characterManager = GameObject.FindObjectOfType<CharacterManager>();
        CharacterEnemy enemy = GameObject.Instantiate<CharacterEnemy>(prefab, (Vector3.right * (characterMain.transform.position.x + i) + Vector3.forward * characterMain.transform.position.z), Quaternion.identity);
        enemy.name += "Old";
        enemy.CharacterMain = characterMain;
        enemy.CharacterManager = characterManager;
        enemy.enabled = true;
        characterManager.GoMainTeam(enemy, true, party[i + 2]);
        enemy.UpdateStatesToFollow();
        enemy.CurrentHealth = party[i + 1];
        enemy.HealthBarController.UpdateBar();
    }
}
