using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] mapChunks; // �� ���� ������ �迭
    public float spawnDistance = 50f; // ���ο� ���� ������ �Ÿ�
    public float moveSpeed = 5f; // �� �̵� �ӵ�
    private float mapTravelled = 0f; // ���� �� �̵� �Ÿ�
    private float nextSpawnZ = 0f; // ���� ûũ ���� ��ġ (���� ��ǥ Z)
    public int chunkLength = 60; // ûũ ���� (Z ���� ũ��)

    void Start()
    {
        // �ʱ� ���� ���� (���ϴ� ���� ��ġ�� nextSpawnZ�� ����)
        nextSpawnZ = 0f;

        // �ʱ� ûũ 3�� ����
        for (int i = 0; i < 3; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // �� �����Ӹ��� �� �̵� �Ÿ��� ���� ������Ʈ
        mapTravelled += moveSpeed * Time.deltaTime;
        Debug.Log($"[DEBUG] mapTravelled: {mapTravelled}, nextSpawnZ: {nextSpawnZ}, ����: {nextSpawnZ - mapTravelled}");

        // �̵��� �Ÿ��� ���� spawnDistance �ȿ� ������, while ������ ���� ûũ�� ���޾� ����
        while (mapTravelled + spawnDistance >= nextSpawnZ)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        if (mapChunks == null || mapChunks.Length == 0)
        {
            Debug.LogError("mapChunks �迭�� ��� �ֽ��ϴ�! �������� �Ҵ��ϼ���.");
            return;
        }

        // ���� ûũ ���� �� ����
        int randIndex = Random.Range(0, mapChunks.Length);
        Vector3 spawnPosition = new Vector3(0, 0, nextSpawnZ);
        GameObject chunk = Instantiate(mapChunks[randIndex], spawnPosition, Quaternion.identity);

        Debug.Log($"���ο� �� ������: {chunk.name}, ��ġ: {spawnPosition}, ���� ûũ ���� ��ġ: {nextSpawnZ + chunkLength}");

        // ���� ûũ ��ġ ������Ʈ
        nextSpawnZ += chunkLength;
    }
}
