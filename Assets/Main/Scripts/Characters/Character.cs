
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    const float HEALTHBAR_FADEOUT_TIME = 0.5f;
    [Header("ExternalElements")]
    public Animator animator;
    public ChatIconHandler chatIconHandler;
    public GameObject healthBarContainer;
    public GameObject currentHealthBar;
    public GameObject model;

    [Header("Atributes")]
    public int id = 0;
    public int team = 0;

    [Header("Stats")]
    public float criticalChance = 0; // 0 to 1
    public float criticalMultiplier = 1.2f;
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
    public CharacterManager CharacterManager { set { characterManager = value; } }

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
        currentHealth = health;
        stateMachine = new StateMachine<StateCharacter>();

        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.Update();
    }
    internal void UpdateBar()
    {
        float result = (currentHealth / health);
        currentHealthBar.transform.localScale = Vector3.forward + Vector3.up + Vector3.right * result;
        if (result <= 0)
        {
            LeanTween.scale(healthBarContainer, Vector3.zero, HEALTHBAR_FADEOUT_TIME).setEaseOutExpo();
        }
    }

}
