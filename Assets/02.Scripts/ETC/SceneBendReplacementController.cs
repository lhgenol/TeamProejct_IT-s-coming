using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SceneBendReplacementController : MonoBehaviour
{
    public Shader replacementShader;
    public string replacementTag = "RenderType";

    Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
        if (_cam != null && replacementShader != null)
        {
            // �� ��ü�� replacementShader ����
            _cam.SetReplacementShader(replacementShader, replacementTag);
        }
    }

    void OnDisable()
    {
        // ī�޶� �����ų� ��Ȱ��ȭ�� ��, ������� ��������
        if (_cam != null)
            _cam.ResetReplacementShader();
    }
}