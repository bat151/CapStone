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

    // distance that the NPC will satrt chasing the player
    public float chaseRange = 15f;
    public Transform player;

    private NavMeshAgent agent;
    private float wait = 0f;
    private bool isChasing = false;

    private PlayerMovement playerMovementScript;

    void Start()
    {
        // Get the navmesh agent
        agent = GetComponent<NavMeshAgent>();

        // send enemy to a random position
        RandomPosition();

        // get reference to player movement script
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // Loop for NPC switiching between patrol and chase
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            isChasing = false;
            RandomPosition();
        }

        if (!isChasing && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // start wait timer
            wait += Time.deltaTime;

            // after waiting enough time move position
            if (wait >= waitTime)
            {
                RandomPosition();
                wait = 0f;
            }
        }
    }

    // if Npc collides with player kill player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer();
            Destroy(other.gameObject);  // Destroy the player on collision
        }
    }

    // destroy player object
    void KillPlayer()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

    }

    void RandomPosition()
    {
        // pick a random location within the Min and Max of X AND Z
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // Keep the NPC at ground level
        Vector3 randomPoint = new Vector3(randomX, transform.position.y, randomZ);

        // make sure that the location is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            // move agent to a valid position
            agent.SetDestination(hit.position);
        }
    }
}
