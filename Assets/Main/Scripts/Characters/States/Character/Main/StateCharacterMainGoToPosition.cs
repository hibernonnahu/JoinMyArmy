using UnityEngine;
using System.Collections;
using System;

public class StateCharacterMainGoToPosition : StateCharacter
{
    private const float ARRIVE_DIST_SQR = 0.5f;
    private const float BUG_DISTANCE_CHECK_SQR = 0.5f;
    private const float TICK = 1f;
    private int stearingMask;
    private Vector3 lastposition;
    private float counter;
    private int normalMask;
    private int emptyMask;
    public StateCharacterMainGoToPosition(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        normalMask = LayerMask.GetMask(new string[] { "Wall", "Water" });
        emptyMask = LayerMask.GetMask(new string[] { });

    }
    public override void Awake()
    {

        character.SetAnimation("walkstory");
        character.Rigidbody.drag = 0;
        stearingMask = normalMask;
    }

    public override void Sleep()
    {
        character.Rigidbody.drag = 100;
    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            counter = TICK;
            if ((lastposition - character.transform.position).sqrMagnitude < BUG_DISTANCE_CHECK_SQR)
            {
                stearingMask = emptyMask;
                character.DisableCollider();
            }
            else
            {
                character.EnableCollider();

                stearingMask = normalMask;
            }
            lastposition = character.transform.position;
        }
        Vector3 difVector = character.destiny - character.transform.position;
        UpdateMovement(difVector.x, difVector.z);
        if ((character.transform.position - character.destiny).sqrMagnitude < ARRIVE_DIST_SQR)
        {
            character.SetAnimation("idle");
            character.Rigidbody.velocity = Vector3.zero;
           
            EventManager.TriggerEvent(EventName.STORY_ARRIVE);
            character.VulnerableTime = 999;
            ChangeState(typeof(StateCharacterVulnerable));
        }
    }
    public override void UpdateMovement(float x, float y)
    {
        Vector3 forward = Vector3.Slerp(character.model.transform.forward, CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y), 43 * Time.deltaTime);
        Vector3 stearing = Utils.StearingVector(character.transform.position + Vector3.up * 2, forward, stearingMask);

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
    //public override void UpdateMovement(float x, float y)
    //{
    //    Vector3 direction = CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y);
    //    if (direction != Vector3.zero)
    //    {
    //        character.model.transform.forward = direction;
    //        character.Rigidbody.velocity = character.speed * direction;
    //    }

    //}
}
