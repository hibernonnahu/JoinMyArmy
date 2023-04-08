
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{

    [Header("ExternalElements")]
    public Animator animator;
    protected ChatIconController chatIconHandler;
    protected new Collider collider;
    private HealthBarController healthBarController;
    public HealthBarController HealthBarController { get { return healthBarController; } }
    public GameObject model;
    public TextShortController textShortHandler;

    public float barScale = 1;
    public bool useBarText = false;
    public float generalParticleYOffset = 0;
    private GeneralParticleManager generalParticleHandler;

    [Header("Atributes")]
    public int id = 0;
    public int level = 1;
    public int team = 0;
    public int behaviour = 0;

    [Header("Stats")]
    public float attackDistanceSqr = 2;
    public float attackSpeed = 1.5f;
    public float criticalChance = 0; // 0 to 1
    public float criticalMultiplier = 1.2f;

    [Header("Temps")]
    public Character lastEnemyTarget;

    private float vulnerableTime;
    public float VulnerableTime { get { return vulnerableTime; } set { vulnerableTime = value; } }
    private Type nextState;
    public Type NextState { get => nextState; set => nextState = value; }
    private Type idleState;
    public Type IdleState { get => idleState; set => idleState = value; }

    internal void SetAnimation(string name, float crossTime = 0, int layer = 0)
    {
        animator.CrossFade(name, crossTime, layer);
    }

    public float health = 100;
    protected float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    public float speed = 1;
    public float strength = 1;

    protected CharacterManager characterManager;
    public CharacterManager CharacterManager { set { characterManager = value; } get { return characterManager; } }

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody
    {
        get
        {
            return rigidbody;
        }
    }

    protected StateMachine<StateCharacter> stateMachine;

    public virtual void Init()
    {
        LoadResources();
        currentHealth = health;
        stateMachine = new StateMachine<StateCharacter>();
        collider = GetComponentInChildren<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        var gph = Resources.Load<GeneralParticleManager>("Prefabs/Particles/GeneralParticleHandler");
        generalParticleHandler = Instantiate<GeneralParticleManager>(gph, transform);
        generalParticleHandler.transform.localPosition += Vector3.up * generalParticleYOffset;
    }

    protected virtual void LoadResources()
    {
        textShortHandler = Instantiate<TextShortController>(Resources.Load<TextShortController>("Prefabs/CharacterUI/TextShortHandler"), transform);
        healthBarController = Instantiate<HealthBarController>(Resources.Load<HealthBarController>("Prefabs/CharacterUI/HealthBar"), transform);
        healthBarController.Init(this);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.Update();
    }
    public float GetHit(Character attacker)//returns damage percent
    {
        float damage = attacker.strength;
        Color color = Color.white;
        if (UnityEngine.Random.value < attacker.criticalChance)
        {
            damage *= attacker.criticalMultiplier;
            damage = Mathf.Ceil(damage);
            color = Color.red;
        }
        textShortHandler.SetDialog(transform.position, damage.ToString(), color);
        float percent = damage / health;
        CurrentStateGetHit(damage);
        return percent;
    }
    internal void Heal(float heal)
    {
        textShortHandler.SetDialog(transform.position, heal.ToString(), Color.green);
        CurrentHealth += heal;
        if (CurrentHealth > health)
        {
            CurrentHealth = health;
        }
        generalParticleHandler.health.Play();
        healthBarController.UpdateBar();
    }

    internal void WallHit()
    {
        EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("wall hit"));
        NextState = idleState;
        VulnerableTime = 1.6f;
        SetAnimation("wall hit", 0.02f);
        generalParticleHandler.wallHit.Play();
        GoVulnerable();
    }

    protected virtual void GoVulnerable()
    {
        throw new NotImplementedException();
    }

    protected virtual void CurrentStateGetHit(float damage)
    {
        stateMachine.CurrentState.GetHit(damage);
    }
}
