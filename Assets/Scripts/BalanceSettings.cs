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
    // how long player has to skate before being able to do a trick
    public static float momentumBuildUpTime = 3f;

    public static IEnumerator ScreenShake()
    {
        Vector3 camPos = Camera.main.transform.position;
        for (int i = 0; i < 5; i++)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + Random.insideUnitCircle.x * 0.1f, Camera.main.transform.position.y + Random.insideUnitCircle.y * 0.1f, camPos.z);
            yield return new WaitForSeconds(0.01f);
            Camera.main.transform.position = camPos;
        }
    }
}
