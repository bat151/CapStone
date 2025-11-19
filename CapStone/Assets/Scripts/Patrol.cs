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

    private NavMeshAgent agent; // navmeshagent for the AI on the navmesh
    private float wait = 0f; // time to wait at the patrol points
    private bool isChasing = false; // chase state flag

    // player movement script reference
    private PlayerMovement playerMovementScript;

    void Start()
    {
        // get the navmeshagent
        agent = GetComponent<NavMeshAgent>();

        // send enemy to a random position at start
        RandomPosition();

        // get reference to player movement script
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // Loop for NPC switching between patrol and chase
        if (player == null) return; // if player isnt assigned, exit

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // if player is in chase range, chase
        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        // if player moves out of chase range then go back to patrol
        else if (isChasing)
        {
            isChasing = false;
            RandomPosition();
        }

        // when AI reaches patrol point wait for 2 seconds before moving to the next
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

            // destroy the player object
            Destroy(other.gameObject);
        }
    }

    // kill player
    void KillPlayer()
    {
        // disable the player movement script
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
    }

    void RandomPosition()
    {
        // pick a random location within the Min and Max of X and Z
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // keep the NPC at ground level
        Vector3 randomPoint = new Vector3(randomX, transform.position.y, randomZ);

        // make sure the location is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {
            // move agent to a valid position
            agent.SetDestination(hit.position);
        }
    }

    // called when a sound is broadcasted either by the player or other objects
    void HearSound(Vector3 soundPosition, float loudness)
    {
        float DistanceToSound = Vector3.Distance(transform.position, soundPosition);
        Debug.Log($"Enemy heard sound! Distance: {DistanceToSound}, Loudness: {loudness}");

        // if sound is in the range of the enemy chase the player
        if (DistanceToSound <= loudness)
        {
            // chase player
            isChasing = true;
            agent.SetDestination(player.position);
        }
    }

    // subscribe to the soundevent manager
    void OnEnable()
    {
        SoundEventManager.OnSoundMade += HearSound;
    }

    // unsubscribe for the soundevent manager
    void OnDisable()
    {
        SoundEventManager.OnSoundMade -= HearSound;
    }
}
