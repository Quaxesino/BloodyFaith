using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshHighlighter : MonoBehaviour
{
    [SerializeField] List<SkinnedMeshRenderer> meshesToHighlight;
    [SerializeField] Material originalMaterial;
    [SerializeField] Material highlightedMaterial;

    public void HighlightMesh(bool highlight)
    {
        foreach (var mesh in meshesToHighlight)
        {
            mesh.material = (highlight) ? highlightedMaterial : originalMaterial;
        }
    }
}
