using UnityEngine;

public class MeshBender : MonoBehaviour
{
    public float bendAmount = 0.5f; // 곡률 정도
    private Mesh originalMesh;
    private Mesh bentMesh;

    void Start()
    {
        originalMesh = GetComponent<MeshFilter>().mesh;
        bentMesh = Instantiate(originalMesh); // 원본 Mesh를 복사
        GetComponent<MeshFilter>().mesh = bentMesh; // 변형된 Mesh 적용
    }

    void Update()
    {
        Vector3[] vertices = originalMesh.vertices;
        Vector3[] modifiedVertices = new Vector3[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i];

            // Z축을 기준으로 곡률 적용
            float offset = v.z * v.z * bendAmount;
            v.y += offset; // y축으로 곡률 적용

            modifiedVertices[i] = v;
        }

        bentMesh.vertices = modifiedVertices;
        bentMesh.RecalculateNormals();
        bentMesh.RecalculateBounds();
    }
}
