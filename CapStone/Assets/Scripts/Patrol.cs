using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    // patrol radius and how long NPC wait
    public float minX = 203f;
    public float maxX = 802f;
    public float minZ = 195f;
    public float maxZ = 802f;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private float wait;             // time for how long NPC has Waited

    // Start is called before the first frame update
    void Start()
    {
        // Get the navmesh agent
        agent = GetComponent<NavMeshAgent>();

        // send enemy to a random position
        RandomPosition();
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if agent has found a position
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // start wait timer
            wait += Time.deltaTime;

            // after waiting enough time move position
            if(wait >= waitTime)
            {
                RandomPosition();
                wait = 0f;
            }
        }
        
    }

    void RandomPosition()
    {
        // pick a random location within the Min and Max of X AND Z
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // Keep the NPC at groumd level
        Vector3 randomPoint = new Vector3(randomX, transform.position.y, randomZ);

        // make sure that the location is on the NavMesh
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            // move agent to a valid position
            agent.SetDestination(hit.position);
        }
    }
}
