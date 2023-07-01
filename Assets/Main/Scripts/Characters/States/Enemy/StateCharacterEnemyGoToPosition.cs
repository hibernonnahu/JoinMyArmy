using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyGoToPosition : StateCharacterEnemy
{
    private const float ARRIVE_DIST_SQR = 0.5f;
    private bool ignoreColliders;
    public StateCharacterEnemyGoToPosition(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, bool ignoreColliders) : base(stateMachine, characterEnemy)
    {
        this.ignoreColliders = ignoreColliders;
    }
    public override void Awake()
    {
        if (ignoreColliders)
        {
            stearingMask = LayerMask.GetMask(new string[] { });
            enemy.DisableCollider();
        }
        else
        {
            stearingMask = LayerMask.GetMask(new string[] { "Wall", "Water", "Enemy", "Ally" });
        }

        enemy.SetAnimation("walkstory");
        enemy.Rigidbody.drag = 0;
    }

    public override void Sleep()
    {
        enemy.Rigidbody.drag = 100;
        enemy.EnableCollider();
    }

    public override void Update()
    {
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
