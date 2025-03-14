using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public float moveSpeed = 10f; // �� �̵� �ӵ�

    // ���� ���� ���� (������ ���� ��ġ)
    public GameObject coinPrefab;
    public Transform[] coinPaths;

    // ������ (�׻� ����)
    public GameObject[] items;
    public Transform[] itemSpawnPoints;

    // ��ֹ� (�׻� ����)
    public GameObject[] obstacles;
    public Transform[] obstacleSpawnPoints;

    // ���� �ǹ� ���� ����
    public GameObject[] leftBuildings;
    public Transform[] leftBuildingSpawnPoints;

    // ������ �ǹ� ���� ����
    public GameObject[] rightBuildings;
    public Transform[] rightBuildingSpawnPoints;

    void Start()
    {
        SpawnCoins();
        SpawnItems();
        SpawnObstacles();
        SpawnBuildings();
    }

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }

    void SpawnCoins()
    {
        foreach (Transform coinPath in coinPaths)
        {
            Instantiate(coinPrefab, coinPath.position, Quaternion.identity, transform);
        }
    }

    void SpawnItems()
    {
        foreach (Transform spawnPoint in itemSpawnPoints)
        {
            int randIndex = Random.Range(0, items.Length);
            Instantiate(items[randIndex], spawnPoint.position, Quaternion.identity, transform);
        }
    }

    void SpawnObstacles()
    {
        foreach (Transform spawnPoint in obstacleSpawnPoints)
        {
            int randIndex = Random.Range(0, obstacles.Length);
            Instantiate(obstacles[randIndex], spawnPoint.position, Quaternion.identity, transform);
        }
    }

    void SpawnBuildings()
    {
        // ���� �ǹ� ��ġ (�⺻ ȸ���� ����)
        foreach (Transform spawnPoint in leftBuildingSpawnPoints)
        {
            int randIndex = Random.Range(0, leftBuildings.Length);
            GameObject building = Instantiate(leftBuildings[randIndex], spawnPoint.position, leftBuildings[randIndex].transform.rotation, transform);
        }

        // ������ �ǹ� ��ġ (�⺻ ȸ���� ����)
        foreach (Transform spawnPoint in rightBuildingSpawnPoints)
        {
            int randIndex = Random.Range(0, rightBuildings.Length);
            GameObject building = Instantiate(rightBuildings[randIndex], spawnPoint.position, rightBuildings[randIndex].transform.rotation, transform);
        }
    }
}
