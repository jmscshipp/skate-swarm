using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("progression of push decay over time")]
    private AnimationCurve pushDecayCurve;
    private float movementSpeed;
    private PushQueue pushQueue;

    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = BalanceSettings.movementSpeed;
        pushQueue = new PushQueue();
        pushQueue.AddPush();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        
        transform.Translate(moveDir * Time.deltaTime * movementSpeed * pushDecayCurve.Evaluate(pushQueue.GetPushProgress()));

        Debug.Log("push progress: " + pushQueue.GetPushProgress());
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            pushQueue.AddPush();
        }

        pushQueue.Update(Time.deltaTime);
    }
}
