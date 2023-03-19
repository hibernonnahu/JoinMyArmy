
using System;
using UnityEngine;

public class CharacterEnemy : Character
{
    [Header("Enemy Attributes")]
    public bool alert = false;
    public bool canBeRecluit = true;

    [Header("Enemy Stats")]
    public float alertDistanceSqr = 100;

    private RecluitIconHandler recluitIconHandler;
    public RecluitIconHandler RecluitIconHandler { get { return recluitIconHandler; } }

    private int formationGrad = 0;
    public int FormationGrad { get => formationGrad; set => formationGrad = value; }

    private Type idleState;
    public Type IdleState { get => idleState; set => idleState = value; }

    private CharacterMain characterMain;
    public CharacterMain CharacterMain
    {
        get { return characterMain; }
        set { characterMain = value; }
    }

    private SkinnedMeshRenderer meshRenderer;
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        model.transform.forward = Vector3.back;
       
        base.Init();
        
        if (alert)
        {
            idleState = typeof(StateCharacterEnemyAlert);
            stateMachine.AddState(new StateCharacterEnemyAlert(stateMachine, this));
            stateMachine.AddState(new StateCharacterEnemyChase(stateMachine, this));
            stateMachine.AddState(new StateCharacterEnemyMelee(stateMachine, this));
        }
        stateMachine.AddState(new StateCharacterEnemyIdle(stateMachine, this));
        stateMachine.AddState(new StateCharacterEnemyDead(stateMachine, this));
        stateMachine.AddState(new StateCharacterVulnerable(stateMachine, this));
        if (canBeRecluit)
        {
            meshRenderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
            stateMachine.AddState(new StateCharacterEnemyHelpAttack(stateMachine, this));
            stateMachine.AddState(new StateCharacterEnemyFollowLeader(stateMachine, this));
            stateMachine.AddState(new StateCharacterEnemyKnocked(stateMachine, this));
        }

    }

    protected override void LoadResources()
    {
        base.LoadResources();
        recluitIconHandler = Instantiate<RecluitIconHandler>(Resources.Load<RecluitIconHandler>("Prefabs/CharacterUI/RecluitIconHandler"), transform);
        recluitIconHandler.Init(gameObject.name.Replace("(Clone)", ""));
    }
    protected override void Update()
    {
        base.Update();//updates stateMachine
    }

    internal void ChangeTeam()
    {
        currentHealth = health;
        HealthBarHandler.GoGreen();
        HealthBarHandler.UpdateBar();
        characterManager.GoMainTeam(this);
        SetAnimation("getup", 0.02f);
        NextState = typeof(StateCharacterEnemyFollowLeader);
        idleState = typeof(StateCharacterEnemyFollowLeader);
        VulnerableTime = 1;
        stateMachine.CurrentState.ChangeState(typeof(StateCharacterVulnerable));
    }

    internal void Kill()
    {
        stateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyDead));
    }

    internal void Tint()
    {
        meshRenderer.material.color = Utils.GetColor(0.8f, 1f, 0.9f, 1);
    }

    internal void Revive()
    {
        team = 0;
        speed = characterMain.speed;
        canBeRecluit = false;
        Tint();
        collider.enabled = true;
    }
    public void DisableCollider()
    {
        collider.enabled = false;

    }
}
