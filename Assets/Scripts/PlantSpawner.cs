using System.Collections;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject monsterPlantPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Config")] 
    [SerializeField] private float spawnRate = 5;

    private void Start()
    {
        StartCoroutine(SpawnPlants());
    }

    private IEnumerator SpawnPlants()
    {
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var plant = Instantiate(monsterPlantPrefab);
        plant.transform.position = spawnPoint.position;
        var controller = plant.GetComponent<EnemyController>();
        controller.player = player;
        
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnPlants());
    }
}