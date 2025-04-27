using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Skateboard : MonoBehaviour
{
    private float rotationTimer = 0f;
    public bool tricking = false;
    private float trickSpeed = 5f;

    private Quaternion startRotation;
    private Quaternion goalRotation;
    private Quaternion checkPointRotation;

    private static Skateboard instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static Skateboard Instance()
    {
        return instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KickFlip();
        }

        if (tricking)
        {
            rotationTimer += Time.deltaTime * trickSpeed;
            transform.localRotation = Quaternion.Slerp(startRotation,
                     goalRotation, rotationTimer);

            if (transform.localRotation.eulerAngles.z >= 180f)
            {
                rotationTimer = 0f;
                startRotation = transform.localRotation;
                goalRotation = Quaternion.Euler(0f, 0f, -360f);
            }
            else if (transform.localRotation.z == 0f)
            {
                transform.localRotation = Quaternion.identity;
                tricking = false;
            }
        }
    }

    public void KickFlip()
    {
        tricking = true;
        rotationTimer = 0f;
        startRotation = Quaternion.identity;
        goalRotation = Quaternion.Euler(0f, 0f, -180f);
        checkPointRotation = goalRotation;
    }

    public void Frontside360()
    {
        tricking = true;
        rotationTimer = 0f;
        startRotation = Quaternion.identity;
        goalRotation = Quaternion.Euler(0f, -180f, 0f);
        checkPointRotation = goalRotation;
    }

    public void Flip360() // 360 flip
    {

    }
}
