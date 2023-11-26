using System;
using UnityEngine;

public class CastleDefenseIconHandler : MonoBehaviour
{
    private const float WARNING_TIME = 1F;
    public AudioSource warningSound;
    private float yFixedOffset = -10;
    private CharacterEnemy enemy;
    public SpriteRenderer imageRenderer;
    public GameObject warning;
    private Vector3 originalWarningSize;
    private Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }
    private Action onUpdate = () => { };
    private Transform cameraRefPoint;
    private float originalYPosition = -1;

    void Awake()
    {
        originalWarningSize = warning.transform.localScale;
        warning.transform.localScale = Vector3.zero;

        if (Camera.main != null)
            cameraRefPoint = Camera.main.transform.GetChild(0);
        EventManager.StartListening(EventName.HIDE_RECLUIT_ICON, OnHide);

    }

    private void OnHit()
    {
        warningSound.Play();
        LeanTween.cancel(warning);
        LeanTween.scale(warning, originalWarningSize, WARNING_TIME).setEaseInOutBack().setOnComplete(() =>
        {
            LeanTween.scale(warning, Vector3.zero, WARNING_TIME).setEaseOutQuad();
        });
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


    Vector3 screenPos;
    private void UpdatePosition()
    {
        Vector3 tempPos = Vector3.right * enemy.transform.position.x + Vector3.up * transform.position.y + Vector3.forward * (enemy.transform.position.z + yFixedOffset);
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



    internal void Init(CharacterEnemy enemy)
    {
        this.enemy = enemy;
        Sprite = Resources.Load<Sprite>("CharacterIcons/" + enemy.name);
        imageRenderer.sprite = Sprite;
        onUpdate = UpdatePosition;
        enemy.AddOnHitAction(OnHit);
    }
    private void OnDestroy()
    {
        LeanTween.cancel(warning);
        LeanTween.cancel(gameObject);
        EventManager.StopListening(EventName.HIDE_RECLUIT_ICON, OnHide);

    }
}
