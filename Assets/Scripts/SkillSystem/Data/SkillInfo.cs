using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class SkillInfo : ScriptableObject, ICloneable
{
    public string ID;
    public List<SkillEffectEventInfo> effectEventInfoList = new List<SkillEffectEventInfo>();
    public List<SkillHitEventInfo> hitEventInfoList = new List<SkillHitEventInfo>();

    public SkillHitEventInfo GetHitEventInfoByIndex(int index)
    {
        foreach (SkillHitEventInfo info in hitEventInfoList)
        {
            if (info.index == index)
            {
                return info;
            }
        }

        return null;
    }


    public SkillEffectEventInfo GetEffectEventInfoByIndex(int index)
    {
        foreach (SkillEffectEventInfo info in effectEventInfoList)
        {
            if (info.index == index)
            {
                return info;
            }
        }

        return null;
    }

    public object Clone()
    {
        SkillInfo cloneSkillInfo = ScriptableObject.CreateInstance<SkillInfo>();
        cloneSkillInfo.ID = this.ID;
        foreach (SkillEffectEventInfo info in effectEventInfoList)
        {
            SkillEffectEventInfo clone = info.Clone() as SkillEffectEventInfo;
            cloneSkillInfo.effectEventInfoList.Add(clone);
        }

        foreach (SkillHitEventInfo info in hitEventInfoList)
        {
            SkillHitEventInfo clone = info.Clone() as SkillHitEventInfo;
            cloneSkillInfo.hitEventInfoList.Add(clone);
        }

        return cloneSkillInfo as object;
    }
}
