﻿using UnityEngine;
using System.Collections;

public class SkillHitEvent : SkillEventBase
{
    protected Unit target; 

    public SkillHitEvent(Skill skill, SkillHitEventInfo info, Unit target): base(skill,info)
    {
        base.Init();
        this.target = target;
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
        switch (newState)
        {
            case State.End:
                {
                    if (target is Character)
                    {
                        Character character = target as Character;
                        character.hitBack = false;
                    }
                    break;
                }
        }
    }

    public override void Tick()
    {
        base.Tick();

        switch (state)
        {
            case State.Work:
                {
                    if (target is Character)
                    {
                        SkillHitEventInfo hitInfo = info as SkillHitEventInfo;
                        Character character = target as Character;
                        character.gameObject.rigidbody2D.AddForce(new Vector2(hitInfo.hitForceX * character.Facing * -1, hitInfo.hitForceY));
                        character.hitBack = true;
                    }
                    break;
                }
        }
    }
}
