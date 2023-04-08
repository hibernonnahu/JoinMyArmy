

public class EnemyStateAddDefaultInitAlert : EnemyStateAddDefaultInitIdle  
{
    override internal void Init(CharacterEnemy characterEnemy)
    {
        InitAttackHandler(characterEnemy);
        characterEnemy.IdleState = typeof(StateCharacterEnemyAlert);
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyAlert(characterEnemy.StateMachine, characterEnemy, enemyStateAttackModeHandler));
        enemyStateAttackModeHandler.Init();
    }
}
