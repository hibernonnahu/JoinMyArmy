
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Character[] prefabs;
    private List<Character>[] teamArray;
    private CharacterMain characterMain;
    private void Awake()
    {
        Init(2);
    }
    private void Init(int teamsAmount)
    {
        teamArray = new List<Character>[teamsAmount];
        for (int i = 0; i < teamArray.Length; i++)
        {
            teamArray[i] = new List<Character>();
        }
        
        MockInit();
    }

    private void MockInit()
    {
        characterMain = Instantiate<Character>(prefabs[0]).GetComponent<CharacterMain>();
        characterMain.transform.position = Vector3.forward * -30;
        AddCharacterMain(characterMain);
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.forward * -25;
        AddCharacterEnemy(Instantiate<Character>(prefabs[1]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.forward * 10;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 5;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * -6;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 8;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 4;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 5 + Vector3.forward * 10;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 6 + Vector3.forward * 10;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 9 + Vector3.forward * 10;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * -3 + Vector3.forward * 10;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 5 + Vector3.forward * 50;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 6 + Vector3.forward * 30;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * 9 + Vector3.forward * 50;
        AddCharacterEnemy(Instantiate<Character>(prefabs[2]).GetComponent<CharacterEnemy>(), 1, characterMain).transform.position += Vector3.right * -3 + Vector3.forward * 30;

    }

    internal Character GetClosestEnemyInRange(int attackTeam, float attackDistanceSqr, Vector3 position)
    {
        Character current = null;
        float closest = float.MaxValue;
        List<Character> enemyList = GetEnemyListForTeam(attackTeam);
        foreach (var enemy in enemyList)
        {
            float dist = (position - enemy.transform.position).sqrMagnitude;
            if (dist < attackDistanceSqr && dist < closest)
            {
                closest = dist;
                current = enemy;
            }
        }
        return current;
    }

    private List<Character> GetEnemyListForTeam(int team)
    {
        return teamArray[(team + 1) % 2];
    }

    private void AddCharacterMain(CharacterMain character)
    {
        character.CharacterManager = this;
        teamArray[0].Add(character);
    }
    private CharacterEnemy AddCharacterEnemy(CharacterEnemy enemy, int team, CharacterMain characterMain)
    {
        enemy.CharacterManager = this;
        teamArray[team].Add(enemy);
        enemy.CharacterMain = characterMain;
        return enemy;
    }

    public void RemoveCharacter(Character character)
    {
        if (character.team == 0)
        {
            characterMain.recluitHandler.Remove(character);
        }
        teamArray[character.team].Remove(character);
    }
    public void GoMainTeam(CharacterEnemy characterEnemy)
    {
        teamArray[0].Add(characterEnemy);
        characterEnemy.Revive();
        characterMain.recluitHandler.Recluit(characterEnemy);
    }
}
