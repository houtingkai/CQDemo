using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSystem : SingletonBehaviour<SkillSystem> 
{
    public List<Skill> skills = new List<Skill>();

    public void UseSkill(Character owner,SkillInfo skillInfo)
    {
        Skill skill = new Skill(owner, skillInfo);
        skills.Add(skill);
    }

    public void Update()
    {
        //Debug.Log("SkillSystem Update");
        RemoveEndSkill();

        foreach (Skill skill in skills)
        {
            skill.Tick();
        }
    }

    private void RemoveEndSkill()
    {
        List<Skill> toRemoveList = new List<Skill>();

        foreach (Skill skill in skills)
        {
            if (skill.state == Skill.State.End)
            {
                toRemoveList.Add(skill);
            }
        }

        foreach (Skill toRemove in toRemoveList)
        {
            skills.Remove(toRemove);
        }
    }

    public void DrawHitCircles()
    {
        foreach (Skill skill in skills)
        {
            skill.DrawHitCircles();
        }
    }
}
