using UnityEngine;
using System;

public class StateCharacterEnemyLead : StateCharacterEnemy
{

    private FloatingJoystick floatingJoystick;
    private EnemyStateAttackModeController enemyStateAttackModeHandler;
    private float alertDistanceSqr;
    private CameraHandler cameraHandler;
    float directionMultiplier;
    public StateCharacterEnemyLead(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy,FloatingJoystick floatingJoystick, EnemyStateAttackModeController enemyStateAttackModeHandler, float alertDistanceSqr,float directionMultiplier) : base(stateMachine, characterEnemy)
    {
        this.enemyStateAttackModeHandler = enemyStateAttackModeHandler;
        this.alertDistanceSqr = alertDistanceSqr;
        this.floatingJoystick = floatingJoystick;
        this.directionMultiplier = directionMultiplier;
    }
    public override void Awake()
    {
        enemy.animator.SetFloat("walkspeed", enemy.speed);
        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            enemy.SetAnimation("idle");
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.lastEnemyTarget = null;
       
        if(cameraHandler==null)
        cameraHandler = GameObject.FindObjectOfType<CameraHandler>();
        cameraHandler.FollowGameObject(enemy.model.gameObject,false);
        enemy.CharacterMain.lastFollowTarget = enemy;
        enemy.CharacterMain.StateMachine.ChangeState<StateCharacterFollow>();
        var currentState = (StateCharacterFollow)(enemy.CharacterMain.StateMachine.CurrentState);
        if(currentState is StateCharacterFollow)
        {
            currentState.directionMultiplier = directionMultiplier;
        }
    }

    public override void Sleep()
    {
       
        enemy.CharacterMain.lastEnemyTarget = null;
    }
    public override void UpdateMovement(float x, float y)
    {
        if (x == 0 && y == 0)
        {
            enemy.lastEnemyTarget = enemy.CharacterManager.GetClosestEnemyInRange(enemy.team, alertDistanceSqr + enemy.extraAlertRange, enemy.model.transform.position);
            if (enemy.lastEnemyTarget == null)
            {
                enemy.SetAnimation("idle", 0);
            }
            else
            {
                enemyStateAttackModeHandler.Attack();
            }
            enemy.Rigidbody.drag = 50;
            enemy.Rigidbody.velocity = Vector3.zero;

        }
        else
        {

            enemy.Rigidbody.drag = 0;

            Vector3 direction = CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y);
            if (direction != Vector3.zero)
            {
                enemy.Rigidbody.velocity = enemy.speed * direction;
                if (enemy.lastEnemyTarget != null)
                {
                    direction = enemy.lastEnemyTarget.transform.position - enemy.transform.position;
                }
                enemy.model.transform.forward = direction;
                enemy.SetAnimation("walk", 0, 0);
            }
        }
    }
    public override void Update()
    {
        UpdateMovement(floatingJoystick.Horizontal, floatingJoystick.Vertical);
    }
    public override bool GetHit(float damage, Character attacker)
    {
        if (enemy.lastEnemyTarget == null || (attacker != null && (attacker.transform.position - enemy.transform.position).sqrMagnitude < (enemy.lastEnemyTarget.transform.position - enemy.transform.position).sqrMagnitude))
        {
            enemy.lastEnemyTarget = attacker;
        }
        return base.GetHit(damage, attacker);

    }
    internal override void OnCollisionEnter(Collision collision)
    {
        if (enemy.HitsLayer(collision.gameObject.layer))
        {
            var colCharacter = collision.gameObject.GetComponent<Character>();
            if (colCharacter != null && colCharacter.team != enemy.team)
            {
                if (colCharacter != enemy.lastEnemyTarget)
                {
                    if (enemy.lastEnemyTarget == null || (colCharacter.transform.position - enemy.transform.position).sqrMagnitude <
                        (enemy.lastEnemyTarget.transform.position - enemy.transform.position).sqrMagnitude)
                    {
                        enemy.lastEnemyTarget = colCharacter;
                    }
                }
                
            }
        }
    }
}
