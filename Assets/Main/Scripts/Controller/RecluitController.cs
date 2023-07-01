

using System;
using UnityEngine;
using UnityEngine.UI;

public class RecluitController : MonoBehaviour
{
    private const float SWAP_SQR_DISTANCE = 6000;
    public Image trash;
    public RectTransform textContainer;
    private Vector3 initialTextPos;
    public Transform gotoTextPosition;
    public Text text;
    public Image[] images;
    public IconUIController[] iconUI;
    private int[] positions = { 0, 45, -45, 90, -90, 135, -135, 180 };
    private CharacterEnemy[] enemies = new CharacterEnemy[8];
    public CharacterEnemy[] Enemies { get { return enemies; } }
    private int max;
    public int Max
    {
        get { return max; }
    }
    private int freeSpace = 0;//-1 means no room

    private void Awake()
    {
        initialTextPos = textContainer.transform.position;
        iconUI = new IconUIController[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            iconUI[i] = images[i].GetComponent<IconUIController>();
            iconUI[i].Init(this, i);

            images[i].gameObject.SetActive(false);
            images[i].transform.SetParent(transform.parent);
        }
        GameObject.FindWithTag("background ui").transform.SetParent(transform.parent);
        trash.transform.localScale = Vector3.zero;
        EventManager.StartListening(EventName.BOUNCE_RECLUIT_TEXT, BounceText);
    }
    public void SetMaxRecluits(int max)
    {
        this.max = max;
        UpdateText();
        UpdateFreeSpace();
    }
    public void AddMaxRecluit()
    {
        if (max < 8)
        {
            max++;
            UpdateText();
            UpdateFreeSpace();
        }
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
        if (count != max)
        {
            text.text = count + "/" + max;
            text.color = Color.white;

        }
        else
        {
            text.text = "MAX";
            text.color = Color.yellow;
        }
        BounceText();
    }
    private void BounceText(EventData arg0 = null)
    {
        LeanTween.cancel(textContainer);
        if (text.text == "MAX")
        {
            LeanTween.move(textContainer.gameObject, gotoTextPosition.position, 0.15f);
            LeanTween.move(textContainer.gameObject, initialTextPos, 0.15f).setDelay(1.1f);
        }
        LeanTween.scale(textContainer, Vector3.one * 2.5f, 0.7f).setEaseInBounce().setOnComplete(
            () =>
            {
                LeanTween.scale(textContainer, Vector3.one, 0.7f).setEaseOutBounce();
            }
            );
    }

