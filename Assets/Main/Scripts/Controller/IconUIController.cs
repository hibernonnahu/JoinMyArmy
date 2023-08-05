
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconUIController : MonoBehaviour
{
    public GameObject container;
    public RectTransform currentBar;
    public Image coldDown;
    private CharacterEnemy characterEnemy;
    public CharacterEnemy CharacterEnemy { set => characterEnemy = value; get { return characterEnemy; } }
    private RecluitController recluitController;
    public RecluitController RecluitController { set { recluitController = value; } }

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

    public int IndexPosition { get { return indexPosition; } }
    private bool drag = false;
    public int tutorialID = 2;
    private Image redDot;


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
    public void SetCoolDownUI(float current, float total)
    {
        totalTime = total;
        if (current == 0)
        {
            EnableButton();
            onUpdateColdDown = () => { };
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
            EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(tutorialID));

            currentTime = totalTime = characterEnemy.UseMainSkill();
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
        coldDown.fillAmount = 0;
        currentTime = -1;
        button.interactable = true;
        CheckDot();
    }
    public void BeginDrag(BaseEventData data)
    {
        if (canBeDragged)
        {
            Transform parent = transform.parent;
            transform.SetParent(null, true);
            transform.SetParent(parent, true);
            button.interactable = false;
            LeanTween.cancel(recluitController.trash.gameObject, true);
            LeanTween.scale(recluitController.trash.gameObject, Vector3.one, 0.1f);

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
        if (characterEnemy.UseCastRedDotUI)
        {
            int code = SaveData.GetInstance().GetValue("redDot" + characterEnemy.id, 0);
            code++;
            SaveData.GetInstance().Save("redDot" + characterEnemy.id, code);
        }
    }
    public void CheckDot()
    {
        if (characterEnemy != null && characterEnemy.UseCastRedDotUI)
        {
            int code = SaveData.GetInstance().GetValue("redDot" + characterEnemy.id, 0);
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
            GameObject NewObj = new GameObject(); //Create the GameObject
            NewObj.name = "redDot";
            redDot = NewObj.AddComponent<Image>();
            redDot.color = Color.red;
            var rect = redDot.GetComponent<RectTransform>();
            rect.SetParent(transform);
            redDot.sprite = Resources.Load<Sprite>("Texture/circle");
            rect.anchoredPosition = Vector2.one * 50;
            rect.localScale = Vector2.one * 0.5f;

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
