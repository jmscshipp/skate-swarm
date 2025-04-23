using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("progression of push decay over time")]
    private AnimationCurve pushDecayCurve;
    private float movementSpeed;
    private PushQueue pushQueue;

    private Rigidbody rb;

    [SerializeField]
    private Transform directionalArrowUI;
    private Vector3 screenCenter;

    // amount the player is raised about the ground
    private float playerDistanceFromGround = 0.132f;
    // used with ground detection ray cast
    [SerializeField]
    private LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false;
        screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        movementSpeed = BalanceSettings.movementSpeed;
        rb = GetComponent<Rigidbody>();
        pushQueue = new PushQueue();
        pushQueue.AddPush();

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 moveDir = (Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition).normalized;
        //rb.velocity = moveDir * movementSpeed;
        //Debug.DrawRay(transform.position, moveDir * 5f, Color.green);
        //transform.Translate(moveDir * Time.deltaTime * movementSpeed * pushDecayCurve.Evaluate(pushQueue.GetPushProgress()));
        //
        //Debug.Log("push progress: " + pushQueue.GetPushProgress());
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        //
        //
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    pushQueue.AddPush();
        //}
        //
        //pushQueue.Update(Time.deltaTime);
    }
    private void FixedUpdate()
    {
        Vector3 moveDir = (screenCenter - Input.mousePosition).normalized;
        Debug.Log(Input.mousePosition);
        moveDir = new Vector3 (moveDir.x, 0f, moveDir.y);
        Debug.DrawRay(transform.position, moveDir * 5f, Color.green);
        rb.velocity = moveDir * Time.fixedDeltaTime * 100f;

        // rotate arrow UI in direction player is going
        directionalArrowUI.transform.localRotation = Quaternion.Euler(
            new Vector3(0f, 0f, 180f - (Mathf.Atan2(screenCenter.x - Input.mousePosition.x, 
            screenCenter.y - Input.mousePosition.y) * Mathf.Rad2Deg)));

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, groundMask))
        //{
        //    Debug.Log("hit " + hit.transform.gameObject.name);
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.white);
        //    transform.position = new Vector3(transform.position.x, hit.point.y + playerDistanceFromGround, transform.position.z);
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.red);
        //}
    }

}
