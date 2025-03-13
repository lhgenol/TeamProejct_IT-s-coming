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
            // 씬 전체에 replacementShader 적용
            _cam.SetReplacementShader(replacementShader, replacementTag);
        }
    }

    void OnDisable()
    {
        // 카메라가 꺼지거나 비활성화될 때, 원래대로 돌려놓음
        if (_cam != null)
            _cam.ResetReplacementShader();
    }
}