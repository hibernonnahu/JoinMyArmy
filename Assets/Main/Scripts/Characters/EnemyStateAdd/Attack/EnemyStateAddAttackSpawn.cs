using System;
using UnityEngine;

public class EnemyStateAddAttackSpawn : EnemyStateAddAttack
{
    private CharacterEnemy characterEnemy;
    public ParticleSystem particles;
    public new AudioSource audio;
    public int[] spawnEnemiesIds;
    public int amount;
    public float castDuration = 30;
    public float castDurationAfter = 30;
    private CharacterEnemy[] spawnEnemies;

    public override IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy) //where Type : StateCharacter
    {
        this.characterEnemy = characterEnemy;
        spawnEnemies = new CharacterEnemy[amount];
        var characterManager = GameObject.FindObjectOfType<CharacterManager>();
        if (characterManager != null)
        {
            for (int i = 0; i < spawnEnemies.Length; i++)
            {
                spawnEnemies[i] = GameObject.Instantiate<CharacterEnemy>(characterManager.Loader.GetCharacter(spawnEnemiesIds[UnityEngine.Random.Range(0, spawnEnemiesIds.Length)]));
                spawnEnemies[i].extra = true;
                spawnEnemies[i].extraAlertRange = characterEnemy.extraAlertRange;
                spawnEnemies[i].xp = 0;
                spawnEnemies[i].team = characterEnemy.team;
                spawnEnemies[i].coins = 0;
                spawnEnemies[i].level = characterEnemy.level;
                spawnEnemies[i].CurrentHealth = 0;
                spawnEnemies[i].UpdateStatsOnLevel(characterEnemy.level, true, false);
                spawnEnemies[i].gameObject.SetActive(false);
                spawnEnemies[i].IsDead = true;
                spawnEnemies[i].CharacterManager = characterManager;
            }

            characterEnemy.StateMachine.AddState(new StateCharacterEnemySpawn(characterEnemy.StateMachine, characterEnemy, particles, audio, spawnEnemies, castDuration, castDurationAfter));
        }
        return this;
    }
    public override void ExecuteWhenRecluted()
    {
        for (int i = 0; i < spawnEnemies.Length; i++)
        {
            if (!spawnEnemies[i].IsDead)
                spawnEnemies[i].Kill();
        }
    }
    public override float Execute()
    {
        characterEnemy.StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemySpawn));
        return base.Execute();
    }

}
