using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecluitIconHandler : MonoBehaviour
{
    const float MAX_MASK_Y = -1F;
    const float FADE_TIME = 0.8F;
    const float ICON_SIZE = 1.8F;

    private CharacterEnemy enemy;
    public GameObject mask;
    public float totalTime = 5;
    private bool disabled = false;

    private Sprite sprite;
    public Sprite Sprite { get => sprite; set => sprite = value; }

    void Awake()
    {
        transform.localScale = Vector3.zero;
        enemy = GetComponentInParent<CharacterEnemy>();
    }
    public void KnockOut()
    {
        transform.SetParent(null);
        FadeIn();

        LeanTween.moveLocalY(mask, MAX_MASK_Y, totalTime).setDelay(FADE_TIME * 2).setOnComplete(OnTimeOut);
    }
    void FadeIn()
    {
        LeanTween.scale(this.gameObject, Vector3.one * ICON_SIZE, FADE_TIME).setEaseInExpo().setOnComplete(Pulse);
    }
    void FadeOut()
    {
        LeanTween.scale(this.gameObject, Vector3.zero, FADE_TIME).setEaseOutExpo().setOnComplete(()=> { Disable();gameObject.SetActive(false); });
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
    void Pulse()
    {
        LeanTween.scale(gameObject, gameObject.transform.localScale * 1.1f, 0.6f).setEaseLinear().setLoopPingPong();
    }
    void OnMouseDown()
    {
        if (!disabled)
        {
            Disable();
            
            mask.gameObject.SetActive(false);
            if (enemy.CharacterMain.recluitHandler.CanRecluit())
            {
                enemy.CharacterMain.recluitHandler.MakeUIAnimation(transform.position);
                enemy.CharacterMain.CastRecluit(enemy);
                gameObject.SetActive(false);
            }
            else
            {
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
