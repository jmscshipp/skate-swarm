using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(EnemyDestinations.Instance().GetRandomDestination().position);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0.05f)
        {
            agent.SetDestination(EnemyDestinations.Instance().GetRandomDestination().position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemey")
            agent.SetDestination(EnemyDestinations.Instance().GetRandomDestination().position);
    }
}
