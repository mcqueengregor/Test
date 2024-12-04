using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ObstaclePrefab;
    [SerializeField] private GameObject ObstaclesParent;

    [SerializeField] private GameObject FloorPlaneObject;

    [SerializeField] private int numObstacles;

    List<GameObject> obstacles;

    private void Awake()
    {
        SpawnObstacles();
    }

    private void SpawnObstacles()
    {
        // Spawn obstacle prefabs around the world randomly:
        for (int i = 0; i < numObstacles; ++i)
        {
            Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            // Use floor's scale as the bounds for where obstacles can spawn, move spawn location to floor's position:
            Vector3 floorPlaneScale = FloorPlaneObject.transform.localScale * 0.5f;

            Vector3 spawnPos = new Vector3(randomPos.x * floorPlaneScale.x, 2f, randomPos.z * floorPlaneScale.z);
            spawnPos += FloorPlaneObject.transform.position;

            GameObject newObstacle = Instantiate(ObstaclePrefab, ObstaclesParent.transform);
            newObstacle.transform.position = spawnPos;
        }
    }
}
