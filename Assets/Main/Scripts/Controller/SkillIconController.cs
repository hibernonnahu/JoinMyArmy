using System;
using UnityEngine;

public class SkillIconController : MonoBehaviour
{
    public ParticleSystem particles;
    private Action onUpdate = () => { };
    private const float SHOW_TIME = 1.6f;
    private const float SPEED = 15f;
    private SkillIconController[] skillIconControllerArray;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialScale;
    private new Collider collider;
    private CharacterMain character;
    ISkill skill;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
        collider = GetComponent<Collider>();
    }
    private void Update()
    {
        onUpdate();
    }
    public void Init(SkillIconController[] skillIconControllerArray, string skillClassName, Vector3 position)
    {
        this.skillIconControllerArray = skillIconControllerArray;

        var type = Type.GetType(skillClassName);
        if (type == null)
        {
            throw new Exception("this is not a class " + skillClassName);
        }
        else if (type.GetInterface("ISkill") == null)
        {
            throw new Exception("this is not an skill " + skillClassName);
        }
        skill = (ISkill)Activator.CreateInstance(type);
        transform.position = position;
        // text.text = skill.GetName();
        spriteRenderer.sprite = Resources.Load<Sprite>("Skills/" + skill.GetName());

    }
    private void Start()
    {
        LeanTween.scale(gameObject, initialScale, SHOW_TIME).setEaseInBounce().setOnComplete(Pulse);
    }
    private void Pulse()
    {
        LeanTween.scale(gameObject, initialScale * 1.1f, 0.5f).setLoopPingPong();
    }
    public void Hide()
    {
        LeanTween.cancel(gameObject);

        collider.enabled = false;
        LeanTween.scale(gameObject, Vector3.zero, SHOW_TIME);
    }
    public void OnMouseDown()
    {
        LeanTween.cancel(gameObject);

        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("stars"));

        collider.enabled = false;
        foreach (var item in skillIconControllerArray)
        {
            if (item != this)
            {
                item.Hide();
            }
        }
        character = FindObjectOfType<CharacterMain>();
        character.SkillController.AddSkill(skill);
        particles.Play();
        LeanTween.scale(gameObject,  initialScale * 1.3f, 0.3f).setEaseLinear().setOnComplete(
            () =>
            {
                LeanTween.scale(gameObject, initialScale*0.5f, 0.5f).setEaseOutBounce();
            }
            );
        onUpdate = FollowCharacter;
    }

    private void FollowCharacter()
    {
        transform.position += Time.deltaTime * SPEED * CustomMath.Normalize(character.transform.position - transform.position);
        if ((character.transform.position - transform.position).sqrMagnitude < 0.1f)
        {
            LeanTween.cancel(gameObject);
            particles.Stop();
            transform.localScale = Vector3.zero;
            EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("haha"));
            character.FxHandler.levelUp.Play();
            onUpdate = () => { };
        }
    }
}
