using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SceneBendReplacementController : MonoBehaviour
{
    public Shader replacementShader;
    public string replacementTag = "RenderType";

    Camera _cam;

    /// <summary>
    /// 씬 전체에 replacementShader 적용
    /// </summary>
    void Start()
    {
        _cam = GetComponent<Camera>();
        if (_cam != null && replacementShader != null)
        {
            
            _cam.SetReplacementShader(replacementShader, replacementTag);
        }
    }

    void OnDisable()
    {
        if (_cam != null)
            _cam.ResetReplacementShader();
    }
}