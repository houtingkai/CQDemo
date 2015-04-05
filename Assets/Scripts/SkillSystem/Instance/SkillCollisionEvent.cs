using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class SkillCollisionEvent : SkillEventBase
{
    protected int hitCount;
    protected float lastHitTime;
    protected Dictionary<Unit, int> eachHitCount = new Dictionary<Unit,int>();
    protected Dictionary<Unit, float> eachLastHitCount = new Dictionary<Unit,float>();


    public SkillCollisionEvent(Skill skill, SkillCollisionEventInfo info)
        : base(skill, info)
    {
        hitCount = 0;
    }

    public void DrawCollisionCircle()
    {
        SkillCollisionEventInfo cinfo = info as SkillCollisionEventInfo;
        GLEx.DrawCircle(Quaternion.Euler(new Vector3(90, 0, 0)), pos, cinfo.hitScope, Color.white);
    }

    private List<Unit> DetectCollision()
    {
        List<Unit> hitList = new List<Unit>();
        SkillCollisionEventInfo cinfo = info as SkillCollisionEventInfo;
        List<Unit> units = Unit.units;
        foreach (Unit unit in units)
        {
            if (unit == mySkill.owner)
                continue;

            if (unit.IsEnemy(mySkill.owner))
            {
                Vector3 targetPos = unit.transform.position;
                if (Vector3.Distance(targetPos, pos) < cinfo.hitScope)
                {
                    hitList.Add(unit);
                }
            }
        }
        return hitList;
    }

    public override void Tick()
    {
        base.Tick();
        switch (state)
        {
            case State.Work:
                {
                    SkillCollisionEventInfo cinfo = info as SkillCollisionEventInfo;
                    if (cinfo.hitType == SkillCollisionEventInfo.HitType.Total)
                    {
                        TickTotalHit(cinfo);
                    }
                    else if(cinfo.hitType == SkillCollisionEventInfo.HitType.Each)
                    {
                        TickEachHit(cinfo);
                    }
                    break;
                }            
        }
    }


    private void TickEachHit(SkillCollisionEventInfo cinfo)
    {
        List<Unit> hitted = DetectCollision();

		Debug.Log("hitted: " + hitted.Count);

        foreach (Unit hit in hitted)
        {
            if(!eachHitCount.ContainsKey(hit))
            {
                eachHitCount.Add(hit, 0);
            }

            if (!eachLastHitCount.ContainsKey(hit))
            {
                eachLastHitCount.Add(hit, 0);
            }

            if(Time.time - eachLastHitCount[hit] > cinfo.hitInterval)
            {
                if(eachHitCount[hit] < cinfo.hitCount)
                {
                    mySkill.TriggerHitEvent(cinfo.hitEventIndex, hit);
                    eachHitCount[hit]++;
                    eachLastHitCount[hit] = Time.time;
                }
            }
        }

    }

    private void TickTotalHit(SkillCollisionEventInfo cinfo)
    {

        if (Time.time - lastHitTime > cinfo.hitInterval)
        {
            List<Unit> hitted = DetectCollision();
            if (hitted.Count > 0 && hitCount < cinfo.hitCount)
            {
                foreach (Unit hit in hitted)
                {
                    mySkill.TriggerHitEvent(cinfo.hitEventIndex, hit);
                }

                hitCount++;
                lastHitTime = Time.time;
            }

            if(hitCount >= cinfo.hitCount)
            {
                ChangeState(State.End);
            }
        }
    }
}
