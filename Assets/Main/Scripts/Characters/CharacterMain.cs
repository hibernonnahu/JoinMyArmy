
using System;
using UnityEngine;

public class CharacterMain : Character
{
    [Header("Main ExternalElements")]
    public FloatingJoystick floatingJoystick;
    public RecluitController recluitHandler;
    public IconUIController barUI;
    private FxManager fxController;
    public FxManager FxHandler { get { return fxController; } }
    public CoinsUIController coinsUIController;

    private XpController xpController;
    public XpController XpController { get { return xpController; } }

    public bool IsMoving { get => isMoving; set => isMoving = value; }
    private bool isMoving = false;

    public int maxRecluits = 5;

    protected override void Awake()
    {
        base.Awake();
        xpController = new XpController();
    }
    private void Start()
    {
        Init();
        recluitHandler.SetMaxRecluits(maxRecluits + skillController.ExtraRecluit);

    }

    public override void Init()
    {
        model.transform.forward = Vector3.forward;
        base.Init();
        HealthBarController.GoGreen();
        HealthBarController.UseBarUI(barUI);

        fxController = GetComponent<FxManager>();
        FindObjectOfType<CameraHandler>().FollowGameObject(model);
        stateMachine.AddState(new StateCharacterMainInGame(stateMachine, this));
        stateMachine.AddState(new StateCharacterMainDead(stateMachine, this));
        stateMachine.AddState(new StateCharacterVulnerable(stateMachine, this));

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
    internal void OnLevelEnd()
    {
        collider.enabled = false;
        Rigidbody.velocity = Vector3.forward;
        model.transform.forward = Vector3.forward;
        VulnerableTime = 5;
        stateMachine.ChangeState<StateCharacterVulnerable>();
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
