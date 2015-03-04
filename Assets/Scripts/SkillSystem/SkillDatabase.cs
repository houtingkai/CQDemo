using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDatabase : SingletonBehaviour<SkillDatabase>
{
    public List<SkillInfo> skillTemplates = new List<SkillInfo>();

    public SkillInfo GetSkillInfo(string skillId)
    {
        foreach (SkillInfo template in skillTemplates)
        {
            if (template.ID == skillId)
            {
                return template;
            }
        }
        return null;
    }
}
