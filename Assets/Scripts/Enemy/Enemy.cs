using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Material[] skins;
    [SerializeField]
    private Material allWhiteMat;
    [SerializeField]
    private SkinnedMeshRenderer[] renderers;
    [SerializeField]
    private MeshRenderer skateBoardMesh;

    [SerializeField]
    private Mesh boyBodyMesh;
    [SerializeField]
    private Mesh boyHeadMesh;

    private NavMeshAgent agent;
    private Material thisSkin;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        thisSkin = skins[Random.Range(0, skins.Length)];
        foreach (SkinnedMeshRenderer renderer in renderers)
            renderer.material = thisSkin;

        if (Random.Range(0f, 1f) > 0.5f)
        {
            renderers[0].sharedMesh = boyBodyMesh;
            renderers[1].sharedMesh = boyHeadMesh;
        }
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

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Enemey")
            agent.SetDestination(EnemyDestinations.Instance().GetRandomDestination().position);

        if (other.gameObject.tag == "PlayerAttack")
            StartCoroutine(Death(other.transform.position));
    }

    private IEnumerator Death(Vector3 playerPos)
    {
        AudioManager.Instance().PlaySound("hit");
        agent.enabled = false;
        // here working on rotating enemies slightly away from player
        //transform.rotation = Quaternion.Euler(Vector3.RotateTowards(transform.position, playerPos, 0.5f, 0.5f));

        // flash white
        foreach (SkinnedMeshRenderer renderer in renderers)
            renderer.material = allWhiteMat;
        skateBoardMesh.material = allWhiteMat;
        yield return new WaitForSeconds(0.1f);
        // flash skin
        foreach (SkinnedMeshRenderer renderer in renderers)
            renderer.material = thisSkin;
        skateBoardMesh.material = skins[0];
        yield return new WaitForSeconds(0.1f);
        // flash white
        foreach (SkinnedMeshRenderer renderer in renderers)
            renderer.material = allWhiteMat;
        skateBoardMesh.material = allWhiteMat;
        
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
