
using System;
using UnityEngine;

public class CharacterMain : Character
{
    [Header("Main ExternalElements")]
    public FloatingJoystick floatingJoystick;
    public RecluitController recluitController;
    public IconUIController barUI;
    public DamageControllerUI damageControllerUI;
    private FxManager fxController;
    public FxManager FxHandler { get { return fxController; } }
    public CoinsUIController coinsUIController;
    public ScrollViewSkillUI scrollViewSkillUI;

    private XpController xpController;
    public XpController XpController { get { return xpController; } }

    public bool IsMoving { get => isMoving; set => isMoving = value; }
    private bool isMoving = false;

    private float coinsMultiplier = 1;
    public float CoinsMultiplier
    {
        set { coinsMultiplier = value; }
        get { return coinsMultiplier; }
    }
    private int maxRecluits = 8;

    public StateMachine<StateCharacter> StateMachine { get { return stateMachine; } }

    protected override void Awake()
    {
        UpdateStats();
        base.Awake();
        xpController = new XpController();
    }

    private void UpdateStats()
    {
        string key = SaveDataKey.STATS + "_" + CurrentPlaySingleton.GetInstance().book + "_Strength";
        strength += SaveData.GetInstance().GetValue(key);

        key = SaveDataKey.STATS + "_" + CurrentPlaySingleton.GetInstance().book + "_Health";
        baseHealth += SaveData.GetInstance().GetValue(key) * 4;

        key = SaveDataKey.STATS + "_" + CurrentPlaySingleton.GetInstance().book + "_Defense";
        defense += SaveData.GetInstance().GetValue(key);

    }

    private void Start()
    {
        Init();
        recluitController.SetMaxRecluits(maxRecluits + skillController.ExtraRecluit);

    }

    public override void Init()
    {
        model.transform.forward = Vector3.forward;
        base.Init();
        HealthBarController.UpdateBarColor(this);
        HealthBarController.UseBarUI(barUI);
        SetLayer(8, 15, new int[] { 9 });

        fxController = GetComponent<FxManager>();
        FindObjectOfType<CameraHandler>().FollowGameObject(model);
        stateMachine.AddState(new StateCharacterMainInGame(stateMachine, this));
        stateMachine.AddState(new StateCharacterMainDead(stateMachine, this));
        stateMachine.AddState(new StateCharacterVulnerable(stateMachine, this));
        stateMachine.AddState(new StateCharacterMainIdle(stateMachine, this));

        CurrentPlaySingleton.GetInstance().LoadGamePlay(this);

        UpdateStatsOnLevel(level, currentHealth != 0, false);

        HealthBarController.UpdateXpBar(XpController.GetXpPercent(level));
        stateMachine.ChangeState<StateCharacterMainInGame>();
    }


    protected override void Update()
    {
        stateMachine.CurrentState.UpdateMovement(floatingJoystick.Horizontal, floatingJoystick.Vertical);
        base.Update();//updates stateMachine
    }

    internal void CastRecluit(CharacterEnemy enemy)
    {
        FxHandler.enemyRecluit.transform.position = enemy.transform.position;
        FxHandler.enemyRecluit.Play();
        FxHandler.startEnemyRecluit.Play();
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("recluit" + UnityEngine.Random.Range(1, 4)));
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("recluitmagic"));
        enemy.ChangeTeam();
        VulnerableTime = 1;
        SetAnimation("cast");
        NextState = typeof(StateCharacterMainInGame);
        stateMachine.CurrentState.ChangeState(typeof(StateCharacterVulnerable));
    }
    protected override void GoVulnerable()
    {
        stateMachine.ChangeState(typeof(StateCharacterVulnerable));
    }
    internal void OnLevelEnd(Vector3 exitPosition)
    {
        collider.enabled = false;
        VulnerableTime = 99999;
        stateMachine.ChangeState<StateCharacterVulnerable>();
        Rigidbody.drag = 0;
        model.transform.forward = (exitPosition.x * Vector3.right + exitPosition.z * 10 * Vector3.forward) - (transform.position.x * Vector3.right + transform.position.z * Vector3.forward);
        Rigidbody.velocity = model.transform.forward * 3;
    }

    public void AddXP(int xp)
    {
        if (xpController.AddXp(xp, level) > level)//level up
        {
            level++;
            textShortHandler.SetDialog(transform.position, "LEVEL UP!", Color.white);
            EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("level up"));
            fxController.levelUp.Play();
            UpdateStatsOnLevel(level);
        }

        HealthBarController.UpdateXpBar(xpController.GetXpPercent(level));
    }
    protected override void GoDissy()
    {
        VulnerableTime = 4;
        SetAnimation("knocked");
        NextState = IdleState;
        GeneralParticleHandler.stun.Play();
        onVulnerableEnd = () =>
        {
            GeneralParticleHandler.stun.Stop();
        };
        StateMachine.ChangeState(typeof(StateCharacterVulnerable));
    }
    public override float GetHit(Character attacker, float multiplier = 1, bool getDissy = false)
    {
        float result = base.GetHit(attacker, multiplier, getDissy);
        UpdateDamageUI();

        return result;
    }
    public override void Heal(float heal, bool showText = true)
    {
        base.Heal(heal, showText);
        UpdateDamageUI();
    }

    internal void GoToPosition(float x, float z, float speed)
    {
        destiny = Vector3.right * x + Vector3.forward * z;
        this.SpeedStory = speed;
        this.StateMachine.AddState(new StateCharacterMainGoToPosition(StateMachine, this));
        this.StateMachine.ChangeState<StateCharacterMainGoToPosition>();
    }

    public void HideArmy(bool hide)
    {
        recluitController.HideArmy(hide, this);

    }
    internal void ArmyOffset(int v)
    {
        recluitController.SetOffset(v);
    }
    public override void UpdateStatsOnLevel(int newLevel, bool forceCurrentHealth = false, bool showText = true)
    {
        base.UpdateStatsOnLevel(newLevel, forceCurrentHealth, showText);
        UpdateDamageUI();
    }
    private void UpdateDamageUI()
    {
        damageControllerUI.UpdateUI(1 - (currentHealth / Health));
    }
    public override void OnNewSkillAdded(string v)
    {
        scrollViewSkillUI.Add(v);
    }
}
