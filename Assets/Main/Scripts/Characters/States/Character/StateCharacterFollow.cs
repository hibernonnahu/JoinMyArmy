using UnityEngine;
using System;

public class StateCharacterFollow : StateCharacter
{
    private const float TELEPORT_DIST = 800;

    private const float FOLLOW_DISTANCE = -7.5F;
    private const float TICK_UPDATE = 0.2f;

    private const float DRAG_FAR = 0;
    private float tick = .25f;
    Character toFollow;
    protected int stearingMask;
    public float directionMultiplier;
    Vector3 stearing;
    Vector3 forward;
    protected ICharacterAttackController attackHandler;

    public StateCharacterFollow(StateMachine<StateCharacter> stateMachine, Character character) : base(stateMachine, character)
    {
        stearingMask = LayerMask.GetMask(new string[] { "Wall", "Water", "Bound", "Enemy", "Ally" });
        attackHandler = character.GetComponent<ICharacterAttackController>();
    }
    public override void Awake()
    {
        toFollow = character.lastFollowTarget;
        tick = 3;
        if (!character.animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            character.SetAnimation("idle");
        character.Rigidbody.velocity = Vector3.zero;
        attackHandler.GoAlert();
        character.IdleState = typeof(StateCharacterFollow);
    }

    public override void Sleep()
    {
        attackHandler.GoIdle();
    }

    public override void Update()
    {
        tick -= Time.deltaTime;
        if (tick < 0)
        {

            if (toFollow != null&&!toFollow.IsDead)
            {
                Vector3 direction = toFollow.model.transform.forward  ;
                var x = toFollow.transform.position.x - character.transform.position.x + direction.x * FOLLOW_DISTANCE*directionMultiplier;
                var z =  (toFollow.transform.position.z - character.transform.position.z) + direction.z * FOLLOW_DISTANCE*directionMultiplier;
                var dist = CustomMath.SqrDistance2(x, z);


                if (dist > TELEPORT_DIST)
                {
                    character.transform.position += CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * z) * 3;
                }

                character.Rigidbody.drag = DRAG_FAR;
                tick = TICK_UPDATE;

                
                if (Mathf.Abs(z)<2 && Mathf.Abs(x) < 2)
                {
                    if (!character.animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                    {
                        character.SetAnimation("idle");
                    }
                    character.Rigidbody.velocity = Vector3.zero;
                }
                else
                {
                    if (!character.animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
                    {
                        character.SetAnimation("walk");
                    }
                    UpdateOverrideMovement(x, z);
                }
            }
            else
            {
                character.lastFollowTarget = null;
                ChangeState(typeof(StateCharacterMainInGame));
            }
        }
       
        attackHandler.GoUpdate();


    }
    private void UpdateOverrideMovement(float x, float y)
    {
        forward = Vector3.Slerp(character.model.transform.forward, CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y), 150 * Time.deltaTime);
        stearing = Utils.StearingVector(character.transform.position + Vector3.up * 2, forward, stearingMask);

        if (stearing != Vector3.zero)
        {

            stearing = CustomMath.XZNormalize((stearing));
            // Debug.DrawRay(enemy.transform.position, stearing *3, Color.magenta, 1);
            character.model.transform.forward = stearing;
        }
        else
        {

            character.model.transform.forward = forward;

        }

        character.Rigidbody.velocity = character.speed * character.model.transform.forward;
    }
    public override void UpdateMovement(float x, float y)
    {
       
    }

}
