
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
    private float currentTime = -1;
    private Vector3 initialPosition;
    private Action onUpdateColdDown = () => { };
    private int indexPosition;
    public int IndexPosition { get { return indexPosition; } }
    private bool drag = false;

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
            EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(2));

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
    public void BeginDrag(BaseEventData data)
    {
        if (button.interactable)
        {
            Transform parent = transform.parent;
            transform.SetParent(null, true);
            transform.SetParent(parent, true);
            button.interactable = false;
            drag = true;
        }
    }
    public void Drag(BaseEventData data)
    {
        if (drag)
        {
            LeanTween.scale(recluitController.trash.gameObject, Vector3.one, 0.1f);
            rectTransform.transform.localPosition = Vector3.right * (Input.mousePosition.x - Screen.width * 0.5f) + Vector3.up * (Input.mousePosition.y - Screen.height * 0.5f);
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
                BounceAnimation(()=> { },Vector3.one);
            }
            LeanTween.move(rectTransform.gameObject, initialPosition, 0.5f).setEaseOutBack().setOnComplete(() => { button.interactable = true; drag = false; });
        }
        else
        {
            rectTransform.transform.position = initialPosition;
        }
    }

    public void BounceAnimation(Action onComplete,Vector3 finalSize)
    {
        LeanTween.scale(rectTransform, Vector3.one * 1.2f, 0.3f).setEaseInBounce().setOnComplete(
                  () =>
                  {
                      LeanTween.scale(rectTransform, finalSize, 0.2f).setEaseOutBounce().setOnComplete(onComplete);
                  }
                  );
    }
}
