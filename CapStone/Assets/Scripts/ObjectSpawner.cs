using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Object Prefabs")]
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    [Header("Spawn Points (Each object has 3)")]
    public Transform[] spawnPointsObject1;
    public Transform[] spawnPointsObject2;
    public Transform[] spawnPointsObject3;

    [Header("NavMesh Settings")]
    public float maxNavMeshDistance = 2f;

    void Start()
    {
        SpawnObjectAtRandomPoint(object1, spawnPointsObject1);
        SpawnObjectAtRandomPoint(object2, spawnPointsObject2);
        SpawnObjectAtRandomPoint(object3, spawnPointsObject3);
    }

    void SpawnObjectAtRandomPoint(GameObject objPrefab, Transform[] spawnPoints)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning($"No spawn points assigned for {objPrefab.name}");
            return;
        }

        // Choose a random spawn point from the array
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