using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("progression of push decay over time")]
    private AnimationCurve pushDecayCurve;
    // how quickly player decreases speed after a push
    private float pushDecay = 2f;
    // current speed from pushes
    private float movementPower = 0f;
    // speed set when a fresh push is queued
    private float maxMovementPower = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * Time.deltaTime * movementPower);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        float movementAmt = movementPower - Time.deltaTime;
        movementPower = Mathf.Clamp(movementAmt * pushDecayCurve.Evaluate(movementAmt / maxMovementPower), 0f, maxMovementPower);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            QueuePush();
        }
    }

    private void QueuePush()
    {
        movementPower = maxMovementPower;
    }
}
