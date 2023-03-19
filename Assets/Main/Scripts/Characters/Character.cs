
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
   
    [Header("ExternalElements")]
    public Animator animator;
    protected ChatIconHandler chatIconHandler;
    protected new Collider collider;
    private HealthBarHandler healthBarHandler;
    public HealthBarHandler HealthBarHandler { get { return healthBarHandler; } }
    public GameObject model;
    public TextShortHandler textShortHandler;

    [Header("Atributes")]
    public int id = 0;
    public float level = 1;
    public int team = 0;

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
    }

    protected virtual void LoadResources()
    {
        textShortHandler = Instantiate<TextShortHandler>(Resources.Load<TextShortHandler>("Prefabs/CharacterUI/TextShortHandler"), transform);
        healthBarHandler = Instantiate<HealthBarHandler>(Resources.Load<HealthBarHandler>("Prefabs/CharacterUI/HealthBar"), transform);
        healthBarHandler.Init(this);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.Update();
    }
    public void GetHit(Character attacker)
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
        stateMachine.CurrentState.GetHit(damage);
    }
}
