
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    protected SkillController skillController;
    public SkillController SkillController { get { return skillController; } set { skillController = value; } }
    [Header("ExternalElements")]
    public Animator animator;
    protected ChatIconController chatIconHandler;
    protected new Collider collider;
    private HealthBarController healthBarController;
    public HealthBarController HealthBarController { get { return healthBarController; } }



    public GameObject model;
    public TextShortController textShortHandler;

    public float barScale = 1;
    public float barOffset = 0;
    public bool useBarText = false;
    public float generalParticleYOffset = 0;
    public float generalParticleScale = 1;
    private GeneralParticleManager generalParticleHandler;
    public GeneralParticleManager GeneralParticleHandler { get { return generalParticleHandler; } }

    [Header("Atributes")]
    public int id = 0;
    public int level = 1;
    public int team = 0;
    public int behaviour = 0;
    public int baseCost = 50;
    public bool extra = false;// It's not important to win. It could be kill or not.

    [Header("Stats")]
    public float attackDistanceSqr = 2;

    public float attackSpeed = 1.5f;
    public float AttackSpeed { get { return attackSpeed + skillController.ExtraSpeed; } }

    public float criticalChance = 0; // 0 to 1
    public float criticalMultiplier = 1.2f;
    public float healWhenKill = 0;

    [Header("Temps")]
    public Character lastEnemyTarget;
    public Vector3 destiny;

    public bool invulnerable = false;
    protected int[] enemyLayer;
    public int[] EnemyLayer { get { return enemyLayer; } }
    private float vulnerableTime;
    public float VulnerableTime { get { return vulnerableTime; } set { vulnerableTime = value; } }
    private Type nextState;
    public Type NextState { get => nextState; set => nextState = value; }
    private Type idleState;
    public Type IdleState { get => idleState; set => idleState = value; }
    private bool isDead = false;
    public bool IsDead { get => isDead; set => isDead = value; }
    private bool isKnocked = false;
    public bool IsKnocked { get => isKnocked; set => isKnocked = value; }
    public Action onVulnerableEnd = () => { };
    internal void SetAnimation(string name, float crossTime = 0, int layer = 0)
    {
        animator.CrossFade(name, crossTime, layer);
    }

    protected float health;
    public float Health
    {
        get { return health; }
    }
    public float baseHealth = 100;
    protected float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    public float speed = 1;
    private float speedStory;
    public float SpeedStory { get { return speedStory; } set { speedStory = value; } }
    public int strength = 1;
    public int defense = 1;

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

    protected virtual void Awake()
    {
        skillController = new SkillController(this);
    }
    public static float CalculateHealth(int level, float baseHealth)
    {
        return (baseHealth + level + level * baseHealth * 0.1f);
    }
    public virtual void UpdateStatsOnLevel(int newLevel, bool forceCurrentHealth = false, bool showText = true)
    {
        float newHealth = CalculateHealth(newLevel, baseHealth) + skillController.ExtraHealth;
        float dif = newHealth - health;

        health = newHealth;
        if (!forceCurrentHealth)
        {
            Heal(dif, showText);
        }

        level = newLevel;
        healthBarController.UpdateBar();
        healthBarController.levelText.text = newLevel.ToString();
    }
    public virtual void Init()
    {
        LoadResources();

        currentHealth = health;
        stateMachine = new StateMachine<StateCharacter>();
        collider = GetComponentInChildren<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        var gph = Resources.Load<GeneralParticleManager>("Prefabs/Particles/GeneralParticleHandler");
        generalParticleHandler = Instantiate<GeneralParticleManager>(gph, transform);
        generalParticleHandler.transform.localScale = Vector3.one * generalParticleScale;
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
    public virtual float GetHit(Character attacker, float multiplier = 1, bool getDissy = false)//returns damage percent
    {
        if (attacker.team != team || CurrentPlaySingleton.GetInstance().dificulty > 0)
        {
            float damage = attacker.GetDamage(GetDefense()) * multiplier;
            Color color = Color.white;
            if (UnityEngine.Random.value < attacker.criticalChance)
            {
                damage *= attacker.criticalMultiplier;
                damage = Mathf.Ceil(damage);
                color = Color.red;
            }
            textShortHandler.SetDialog(transform.position, damage.ToString(), color);
            float percent = damage / health;
            if (CurrentStateGetHit(damage, attacker))
            {
                attacker.Heal(GetHealWhenKillValue());
                attacker.skillController.OnKill(attacker, this);
            }
            else if (getDissy)
            {
                GoDissy();
            }
            return percent;
        }
        return 0;
    }
    protected virtual void GoDissy()
    {

    }
    private float GetHealWhenKillValue()
    {
        return healWhenKill + healWhenKill * level;
    }

    internal void Spawn(float spawnTime)
    {
        LeanTween.delayedCall(gameObject, spawnTime, OnSpawn);
    }
    private void OnSpawn()
    {
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("pop"));
        transform.position = Vector3.right * transform.position.x + Vector3.forward * transform.position.z;
        this.enabled = true;
        gameObject.SetActive(true);
        generalParticleHandler.wallHit.Play();
    }
    public static float GetBaseDefense(int defense, int level)
    {
        return defense + level * 0.1f;
    }
    private float GetDefense()
    {
        return GetBaseDefense(defense, level) + skillController.ExtraDefense;
    }

    public static float CalculateBaseDamage(int level, int strength, int extraDamage = 0)
    {
        return level * 0.1f + strength + extraDamage;
    }
    private int GetDamage(float defense)
    {
        int result = Mathf.FloorToInt(CalculateBaseDamage(level, strength, skillController.ExtraDamage) + level * 0.1f * UnityEngine.Random.Range(1, strength) - defense);
        if (result < 1)
        {
            result = 1;
        }
        return result;
    }

    public virtual void Heal(float heal, bool showText = true)
    {
        if (heal > 0)
        {

            CurrentHealth += heal;
            if (CurrentHealth > health)
            {
                CurrentHealth = health;
            }
            if (showText)
            {
                textShortHandler.SetDialog(transform.position, Mathf.FloorToInt(heal).ToString(), Color.green);
                generalParticleHandler.health.Play();
            }
            healthBarController.UpdateBar();
        }
    }

    internal void WallHit()
    {
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("wall hit"));
        EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(0.25f));

        NextState = idleState;
        VulnerableTime = 1.6f;
        LeanTween.delayedCall(gameObject, vulnerableTime, () => { transform.position += model.transform.forward; });
        SetAnimation("wall hit", 0.02f);
        generalParticleHandler.wallHit.Play();
        GoVulnerable();
    }
    public void SetLayer(int layer, int bulletLayer, int[] enemyLayer)
    {
        collider.gameObject.layer = layer;
        Transform parent = collider.transform.parent;
        while (parent != null)
        {
            parent.gameObject.layer = layer;
            parent = parent.parent;
        }
        this.enemyLayer = enemyLayer;
        var bulletPool = GetComponentInChildren<BulletPool>();
        bulletPool?.SetLayer(bulletLayer);
    }
    public bool HitsLayer(int layer)
    {
        foreach (var item in enemyLayer)
        {
            if (item == layer)
            {
                return true;
            }
        }
        return false;
    }
    internal bool IsEnemy()
    {
        if (team == 0)
        {
            return false;
        }
        foreach (var enemyId in characterManager.teamEnemiesID[0])
        {
            if (enemyId == team)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void GoVulnerable()
    {
        throw new NotImplementedException();
    }

    protected virtual bool CurrentStateGetHit(float damage, Character attacker)
    {
        return stateMachine.CurrentState.GetHit(damage, attacker);
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {

    }
    public virtual void OnNewSkillAdded(string v)
    {

    }

    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }
    public void DisableCollider()
    {
        collider.enabled = false;

    }
    public void EnableCollider()
    {
        collider.enabled = true;

    }
    internal virtual bool CanGetEffect()
    {
        return true;
    }
}
