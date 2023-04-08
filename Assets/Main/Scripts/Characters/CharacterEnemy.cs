
using System;
using UnityEngine;

public class CharacterEnemy : Character
{
    protected new StateMachine<StateCharacterEnemy> stateMachine;
    public StateMachine<StateCharacterEnemy> StateMachine { get => stateMachine; }
    public enum EnemyType
    {
        human, beast
    }
    [Header("Enemy Attributes")]
    public EnemyType enemyType;
    private bool canBeRecluit = false;
    public bool CanBeRecluit { get { return canBeRecluit; } set { canBeRecluit = value; } }

    [Header("Enemy Stats")]
    public float alertDistanceSqr = 100;

    private RecluitIconController recluitIconHandler;
    public RecluitIconController RecluitIconHandler { get { return recluitIconHandler; } }

    private int formationGrad = 0;
    public int FormationGrad { get => formationGrad; set => formationGrad = value; }

    private CharacterMain characterMain;
    public CharacterMain CharacterMain
    {
        get { return characterMain; }
        set { characterMain = value; }
    }

    public SkinnedMeshRenderer MeshRenderer { set { meshRenderer = value; } }

    private Func<float> onCastMainPower = () => { return -1; };
    public Func<float> OnCastMainPower { get { return onCastMainPower; } }

    private EnemyStateAddDefaultInit enemyStateAddInit;

    private SkinnedMeshRenderer meshRenderer;
    private void Awake()
    {
        Init();
        
    }
    public override void Init()
    {
        model.transform.forward = Vector3.back;

        base.Init();
        stateMachine = new StateMachine<StateCharacterEnemy>();
        enemyStateAddInit = GetComponent<EnemyStateAddDefaultInit>();
        if (enemyStateAddInit != null) { enemyStateAddInit.Init(this); } else { throw new Exception("No initial Default state found in " + gameObject.name); }

        EnemyStateAddCanBeRecluit ESACBR = GetComponent<EnemyStateAddCanBeRecluit>();
        ESACBR?.Init(this);

        stateMachine.AddState(new StateCharacterEnemyDead(stateMachine, this));
        StateMachine.AddState(new StateCharacterEnemyVulnerable(StateMachine, this));
    }

    protected override void LoadResources()
    {
        base.LoadResources();
        recluitIconHandler = Instantiate<RecluitIconController>(Resources.Load<RecluitIconController>("Prefabs/CharacterUI/RecluitIconHandler"), transform);
        recluitIconHandler.Init(gameObject.name.Replace("(Clone)", ""));
    }
    protected override void Update()
    {
        stateMachine.Update();
    }

    protected override void CurrentStateGetHit(float damage)
    {
        stateMachine.CurrentState.GetHit(damage);
    }

    internal void SetLevel(int level)
    {
        this.level = level;
    }

    internal void ChangeTeam()
    {
        lastEnemyTarget = null;
        currentHealth = health;
        HealthBarController.UpdateBar();
        characterManager.GoMainTeam(this);
        SetAnimation("getup", 0.1f);
        UpdateStatesToFollow();   
    }

    public void UpdateStatesToFollow()
    {
        HealthBarController.GoGreen();
        NextState = typeof(StateCharacterEnemyFollowLeader);
        IdleState = typeof(StateCharacterEnemyFollowLeader);
        VulnerableTime = 1;
        StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyVulnerable));
        enemyStateAddInit.OnRecluit();
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

    internal void SetCastMainPower(IEnemyStateAddAttack component)
    {
        onCastMainPower = component.Execute;
    }

    internal float UseMainSkill()
    {
        return stateMachine.CurrentState.OnCastMainPower();
    }

    public void DisableCollider()
    {
        collider.enabled = false;

    }
    protected override void GoVulnerable()
    {
        stateMachine.ChangeState(typeof(StateCharacterEnemyVulnerable));
    }
}
