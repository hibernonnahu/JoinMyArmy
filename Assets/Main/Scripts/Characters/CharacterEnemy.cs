
using System;
using UnityEngine;

public class CharacterEnemy : Character
{
    [Header("Enemy")]
    public RecluitIconHandler recluitIconHandler;

    private float followOffsetX = 0;
    public float FollowOffsetX
    {
        get { return followOffsetX; }
        set { followOffsetX = value; }
    }
    private float followOffsetZ = 3;
    public float FollowOffsetZ
    {
        get { return followOffsetZ; }
        set { followOffsetZ = value; }
    }
    private CharacterMain characterMain;
    public CharacterMain CharacterMain
    {
        get { return characterMain; }
        set { characterMain = value; }
    }

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        stateMachine.AddState(new StateCharacterEnemyIdle(stateMachine, this));
        stateMachine.AddState(new StateCharacterEnemyDead(stateMachine, this));
        stateMachine.AddState(new StateCharacterEnemyFollowLeader(stateMachine, this));
        stateMachine.AddState(new StateCharacterEnemyKnocked(stateMachine, this));

    }


    protected override void Update()
    {
        base.Update();//updates stateMachine
    }

    internal void ChangeTeam()
    {
        characterManager.GoMainTeam(this);
        stateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyFollowLeader));
    }

    internal void Kill()
    {
        stateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyDead));
    }
}
