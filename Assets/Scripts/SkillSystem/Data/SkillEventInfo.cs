using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SkillEventBaseInfo : ICloneable
{
    public int index;
    public enum NodeType
    {
        Self,
        TargetFirst,
        TargetLast,
        TargetMiddle,
        Parent,
    }

    public NodeType node;
    public float offsetX;
    public float offsetY;
    public float lifeSeconds;

    public Vector3 CreatePos(Unit target)
    {
        if (target is Character)
        {
            Character character = target as Character;
            Vector3 thePos = target.transform.position + new Vector3(offsetX * character.Facing, offsetY, 0);
            return thePos;
        }

        return Vector3.zero;
    }

    public object Clone()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, this);
        ms.Position = 0;
        SkillEventBaseInfo cloneEventInfo = (bf.Deserialize(ms)) as SkillEventBaseInfo;
        return cloneEventInfo as object;
    }
}

[Serializable]
public class SkillCollisionEventInfo : SkillEventBaseInfo
{
    public enum HitType
    {
        Total,
        Each,
        None,
    }
    public HitType hitType;
    public float hitScope;
    public float hitInterval;
    public int hitCount;
    public int hitEventIndex;
}

[Serializable]
public class SkillEffectEventInfo : SkillCollisionEventInfo
{
    public enum SkillEffectType
    {
        Melee,
        Parabola,
        P2P,
        Parent
    }

    public SkillEffectType skillEffectType;
	
    //for Parabola, P2P
    public string effect;
    public float vx;
    public float vy;

    //for Parent
    public float range;
    public int subEventCount;
    public int subEventId;
}

[Serializable]
public class SkillHitEventInfo : SkillEventBaseInfo
{
    public string effect;
    public string sound;
    public float hitForceX;
    public float hitForceY;
}
