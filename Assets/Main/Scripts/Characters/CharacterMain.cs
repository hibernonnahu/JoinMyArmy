
using System;
using UnityEngine;

public class CharacterMain : Character
{
    [Header("Main ExternalElements")]
    public FloatingJoystick floatingJoystick;
    public RecluitHandler recluitHandler;
    private FxHandler fxHandler;
    public FxHandler FxHandler { get { return fxHandler; } }

    public bool IsMoving { get => isMoving; set => isMoving = value; }
    private bool isMoving = false;

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        //Stats should be set it here
        base.Init();
        HealthBarHandler.GoGreen();
        recluitHandler.SetMaxRecluits(8);
        fxHandler = GetComponent<FxHandler>();
        FindObjectOfType<CameraHandler>().FollowGameObject(model);
        stateMachine.AddState(new StateCharacterMainInGame(stateMachine, this));
        stateMachine.AddState(new StateCharacterMainDead(stateMachine, this));
        stateMachine.AddState(new StateCharacterVulnerable(stateMachine, this));
    }
    protected override void Update()
    {
        stateMachine.CurrentState.UpdateMovement(floatingJoystick.Horizontal, floatingJoystick.Vertical);
        base.Update();//updates stateMachine
    }

    internal void CastRecluit(CharacterEnemy enemy)
    {
        enemy.ChangeTeam();
        VulnerableTime = 1;
        SetAnimation("cast");
        NextState = typeof(StateCharacterMainInGame);
        stateMachine.CurrentState.ChangeState(typeof(StateCharacterVulnerable));
    }
}
