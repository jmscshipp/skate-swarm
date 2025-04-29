using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDestinations : MonoBehaviour
{
    private Transform[] destinations;
    private static EnemyDestinations instance;

    [SerializeField]
    private GameObject enemyPrefab;
    private int enemyCount = 0;
    private float enemyDifficultyScale = 1f;
    [SerializeField]
    private GameObject victoryText;
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

    public Vector3 GetCentralDestination()
    {
        NavMeshHit hit;
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(0f, 2f);
            NavMesh.SamplePosition(randomDirection, out hit, Random.Range(5f, 10f), NavMesh.AllAreas);
        } while (hit.position == null);

        return hit.position;
    }

    public Vector3 GetRandomDestination(Vector3 startPoint)
    {
        NavMeshHit hit;
        do
        {
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(5f, 10f);
            NavMesh.SamplePosition(startPoint + randomDirection, out hit, Random.Range(5f, 10f), NavMesh.AllAreas);
        } while (hit.position == null);

        return hit.position;
    }

    private void Update()
    {
        enemyDifficultyScale = Mathf.Clamp(Mathf.Exp(Time.timeSinceLevelLoad / 40f), 0, 100);
        if (enemyCount < enemyDifficultyScale)
        {
            CreateEnemy();
        }
        if (enemyCount >= 100 && GameObject.FindGameObjectWithTag("Enemy") == null)
            victoryText.SetActive(true);
    }

    private void CreateEnemy()
    {
        Instantiate(enemyPrefab, GetCentralDestination(), Quaternion.identity);
        enemyCount++;
    }

    public void Reset()
    {
       enemyCount = 0;
       enemyDifficultyScale = 1f;
    }
}
