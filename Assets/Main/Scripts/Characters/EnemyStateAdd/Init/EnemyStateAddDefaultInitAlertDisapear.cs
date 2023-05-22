

public class EnemyStateAddDefaultInitAlertDisapear : EnemyStateAddDefaultInitAlert
{
    public int disapearIterations = 10;
    override internal void Init(CharacterEnemy characterEnemy)
    {
        character = characterEnemy;
        InitAttackHandler(characterEnemy);
        characterEnemy.IdleState = typeof(StateCharacterEnemyAlertDisapear);
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyAlertDisapear(characterEnemy.StateMachine, characterEnemy, enemyStateAttackModeHandler, alertDistanceSqr,disapearIterations));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyGoBack(characterEnemy.StateMachine, characterEnemy));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyIdle(characterEnemy.StateMachine, characterEnemy));

        enemyStateAttackModeHandler.Init();
    }
    
}
