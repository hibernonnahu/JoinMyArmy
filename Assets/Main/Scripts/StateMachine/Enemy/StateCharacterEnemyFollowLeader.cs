using UnityEngine;
using System;

public class StateCharacterEnemyFollowLeader : StateCharacterEnemy
{
    private const float FOLLOW_DISTANCE = 5.5F;
    private const float TICK_UPDATE = 0.5f;
    private const float TICK_UPDATE_RANGE = 1f;
    private const float DRAG_DISTANCE = 8;
    private const float DRAG_NEAR = 100;
    private const float DRAG_FAR = 0;
    private float tick = 0.25f;
    public StateCharacterEnemyFollowLeader(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        enemy.animator.SetFloat("walkspeed", enemy.speed);
        enemy.SetAnimation("idle");
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.lastEnemyTarget = null;
        tick = 0;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        tick -= Time.deltaTime;
        if (tick < 0)
        {
            if ((enemy.CharacterMain.transform.position - enemy.transform.position).sqrMagnitude <= enemy.attackDistanceSqr * 2)
            {
                enemy.lastEnemyTarget = enemy.CharacterManager.GetClosestEnemyInRange(enemy.team, enemy.attackDistanceSqr, enemy.model.transform.position);
            }
            if (enemy.lastEnemyTarget == null)
            {
                Vector3 direction = Quaternion.AngleAxis(enemy.FormationGrad, Vector3.up) * enemy.CharacterMain.model.transform.forward;
                var x = enemy.CharacterMain.transform.position.x - enemy.transform.position.x + direction.x * FOLLOW_DISTANCE;
                var z = (enemy.CharacterMain.transform.position.z - enemy.transform.position.z) + direction.z * FOLLOW_DISTANCE;

                if (CustomMath.SqrDistance2(x, z) < DRAG_DISTANCE && !enemy.CharacterMain.IsMoving)
                {
                    if (enemy.CharacterMain.lastEnemyTarget == null)
                    {
                        enemy.SetAnimation("idle");
                        enemy.Rigidbody.drag = DRAG_NEAR;
                        tick = UnityEngine.Random.Range(TICK_UPDATE, TICK_UPDATE_RANGE);
                    }
                    else
                    {
                        ChangeState(typeof(StateCharacterEnemyHelpAttack));
                    }                  
                }
                else
                {
                    enemy.SetAnimation("walk");
                    enemy.Rigidbody.drag = DRAG_FAR;
                    tick = TICK_UPDATE;
                    UpdateMovement(x, z);
                }
            }
            else
            {
                ChangeState(typeof(StateCharacterEnemyMelee));
            }
        }
    }
}
