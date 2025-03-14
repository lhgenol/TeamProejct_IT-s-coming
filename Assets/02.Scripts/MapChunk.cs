using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public float moveSpeed = 10f; // 맵 이동 속도

    // 코인 관련 설정 (코인은 고정 위치)
    public GameObject coinPrefab;
    public Transform[] coinPaths;

    // 아이템 (항상 생성)
    public GameObject[] items;
    public Transform[] itemSpawnPoints;

    // 장애물 (항상 생성)
    public GameObject[] obstacles;
    public Transform[] obstacleSpawnPoints;

    // 왼쪽 건물 관련 설정
    public GameObject[] leftBuildings;
    public Transform[] leftBuildingSpawnPoints;

    // 오른쪽 건물 관련 설정
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
        // 왼쪽 건물 배치 (기본 회전값 유지)
        foreach (Transform spawnPoint in leftBuildingSpawnPoints)
        {
            int randIndex = Random.Range(0, leftBuildings.Length);
            GameObject building = Instantiate(leftBuildings[randIndex], spawnPoint.position, leftBuildings[randIndex].transform.rotation, transform);
        }

        // 오른쪽 건물 배치 (기본 회전값 유지)
        foreach (Transform spawnPoint in rightBuildingSpawnPoints)
        {
            int randIndex = Random.Range(0, rightBuildings.Length);
            GameObject building = Instantiate(rightBuildings[randIndex], spawnPoint.position, rightBuildings[randIndex].transform.rotation, transform);
        }
    }
}
