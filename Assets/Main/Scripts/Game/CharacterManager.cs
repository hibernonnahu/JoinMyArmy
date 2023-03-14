
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
        AddCharacterMain(characterMain);
        AddCharacterEnemy(Instantiate<Character>(prefabs[1]).GetComponent<CharacterEnemy>(), 1,characterMain).transform.position+=Vector3.forward*10;

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

    public void GoMainTeam(CharacterEnemy characterEnemy)
    {
        teamArray[characterEnemy.team].Remove(characterEnemy);
        teamArray[0].Add(characterEnemy);
        characterEnemy.team = 0;
        characterEnemy.speed = characterMain.speed;
       
    }
}
