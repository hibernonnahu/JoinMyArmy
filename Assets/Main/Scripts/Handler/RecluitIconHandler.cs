using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecluitIconHandler : MonoBehaviour
{
    const float MAX_MASK_Y = -1F;
    const float FADE_TIME = 0.5F;

    private CharacterEnemy enemy;
    public GameObject mask;
    public float totalTime = 5;
    private bool disabled = false;

    void Awake()
    {
        transform.localScale = Vector3.zero;
        enemy = GetComponentInParent<CharacterEnemy>();
    }
    public void KnockOut()
    {
        transform.SetParent(null);
        FadeIn();
        LeanTween.moveLocalY(mask, MAX_MASK_Y, totalTime).setDelay(FADE_TIME*2).setOnComplete(OnTimeOut);
    }
    void FadeIn()
    {
        LeanTween.scale(this.gameObject, Vector3.one, FADE_TIME).setEaseInExpo();
    }
    void FadeOut()
    {
        LeanTween.scale(this.gameObject, Vector3.zero, FADE_TIME).setEaseOutExpo();
    }
    private void Disable()
    {
        disabled = true;
        LeanTween.cancel(mask);
        FadeOut();
    }
    void OnTimeOut()
    {
        Disable();
        enemy.Kill();
    }

    void OnMouseDown()
    {
        if (!disabled)
        {
            Disable();
            enemy.ChangeTeam();
        }
    }
}
