using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum StateEvent : int
{
    Reset,

}

public class StateBase 
{
    string name_;

    public string Name
    {
        get
        {
            return name_;
        }
        set
        {
            name_ = value;
        }
    }

    public StateMachine stateMachine;
    public Unit owner;
    public float enterTime;

    public StateBase()
    {
        String[] name = this.GetType().ToString().Split('.');
        Name = name[name.Length - 1];
    }

    public int currentNameHash()
    {
        return stateMachine.currentNameHash;
    }

    public virtual void OnInit()
    {
        owner = stateMachine.GetComponent<Unit>();

    }

    public virtual void OnEnter(object param)
    {
        enterTime = Time.time;
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate(float deltaTime)
    {

    }

    public virtual void OnReset(object param)
    {
        //if (owner is Player)
        //{
        //    Debug.Log(this + " OnReset");
        //}
    }

    public virtual void OnEvent(int evt, object param)
    {
        if(evt == (int)StateEvent.Reset)
        {
            OnReset(param);
        }
    }

    public virtual void TickPlayer(float deltaTime)
    {

    }

    public virtual void TickMonster(float deltaTime)
    {

    }
}
