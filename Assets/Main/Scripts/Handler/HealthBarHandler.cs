
using System;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    const float HEALTHBAR_FADEOUT_TIME = 0.5f;
    public Material green;
    private GameObject currentHealthBar;
    private Character character;
    private Vector3 initScale;

    public void Init(Character character)
    {
        this.character = character;
        currentHealthBar = transform.GetChild(0).gameObject;
        initScale = transform.localScale;
    }
    public void GoGreen()
    {
        transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material=green;
        LeanTween.scale(gameObject, initScale, HEALTHBAR_FADEOUT_TIME).setEaseInExpo();
    }
    internal void UpdateBar()
    {
        float result = (character.CurrentHealth / character.health);
        currentHealthBar.transform.localScale = Vector3.forward + Vector3.up + Vector3.right * result;
        if (result <= 0)
        {
            LeanTween.scale(gameObject, Vector3.zero, HEALTHBAR_FADEOUT_TIME).setEaseOutExpo();
        }
    }
}
