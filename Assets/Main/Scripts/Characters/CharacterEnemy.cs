
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnemy : Character
{
    protected new StateMachine<StateCharacterEnemy> stateMachine;
    public StateMachine<StateCharacterEnemy> StateMachine { get => stateMachine; }
    public enum EnemyType
    {
        human, beast, dead
    }
    [Header("Enemy Attributes")]
    public EnemyType enemyType;
    public int xp = 1;
    public int coins = 1;
    public int extraAlertRange = 0;
    public int belongToWave = 0;
    public bool isBoss = false;

    [Header("Other")]
#if UNITY_EDITOR
    public bool debug = false;
#endif

    public float stearingRotationOffset = 0;
    private float stearingRotation = 150;
    public float StearingRotation { get { return stearingRotation + stearingRotationOffset; } }
    public string triggerOnDeath = "";
    public float followDistance = 1;
    private Vector3 returnPosition;
    public Vector3 ReturnPosition { get { return returnPosition; } set { returnPosition = value; } }
    private bool helpAttack = false;
    public bool HelpAttack { get { return helpAttack; } set { helpAttack = value; } }
    private Type attackState;
    public Type AttackState { get => attackState; set => attackState = value; }
    private bool canBeRecluit = false;
    public bool CanBeRecluit { get { return canBeRecluit; } set { canBeRecluit = value; } }
    private List<Action> onDeadActionList = new List<Action>();
    public List<Action> OnDeadActionList { get { return onDeadActionList; } }
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
    private bool useCastRedDotUI = false;
    public bool UseCastRedDotUI { get { return useCastRedDotUI; } }
    private Func<float> onCastMainPower = () => { return -1; };
    public Func<float> OnCastMainPower { get { return onCastMainPower; } }

    private EnemyStateAddDefaultInit enemyStateAddInit;

    private EnemyStateAddCanBeRecluit enemyStateAddCanBeRecluit;
    public EnemyStateAddCanBeRecluit EnemyStateAddCanBeRecluit { set { enemyStateAddCanBeRecluit = value; } }
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    public override void Init()
    {
        model.transform.forward = Vector3.back;

        base.Init();
        returnPosition = transform.position;
        SetLayer(9, 14, new int[] { 8, 16 });//Enemy,BulletEnemy,[Player,Ally]
        stateMachine = new StateMachine<StateCharacterEnemy>();
        enemyStateAddInit = GetComponent<EnemyStateAddDefaultInit>();
        if (enemyStateAddInit != null) { enemyStateAddInit.Init(this); } else { throw new Exception("No initial Default state found in " + gameObject.name); }

        stateMachine.AddState(new StateCharacterEnemyDead(stateMachine, this));
        StateMachine.AddState(new StateCharacterEnemyVulnerable(StateMachine, this));
        foreach (var item in GetComponents<IEnemySimpleAdd>())
        {
            item.Init(this);
        }


    }
    internal void ForceIdle()
    {
        NextState = IdleState;
        stateMachine.ChangeState(IdleState);
    }
    protected override void LoadResources()
    {
        base.LoadResources();
        recluitIconHandler = Instantiate<RecluitIconController>(Resources.Load<RecluitIconController>("Prefabs/CharacterUI/RecluitIconHandler"), transform);
        recluitIconHandler.Init(gameObject.name.Replace("(Clone)", ""));
    }
    protected override void Update()
    {
#if UNITY_EDITOR
        if (debug)
        {
            Debug.Log("");
        }
#endif
        stateMachine.Update();
    }

    internal void GoVulnerable(int v)
    {
        VulnerableTime = v;
        NextState = IdleState;
        StateMachine.ChangeState(typeof(StateCharacterEnemyVulnerable));
    }

    protected override bool CurrentStateGetHit(float damage, Character attacker)
    {
        return stateMachine.CurrentState.GetHit(damage, attacker);

    }

    internal void SetLevel(int level)
    {
        UpdateStatsOnLevel(level, false, false);
    }

    internal void ChangeTeam()
    {
        lastEnemyTarget = null;
        currentHealth = Health;
        characterManager.GoMainTeam(this);
        currentHealth = Health;
        HealthBarController.UpdateBar();
        SetAnimation("getup", 0.1f);
        UpdateStatesToFollow();
    }

    public void UpdateStatesToFollow()
    {
        HealthBarController.UpdateBarColor(this);
        UpdateColor();
        NextState = typeof(StateCharacterEnemyFollowLeader);
        IdleState = typeof(StateCharacterEnemyFollowLeader);
        VulnerableTime = 1;
        StateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyVulnerable));
        enemyStateAddInit.OnRecluit();
    }
    public void UpdateColor(bool mainTeam = true)
    {
        if (enemyStateAddCanBeRecluit != null && enemyStateAddCanBeRecluit.materialB != null)
        {
            if (mainTeam)
            {
                enemyStateAddCanBeRecluit.SetMaterialB();
            }
            else
            {
                enemyStateAddCanBeRecluit.SetMaterialA();
            }
        }
    }
    public void Kill()
    {
        currentHealth = 0;
        HealthBarController.UpdateBar();
        stateMachine.CurrentState.ChangeState(typeof(StateCharacterEnemyDead));
    }

    internal void Revive()
    {
        team = 0;
        IsDead = false;
        IsKnocked = false;
        speed = characterMain.speed;
        canBeRecluit = false;
        collider.enabled = true;
    }

    internal void SetCastMainPower(IEnemyStateAddAttack component)
    {
        onCastMainPower = component.Execute;
        useCastRedDotUI = component.UseRedDotUI();
    }

    internal float UseMainSkill()
    {
        return stateMachine.CurrentState.OnCastMainPower();
    }
    public int GetXp()
    {
        return xp;
    }
    public int GetCoins()
    {
        return (coins * level);
    }


    protected override void GoVulnerable()
    {
        stateMachine.ChangeState(typeof(StateCharacterEnemyVulnerable));
    }

    protected override void GoDissy()
    {
        if (stateMachine.CurrentState.CanGetEffect())
        {
            VulnerableTime = 4;
            SetAnimation("knocked");
            NextState = IdleState;

            GeneralParticleHandler.stun.Play();
            onVulnerableEnd = () =>
            {
                GeneralParticleHandler.stun.Stop();
            };
            StateMachine.ChangeState(typeof(StateCharacterEnemyVulnerable));
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        stateMachine.CurrentState.OnCollisionEnter(collision);
    }
    internal override bool CanGetEffect()
    {
        return stateMachine.CurrentState.CanGetEffect();
    }
}
