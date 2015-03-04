using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization;

public class SkillBlockPanel : MonoBehaviour 
{
    private Transform[] slotTRs;
    private List<SkillTemplate> skillSlots = new List<SkillTemplate>();
    public float blockGenerateInterval = 1.5f;
    private float lastTimeBlockGenerate = 0;

    [Serializable]
    public class SkillTemplate
    {
        public int skillId;
        public int characterIdex;
    }

    public SkillTemplate[] SkillTemplateList;
    private bool Dirty = false;

	void Start ()
    {
        UISprite[] blackSprites = gameObject.transform.FindChild("slot").GetComponentsInChildren<UISprite>();
        slotTRs = new Transform[blackSprites.Length];
        for (int i = 0; i < blackSprites.Length; i++)
        {
            slotTRs[i] = blackSprites[i].transform;
            UIEventListener.Get(slotTRs[i].gameObject).onClick = SkillBlockOnClick;
        }

        for(int i = 0; i < blackSprites.Length; i++)
        {
            SkillTemplate t = new SkillTemplate();
            t.skillId = -1;
            skillSlots.Add(t);
        }
	}

    private void SkillBlockOnClick(GameObject go)
    {
        int index = int.Parse(go.name) - 1;
        SkillTemplate t = skillSlots[index];
        int skillIdTrigger = t.skillId;
        if (skillIdTrigger == -1)
            return;

        List<int> chains = FindMaxChain(index);
        int level = chains.Count;
        EliminateChain(chains);

        Debug.Log("skillIdTrigger: " + skillIdTrigger + ", level: " + level + ", characterIdex: " + t.characterIdex);

        Character theCharacter = Unit.GetCharacterByIndex(t.characterIdex);
        if(theCharacter != null)
        {
            theCharacter.gameObject.GetComponent<TestAI>().ToCastSkill(skillIdTrigger, level);
        }

    }

    private void EliminateChain(List<int> chainIndices)
    {
        skillSlots.RemoveRange(chainIndices[0], chainIndices.Count);
        for (int i = 0; i < chainIndices.Count; i++)
        {
            SkillTemplate t = new SkillTemplate();
            t.skillId = -1;
            skillSlots.Insert(0, t);
        }
        Dirty = true;
    }

    private List<int> FindMaxChain(int index)
    {
        List<int> chainList = new List<int>();
        chainList.Add(index);
        SkillTemplate t = skillSlots[index];
        for(int i=index-1; i >=0; i--)
        {
            if (chainList.Count == 3)
                break;

            if (t.skillId == skillSlots[i].skillId && t.characterIdex == skillSlots[i].characterIdex)
            {
                chainList.Add(i);
            }
            else
            {
                break;
            }
        }

        for(int i =index+1; i < skillSlots.Count; i++)
        {
            if (chainList.Count == 3)
                break;

            if (t.skillId == skillSlots[i].skillId && t.characterIdex == skillSlots[i].characterIdex)
            {
                chainList.Add(i);
            }
            else
            {
                break;
            }
        }

        chainList.Sort();

        return chainList;
    }

    int FindLastEmptySlot()
    {
        for (int i = skillSlots.Count - 1; i >= 0; i--)
        {
            if(skillSlots[i].skillId == -1)
            {
                return i;
            }
        }

        return -1;
    }
	
    void SetSlot(int index, int skillId, int charaterIdex)
    {
        SkillTemplate t = new SkillTemplate();
        t.skillId = skillId;
        t.characterIdex = charaterIdex;
        skillSlots[index] = t;
        Dirty = true;
    }

    void Refresh()
    {
        if(Dirty)
        {
            string str = "";
            for (int i = 0; i < skillSlots.Count; i++)
            {
                str += skillSlots[i] + ",";

                if (skillSlots[i].skillId == -1)
                {
                    slotTRs[i].gameObject.GetComponent<UISprite>().spriteName = "empty";
                }
                else
                {
                    slotTRs[i].gameObject.GetComponent<UISprite>().spriteName = skillSlots[i].skillId + "";
                }
            }
            Dirty = false;
        }
    }

	void Update () 
    {
	    if(Time.time - lastTimeBlockGenerate > blockGenerateInterval)
        {
            SkillTemplate skillTempl = SkillTemplateList[UnityEngine.Random.Range(0, SkillTemplateList.Length)];
            int lastEmptySlot = FindLastEmptySlot();
            if(lastEmptySlot >=0)
            {
                SetSlot(lastEmptySlot, skillTempl.skillId, skillTempl.characterIdex);
            }
            lastTimeBlockGenerate = Time.time;
        }

        Refresh();
	}
}
