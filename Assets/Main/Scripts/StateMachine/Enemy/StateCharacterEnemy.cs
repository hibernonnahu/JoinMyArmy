using UnityEngine;


public class StateCharacterEnemy : StateCharacter
{
    protected CharacterEnemy enemy;
    public StateCharacterEnemy(StateMachine<StateCharacter> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {
        this.enemy = characterEnemy;
    }

    public override void UpdateMovement(float x, float y)
    {
        character.model.transform.forward = CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y);
        character.Rigidbody.velocity = character.speed * character.model.transform.forward;
    }
    public override void GetHit(float damage)
    {
        enemy.CurrentHealth -= damage;
        if (enemy.CurrentHealth <= 0)
        {
            enemy.CurrentHealth = 0;
            if (enemy.canBeRecluit && enemy.CharacterMain.recluitHandler.CanRecluit())
            {
                enemy.RecluitIconHandler.KnockOut();
                ChangeState(typeof(StateCharacterEnemyKnocked));
            }
            else
            {
                enemy.CharacterManager.RemoveCharacter(enemy);
                enemy.Kill();
            }
        }
        enemy.HealthBarHandler.UpdateBar();
    }
}
