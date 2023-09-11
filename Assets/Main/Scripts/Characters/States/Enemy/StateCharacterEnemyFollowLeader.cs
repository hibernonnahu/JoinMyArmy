using UnityEngine;
using System;

public class StateCharacterEnemyFollowLeader : StateCharacterEnemy
{
    private const float TELEPORT_DIST = 800;
    private const float FOLLOW_DISTANCE = 5.5F;
    private const float HELP_SQR_DISTANCE = 600f;
    private const float TICK_UPDATE = 0.2f;
    private const float TICK_UPDATE_RANGE = 1f;
    private const float DRAG_DISTANCE = 16;
    private const float DRAG_NEAR = 100;
    private const float DRAG_FAR = 0;
    private float tick = 0.25f;
   
    private bool helpAttack;
    public StateCharacterEnemyFollowLeader(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy,bool helpAttack) : base(stateMachine, characterEnemy)
    {
       
        this.helpAttack = helpAttack;
    }
    public override void Awake()
    {
        enemy.animator.SetFloat("walkspeed", enemy.speed);
        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
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
            if (helpAttack && (enemy.CharacterMain.transform.position - enemy.transform.position).sqrMagnitude <= HELP_SQR_DISTANCE)
            {
                enemy.lastEnemyTarget = enemy.CharacterManager.GetClosestEnemyInRange(enemy.team, enemy.attackDistanceSqr, enemy.model.transform.position);
            }
            if (enemy.lastEnemyTarget == null)
            {
                Vector3 direction = Quaternion.AngleAxis(enemy.FormationGrad, Vector3.up) * enemy.CharacterMain.model.transform.forward * enemy.followDistance;
                var x = enemy.CharacterMain.transform.position.x - enemy.transform.position.x + direction.x * FOLLOW_DISTANCE;
                var z = (enemy.CharacterMain.transform.position.z - enemy.transform.position.z) + direction.z * FOLLOW_DISTANCE;
                var dist = CustomMath.SqrDistance2(x, z);
                if (dist < DRAG_DISTANCE && !enemy.CharacterMain.IsMoving)
                {
                    if (!helpAttack || enemy.CharacterMain.lastEnemyTarget == null)
                    {
                        if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
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
                    if(dist>TELEPORT_DIST)
                    {
                        enemy.transform.position += CustomMath.XZNormalize(Vector3.right*x+Vector3.forward*z)*5;
                    }
                    if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
                    {
                        enemy.SetAnimation("walk");
                    }
                    enemy.Rigidbody.drag = DRAG_FAR;
                    tick = TICK_UPDATE;
                    enemy.ReturnPosition = Vector3.right * x + Vector3.forward * z;
                    UpdateMovement(x, z);
                }
            }
            else
            {
                ChangeState(enemy.AttackState);
            }
        }
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
                tick = -1;
            }
        }
    }
}
