using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    // Patrol area limits and wait time
    public float minX = 204f;
    public float maxX = 801f;
    public float minZ = 195f;
    public float maxZ = 802f;
    public float waitTime = 2f;

    // Distance that the NPC will start chasing the player
    public float chaseRange = 15f;
    public Transform player;

    private NavMeshAgent agent;
    private float wait = 0f;
    private bool isChasing = false;

    private PlayerMovement playerMovementScript;

    void Start()
    {
        // Get the NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Send enemy to a random position at start
        RandomPosition();

        // Get reference to player movement script
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // Loop for NPC switching between patrol and chase
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
            // Start wait timer
            wait += Time.deltaTime;

            // After waiting enough time, move to new random position
            if (wait >= waitTime)
            {
                RandomPosition();
                wait = 0f;
            }
        }
    }

    // When NPC collides with player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer();

            // Load Lose scene 
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseGame();
            }

            // Optionally destroy the player object
            Destroy(other.gameObject);
        }
    }

    void KillPlayer()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
    }

    void RandomPosition()
    {
        // Pick a random location within the Min and Max of X and Z
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // Keep the NPC at ground level
        Vector3 randomPoint = new Vector3(randomX, transform.position.y, randomZ);

        // Make sure the location is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            // Move agent to a valid position
            agent.SetDestination(hit.position);
        }
    }
}
