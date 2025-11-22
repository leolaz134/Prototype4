using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    public GameObject BossPrefab;
    public int enemyCount;
    public int waveNumber = 1;
    private float spawnRange = 9.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPowerUp();
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        //gets all enemy objects in scene, since FindObjectsOfType returns an array we use length for the amount of enemies
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0 && waveNumber % 5 == 0)
        {
            SpawnPowerUp();
            SpawnPowerUp();
            SpawnBossWave();
            ++waveNumber;
        }else if (enemyCount == 0)
        {
            SpawnPowerUp();
            SpawnEnemyWave(++waveNumber);
        }
    }

    //Generates random position within set range
    private Vector3 GenerateSpawnPosition(Vector3 transformPos)
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, transformPos.y, spawnPosZ);
        return randomPos;
    }

    void SpawnPowerUp()
    {
        int randomIndex = Random.Range(0, powerupPrefabs.Length);
        GameObject powerUp = powerupPrefabs[randomIndex];
        Instantiate(powerUp, GenerateSpawnPosition(powerUp.transform.position), powerUp.transform.rotation);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject Enemy = enemyPrefabs[randomIndex];
            Instantiate(Enemy, GenerateSpawnPosition(Enemy.transform.position), Enemy.transform.rotation);
        }
    }

    void SpawnBossWave()
    {
               Instantiate(BossPrefab, GenerateSpawnPosition(BossPrefab.transform.position), BossPrefab.transform.rotation);
    }



}
