
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconUIController : MonoBehaviour
{
    public bool doNotDisable = false;
    public bool canSwap = true;
    public bool canMoveToTrash = true;
    public GameObject container;
    public RectTransform currentBar;
    public Image coldDown;
    private Character character;
    public Character Character { set => character = value; get { return character; } }
    private RecluitController recluitController;
    public RecluitController RecluitController { set { recluitController = value; }get { return recluitController; } }

    public Button button;
    public RectTransform rectTransform;
    private float totalTime = -1;
    public float TotalTime { get { return totalTime; } }
    private float currentTime = -1;
    public float CurrentTime { get { return currentTime; } }
    private Vector3 initialPosition;
    private Action onUpdateColdDown = () => { };
    private int indexPosition;
    private bool canBeDragged = true;

    public virtual int IndexPosition { get { return indexPosition; } }
    private bool drag = false;
    public int tutorialID = 2;
    public bool tutorialOnClick = false;
    private Image redDot;
    private Func<bool> extraRequirement = () => { return true; };
    public void AddExtraRequirement(Func<bool> e)
    {
        extraRequirement = e;
    }
    internal void Init(RecluitController recluitController, int indexPosition)
    {
        this.recluitController = recluitController;
        this.indexPosition = indexPosition;
        initialPosition = rectTransform.transform.position;// + Vector3.right * Screen.width * 0.5f + Vector3.down * Screen.height * 0.5f;
    }

    private void Awake()
    {
        coldDown.fillAmount = 0;
       
    }
    private void Start()
    {
        EnableButton();
    }
    public void SetCoolDownUI(float current, float total)
    {
        totalTime = total;
        if (current == 0)
        {
            onUpdateColdDown = () => { };
            EnableButton();
        }
        else
        {
            currentTime = current;
            onUpdateColdDown = OnColdDown;
            button.interactable = false;
        }
    }

    internal void Hide(bool boolData)
    {
        if (boolData)
        {
            transform.position = Vector3.right * 9999;
        }
        else
        {
            rectTransform.transform.position = initialPosition;

        }
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
            LeanTween.scale(rectTransform, Vector3.one * 1.5f, 0.4f).setEaseInCirc().setOnComplete(
               () =>
               {
                   LeanTween.scale(rectTransform, Vector3.one, 0.3f).setEaseOutCirc();
                   EnableButton();
               }
               );

        }
        coldDown.fillAmount = (currentTime / totalTime);
    }
    public void OnClick()
    {
        if (currentTime <= 0&&extraRequirement())
        {
            if(tutorialOnClick)
            EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(tutorialID));

            currentTime = totalTime = character.UseMainSkill();
            SaveDot();
            if (currentTime > 0)
            {
                LeanTween.delayedCall(gameObject, 0.3f, StartColdDown);
            }
        }
        DotDiactivate();
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
        DotDiactivate();
    }

    internal void EnableButton()
    {
        if (button != null)
        {
            coldDown.fillAmount = 0;
            currentTime = -1;
            button.interactable = true;
            if (character != null )
            {
                //OnClick();
            }
            else
            {
                CheckDot();
            }
        }
    }
    public void BeginDrag(BaseEventData data)
    {
        if (canBeDragged)
        {
            Transform parent = transform.parent;
            transform.SetParent(null, true);
            transform.SetParent(parent, true);
            button.interactable = false;
            if (canMoveToTrash)
            {
                LeanTween.cancel(recluitController.trash.gameObject, true);
                LeanTween.scale(recluitController.trash.gameObject, Vector3.one, 0.1f);
            }

            drag = true;
        }
    }
    public void Drag(BaseEventData data)
    {
        if (drag)
        {
            rectTransform.anchoredPosition = Vector3.right * (Input.mousePosition.x / Screen.width   )* recluitController.canvas.rect.width 
                + Vector3.up * (Input.mousePosition.y / Screen.height) * recluitController.canvas.rect.height;
        }
    }
    public void EndDrag(BaseEventData data)
    {
        if (drag)
        {
            recluitController.OnEndDrag(this);
        }
    }

    public void ReturnToOriginalPosition(bool swaped = false, bool animation = true)
    {

        if (animation)
        {
            button.interactable = false;
            if (swaped)
            {
                BounceAnimation(() => { }, Vector3.one);
            }
            LeanTween.move(rectTransform.gameObject, initialPosition, 0.5f).setEaseOutBack().setIgnoreTimeScale(true).setOnComplete(() =>
            {
                button.interactable = currentTime<=0; drag = false;
                CheckDot();

            });
        }
        else
        {
            rectTransform.transform.position = initialPosition;
            CheckDot();

        }
    }

    private void SaveDot()
    {
        if (character.UseCastRedDotUI)
        {
            int code = SaveData.GetInstance().GetValue("redDot" + character.id, 0);
            code++;
            SaveData.GetInstance().Save(SaveDataKey.RED_DOT + character.id, code);
        }
    }
    public void CheckDot()
    {
        if (character != null && character.UseCastRedDotUI)
        {
            int code = SaveData.GetInstance().GetValue("redDot" + character.id, 0);
            if (code < 2)
            {
               
                DotActivate();
            }
            else
            {
                DotDiactivate();
            }
        }
        else
        {
            DotDiactivate();
        }
    }

    public void BounceAnimation(Action onComplete, Vector3 finalSize)
    {
        LeanTween.scale(rectTransform, Vector3.one * 1.2f, 0.3f).setEaseInBounce().setIgnoreTimeScale(true).setOnComplete(
                  () =>
                  {
                      LeanTween.scale(rectTransform, finalSize, 0.2f).setEaseOutBounce().setIgnoreTimeScale(true).setOnComplete(onComplete);
                  }
                  );
    }
    internal void DisableDrag(bool v)
    {
        canBeDragged = !v;
    }

    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }
    private void DotActivate()
    {

        if (redDot == null)
        {
           redDot= Utils.CreateRedDot(transform,redDot);

        }
        redDot.gameObject.SetActive(true);

    }
    private void DotDiactivate()
    {
        if (redDot != null)
        {
            redDot.gameObject.SetActive(false);
        }
    }
}
