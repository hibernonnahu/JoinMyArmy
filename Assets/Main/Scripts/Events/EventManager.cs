﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventData
{
    public System.Action actionData = null;
    public EventData SetAction(System.Action a)
    {
        actionData = a;
        return this;
    }
    public long longData = -1;
    public EventData SetLong(long f)
    {
        longData = f;
        return this;
    }
    public float floatData = -1;
    public EventData SetFloat(float f)
    {
        floatData = f;
        return this;
    }
    public float floatData2 = -1;
    public EventData SetFloat2(float f)
    {
        floatData2 = f;
        return this;
    }
    public float floatData3 = -1;
    public EventData SetFloat3(float f)
    {
        floatData3 = f;
        return this;
    }
    public int intData = -1;
    public EventData SetInt(int i)
    {
        intData = i;
        return this;
    }
    public int intData2 = -1;
    public EventData SetInt2(int i)
    {
        intData2 = i;
        return this;
    }
    public Vector4 vec4;
    public EventData SetVec4(Vector4 v)
    {
        vec4 = v;
        return this;
    }
    public bool boolData = false;
    public EventData SetBool(bool b)
    {
        boolData = b;
        return this;
    }
    public bool boolData2 = false;
    public EventData SetBool2(bool b)
    {
        boolData2 = b;
        return this;
    }
    public string stringData;
    public EventData SetString(string s)
    {
        stringData = s;
        return this;
    }
    public string stringData2;
    public EventData SetString2(string s)
    {
        stringData2 = s;
        return this;
    }
    public string stringData3;
    public EventData SetString3(string s)
    {
        stringData3 = s;
        return this;
    }
    public Transform transformData;
    public EventData SetTransform(Transform t)
    {
        transformData = t;
        return this;
    }
    public GameObject[] gameObjects;
    public string eventName = "";
}
public class EventDataConstruct : UnityEvent<EventData>
{
}

public class EventManager
{
    private EventData eventData = new EventData();

    public EventData GetEventData()
    {

        return eventData;
    }
    private Dictionary<string, UnityEvent<EventData>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {

            if (eventManager == null)
            {
                eventManager = new EventManager();
            }

            return eventManager;
        }
    }

    public EventManager()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent<EventData>>();
        }
    }

    public static void StartListening(string eventName, UnityAction<EventData> listener)
    {
        UnityEvent<EventData> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new EventDataConstruct();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<EventData> listener)
    {

        if (eventManager == null) return;
        UnityEvent<EventData> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public void ClearAll()
    {
        if (eventManager == null) return;
        foreach (var eventName in eventDictionary.Keys)
        {
            UnityEvent<EventData> thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveAllListeners();
            }
        }
    }
   
    public static void TriggerEvent(string eventName, EventData eventData = null)
    {
        //Debug.Log("TriggerEvent "+eventName);
        UnityEvent<EventData> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            if (eventData == null)
            {
                eventData = Instance.eventData;
            }
            eventData.eventName = eventName;
            thisEvent.Invoke(eventData);
        }
    }

    internal static void StartListening(object sHUFFLE_SKILL, object onShuffleSkill)
    {
        throw new System.NotImplementedException();
    }
}
