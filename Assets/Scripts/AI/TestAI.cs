using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillCastInfo
{
    public int skillId;
    public int level;
}

public class TestAI : MonoBehaviour 
{
    Character character;

    private Queue<SkillCastInfo> skillsToCast = new Queue<SkillCastInfo>();

    public void ToCastSkill(int skillId, int level)
    {
        SkillCastInfo info = new SkillCastInfo();
        info.skillId = skillId;
        info.level = level;
        skillsToCast.Enqueue(info);
    }

    public void DeQueueCastSkill()
    {
        skillsToCast.Dequeue();
    }

	void Start () 
    {
        character = GetComponent<Character>();
	}

    private Character GetClosestEnemy(List<Character> enemyList)
    {
        Character closest = null;
        float distMin = float.MaxValue;
        Vector3 myPos = character.transform.position;
        foreach (Character enemy in enemyList)
        {
            Vector3 thePos = enemy.transform.position;
            float dist = Vector3.Distance(myPos, thePos);
            if (dist < distMin)
            {
                distMin = dist;
                closest = enemy;
            }
        }

        return closest;
    }

	void Update () 
    {
        if(skillsToCast.Count > 0)
        {
            SkillCastInfo info = skillsToCast.Peek();
            character.SkillCast(info);
        }

        List<Character>  charList = Unit.SearchEnemy(character);
        if (charList.Count <= 0)
        {
            character.Run(true);
        }
        else
        {
            character.Idle();
        }

        Character target = GetClosestEnemy(charList);
        if (target != null)
        {

            character.Attack();
        }

        Character friendT = Unit.SearchFriendOfT(character);
        if(friendT != null)
        {
            Vector3 thePos = friendT.transform.position;
            Vector3 myPos = character.transform.position;
            float dist = Vector3.Distance(thePos, myPos);
            if(dist < 0.3)
            {
                //character.Run(false);
            }
        }
	}
}
