using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushQueue
{
    private float timer = 0f;
    private float pushTime;
    private Image pushUI; // arrow on player direction canvas

    public PushQueue(Image newPushUI)
    {
        pushUI = newPushUI;
        pushTime = BalanceSettings.pushTime;
    }

    public void Push()
    {
        timer = pushTime;
    }

    public void Update(float deltaTime)
    {
        timer = Mathf.Clamp(timer - deltaTime, 0f, pushTime);
        pushUI.color = Color.Lerp(Color.white, new Color(255f, 255f, 255f, 0f),
            1 - (timer / pushTime));
    }

    // returns num between 0 - 1 that represents how close the current push is to being complete
    public float GetPushProgress()
    {
        return timer / pushTime;
    }
}
