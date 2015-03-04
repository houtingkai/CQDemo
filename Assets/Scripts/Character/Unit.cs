using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public static List<Unit> units = new List<Unit>();

    public static Character GetCharacterByIndex(int index)
    {
        foreach (Unit unit in Unit.units)
        {
            if (unit is Character)
            {
                Character theCharacter = unit as Character;
                if (theCharacter.characterIdex == index)
                    return theCharacter;
            }
        }

        return null;
    }

    public class UnitPosComparer : IComparer<Unit>
    {
        public int Compare(Unit x, Unit y)
        {
            if(x.camp == y.camp)
            {
                if(x.camp == Camp.Friend)
                {
                    if(y.transform.position.x - x.transform.position.x > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if(x.camp == Camp.Enemy)
                {
                    if (y.transform.position.x - x.transform.position.x > 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }

                return 0;
            }
            else
            {
                return 0;
            }
        }
    }

    public static void PrintCharList(List<Unit> list)
    {
        int i = 0;
        string str = "";
        foreach(Unit unit in list)
        {
            str+= string.Format("{0}:{1},", i++, unit.transform.gameObject.name);
        }
        Debug.Log(str);
    }

    public static Character GetCharacterByPos(Camp camp, int pos)
    {
        List<Unit> friendCamp = new List<Unit>();
        List<Unit> enemyCamp = new List<Unit>();

        foreach(Unit unit in units)
        {
            if(unit.camp == Camp.Friend)
            {
                friendCamp.Add(unit);
            }
            else if(unit.camp == Camp.Enemy)
            {
                enemyCamp.Add(unit);
            }
        }

        friendCamp.Sort(new UnitPosComparer());
        enemyCamp.Sort(new UnitPosComparer());

        if(camp == Camp.Enemy)
        {
            if (pos > enemyCamp.Count)
                return null;
            Character character =  enemyCamp[pos - 1] as Character;
            return character;
        }
        else if(camp == Camp.Friend)
        {
            if (pos > friendCamp.Count)
                return null;
            Character character = friendCamp[pos - 1] as Character;
            return character;
        }
        return null;
    }

    public static List<Character> SearchEnemy(Character character)
    {
        List<Character> charList = new List<Character>();
        Vector3 myPos = character.transform.position;

        foreach (Unit unit in Unit.units)
        {
            if (unit is Character)
            {
                Character theCharacter = unit as Character;

                if (theCharacter == character)
                    continue;

                if (!character.IsEnemy(unit))
                    continue;

                Vector3 thePos = theCharacter.transform.position;
                float dist = Vector3.Distance(thePos, myPos);
                if (dist < character.attackRange)
                {
                    charList.Add(theCharacter);
                }
            }
        }
        return charList;
    }

    public static Character SearchFriendOfT(Character character)
    {
        foreach (Unit unit in Unit.units)
        {
            if (unit is Character)
            {
                Character theCharacter = unit as Character;

                if (theCharacter == character)
                    continue;

                if (character.IsEnemy(unit))
                    continue;

                if (theCharacter.careerType != Character.CareerType.Melee)
                    continue;

                return theCharacter;
            }
        }

        return null;
    }

    public enum Camp
    {
        Friend,
        Enemy,
    }

    public Camp camp;

    public bool IsEnemy(Unit unit)
    {
        return camp != unit.camp;
    }

    public Camp GetEnemyCamp()
    {
        if(camp == Camp.Enemy)
        {
            return Camp.Friend;
        }
        else
        {
            return Camp.Enemy;
        }
    }

    public virtual void init()
    {
        units.Add(this);
    }

    public void Awake()
    {
        init();
    }
}
