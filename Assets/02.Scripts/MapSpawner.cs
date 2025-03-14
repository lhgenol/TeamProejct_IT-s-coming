using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] mapChunks; // 맵 조각 프리팹 배열
    public float spawnDistance = 50f; // 새로운 맵을 생성할 거리
    public float moveSpeed = 5f; // 맵 이동 속도
    private float mapTravelled = 0f; // 누적 맵 이동 거리
    private float nextSpawnZ = 0f; // 다음 청크 생성 위치 (월드 좌표 Z)
    public int chunkLength = 60; // 청크 길이 (Z 방향 크기)

    void Start()
    {
        // 초기 맵을 생성 (원하는 시작 위치로 nextSpawnZ를 설정)
        nextSpawnZ = 0f;

        // 초기 청크 3개 생성
        for (int i = 0; i < 3; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // 매 프레임마다 맵 이동 거리를 누적 업데이트
        mapTravelled += moveSpeed * Time.deltaTime;
        Debug.Log($"[DEBUG] mapTravelled: {mapTravelled}, nextSpawnZ: {nextSpawnZ}, 차이: {nextSpawnZ - mapTravelled}");

        // 이동한 거리에 따라 spawnDistance 안에 들어오면, while 루프로 여러 청크를 연달아 생성
        while (mapTravelled + spawnDistance >= nextSpawnZ)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        if (mapChunks == null || mapChunks.Length == 0)
        {
            Debug.LogError("mapChunks 배열이 비어 있습니다! 프리팹을 할당하세요.");
            return;
        }

        // 랜덤 청크 선택 후 생성
        int randIndex = Random.Range(0, mapChunks.Length);
        Vector3 spawnPosition = new Vector3(0, 0, nextSpawnZ);
        GameObject chunk = Instantiate(mapChunks[randIndex], spawnPosition, Quaternion.identity);

        Debug.Log($"새로운 맵 생성됨: {chunk.name}, 위치: {spawnPosition}, 다음 청크 생성 위치: {nextSpawnZ + chunkLength}");

        // 다음 청크 위치 업데이트
        nextSpawnZ += chunkLength;
    }
}
