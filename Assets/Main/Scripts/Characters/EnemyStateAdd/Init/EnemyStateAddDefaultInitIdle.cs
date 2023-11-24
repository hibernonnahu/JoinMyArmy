
public class EnemyStateAddDefaultInitIdle : EnemyStateAddDefaultInit
{
    protected Character character;
    override internal void Init(CharacterEnemy characterEnemy)
    {
        this.character = characterEnemy;
        characterEnemy.IdleState = typeof(StateCharacterEnemyIdle);
        characterEnemy.AttackState = typeof(StateCharacterEnemyIdle);
        characterEnemy.StateMachine.AddState(new StateCharacterEnemyIdle(characterEnemy.StateMachine, characterEnemy));
    }
    public override void SetDefaultInit()
    {
        character.IdleState = typeof(StateCharacterEnemyIdle);
    }
}
