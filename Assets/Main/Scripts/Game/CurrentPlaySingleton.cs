using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlaySingleton
{

    public int[] initialChapter ;

    public int book = 1;
    public int chapter = 1;
    public int level = 1;

    private List<int>[] party;
    public int currentCharacterId = 0;
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
            instance.initialChapter = new int[3];
            instance.party = new List<int>[3];
            for (int i = 0; i < 3; i++)
            {
                instance.party[i] = new List<int>();
                instance.initialChapter[i] = 1;
            }
        }
        return instance;
    }
    public int GetInitialChapter()
    {
        return instance.initialChapter[book-1];
    }
    public int GetInitialChapter(int book)
    {
        return instance.initialChapter[book-1];
    }
    internal string GetArmyString(int index)
    {
        string code = "";
        if (party != null)
        {
            foreach (var item in party[index])
            {
                code += item + "_";
            }
            if (code.Length > 0)
            {
                code.Remove(code.Length - 1, 1);
            }
        }
        return code;
    }
    public string GameType()
    {
        return SaveData.GetInstance().GetMetric(SaveDataKey.GAME_TYPE, "Campaign");
    }
    public void UpdateArmy(string code,bool forceSave=false)
    {
        if (GameType() == "Campaign"|| forceSave)
        {
            string[] split = code.Split("_");
            party[currentCharacterId] = new List<int>();
            foreach (var item in split)
            {
                if (item != "")
                    party[currentCharacterId].Add(int.Parse(item));
            }
        }
    }

    internal void SaveGamePlay(CharacterMain characterMain)
    {
        if (GameType() == "Campaign")
        {
            SaveMainCharacter(characterMain);
            SaveParty();
        }
    }

    private void SaveMainCharacter(CharacterMain characterMain)
    {
        characterMainStats = new int[] { characterMain.level, characterMain.XpController.CurrentXp, (int)characterMain.CurrentHealth };
        skillController = characterMain.SkillController;
    }

    private void SaveParty()
    {
        party[currentCharacterId] = new List<int>();
        var rc = GameObject.FindObjectOfType<CharacterMain>().recluitController;
        if (rc)
        {
            var currentTeam = rc.Enemies;
            for (int i = 0; i < currentTeam.Length; i++)
            {
                if (currentTeam[i] != null)
                {
                    party[currentCharacterId].Add(currentTeam[i].id);
                    party[currentCharacterId].Add(Mathf.RoundToInt(currentTeam[i].CurrentHealth));
                    party[currentCharacterId].Add(i);
                }
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
        if (GameType() == "Campaign")
        {
            currentCharacterId = characterMain.id;
            LoadMainCharacter(characterMain);
            LoadParty(characterMain);
        }
    }
    public void AddSkill<T>(Type type) where T : ISkill
    {
        if (skillController != null)
        {
            T t = (T)Activator.CreateInstance(type);
            skillController.AddSkill(t);
        }
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
            //characterMain.scrollViewSkillUI.Refresh();
            foreach (var skill in skillController.Skills)
            {
                characterMain.OnNewSkillAdded(skill.GetName());
            }

        }
    }

    internal void RefillHP()
    {
        if (characterMainStats != null)
            characterMainStats[2] = (int)(GameObject.FindObjectOfType<CharacterMain>().Health);
    }

    private void LoadParty(CharacterMain characterMain)
    {
        var loader = GameObject.FindObjectOfType<LevelJsonLoader>();

        if (party != null)
        {
            for (int i = 0; i < party[currentCharacterId].Count; i += 3)
            {
                CharacterEnemy prefab = loader.GetCharacter(party[currentCharacterId][i]);
                LoadEnemy(prefab, characterMain, i);
            }
        }
    }

    private void LoadEnemy(CharacterEnemy prefab, CharacterMain characterMain, int i)//0-id 1-currentHealth 2-UIposition 
    {
        currentCharacterId = characterMain.id;
        var characterManager = GameObject.FindObjectOfType<CharacterManager>();
        CharacterEnemy enemy = GameObject.Instantiate<CharacterEnemy>(prefab, (Vector3.right * (characterMain.transform.position.x + 0.4f * i) + Vector3.forward * characterMain.transform.position.z), Quaternion.identity);
        enemy.name += "Old";
        enemy.CharacterMain = characterMain;
        enemy.CharacterManager = characterManager;
        enemy.enabled = true;
        enemy.RecluitInit();
        enemy.UpdateStatesToFollow();
        characterManager.GoMainTeam(enemy, true, party[currentCharacterId][i + 2]);
        enemy.CurrentHealth = party[currentCharacterId][i + 1];
        enemy.HealthBarController.UpdateBar();
        enemy.model.transform.forward = Vector3.forward;
    }
}
