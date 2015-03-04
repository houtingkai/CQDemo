using UnityEngine;
using System.Collections;

public class SkillEffectEvent : SkillCollisionEvent
{
    private GameObject effect;

    public SkillEffectEvent(Skill skill, SkillEffectEventInfo info): base(skill,info)
    {
        //base.Init();
    }

    public override void Tick()
    {
        base.Tick();
        SkillEffectEventInfo einfo = info as SkillEffectEventInfo;
        switch (state)
        {
            case State.Work:
                {
                    if (einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.Parabola)
                    {
                        TickParabola(einfo);
                    }
                    else if (einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.P2P)
                    {
                        TickP2P(einfo);
                    }
                    else if (einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.Parent)
                    {
                        TickParent(einfo);
                    }
                    break;
                }
        }
    }

    private void TickParent(SkillEffectEventInfo einfo)
    {
        float interval = einfo.range / einfo.subEventCount;

        for(int i = 0; i < einfo.subEventCount; i++)
        {
            SkillEffectEvent evt = mySkill.TriggerEffectEvent(einfo.subEventId,this);

            Vector3 offset = new Vector3(interval * i, 0, 0);
            evt.Init();
            evt.AdjustPos(offset);
        }

        ChangeState(State.End);
    }

    private void TickP2P(SkillEffectEventInfo einfo)
    {
        if(this.parent != null)
        {
            int i = 1;
            i++;

        }

        float sx = Time.deltaTime * einfo.vx;
        float sy = Time.deltaTime * einfo.vy;
        pos += new Vector3(sx * mySkill.owner.Facing, sy, 0);
        if (effect != null)
        {
            effect.transform.position = pos;
        }

        if (pos.y < Contants.groundHeight)
        {
            ChangeState(State.End);
        }
    }

    private void TickParabola(SkillEffectEventInfo einfo)
    {
        //float sx = Time.deltaTime * einfo.vx;
        float elapsed = Time.time - createTime;
        float sx = startPos.x + elapsed * einfo.vx * mySkill.owner.Facing;
        float sy = einfo.vy * elapsed - 0.5f * elapsed * elapsed * 10;
        float vx = einfo.vx;
        float vy = einfo.vy - 10 * elapsed;
        float angle = Mathf.Atan2(vy, vx) * Mathf.Rad2Deg;


        pos = new Vector3(sx, sy, 0);
        //float sy = einfo.vy * Time.deltaTime - 
        //pos += new Vector3(s, 0, 0);
        if (effect != null)
        {
            effect.transform.position = pos;
            effect.transform.rotation = Quaternion.Euler(0, mySkill.owner.Facing == 1 ? 0 : 180, angle);
        }

        if (sy < Contants.groundHeight)
        {
            ChangeState(State.End);
        }
    }


    public void AdjustPos(Vector3 offset)
    {
        pos += offset;

        if (effect == null)
            return;

        SkillEffectEventInfo einfo = info as SkillEffectEventInfo;
        if (einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.Parabola || einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.P2P)
        {
            effect.transform.position = pos;
        }
    }


    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
        SkillEffectEventInfo einfo = info as SkillEffectEventInfo;

        switch (newState)
        {
            case State.Init:
                {

                    if (einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.Melee)
                        return;

                    if (einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.Parabola)
                    {
                        float angle = Mathf.Atan2(einfo.vx, einfo.vy) * Mathf.Rad2Deg;
                        Object go = Resources.Load(einfo.effect);
                        effect = GameObject.Instantiate(go, pos, Quaternion.Euler(0, mySkill.owner.Facing == 1? 0: 180, angle)) as GameObject;
                    }
                    else if(einfo.skillEffectType == SkillEffectEventInfo.SkillEffectType.P2P)
                    {
                        Object go = Resources.Load(einfo.effect);
                        effect = GameObject.Instantiate(go, pos, Quaternion.Euler(0, mySkill.owner.Facing == 1 ? 0 : 180, 0)) as GameObject;
                    }
                    break;
                }
            case State.End:
                {
                    GameObject.Destroy(effect);
                    break;
                }
        }
    }
}
