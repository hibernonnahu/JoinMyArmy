

using System;
using UnityEngine;
using UnityEngine.UI;

public class RecluitController : MonoBehaviour
{
    public RectTransform canvas;
    private const float SWAP_SQR_DISTANCE = 6000;
    public Image trash;
    public RectTransform textContainer;
    private Vector3 initialTextPos;
    public Transform gotoTextPosition;
    public Text text;
    public Image[] images;
    public IconUIController[] iconUI;
    public bool trashEnable = true;
    private int[] positions = { 0, 45, -45, 90, -90, 135, -135, 180 };
    private CharacterEnemy[] enemies = new CharacterEnemy[8];
    public CharacterEnemy[] Enemies { get { return enemies; } }
    private int max;
    public int Max
    {
        get { return max; }
    }
    private int freeSpace = 0;//-1 means no room
    private int recluitIconMask;
    public bool canSwap = true;

    private void Awake()
    {
        recluitIconMask = LayerMask.GetMask("UI");
        initialTextPos = textContainer.transform.position;
        iconUI = new IconUIController[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            iconUI[i] = images[i].GetComponent<IconUIController>();

            iconUI[i].Init(this, i);
            if (!iconUI[i].doNotDisable)
            {
                images[i].gameObject.SetActive(false);
            }
            images[i].transform.SetParent(transform.parent);

        }
        GameObject.FindWithTag("background ui").transform.SetParent(transform.parent);
        trash.transform.localScale = Vector3.zero;
        EventManager.StartListening(EventName.BOUNCE_RECLUIT_TEXT, BounceText);
        EventManager.StartListening(EventName.HIDE_CHARACTER_UI, HideRecluitUI);
    }

    private void HideRecluitUI(EventData arg0)
    {
        gameObject.SetActive(!arg0.boolData);
        for (int i = 0; i < images.Length; i++)
        {
            iconUI[i].Hide(arg0.boolData);
        }
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

    public bool HasNoRecluits()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    public bool HasAtLeastOneNormalRecluit()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && !enemies[i].isBoss)
            {
                return true;
            }
        }
        return false;
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
            LeanTween.cancel(trash.gameObject, true);

            LeanTween.scale(trash.gameObject, Vector3.one, 0.2f).setEaseOutElastic().setOnComplete(() =>
            {
                LeanTween.scale(trash.gameObject, Vector3.zero, 0.2f).setDelay(1.5f).setEaseOutElastic();
            });
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
        iconUI[freeSpace].Character = enemy;
        images[freeSpace].sprite = enemy.RecluitIconHandler.Sprite;
        images[freeSpace].color = Color.white;
        enemy.HealthBarController.UseBarUI(iconUI[freeSpace]);

        if (!direct)
        {
            iconUI[freeSpace].DisableButton();
            iconUI[freeSpace].container.gameObject.SetActive(false);
        }
        else
        {
            iconUI[freeSpace].CheckDot();
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
        if (iconUIController.canSwap)
            for (int i = 0; i < iconUI.Length; i++)
            {

                if (canSwap && iconUIController != iconUI[i] && (iconUI[i].transform.localPosition - iconUIController.transform.localPosition).sqrMagnitude < SWAP_SQR_DISTANCE)
                {
                    EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(3));
                    Swap(i, iconUIController.IndexPosition);
                    fail = false;
                    break;
                }
            }
        LeanTween.cancel(trash.gameObject, true);
        if (fail)
        {
            if (trashEnable && iconUIController.canMoveToTrash && (trash.transform.position - iconUIController.transform.position).sqrMagnitude < SWAP_SQR_DISTANCE)
            {
                EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(3));
                EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(101));

                iconUIController.BounceAnimation(() =>
                {
                    LeanTween.scale(trash.gameObject, Vector3.zero, 0.5f).setEaseOutElastic().setOnComplete(() =>
                    {//bug
                        if (enemies[iconUIController.IndexPosition] != null)
                        {
                            enemies[iconUIController.IndexPosition].StateMachine.CurrentState.GetHit(enemies[iconUIController.IndexPosition].CurrentHealth, null);
                            Remove(enemies[iconUIController.IndexPosition]);
                        }
                        iconUIController.ReturnToOriginalPosition(false, false);

                        SaveData.GetInstance().Save(SaveDataKey.TRASH, SaveData.GetInstance().GetValue(SaveDataKey.TRASH, 0) + 1);

                    });
                }, Vector3.zero);
            }
            else
            {
                //try swap
                Vector3 pos = Camera.main.ScreenToWorldPoint(iconUIController.transform.position);
                //pos = Vector3.right * pos.x + Vector3.forward * (pos.z) + Vector3.up * 40;
                pos -= Camera.main.transform.forward * 40;
                //Debug.Log("pos " + pos);
                RaycastHit hit;
                if (Physics.Raycast(pos, Camera.main.transform.forward, out hit, 100, recluitIconMask))// Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask);
                {
                    int tempFreeSpace = iconUIController.IndexPosition;

                    if (enemies[iconUIController.IndexPosition] != null)
                    {
                        enemies[iconUIController.IndexPosition].StateMachine.CurrentState.GetHit(enemies[iconUIController.IndexPosition].CurrentHealth, null);
                        Remove(enemies[iconUIController.IndexPosition]);
                    }
                   
                    iconUIController.ReturnToOriginalPosition();
                    freeSpace = tempFreeSpace;
                    
                    var controller = hit.collider.gameObject.GetComponent<RecluitIconController>();
                    controller.ForceEnable();
                    controller.Recluit(false);
                    SaveData.GetInstance().Save(SaveDataKey.RECLUIT_SWAP, SaveData.GetInstance().GetValue(SaveDataKey.RECLUIT_SWAP, 0) + 1);

                    EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(iconUI[0].tutorialID));

                }
                else
                {
                    iconUIController.ReturnToOriginalPosition();
                    LeanTween.scale(trash.gameObject, Vector3.zero, 0.2f).setEaseOutElastic();
                }
