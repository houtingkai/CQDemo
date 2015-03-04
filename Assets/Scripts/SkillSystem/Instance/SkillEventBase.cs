using UnityEngine;
using System.Collections;

abstract public class SkillEventBase
{
    public enum State
    {
        Init,
        Work,
        End,
    }

    public State state;
    protected Vector3 pos;
    protected Vector3 startPos;
    protected SkillEventBaseInfo info;
    protected float createTime;
    protected Skill mySkill;
    protected SkillEventBase parent;

    public SkillEventBase(Skill skill, SkillEventBaseInfo info)
    {
        this.mySkill = skill;
        this.info = info;
    }

    public void SetParent(SkillEventBase parent)
    {
        this.parent = parent;
    }

    public virtual void ChangeState(State newState)
    {
        state = newState;
        switch (state)
        {
            case State.Init:
                {
                    if(info.node == SkillEventBaseInfo.NodeType.Self)
                    {
                        pos = info.CreatePos(mySkill.owner);
                        startPos = pos;
                        createTime = Time.time;
                    }
                    else if(info.node == SkillEventBaseInfo.NodeType.TargetFirst)
                    {
                        Character target = Unit.GetCharacterByPos(mySkill.owner.GetEnemyCamp(), 1);
                        pos = info.CreatePos(target);
                        startPos = pos;
                        createTime = Time.time;
                    }
                    else if(info.node == SkillEventBaseInfo.NodeType.Parent)
                    {
                        if(this.parent != null)
                        {
                            pos = parent.pos;
                        }
                        startPos = pos;
                        createTime = Time.time;
                    }
                    break;
                }
        }
    }

    public virtual void Init()
    {
        ChangeState(State.Init);
    }

    public virtual void Tick()
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
                    if (info.lifeSeconds < 0)
                        break;

                    if (Time.time - createTime > info.lifeSeconds)
                    {
                        ChangeState(State.End);
                    }
                    break;
                }
        }
    }
}

