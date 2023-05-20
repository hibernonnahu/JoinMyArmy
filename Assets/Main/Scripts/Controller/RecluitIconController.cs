using UnityEngine;

public class RecluitIconController : MonoBehaviour
{
    const float MAX_MASK_Y = -1F;
    const float FADE_TIME = 0.8F;
    const float ICON_SIZE = 1.8F;

    private CharacterEnemy enemy;
    public GameObject mask;
    private Vector3 maskOriginalPosition;
    public float totalTime = 5;
    private bool disabled = false;
    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }

    private bool knocked = false;
    void Awake()
    {
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
        maskOriginalPosition = mask.transform.localPosition;
        transform.localScale = Vector3.zero;
        enemy = GetComponentInParent<CharacterEnemy>();
    }
    public void ForceKnocked()
    {
        if (knocked && !disabled)
        {
            OnTimeOut();
        }
    }
    public void KnockOut()
    {
        if (!knocked)
        {
            FadeIn();
            knocked = true;
            LeanTween.moveLocalY(mask, MAX_MASK_Y, totalTime).setDelay(FADE_TIME * 2).setOnComplete(OnTimeOut);
        }
    }
    void FadeIn()
    {
        Reset();

        LeanTween.scale(this.gameObject, Vector3.one * ICON_SIZE, FADE_TIME).setEaseInExpo().setOnComplete(Pulse);
    }

    private void Reset()
    {
        LeanTween.cancel(mask);
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.zero;
        mask.transform.localPosition = maskOriginalPosition;
    }

    void FadeOut()
    {
        LeanTween.scale(this.gameObject, Vector3.zero, FADE_TIME).setEaseOutExpo().setOnComplete(() => { Disable(); gameObject.SetActive(false); });
    }
    private void Disable()
    {
        disabled = true;
        LeanTween.cancel(mask);
        LeanTween.cancel(gameObject);

    }
    void OnTimeOut()
    {
        Disable();
        FadeOut();
        enemy.Kill();
    }
    public void Restore()
    {
        mask.gameObject.SetActive(true);
        gameObject.SetActive(true);
        transform.SetParent(originalParent);
        transform.localPosition = originalLocalPosition;
        gameObject.transform.localScale = Vector3.zero;
        disabled = false;
        knocked = false;
    }
    void Pulse()
    {
        transform.SetParent(null);


        EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetFloat(transform.position.x).SetFloat2(transform.position.y).SetFloat3(transform.position.z).SetInt(1));

        LeanTween.scale(gameObject, gameObject.transform.localScale * 1.1f, 0.6f).setEaseLinear().setLoopPingPong();
    }

    void OnMouseDown()
    {
        if (!disabled)
        {
            EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(1));
            Disable();

            mask.gameObject.SetActive(false);
            if (enemy.CharacterMain.recluitController.CanRecluit() && !enemy.CharacterMain.IsDead)
            {
                enemy.CharacterMain.recluitController.UpdateFreeSpace();
                enemy.CharacterMain.recluitController.MakeUIAnimation(transform.position);
                enemy.CharacterMain.CastRecluit(enemy);
                gameObject.SetActive(false);
            }
            else
            {
                EventManager.TriggerEvent(EventName.BOUNCE_RECLUIT_TEXT);
                enemy.Kill();
                FadeOut();
            }
        }
    }

    internal void Init(string name)
    {
        Sprite = Resources.Load<Sprite>("CharacterIcons/" + name);
        GetComponent<SpriteRenderer>().sprite = Sprite;
    }
}
