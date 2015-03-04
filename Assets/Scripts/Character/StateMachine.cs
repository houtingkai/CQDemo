using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour {
    public Dictionary<int, StateBase> stateDict = new Dictionary<int, StateBase>();
    public Animator animator;
    public Character owner;
    Dictionary<int, object> parms = new Dictionary<int, object>();

    int nextState = -1;
    public int currentTagHash;
    public int currentNameHash;
    public StateBase currentState;

    void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        owner = GetComponent<Character>();
    }


    public object this[int key]
    {
        get
        {
            object p;
            if (parms.TryGetValue(key, out p))
            {
                return p;
            }
            else
            {
                return null;
            }
        }

        set
        {
            parms[key] = value;
        }
    }

    //void FixedUpdate()
    void Update()
    {
        AnimatorStateInfo anim = animator.IsInTransition(0) ? animator.GetNextAnimatorStateInfo(0) : animator.GetCurrentAnimatorStateInfo(0);
        int hash = anim.tagHash;
        OnChangeNameHash(anim.nameHash);
        OnChangeState(hash);
        if(currentState != null)
        {
            currentState.OnUpdate(Time.deltaTime);
        }
    }

    private void OnChangeNameHash(int hash)
    {
        if (currentNameHash != hash)
        {
            currentNameHash = hash;
        }
    }

    private void OnChangeState(int hash)
    {
        if (hash != currentTagHash) //&& (nextState == -1 || (hash == nextState))
        {
            animator.SetBool(hash, false);
            nextState = -1;
            if(currentState != null)
            {
                currentState.OnExit();
            }

            currentTagHash = hash;

            StateBase state;
            if (stateDict.TryGetValue(currentTagHash, out state))
            {
                currentState = state as StateBase;
                object param;
                if (parms.TryGetValue(currentTagHash, out param))
                {
                    currentState.OnEnter(param);
                    parms.Remove(currentTagHash);
                    return;
                }
                else
                {
                    currentState.OnEnter(null);
                    return;
                }
            }
        }
        
        if((hash != currentTagHash &&  (hash != nextState)))
        {
            Debug.Log("hash: " + hash + ", currentTagHash: " + currentTagHash + ", nextState: " + nextState);
        }
    }

    public void ChangeState(string name, object param = null)
    {
        int hash = Animator.StringToHash(name);
        if (hash != currentTagHash)
        {
            nextState = hash;
            animator.SetBool(nextState, true);
            parms[hash] = param;
        }
        else
        {
            currentState.OnEvent((int)StateEvent.Reset, param);
        }
    }

    public void SetAnimParam(string name, object value)
    {
        int hash = Animator.StringToHash(name);
        if(value is int)
        {
            animator.SetInteger(hash, (int)value);
        }
        else if(value is bool)
        {
            animator.SetBool(hash, (bool)value);
        }
        StartCoroutine(CancelSetAnimParam(hash, currentNameHash, value));
    }

    IEnumerator CancelSetAnimParam(int id, int animHash, object paramVal)
    {
        while (animHash == currentNameHash)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (paramVal is int)
        {
            animator.SetInteger(id, 0);
        }
        else if (paramVal is bool)
        {
            animator.SetBool(id, false);
        }
    }

    public void AddState(StateBase state)
    {
        int hash = Animator.StringToHash(state.Name);
        StateBase oldState;
        if(stateDict.TryGetValue(currentTagHash, out oldState))
        {
            return;
        }

        state.stateMachine = this;
        state.OnInit();
        stateDict[hash] = state;
    }
}
