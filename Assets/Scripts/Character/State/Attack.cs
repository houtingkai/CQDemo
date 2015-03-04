using UnityEngine;
using System.Collections;

public class Attack : StateBase
{
    public override void OnEnter(object param)
    {
        //Debug.Log("Attack OnEnter");
        Character me = owner as Character;



    }

    public override void OnExit()
    {
        //Debug.Log("Attack OnExit");
        Character me = owner as Character;

    }

    public override void OnReset(object param)
    {
//        Debug.Log("Attack OnReset");
    }
}
