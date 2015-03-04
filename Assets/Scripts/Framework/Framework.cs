using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Generic;


public class Framework : MonoBehaviour
{
    static GameObject rootObject_;
    static Framework instance_ = null;

    public static bool HasInstance
    {
        get { return instance_ != null; }
    }

    public static Framework Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = new GameObject("GameRoot").AddComponent<Framework>();
            }

            return instance_;
        }
    }

    static Dictionary<string, List<System.Action<object>>> eventRegistry_ = new Dictionary<string, List<System.Action<object>>>();

    public static void RegisterEvent(string eventName, System.Action<object> eventHandle)
    {
        List<System.Action<object> >evt;
        if (eventRegistry_.TryGetValue(eventName, out evt))
        {
            evt.Add(eventHandle);
        }
        else
        {
            evt = new List<System.Action<object>>();
            evt.Add(eventHandle);
            eventRegistry_[eventName] = evt;
        }
    }

    public static void UnregisterEvent(string eventName, System.Action<object> eventHandle)
    {
        List<System.Action<object>> evtList;
        if (eventRegistry_.TryGetValue(eventName, out evtList))
        {
            evtList.Remove(eventHandle);
        }
    }

    public static void SendEvent(string eventName, object param = null)
    {
        List<System.Action<object>> evtList;
        if (eventRegistry_.TryGetValue(eventName, out evtList))
        {
            foreach (System.Action<object> evt in evtList)
            {
                evt(param);
            }

        }
    }

    public static T AddComponent<T>() where T : Component
    {
        return rootObject_.AddComponent<T>();
    }

    public static T Get<T>() where T : MonoBehaviour
    {
        return rootObject_.GetComponent<T>();
    }

    void Awake()
    {
        instance_ = this;
        rootObject_ = gameObject;
        DontDestroyOnLoad(rootObject_);

        ConfigComponents();

    }

    protected virtual void ConfigComponents()
    {
        GameObject go = new GameObject("SkillSystem");
        go.transform.parent = gameObject.transform;
        go.AddComponent<SkillSystem>();
    }
    
    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


}
