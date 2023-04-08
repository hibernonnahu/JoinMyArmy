
using System;
using UnityEngine;
using UnityEngine.UI;

public class IconUIController : MonoBehaviour
{
    public GameObject container;
    public RectTransform currentBar;
    public Image coldDown;
    private CharacterEnemy characterEnemy;
    public CharacterEnemy CharacterEnemy { set => characterEnemy = value; }
    public Button button;
    public RectTransform rectTransform;
    private float totalTime = -1;
    private float currentTime=-1;
    private Action onUpdateColdDown = () => { };
    private void Awake()
    {
        coldDown.fillAmount = 0;
    }
    private void Update()
    {
        onUpdateColdDown();
    }
    private void OnColdDown()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            currentTime = 0;
            onUpdateColdDown = () => { };
            LeanTween.scale(rectTransform, Vector3.one * 1.5f, 0.7f).setEaseInCirc().setOnComplete(
               () =>
               {
                   EnableButton();
                   LeanTween.scale(rectTransform, Vector3.one, 0.3f).setEaseOutCirc();
               }
               );

        }
        coldDown.fillAmount = (currentTime / totalTime);
    }
    public void OnClick()
    {
        if (currentTime <= 0)
        {
            currentTime = totalTime = characterEnemy.UseMainSkill();
            if (currentTime > 0)
            {
                LeanTween.delayedCall(0.3f, StartColdDown);
            }
        }
    }
    private void StartColdDown()
    {
        coldDown.fillAmount = 1;
        onUpdateColdDown = OnColdDown;
        DisableButton();
    }
    internal void DisableButton()
    {
        button.interactable = false;
    }

    internal void EnableButton()
    {
        coldDown.fillAmount = 0;
        currentTime = -1;
        button.interactable = true;
    }
}
