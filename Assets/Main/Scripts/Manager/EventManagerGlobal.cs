using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;




public class EventManagerGlobal
{
    private EventData eventData = new EventData();
   
    public EventData GetEventData()
    {
       
        return eventData;
    }
    private Dictionary <string, UnityEvent<EventData>> eventDictionary;

    private static EventManagerGlobal eventManager;

    public static EventManagerGlobal Instance
    {
        get
        {
            
            if(eventManager==null)
            {
                eventManager = new EventManagerGlobal();
            }

            return eventManager;
        }
    }

   public EventManagerGlobal()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent<EventData>>();
        }
    }

    public static void StartListening (string eventName, UnityAction<EventData> listener)
    {
       
        UnityEvent<EventData> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.AddListener (listener);
        } 
        else
        {
            thisEvent = new EventDataConstruct();
            thisEvent.AddListener (listener);
            Instance.eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction<EventData> listener)
    {

        if (eventManager == null) return;
        UnityEvent<EventData> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.RemoveListener (listener);
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

    public static void TriggerEvent (string eventName,EventData eventData = null)
    {
        //Debug.Log("TriggerEvent");
        UnityEvent<EventData> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            if (eventData == null)
            {
                eventData = Instance.eventData;
            }
            eventData.eventName = eventName;
            thisEvent.Invoke (eventData);
        }
    }
}
