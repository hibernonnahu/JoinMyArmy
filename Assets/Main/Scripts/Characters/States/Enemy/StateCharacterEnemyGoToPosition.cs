using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyGoToPosition : StateCharacterEnemy
{
    private const float ARRIVE_DIST_SQR = 0.5f;
    private const float BUG_DISTANCE_CHECK_SQR = 0.5f;
    private const float TICK = 1f;
    private bool ignoreColliders;
    private Vector3 lastposition;
    private float counter;
    private int normalMask;
    private int emptyMask;
    public string animation = "walkstory";
    public StateCharacterEnemyGoToPosition(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, bool ignoreColliders) : base(stateMachine, characterEnemy)
    {
        this.ignoreColliders = ignoreColliders;
        emptyMask = LayerMask.GetMask(new string[] { });
        normalMask = LayerMask.GetMask(new string[] { "Wall", "Water", "Enemy", "Ally" });

    }
    public override void Awake()
    {
        if (ignoreColliders)
        {
            counter = 99999;
            stearingMask = emptyMask;
            enemy.DisableCollider();
        }
        else
        {
            counter = -1;
            stearingMask = normalMask;  
        }

        enemy.SetAnimation(animation);
        enemy.Rigidbody.drag = 0;
       
        lastposition = enemy.transform.position + Vector3.right * 1000;
    }

    public override void Sleep()
    {
        enemy.Rigidbody.drag = 100;
        //enemy.EnableCollider();
    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            counter = TICK;
            if((lastposition-enemy.transform.position).sqrMagnitude< BUG_DISTANCE_CHECK_SQR)
            {
                stearingMask = emptyMask;
            }
            else
            {
                stearingMask = normalMask;
            }
            lastposition = enemy.transform.position;
        }
#if UNITY_EDITOR
        if (enemy.debug)
        {
            RaycastHit hit;
            if (Physics.Raycast(enemy.transform.position - Vector3.up * 2, enemy.model.transform.forward, out hit, 3, stearingMask))
            {
                Debug.Log("out " + LayerMask.LayerToName(hit.collider.gameObject.layer)+" "+ hit.collider.gameObject.transform.parent.gameObject.name);
            }
        }
#endif
        Vector3 difVector = enemy.destiny - enemy.transform.position;
        UpdateMovement(difVector.x, difVector.z);
        if ((enemy.transform.position - enemy.destiny).sqrMagnitude < ARRIVE_DIST_SQR)
        {
            enemy.SetAnimation("idle");
            enemy.Rigidbody.velocity = Vector3.zero;
            ChangeState(typeof(StateCharacterEnemyIdle));
            EventManager.TriggerEvent(EventName.STORY_ARRIVE);
        }
    }
}
