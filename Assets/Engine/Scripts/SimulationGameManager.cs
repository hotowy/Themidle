using UnityEngine;

public class SimulationGameManager : MonoBehaviour
{
    public int totalNumberOfEnemiesSpawned = 0;

    private EnemySpawner[] spawners;
    void Start()
    {
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        int sum = 0;
        foreach (var spawner in spawners)
        {
            sum += spawner.currentNumberOfEnemies;
        }
        totalNumberOfEnemiesSpawned = sum;
    }
}
