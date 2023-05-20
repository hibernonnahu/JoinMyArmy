

public class EnemyStateAddDefaultInitAlert : EnemyStateAddDefaultInitIdle
{
    public float alertDistanceSqr = 300;
    override internal void Init(CharacterEnemy characterEnemy)
    {
        character = characterEnemy;
        InitAttackHandler(characterEnemy);
        characterEnemy.IdleState = typeof(StateCharacterEnemyAlert);
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyAlert(characterEnemy.StateMachine, characterEnemy, enemyStateAttackModeHandler, alertDistanceSqr));
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyGoBack(characterEnemy.StateMachine, characterEnemy));

        enemyStateAttackModeHandler.Init();
    }
    public override void SetDefaultInit()
    {

        character.IdleState = typeof(StateCharacterEnemyAlert);
    }
}
