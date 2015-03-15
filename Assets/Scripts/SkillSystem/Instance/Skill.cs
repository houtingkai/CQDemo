using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill 
{
    public enum State
    {
        Init,
        Work,
        End,
    }

    public State state;

    public Character owner;
    private SkillInfo skillInfo;
    private List<SkillEventBase> addEvents = new List<SkillEventBase>();
    private List<SkillEventBase> runningEvents = new List<SkillEventBase>();

    public Skill(Character owner, SkillInfo skillInfo)
    {
        this.owner = owner;
        this.skillInfo = skillInfo.Clone() as SkillInfo;
        ChangeState(State.Init);
    }

    public SkillEffectEvent TriggerEffectEvent(int index, SkillEventBase parent = null)
    {
        if(parent != null)
        {
            int i = 1;
            i++;
        }

        SkillEffectEventInfo eventInfo = skillInfo.GetEffectEventInfoByIndex(index);
        if (eventInfo != null)
        {
            SkillEffectEvent evt = new SkillEffectEvent(this, eventInfo.Clone() as SkillEffectEventInfo);
            if(parent != null)
            {
                evt.SetParent(parent);
            }
            addEvents.Add(evt);
            
            if(eventInfo.index == 1)
            {
                skillInfo.effectEventInfoList.Remove(eventInfo);
            }


            return evt;
        }

        return null;
    }

    public void TriggerHitEvent(int index, Unit target)
    {
        SkillHitEventInfo eventInfo = skillInfo.GetHitEventInfoByIndex(index);
        if (eventInfo != null)
        {
            SkillHitEvent evt = new SkillHitEvent(this, eventInfo.Clone() as SkillHitEventInfo, target);
            addEvents.Add(evt);
        }
    }

    private void ChangeState(State state)
    {
        this.state = state;
        switch (state)
        {
            case State.Init:
                {
                    SkillEffectEvent evt = TriggerEffectEvent(1);
					evt.Init();
                    break;
                }
            case State.Work:
                {
                    break;
                }
            case State.End:
                {
//                    Debug.Log("Skill end");
                    break;
                }
        }
    }

    private void AddEvent()
    {
        foreach (SkillEventBase addEvent in addEvents)
        {
            runningEvents.Add(addEvent);
        }
        addEvents.Clear();
    }

    private void RemoveEndEvent()
    {
        List<SkillEventBase> toRemoveList = new List<SkillEventBase>();
        foreach (SkillEventBase evt in runningEvents)
        {
            if (evt.state == SkillEventBase.State.End)
            {
                toRemoveList.Add(evt);
            }
        }

        foreach (SkillEventBase toRemove in toRemoveList)
        {
            runningEvents.Remove(toRemove);
        }
    }

    public void Tick()
    {
        switch (state)
        {
            case State.Init:
                {
                    ChangeState(State.Work);
                    break;
                }
            case State.Work:
                {
                    AddEvent();
                    RemoveEndEvent();

                    foreach (SkillEventBase evt in runningEvents)
                    {
                        evt.Tick();
                    }

                    SkillEffectEventInfo eventInfo = skillInfo.GetEffectEventInfoByIndex(1);
                    if (eventInfo == null && runningEvents.Count == 0)
                    {
                        ChangeState(State.End);
                    }

                    break;
                }
        }
    }

    public void DrawHitCircles()
    {
        foreach (SkillEventBase evt in runningEvents)
        {
            if (evt is SkillCollisionEvent)
            {
                SkillCollisionEvent scevt = evt as SkillCollisionEvent;
                scevt.DrawCollisionCircle();
            }
        }
    }
}
