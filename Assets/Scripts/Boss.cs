using UnityEngine;

public class Boss : Enemy
{
    public GameObject minionPrefab;
    public float minionSpawnCount;
    private float spawnRange = 9.0f;
    private float spawnDelay = 2.0f;
    private float spawnInterval = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyRb = GetComponent<Rigidbody>();
        InvokeRepeating("SpawnMinions", spawnDelay, spawnInterval);
    }

    void SpawnMinions()
    {
        //If I want to change the number of minions later I would need to make a coroutine to invoke the spawn method instead
        for (int i = 0; i < 3; i++)
        {
            Instantiate(minionPrefab, GenerateSpawnPosition(minionPrefab.transform.position), minionPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition(Vector3 transformPos)
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, transformPos.y, spawnPosZ);
        return randomPos;
    }


}
