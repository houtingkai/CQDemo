using UnityEngine;
using System.Collections;

public class Layer 
{
    public static int groundMask = 1 << LayerMask.NameToLayer("Ground");
    public static int playerMask = 1 << LayerMask.NameToLayer("Player");
    public static int enemyMask = 1 << LayerMask.NameToLayer("Enemy");

    public static int ground = LayerMask.NameToLayer("Ground");
    public static int player = LayerMask.NameToLayer("Player");
    public static int enemy = LayerMask.NameToLayer("Enemy");
}
