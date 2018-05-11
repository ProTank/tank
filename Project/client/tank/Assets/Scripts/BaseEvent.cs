using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class BaseEvent : EventDispatcher
{
    private static BaseEvent instance;
    public static BaseEvent Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BaseEvent();
            }
            return instance;
        }
    }

    public void RegisterEvent(string eventId, EventCallback0 func)
    {
        AddEventListener(eventId, func);
    }

    public void RegisterEvent(string eventId, EventCallback1 func)
    {
        AddEventListener(eventId, func);
    }

    public void TriggerEvent(string eventId)
    {
        DispatchEvent(eventId);
    }

    public void TriggerEvent(string eventId, object data)
    {
        DispatchEvent(eventId, data);
    }

    public void RemoveEvent(string eventId, EventCallback0 func)
    {
        RemoveEventListener(eventId, func);
    }

    public void RemoveEvent(string eventId, EventCallback1 func)
    {
        RemoveEventListener(eventId, func);
    }

}
