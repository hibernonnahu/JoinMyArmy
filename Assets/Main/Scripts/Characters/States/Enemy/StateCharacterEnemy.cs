using System;
using UnityEngine;


public class StateCharacterEnemy : State<StateCharacterEnemy>
{
    protected CharacterEnemy enemy;
    protected int stearingMask;

    Vector3 stearing;
    Vector3 forward;
    public StateCharacterEnemy(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine)
    {
        this.enemy = characterEnemy;
        stearingMask = LayerMask.GetMask(new string[] { "Wall", "Water", "Bound", "Enemy", "Ally" });
    }

    public virtual void UpdateMovement(float x, float y)
    {
        forward = Vector3.Slerp(enemy.model.transform.forward, CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y), enemy.StearingRotation * Time.deltaTime);
        stearing = Utils.StearingVector(enemy.transform.position + Vector3.up * 2, forward, stearingMask);

        if (stearing != Vector3.zero)
        {

            stearing = CustomMath.XZNormalize((stearing));
            // Debug.DrawRay(enemy.transform.position, stearing *3, Color.magenta, 1);
            enemy.model.transform.forward = stearing;
        }
        else
        {

            enemy.model.transform.forward = forward;

        }

        enemy.Rigidbody.velocity = enemy.speed * enemy.model.transform.forward;
    }
    public virtual bool GetHit(float damage, Character attacker)//returns if character die
    {
        enemy.CurrentHealth -= damage;
        if (enemy.CurrentHealth <= 0)
        {
            enemy.CurrentHealth = 0;
            if (enemy.CanBeRecluit && !enemy.CharacterMain.IsDead)
            {
                enemy.RecluitIconHandler.KnockOut();
                ChangeState(typeof(StateCharacterEnemyKnocked));
            }
            else
            {
                
                enemy.Kill();
            }
            enemy.HealthBarController.UpdateBar();
            return true;
        }
        enemy.HealthBarController.UpdateBar();
        return false;
    }
    public virtual float OnCastMainPower()
    {
        return enemy.OnCastMainPower();
    }

    internal virtual void OnCollisionEnter(Collision collision)
    {

    }
}
