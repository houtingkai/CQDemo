using UnityEngine;
using System.Collections;

public class SkillCast : StateBase
{
    private SkillCastInfo skillCast;

    public override void OnEnter(object param)
    {
        Character me = owner as Character;
        skillCast = (SkillCastInfo)param;
    }

    public override void OnExit()
    {
        Character me = owner as Character;
        me.gameObject.GetComponent<TestAI>().DeQueueCastSkill();
    }

    public override void OnReset(object param)
    {
        //Debug.Log("Skill OnReset");
    }
}
