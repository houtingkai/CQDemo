using UnityEngine;
using System.Collections;

public class Run : StateBase
{
    public override void OnEnter(object param)
    {
        Character character = owner as Character;
        bool forth = (bool)param;
        int dir = forth ? character.Facing : character.Facing * -1;
        Vector2 v = new Vector2(character.runSpeed * dir, 0.0f);
        character.SetVelocity(v);
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    public override void OnExit()
    {
        Character character = owner as Character;
        character.SetVelocity(Vector2.zero);
    }
}
