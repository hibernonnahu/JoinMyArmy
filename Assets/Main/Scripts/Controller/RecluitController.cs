

using System;
using UnityEngine;
using UnityEngine.UI;

public class RecluitController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public Image[] images;
    public IconUIController[] iconUI;
    private int[] positions = { 0, 45, -45, 90, -90, 135, -135, 180 };
    private CharacterEnemy[] enemies = new CharacterEnemy[8];
    public CharacterEnemy[] Enemies { get { return enemies; } }
    private int max;
    private int freeSpace = 0;//-1 means no room

    private void Awake()
    {
        iconUI = new IconUIController[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            iconUI[i] = images[i].GetComponent<IconUIController>();
            images[i].gameObject.SetActive(false);
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

    public void Recluit(CharacterEnemy enemy, bool direct = false,int forcePosition=-1)
    {
        if (forcePosition != -1)
        {
            freeSpace = forcePosition;
        }
        
        
        enemies[freeSpace] = enemy;
        enemy.FormationGrad = positions[freeSpace];
        images[freeSpace].gameObject.SetActive(true);
        iconUI[freeSpace].CharacterEnemy = enemy;
        images[freeSpace].sprite = enemy.RecluitIconHandler.Sprite;
        images[freeSpace].color = Color.white;
        enemy.HealthBarController.UseBarUI(iconUI[freeSpace]);
       
        if (!direct)
        {
            enemy.CharacterMain.FxHandler.enemyRecluit.transform.position = enemy.transform.position;
            enemy.CharacterMain.FxHandler.enemyRecluit.Play();
            enemy.CharacterMain.FxHandler.startEnemyRecluit.Play();
            EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("recluit" + UnityEngine.Random.Range(1, 4)));
            EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("recluitmagic"));
            iconUI[freeSpace].DisableButton();
            iconUI[freeSpace].container.gameObject.SetActive(false);
        }
        UpdateFreeSpace();
        UpdateText();
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
                images[i].gameObject.SetActive(false);
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
        IconUIController iconUITemp = iconUI[freeSpace];
        LeanTween.cancel(image.gameObject);
        LeanTween.scale(image.rectTransform, Vector3.one * 1.7f, 0.8f).setEaseOutElastic();
        LeanTween.scale(image.rectTransform, Vector3.one , 0.5f).setDelay(0.8f);
        LeanTween.move(image.gameObject, image.transform.position, 1.5f).setEaseOutCubic().setOnComplete(
            () => { LeanTween.scale(image.rectTransform, Vector3.one * 1.5f, 0.7f).setEaseInBounce().setOnComplete(
                () =>
                {
                    iconUITemp.container.gameObject.SetActive(true);
                    iconUITemp.currentBar.localScale = Vector3.one;
                    iconUITemp.EnableButton();
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
