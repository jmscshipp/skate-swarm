using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        int sortingLayer = -(int)((transform.position.y + 5) * 1000); // sorting layer will range from -1000 to 0
        GetComponent<SpriteRenderer>().sortingOrder = sortingLayer;
    }
}
