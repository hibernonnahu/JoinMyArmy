
using System;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    const float HEALTHBAR_FADEOUT_TIME = 0.5f;
    public TMPro.TextMeshPro text;
    public TMPro.TextMeshPro levelText;
    public Material green;
    private Material red;
    private GameObject currentHealthBar;
    public GameObject currentXpBar;
    private Action<float> updateBar = (x) => { };
    private Character character;
    private Vector3 initScale;
    private Action onUpdateText = () => { };

    public void Init(Character character)
    {
        this.character = character;
        red = transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material;
        currentHealthBar = transform.GetChild(0).gameObject;
        transform.localScale *= character.barScale;
        transform.position += Vector3.up * character.barOffset;
        initScale = transform.localScale;
        if (character.useBarText)
        {
            onUpdateText = () =>
            {
                text.text = ((int)(character.CurrentHealth)).ToString();
            };
            text.text = character.Health.ToString();
        }
    }
    public void UpdateBarColor(Character character)
    {
        if (character.IsEnemy())
        {
            transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material = red;
        }
        else
        {
            transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material = green;
        }
        ShowBarAgain();
    }
    public void ShowBarAgain()
    {
        LeanTween.scale(gameObject, initScale, HEALTHBAR_FADEOUT_TIME).setEaseInExpo();
    }
    internal void UpdateBar()
    {
        float result = (character.CurrentHealth / character.Health);
        currentHealthBar.transform.localScale = Vector3.forward + Vector3.up + Vector3.right * result;
        if (result <= 0)
        {
            LeanTween.scale(gameObject, Vector3.zero, HEALTHBAR_FADEOUT_TIME).setEaseOutExpo();
        }
        updateBar(result);
        onUpdateText();
    }
    public void UseBarUI(IconUIController barUI)
    {

        updateBar = (x) =>
        {
            if (x <= 0)
            {
                barUI.currentBar.localScale = Vector3.zero;
            }
            else
            {
                barUI.currentBar.localScale = Vector3.forward + Vector3.up + Vector3.right * x;
            }
        };
    }

    internal void UpdateXpBar(float result)
    {
        currentXpBar.transform.localScale = Vector3.forward + Vector3.up + Vector3.right * result;
    }
}
