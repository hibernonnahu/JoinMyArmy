
public class EnemyStateAddDefaultInitIdle : EnemyStateAddDefaultInit 
{
    override internal void Init(CharacterEnemy characterEnemy)
    {
        characterEnemy.IdleState = typeof(StateCharacterEnemyIdle);
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyIdle(characterEnemy.StateMachine, characterEnemy));
    }
}
