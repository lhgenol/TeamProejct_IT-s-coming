using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum Parts
{ 
Hat,
OutWear,
Pants
}
public class Customizing : MonoBehaviour
{
    [Header("Renderer")]
    public SkinnedMeshRenderer hatRenderer;
    public SkinnedMeshRenderer outWearRenderer;
    public SkinnedMeshRenderer pantsRenderer;

    [Header("Mesh")]
    public Mesh[] hatMeshs;
    public Mesh[] outWearMeshs;
    public Mesh[] pantsMeshs;

    public void ChangeMesh(Parts part,int index)
    {
        switch (part)
        {
            case Parts.Hat:
                hatRenderer.sharedMesh = hatMeshs[index];
                break;
            case Parts.OutWear:
                outWearRenderer.sharedMesh = outWearMeshs[index];
                break;
            case Parts.Pants:
                pantsRenderer.sharedMesh = pantsMeshs[index];
                break;
        }
    }
}
