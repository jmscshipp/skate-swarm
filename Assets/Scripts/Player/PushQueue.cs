using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushQueue
{
    private class Push
    {
        private float timer = 0f;

        public float GetTimer() => timer;
        public Push(float pushTime)
        {
            timer = pushTime;
        }

        // returns true if the push is still active
        public bool Update(float deltaTime)
        {
            timer -= deltaTime;
            Debug.Log(timer);
            if (timer <= 0)
                return false;
            return true;
        }
    }
    private float pushTime;
    private Queue<Push> activePushes;

    public PushQueue()
    {
        pushTime = BalanceSettings.pushTime;
        activePushes = new Queue<Push>();
    }

    public void AddPush()
    {
        activePushes.Clear();
        activePushes.Enqueue(new Push(pushTime));
    }

    public void Update(float deltaTime)
    {
        int deQueueNum = 0;

        // update each push timer, keeping track of how many are expired
        foreach (Push push in activePushes)
        {
            if (!push.Update(deltaTime))
                deQueueNum++;
        }

        // remove expired pushes
        for (int i = 0; i < deQueueNum; i++)
            activePushes.Dequeue();
    }

    // returns num between 0 - 1 that represents how close the current push is to being complete
    public float GetPushProgress()
    {
        // no pushes in queue
        if (activePushes.Count == 0)
            return 1f;

        return activePushes.Peek().GetTimer() / pushTime;
    }
}
