using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardShadow : MonoBehaviour
{
    // used with ground detection ray cast
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private Transform shadowParent;
    private float shadowStartDistance;
    private float shadowOffsetDistance = 0.001f;
    private float minShadowSize = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        shadowStartDistance = Vector3.Distance(shadowParent.position, transform.position);
    }

    private void Update()
    {
        shadowParent.localScale = Vector3.one * Mathf.Lerp(1f, minShadowSize, Vector3.Distance(
            shadowParent.position, transform.position));
    }

    private void FixedUpdate()
    {
        // snap player to whatever elevation is below them
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),
            out hit, Mathf.Infinity, groundMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) *
                hit.distance, Color.white);
            shadowParent.position = new Vector3(transform.position.x, hit.point.y
                + shadowOffsetDistance, transform.position.z);
        }
    }
}
