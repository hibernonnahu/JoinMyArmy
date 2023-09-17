using System;
using UnityEngine;

public class RecluitIconController : MonoBehaviour
{
    const float MAX_MASK_Y = -1F;
    const float FADE_TIME = 0.8F;
    const float ICON_SIZE = 1.8F;
    const float PRESS_TIME = 20;

    public float y_fixed_offset = 0;
    private CharacterEnemy enemy;
    public GameObject mask;
    private Vector3 maskOriginalPosition;
    private bool disabled = false;
    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }

    private bool knocked = false;

    private Action onUpdate = () => { };

    private Game game;
    private Transform cameraRefPoint;
    private bool checkToRecluitCount;
    private float originalYPosition = -1;
    void Awake()
    {
        originalParent = transform.parent;
        originalLocalPosition = transform.localPosition;
        maskOriginalPosition = mask.transform.localPosition;
        transform.localScale = Vector3.zero;
        enemy = GetComponentInParent<CharacterEnemy>();
        game = FindObjectOfType<Game>();
        cameraRefPoint = Camera.main.transform.GetChild(0);
        EventManager.StartListening(EventName.HIDE_RECLUIT_ICON, OnHide);
    }

    private void OnHide(EventData arg0)
    {
        if (arg0.boolData)
        {
            originalYPosition = transform.position.y;
            transform.position += Vector3.up * 9999;
        }
        else
        {
            if (originalYPosition != -1)
            {
                transform.position = Vector3.right * transform.position.x + Vector3.forward * transform.position.z + Vector3.up * originalYPosition;
            }
        }
    }

    public int GetId()
    {
        return enemy.id;
    }
    public void ForceKnocked()
    {
        if (knocked && !disabled)
        {
            OnTimeOut();
        }
    }
    public bool KnockOut()
    {
        if (!knocked)
        {
            CheckRecluitCounter();
            checkToRecluitCount = false;
            knocked = true;
            if (enemy.isBoss && !enemy.CharacterMain.recluitController.IsIdRecluited(enemy.id))
            {
                FadeIn();
                return true;

            }
            else if (!((enemy.isBoss && enemy.CharacterMain.recluitController.IsIdRecluited(enemy.id)) || !game.toRecluitManager.HasRoom()))
            {
                checkToRecluitCount = true;

                game.toRecluitManager.count++;
                FadeIn();
                LeanTween.moveLocalY(mask, MAX_MASK_Y, PRESS_TIME).setDelay(FADE_TIME * 2).setOnComplete(OnTimeOut);
                return true;
            }
            else
            {
                OnTimeOut();
            }
        }
        return false;
    }
    void FadeIn()
    {
        Reset();

        onUpdate = UpdatePosition;
        LeanTween.scale(this.gameObject, Vector3.one * ICON_SIZE, FADE_TIME).setEaseInExpo().setOnComplete(Pulse);
    }

    private void Reset()
    {
        LeanTween.cancel(mask);
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.zero;
        mask.transform.localPosition = maskOriginalPosition;

    }

    void CheckRecluitCounter()
    {
        if (checkToRecluitCount)
        {
            checkToRecluitCount = false;
            game.toRecluitManager.count--;
        }
    }
    void FadeOut()
    {
        onUpdate = () => { };
        LeanTween.scale(this.gameObject, Vector3.zero, FADE_TIME).setEaseOutExpo().setOnComplete(() => { Disable(); gameObject.SetActive(false); });
    }
    public void DisableButtonOnly()
    {
        disabled = true;
    }
    private void Disable()
    {
        disabled = true;
        LeanTween.cancel(mask);
        LeanTween.cancel(gameObject);
        CheckRecluitCounter();
    }
    void OnTimeOut()
    {
        Disable();
        FadeOut();
        enemy.Kill();

    }
    public void Restore()
    {
        LeanTween.cancel(gameObject);
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

        UpdatePosition();
        EventManager.TriggerEvent(EventName.RECLUIT_ICON_POP);

        DispatchTutorialEvent();
        LeanTween.scale(gameObject, gameObject.transform.localScale * 1.1f, 0.6f).setEaseLinear().setLoopPingPong();
    }
    public void DispatchTutorialEvent()
    {
        EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetFloat(transform.position.x).SetFloat2(transform.position.y).SetFloat3(transform.position.z).SetBool(screenPos.x > 0.5f).SetInt(enemy.id));
    }
    public void Recluit(bool updateFreeSpace)
    {
        if (!disabled)
        {
            EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(1));

            if (enemy.isBoss && (enemy.CharacterMain.recluitController.IsIdRecluited(enemy.id)))
            {
                enemy.Kill();
                FadeOut();
            }
            else if (enemy.CharacterMain.recluitController.CanRecluit() && !enemy.CharacterMain.IsDead)
            {
                mask.gameObject.SetActive(false);
                if (updateFreeSpace)
                    enemy.CharacterMain.recluitController.UpdateFreeSpace();
                enemy.CharacterMain.recluitController.MakeUIAnimation(transform.position);
                enemy.CharacterMain.CastRecluit(enemy);

                gameObject.SetActive(false);
                Disable();

            }
            else
            {
                EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("error"));
                EventManager.TriggerEvent(EventName.BOUNCE_RECLUIT_TEXT);
                BounceButton();
                //enemy.Kill();
                //FadeOut();
            }
        }
    }
    void OnMouseDown()
    {
        Recluit(true);
    }

    private void BounceButton()
    {
        LeanTween.cancel(gameObject);
        gameObject.transform.localScale = Vector3.one * ICON_SIZE;
        LeanTween.scale(gameObject, Vector3.one * ICON_SIZE * 0.5f, 0.3f).setEaseLinear().setOnComplete(
            () =>
            {
                LeanTween.scale(gameObject, Vector3.one * ICON_SIZE, 0.7f).setEaseOutBounce();
            }
            );

    }
    Vector3 screenPos;
    private void UpdatePosition()
    {
        Vector3 tempPos = Vector3.right * enemy.transform.position.x + Vector3.up * transform.position.y + Vector3.forward * (enemy.transform.position.z + originalLocalPosition.z);
        screenPos = (Camera.main.WorldToViewportPoint(tempPos));
        Vector3 camTempMin = (Camera.main.ScreenToWorldPoint(Camera.main.transform.position));
        Vector3 camTempMax = (Camera.main.ScreenToWorldPoint(Camera.main.transform.position + Vector3.right * Screen.width));

        if (screenPos.x < 0.05f)
        {
            tempPos = Vector3.right * (camTempMin.x + 2) + Vector3.up * tempPos.y + Vector3.forward * tempPos.z;
        }
        else if (screenPos.x > 0.95f)
        {
            tempPos = Vector3.right * (camTempMax.x - 2) + Vector3.up * tempPos.y + Vector3.forward * tempPos.z;
        }
        if (screenPos.y < 0.05f)
        {
            tempPos = Vector3.right * tempPos.x + Vector3.up * tempPos.y + Vector3.forward * (cameraRefPoint.position.z - Camera.main.orthographicSize);

        }
        else if (screenPos.y > 0.95f)
        {
            tempPos = Vector3.right * tempPos.x + Vector3.up * tempPos.y + Vector3.forward * (cameraRefPoint.position.z + Camera.main.orthographicSize);
        }
        transform.position = tempPos;
    }
    private void Update()
    {
        onUpdate();
    }

    internal void ForceEnable()
    {
        disabled = false;
    }

    internal void Init(string name)
    {
        Sprite = Resources.Load<Sprite>("CharacterIcons/" + name);
        GetComponent<SpriteRenderer>().sprite = Sprite;
    }
}
