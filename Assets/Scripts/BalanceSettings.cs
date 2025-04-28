using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BalanceSettings
{
    // how strong the jump is
    public static float jumpPower = 2f;
    // how long it takes for a push to peter out to 0f
    public static float pushTime = 2f;
    // how fast player travels at beginning of a push
    public static float movementSpeed = 140f;
    // how long player has to complete trick after popup appears
    public static float trickOpportunityTime = 0.5f;
}
