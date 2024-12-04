using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Obstacles data:")]
    [SerializeField] private GameObject ObstaclePrefab;
    [SerializeField] private GameObject ObstaclesParent;
    [SerializeField] private int numObstacles;

    [SerializeField] private GameObject FloorPlaneObject;

    [Header("Obstacles data:")]
    [SerializeField] private PlayerController PlayerRef;


    private List<GameObject> Obstacles = new List<GameObject>();

    [SerializeField] private GameObject[] Destinations;

    private void Awake()
    {
        SpawnObstacles();
    }

    private void Start()
    {
        foreach (var d in Destinations)
            PlayerRef.AddDestination(d.transform.position);
    }

    public void ResetGame()
    {
        PlayerRef.ResetPlayer();

        // Reactivate destination objects, destroy old obstacles and generate new ones:
        foreach (var d in Destinations)
        {
            d.gameObject.SetActive(true);
            PlayerRef.AddDestination(d.transform.position);
        }
        foreach (var o in Obstacles)
        {
            Destroy(o.gameObject);
        }
        Obstacles.Clear();
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

            Vector3 spawnPos = new Vector3(
                randomPos.x * floorPlaneScale.x,
                FloorPlaneObject.transform.position.y + 10f,
                randomPos.z * floorPlaneScale.z);

            spawnPos += FloorPlaneObject.transform.position;

            GameObject newObstacle = Instantiate(ObstaclePrefab, ObstaclesParent.transform);
            newObstacle.transform.position = spawnPos;

            Obstacles.Add(newObstacle);
        }
    }
}
