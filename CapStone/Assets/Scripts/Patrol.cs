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

    // Navmeshagent for the AI on the navmesh, time to wait at the patrol points, and chase state flag
    private NavMeshAgent agent; 
    private float wait = 0f; 
    private bool isChasing = false; 

    // Player movement script reference
    private PlayerMovement playerMovementScript;

    // Audio for enemy to play while patroling and chasing
    public AudioClip patrolLoop;
    public AudioClip chaseLoop;

    public float patrolVolume = 6f;
    public float chaseVolume = 50f;

    private AudioSource audioSource;

    void Start()
    {
        // Get the navmeshagent
        agent = GetComponent<NavMeshAgent>();

        // Send enemy to a random position at start
        RandomPosition();

        // Get reference to player movement script
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
        }

        // AudioSource 
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1f;  // Enemy location
        audioSource.loop = true;
        PlayPatrolAudio(); // Play patrol by default

    }

    void Update()
    {
        // Loop for NPC switching between patrol and chase
        if (player == null) return; // if player isnt assigned, exit

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If player is in chase range, chase
        if (distanceToPlayer <= chaseRange)
        {
            if (!isChasing)      
            {
                isChasing = true;
                PlayChaseAudio();
            }

            agent.SetDestination(player.position);
        }
        // If player moves out of chase range then go back to patrol
        else if (isChasing)
        {
            isChasing = false;
            PlayPatrolAudio();
            RandomPosition();
        }

        // When AI reaches patrol point wait for 2 seconds before moving to the next
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

            // Destroy the player object
            Destroy(other.gameObject);
        }
    }

    // Kill player
    void KillPlayer()
    {
        // Disable the player movement script
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

    // Called when a sound is broadcasted either by the player or other objects
    void HearSound(Vector3 soundPosition, float loudness)
    {
        float DistanceToSound = Vector3.Distance(transform.position, soundPosition);
        Debug.Log($"Enemy heard sound! Distance: {DistanceToSound}, Loudness: {loudness}");

        // If sound is in the range of the enemy chase the player
        if (DistanceToSound <= loudness)
        {
            if (!isChasing)
            {
                isChasing = true;
                PlayChaseAudio();
            }

            agent.SetDestination(player.position);
        }
    }

    // Subscribe to the soundevent manager
    void OnEnable()
    {
        SoundEventManager.OnSoundMade += HearSound;
    }

    // Unsubscribe for the soundevent manager
    void OnDisable()
    {
        SoundEventManager.OnSoundMade -= HearSound;
    }

    // Play the patrol audio
    void PlayPatrolAudio()
    {
        if (patrolLoop != null)
        {
            audioSource.volume = patrolVolume;
            audioSource.clip = patrolLoop;
            audioSource.Play();
        }
    }

    // Play the chase audio 
    void PlayChaseAudio()
    {
        if (chaseLoop != null)
        {
            audioSource.volume = chaseVolume;
            audioSource.clip = chaseLoop;
            audioSource.Play();
        }
    }
}
