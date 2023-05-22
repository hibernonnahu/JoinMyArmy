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
    private int[] characterMainStats;
    private SkillController skillController;
    static CurrentPlaySingleton instance;
    public int coins = 0;
    public int dificulty = 0;

    public bool animateTransition = false;

    public static CurrentPlaySingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new CurrentPlaySingleton();
            var loader = GameObject.FindAnyObjectByType<LevelJsonLoader>();
            if (loader)
            {
                instance.book = loader.book;
                instance.chapter = loader.chapter;
                instance.level = loader.level;
            }
        }
        return instance;
    }

    internal void SaveGamePlay(CharacterMain characterMain)
    {
        SaveMainCharacter(characterMain);
        SaveParty();
    }

    private void SaveMainCharacter(CharacterMain characterMain)
    {
        characterMainStats = new int[] { characterMain.level, characterMain.XpController.CurrentXp, (int)characterMain.CurrentHealth };
        skillController = characterMain.SkillController;
    }

    private void SaveParty()
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

    internal void Reset()
    {
        int chapter = instance.chapter;
        int book = instance.book;
        var party = instance.party;
        var animateTransition = instance.animateTransition;
        instance = null;
        GetInstance().level = 1;

        instance.party = party;
        instance.chapter = chapter;
        instance.book = book;
        instance.animateTransition = animateTransition;
    }

    internal void AnimateTransition()
    {
        throw new NotImplementedException();
    }

    public void ResetStats()
    {
        skillController = null;
        characterMainStats = null;
        coins = 0;
    }

    public void LoadGamePlay(CharacterMain characterMain)
    {
        LoadMainCharacter(characterMain);
        LoadParty(characterMain);
    }

    private void LoadMainCharacter(CharacterMain characterMain)
    {
        if (characterMainStats != null)//0-lvl 1-currentXp 2-currentHealth
        {
            skillController.Character = characterMain;
            characterMain.SkillController = skillController;
            characterMain.model.transform.forward = Vector3.forward;

            characterMain.level = characterMainStats[0];
            characterMain.XpController.ForceXp(characterMainStats[1]);
            characterMain.CurrentHealth = (characterMainStats[2]);
        }
    }

    private void LoadParty(CharacterMain characterMain)
    {
        var loader = GameObject.FindObjectOfType<LevelJsonLoader>();

        if (party != null)
        {
            for (int i = 0; i < party.Count; i += 3)
            {
                CharacterEnemy prefab = loader.GetCharacter(party[i]);
                LoadEnemy(prefab, characterMain, i);
            }
        }
    }

    private void LoadEnemy(CharacterEnemy prefab, CharacterMain characterMain, int i)//0-id 1-currentHealth 2-UIposition 
    {
        var characterManager = GameObject.FindObjectOfType<CharacterManager>();
        CharacterEnemy enemy = GameObject.Instantiate<CharacterEnemy>(prefab, (Vector3.right * (characterMain.transform.position.x + 0.4f * i) + Vector3.forward * characterMain.transform.position.z), Quaternion.identity);
        enemy.name += "Old";
        enemy.CharacterMain = characterMain;
        enemy.CharacterManager = characterManager;
        enemy.enabled = true;
        characterManager.GoMainTeam(enemy, true, party[i + 2]);
        enemy.UpdateStatesToFollow();
        enemy.CurrentHealth = party[i + 1];
        enemy.HealthBarController.UpdateBar();
        enemy.model.transform.forward = Vector3.forward;

    }
}
