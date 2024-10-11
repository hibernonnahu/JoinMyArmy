using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintDragUITrash : HintDragUI
{
    private RecluitController recluitController;

    protected override void Start()
    {
        base.Start();
        recluitController = FindObjectOfType<RecluitController>();
        EventManager.StartListening(EventName.TRY_TRASH_TUTORIAL, TryTrashTutorial);
    }
    protected override void OnTrigger(EventData arg0)
    {
        if (arg0.intData == id)
        {
            arg0.transformData.localScale = Vector3.one;
            base.OnTrigger(arg0);
            var parent = arg0.transformData.parent;
            Vector3 pos = recluitController.trash.transform.position;
            recluitController.trash.transform.SetParent(null);
            recluitController.trash.transform.SetParent(parent);
            recluitController.trash.transform.position = pos;
            EventManager.TriggerEvent(EventName.ENABLE_ICON_CONTROLLER_COLLIDER, EventManager.Instance.GetEventData().SetBool2(false));
            EventManager.TriggerEvent(EventName.HIDE_OPTIONS_UI, EventManager.Instance.GetEventData().SetBool(true));


        }
    }
    private void TryTrashTutorial(EventData arg0)
    {
        if (recluitController.text.text == "MAX")
        {
            int index = 0;
            for (int i = 0; i < recluitController.iconUI.Length; i++)
            {
                if ( !recluitController.iconUI[i].Character.UseCastRedDotUI && recluitController.iconUI[i].Character.CurrentHealth > 1 && (recluitController.iconUI[i].Character.CurrentHealth < recluitController.iconUI[index].Character.CurrentHealth))
                {
                    index = i;
                }
            }
           

            LeanTween.cancel(recluitController.iconUI[index].gameObject);
            LeanTween.cancel(recluitController.trash.gameObject);
            recluitController.trash.transform.localScale = Vector3.one;
            EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetInt(101).SetTransform(recluitController.iconUI[index].transform).SetFloat(recluitController.trash.transform.position.y).SetFloat2(recluitController.trash.transform.position.x));
        }

    }
    protected override void OnDestroy()
    {
        EventManager.StopListening(EventName.TRY_TRASH_TUTORIAL, TryTrashTutorial);

        base.OnDestroy();
    }
}
