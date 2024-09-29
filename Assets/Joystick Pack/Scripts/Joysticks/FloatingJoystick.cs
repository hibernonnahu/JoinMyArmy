using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private Vector2 initPosition;
    protected override void Start()
    {
        base.Start();

#if UNITY_STANDALONE || UNITY_WEBGL
        //gameObject.SetActive(false);
        transform.position = Vector3.right * 9999;
#else
        EventManager.StartListening(EventName.HIDE_CHARACTER_UI, Hide);
#endif

        initPosition = background.anchoredPosition;
    }
#if UNITY_STANDALONE || UNITY_WEBGL
    private void Update()
    {
        input = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.A))
        {
            input = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            input = Vector2.right;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            input += Vector2.down;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            input += Vector2.up;
        }
    }
#else
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.HIDE_CHARACTER_UI, Hide);
    }

    private void Hide(EventData arg0)
    {
        gameObject.SetActive(!arg0.boolData);
    }
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //background.gameObject.SetActive(false);
        background.anchoredPosition = initPosition;
        base.OnPointerUp(eventData);
    }
}