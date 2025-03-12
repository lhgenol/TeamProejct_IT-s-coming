using UnityEngine;

[ExecuteInEditMode]
public class CurvedPostEffect : MonoBehaviour
{
    public Material curvedMaterial;
    public float bendStrength = 0.1f; // 곡률 강도

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (curvedMaterial != null)
        {
            curvedMaterial.SetFloat("_BendStrength", bendStrength);
            Graphics.Blit(src, dest, curvedMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
