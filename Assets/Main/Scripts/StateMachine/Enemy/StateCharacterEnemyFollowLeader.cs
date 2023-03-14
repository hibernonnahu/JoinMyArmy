using UnityEngine;
using System;

public class StateCharacterEnemyFollowLeader : StateCharacterEnemy
{
    private const float TICK_UPDATE = 1;
    private const float TICK_UPDATE_RANGE = 1.5f;
    private const float DRAG_DISTANCE = 15;
    private const float DRAG_NEAR = 5;
    private const float DRAG_FAR = 0;
    private float tick = 1;
    public StateCharacterEnemyFollowLeader(StateMachine<StateCharacter> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {

    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        tick -= Time.deltaTime;
        if (tick < 0)
        {
            var x = enemy.CharacterMain.transform.position.x + enemy.FollowOffsetX-1+UnityEngine.Random.Range(0,2) - enemy.transform.position.x;
            var z = (enemy.CharacterMain.transform.position.z - 1 + UnityEngine.Random.Range(0, 2) + enemy.FollowOffsetZ - enemy.transform.position.z);
            
            if (CustomMath.SqrDistance2(x, z) < DRAG_DISTANCE)
            {
                enemy.Rigidbody.drag = DRAG_NEAR;
                tick = UnityEngine.Random.Range(TICK_UPDATE, TICK_UPDATE_RANGE);
            }
            else
            {
                enemy.Rigidbody.drag = DRAG_FAR;
                tick = TICK_UPDATE;
            }
            UpdateMovement(x, z);

        }
    }
}
