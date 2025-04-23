using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestinations : MonoBehaviour
{
    private Transform[] destinations;
    private static EnemyDestinations instance;

    public static EnemyDestinations Instance()
    {
        return instance;
    }
    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        destinations = GetComponentsInChildren<Transform>();
    }

    public Transform GetRandomDestination()
    {
        return destinations[Random.Range(0, destinations.Length)];
    }
}