#if UNITY_EDITOR
                Debug.DrawRay(pos, Camera.main.transform.forward * 100, Color.red, 3);
#endif
            }
        }
        else
        {
            LeanTween.scale(trash.gameObject, Vector3.zero, 0.2f).setEaseOutElastic();
        }
    }

    public void HideArmy(bool hide, CharacterMain character)
    {

        foreach (var item in enemies)
        {
            if (item != null)
            {
                if (hide)
                {
                    item.transform.position = Vector3.down * 100;
                    item.StateMachine.ChangeState<StateCharacterEnemyIdle>();
                }
                else
                {
                    item.transform.position = character.transform.position;
                    item.StateMachine.ChangeState(item.IdleState);
                }
            }
        }
    }
    internal void SetOffset(int v)
    {
        foreach (var item in enemies)
        {
            if (item != null)
            {
                item.followDistance = v;
            }
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
            var auxTotaltime = iconUI[iconUIController1Position].TotalTime;
            var auxCurrentltime = iconUI[iconUIController1Position].CurrentTime;
            iconUI[iconUIController1Position].SetCoolDownUI(iconUI[iconUIControllerDragedPosition].CurrentTime, iconUI[iconUIControllerDragedPosition].TotalTime);
            iconUI[iconUIController1Position].transform.position = iconUI[iconUIControllerDragedPosition].transform.position;
            iconUI[iconUIControllerDragedPosition].transform.position = auxPos;
            iconUI[iconUIControllerDragedPosition].SetCoolDownUI(auxCurrentltime, auxTotaltime);
            Recluit(enemy1, true, iconUIControllerDragedPosition);
            enemy1.HealthBarController.UpdateBar();
            iconUI[iconUIControllerDragedPosition].ReturnToOriginalPosition(true);
        }
        else
        {
            iconUI[iconUIController1Position].SetCoolDownUI(iconUI[iconUIControllerDragedPosition].CurrentTime, iconUI[iconUIControllerDragedPosition].TotalTime);
            iconUI[iconUIControllerDragedPosition].ReturnToOriginalPosition(true, true);
            iconUI[iconUIControllerDragedPosition].Character = null;
        }
        Recluit(enemy2, true, iconUIController1Position);
        iconUI[iconUIController1Position].ReturnToOriginalPosition(true);
        iconUI[iconUIControllerDragedPosition].CheckDot();

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

        Vector2 destiny = initialPos.x * canvas.rect.width * Vector3.right + initialPos.y * canvas.rect.height * Vector3.up;

        Image image = images[freeSpace];
        IconUIController iconUITemp = iconUI[freeSpace];

        iconUITemp.ReturnToOriginalPosition(false, false);
        iconUITemp.SetCoolDownUI(0, 1);
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

                EventManager.TriggerEvent(EventName.RECLUIT_ICON_ARRIVE);
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
        EventManager.StopListening(EventName.HIDE_CHARACTER_UI, HideRecluitUI);

    }
}
