

public interface  IEnemyStateAddAttack 
{
    IEnemyStateAddAttack InitStates(CharacterEnemy characterEnemy);
    float Execute();
    float GetWeight();
    bool IsCastMainPower();
    bool RemoveWhenRecluted();
    bool UseRedDotUI();
}