    public void Recluit(CharacterEnemy enemy, bool direct = false, int forcePosition = -1)
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
            iconUI[freeSpace].DisableButton();
            iconUI[freeSpace].container.gameObject.SetActive(false);
        }
        UpdateFreeSpace();
        UpdateText();
    }

    internal bool IsIdRecluited(int id)
    {
        foreach (var item in enemies)
        {
            if (item != null && item.id == id)
            {
                return true;
            }
        }
        return false;
    }

    internal void OnEndDrag(IconUIController iconUIController)
    {
        bool fail = true;
        for (int i = 0; i < iconUI.Length; i++)
        {

            if (iconUIController != iconUI[i] && (iconUI[i].transform.localPosition - iconUIController.transform.localPosition).sqrMagnitude < SWAP_SQR_DISTANCE)
            {
                EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(3));
                Swap(i, iconUIController.IndexPosition);
                fail = false;
                break;
            }
        }
        LeanTween.cancel(trash.gameObject);
        if (fail)
        {
            if ((trash.transform.position - iconUIController.transform.position).sqrMagnitude < SWAP_SQR_DISTANCE)
            {
                EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(3));

                iconUIController.BounceAnimation(() =>
                {
                    LeanTween.scale(trash.gameObject, Vector3.zero, 0.5f).setEaseOutElastic().setOnComplete(() =>
                    {
                        enemies[iconUIController.IndexPosition].StateMachine.CurrentState.GetHit(enemies[iconUIController.IndexPosition].CurrentHealth, null);
                        Remove(enemies[iconUIController.IndexPosition]);
                        iconUIController.ReturnToOriginalPosition(false, false);
                    });
                }, Vector3.zero);
            }
            else
            {
                iconUIController.ReturnToOriginalPosition();
                LeanTween.scale(trash.gameObject, Vector3.zero, 0.2f).setEaseOutElastic();
            }
        }
        else
        {
            LeanTween.scale(trash.gameObject, Vector3.zero, 0.2f).setEaseOutElastic();
        }
    }

    internal void SetOffset(int v)
    {
        foreach (var item in enemies)
        {
            if (item != null)
                item.followDistance = v;
        }
    }

    private void Swap(int iconUIController1Position, int iconUIControllerDragedPosition)
    {
        images[iconUIController1Position].gameObject.SetActive(false);
        images[iconUIControllerDragedPosition].gameObject.SetActive(false);
        var enemy1 = enemies[iconUIController1Position];
        enemies[iconUIController1Position] = null;
        var enemy2 = enemies[iconUIControllerDragedPosition];
        enemies[iconUIControllerDragedPosition] = null;
        if (enemy1 != null)
        {
            var auxPos = iconUI[iconUIController1Position].transform.position;
            iconUI[iconUIController1Position].transform.position = iconUI[iconUIControllerDragedPosition].transform.position;
            iconUI[iconUIControllerDragedPosition].transform.position = auxPos;
            Recluit(enemy1, true, iconUIControllerDragedPosition);
            enemy1.HealthBarController.UpdateBar();
            iconUI[iconUIControllerDragedPosition].ReturnToOriginalPosition(true);
        }
        else
        {
            iconUI[iconUIControllerDragedPosition].ReturnToOriginalPosition(true, true);
        }
        Recluit(enemy2, true, iconUIController1Position);
        iconUI[iconUIController1Position].ReturnToOriginalPosition(true);

        enemy2.HealthBarController.UpdateBar();
    }

    public void UpdateFreeSpace()
    {
        freeSpace = -1;
        int current = 0;
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                freeSpace = i;
            }
            else
            {
                current++;
            }
        }
        if (current >= max)
        {
            freeSpace = -1;
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
        iconUITemp.ReturnToOriginalPosition(false, false);
        LeanTween.cancel(image.gameObject);
        LeanTween.scale(image.rectTransform, Vector3.one * 1.7f, 0.8f).setIgnoreTimeScale(true).setEaseOutElastic();
        LeanTween.scale(image.rectTransform, Vector3.one, 0.5f).setIgnoreTimeScale(true).setDelay(0.8f);
        LeanTween.move(image.gameObject, image.transform.position, 1.5f).setEaseOutCubic().setOnComplete(
            () =>
            {
                LeanTween.scale(image.rectTransform, Vector3.one * 1.5f, 0.7f).setIgnoreTimeScale(true).setEaseInBounce().setOnComplete(
            () =>
            {
                iconUITemp.container.gameObject.SetActive(true);
                iconUITemp.currentBar.localScale = Vector3.one;
                iconUITemp.EnableButton();
                if (iconUITemp.CharacterEnemy.id == 0)//tree
                {
                    iconUITemp.tutorialID = 2;
                    EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetInt(2).SetTransform(iconUITemp.transform).SetBool(false));
                }
                else if (iconUITemp.CharacterEnemy.id == 10 && iconUITemp.IndexPosition < 3)//spider
                {
                    EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetInt(3).SetTransform(iconUITemp.transform).SetFloat(iconUI[7].transform.position.y));
                }
                LeanTween.scale(image.rectTransform, Vector3.one, 0.3f).setIgnoreTimeScale(true).setEaseOutBounce();
            }
            );
            }
            );
        image.rectTransform.anchoredPosition = destiny;
    }

    public bool CanRecluit()
    {
        return freeSpace != -1;
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.BOUNCE_RECLUIT_TEXT, BounceText);
    }
}
