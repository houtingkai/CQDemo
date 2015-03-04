using UnityEngine;
using System.Collections;

public class SkillController : MonoBehaviour
{
    public void NormalAttack(string skillId)
    {
        SkillInfo skillInfo = SkillDatabase.Instance.GetSkillInfo(skillId);
        Character owner = GetComponent<Character>();
        SkillInfo clone = skillInfo.Clone() as SkillInfo;
        SkillSystem.Instance.UseSkill(owner, clone);
    }

    public void CastSkill()
    {
        Character owner = GetComponent<Character>();
        string skillId = owner.currentSkillCast.skillId + "_" + owner.currentSkillCast.level;
        SkillInfo skillInfo = SkillDatabase.Instance.GetSkillInfo(skillId);
        if(skillInfo != null)
        {
            SkillInfo clone = skillInfo.Clone() as SkillInfo;
            SkillSystem.Instance.UseSkill(owner, clone);
        }
        else
        {
            Debug.LogWarning("SkillInfo not found, skillId: " + skillId);
        }
    }
}
