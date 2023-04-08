using UnityEngine;


public class StateCharacterEnemy : State<StateCharacterEnemy>
{
    protected CharacterEnemy enemy;
    public StateCharacterEnemy(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine)
    {
        this.enemy = characterEnemy;
    }

    public virtual void UpdateMovement(float x, float y)
    {
        enemy.model.transform.forward = CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y);
        enemy.Rigidbody.velocity = enemy.speed * enemy.model.transform.forward;
    }
    public virtual void GetHit(float damage)
    {
        enemy.CurrentHealth -= damage;
        if (enemy.CurrentHealth <= 0)
        {
            enemy.CurrentHealth = 0;
            if (enemy.CanBeRecluit && enemy.CharacterMain.recluitHandler.CanRecluit() && !enemy.CharacterMain.IsDead)
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
        enemy.HealthBarController.UpdateBar();
    }
    public virtual float OnCastMainPower()
    {
        return enemy.OnCastMainPower();
    }
}
