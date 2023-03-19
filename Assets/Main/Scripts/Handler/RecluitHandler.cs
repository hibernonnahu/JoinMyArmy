

using System;
using UnityEngine;
using UnityEngine.UI;

public class RecluitHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public Image[] images;
    private int[] positions = { 0, 45, -45, 90, -90, 135, -135, 180 };
    CharacterEnemy[] enemies = new CharacterEnemy[8];
    private int max;
    private int freeSpace = 0;//-1 means no room

    private void Start()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.clear;
            images[i].transform.SetParent(transform.parent);
        }

    }
    public void SetMaxRecluits(int max)
    {
        this.max = max;
        UpdateText();
    }

    private void UpdateText()
    {
        int count = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                count++;
            }
        }
        text.text = count + "/" + max;
    }

    public void Recluit(CharacterEnemy enemy)
    {
        enemy.CharacterMain.FxHandler.enemyRecluit.transform.position = enemy.transform.position;
        enemy.CharacterMain.FxHandler.enemyRecluit.Play();
        enemy.CharacterMain.FxHandler.startEnemyRecluit.Play();
        enemies[freeSpace] = enemy;
        enemy.FormationGrad = positions[freeSpace];
        images[freeSpace].sprite = enemy.RecluitIconHandler.Sprite;
        images[freeSpace].color = Color.white;
        UpdateFreeSpace();
        UpdateText();
        EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("recluit"+ UnityEngine.Random.Range(1,4)));
        EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("recluitmagic"));
    }
    private void UpdateFreeSpace()
    {
        freeSpace = -1;
        int count = 0;
        while (count < max && freeSpace == -1)
        {
            if (enemies[count] == null)
            {
                freeSpace = count;
            }
            count++;
        }
    }

    internal void Remove(Character character)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == character)
            {
                images[i].color = Color.clear;
                enemies[i] = null;
                break;
            }
        }
        UpdateFreeSpace();
        UpdateText();

    }

    internal void MakeUIAnimation(Vector3 position)
    {
        Vector3 initialPos = Camera.main.WorldToViewportPoint(position);
        Vector2 destiny = initialPos.x * Screen.width * Vector3.right + initialPos.y * Screen.height * Vector3.up;

        Image image = images[freeSpace];
        LeanTween.cancel(image.gameObject);
        LeanTween.scale(image.rectTransform, Vector3.one * 1.7f, 0.8f).setEaseOutElastic();
        LeanTween.scale(image.rectTransform, Vector3.one , 0.5f).setDelay(0.8f);
        LeanTween.move(image.gameObject, images[freeSpace].transform.position, 1.5f).setEaseOutCubic().setOnComplete(
            () => { LeanTween.scale(image.rectTransform, Vector3.one * 1.5f, 0.7f).setEaseInBounce().setOnComplete(
                () =>
                {
                    LeanTween.scale(image.rectTransform, Vector3.one, 0.3f).setEaseOutBounce();
                }
                ); }
            );
        image.rectTransform.anchoredPosition = destiny;
    }

    public bool CanRecluit()
    {
        return freeSpace != -1;
    }
}
