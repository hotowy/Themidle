using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;

    [SerializeField]
    private float swarmerInterval = 3.5f;
    [SerializeField]
    private float bigSwarmerInterval = 3.5f;
    
    [SerializeField]
    private float spreadX = 0.25f;
    [SerializeField]
    private float spreadY = 0.08f;
    
    public int currentNumberOfEnemies = 0;
    [SerializeField] private int totalEnemiesToSpawn = 10;
    
    void Start()
    {
     StartCoroutine(SpawnEnemy(swarmerInterval, swarmerPrefab));   
     StartCoroutine(SpawnEnemy(bigSwarmerInterval, bigSwarmerPrefab));   
    }

    void Update()
    {
        
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        float rX = Random.Range(-spreadX, spreadX);
        float rY = Random.Range(-spreadY, spreadY);
        
        GameObject newEnemy = Instantiate(enemy, this.transform, false);
        currentNumberOfEnemies++;
        newEnemy.transform.position += new Vector3(rX, rY, 0);
        StartCoroutine(SpawnEnemy(interval, enemy));
    }
}
