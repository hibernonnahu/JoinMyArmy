
using System;
using UnityEngine;

public class CharacterMain : Character
{
    [Header("Main ExternalElements")]
    public FloatingJoystick floatingJoystick;
    public RecluitController recluitController;
    public IconUIController barUI;
    private FxManager fxController;
    public FxManager FxHandler { get { return fxController; } }
    public CoinsUIController coinsUIController;

    private XpController xpController;
    public XpController XpController { get { return xpController; } }

    public bool IsMoving { get => isMoving; set => isMoving = value; }
    private bool isMoving = false;

    private int maxRecluits = 6;

    public StateMachine<StateCharacter> StateMachine { get { return stateMachine; } }

    protected override void Awake()
    {
        base.Awake();
        xpController = new XpController();
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
}
