using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{
    // objects
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    // spawn points for each object, theyll have 3 each, define in the inspector
    public Transform[] spawnPointsObject1;
    public Transform[] spawnPointsObject2;
    public Transform[] spawnPointsObject3;

    // make sure spawn point is withtin the nav mesh
    public float maxNavMeshDistance = 2f;

    void Start()
    {
        // spawn each object at a random point
        SpawnObjectAtRandomPoint(object1, spawnPointsObject1);
        SpawnObjectAtRandomPoint(object2, spawnPointsObject2);
        SpawnObjectAtRandomPoint(object3, spawnPointsObject3);
    }

    void SpawnObjectAtRandomPoint(GameObject objPrefab, Transform[] spawnPoints)
    {
        // make sure one spawn point is choose for each object
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning($"No spawn points assigned for {objPrefab.name}");
            return;
        }

        // Choose a random spawn point from the list
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Make sure it's on the NavMesh
        if (NavMesh.SamplePosition(randomSpawn.position, out NavMeshHit hit, maxNavMeshDistance, NavMesh.AllAreas))
        {
            Instantiate(objPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Spawn point for {objPrefab.name} is not on the NavMesh: {randomSpawn.position}");
        }
    }
}