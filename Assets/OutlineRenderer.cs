using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OutlineRenderer : MonoBehaviour
{
    [SerializeField] Material OutlineDrawMaterial;
    [SerializeField] Shader OutlinePostProcessing;

    [SerializeField] private Color outlineColor = Color.white;

    private LinkedList<Outline> outlines = new LinkedList<Outline>();

    private Material ppeMaterial;
    private RenderTexture shapeTexture;

    private void Start()
    {
        ppeMaterial = new Material(OutlinePostProcessing);
        ppeMaterial.SetColor("_Color", outlineColor);
    }

    public void DrawOutline(Outline outline)
    {
        outlines.AddLast(outline);
    }

    public void OnDestroy()
    {
        if (shapeTexture) shapeTexture.Release();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (outlines.Count != 0)
        {
            if (shapeTexture == null)
            {
                shapeTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);
                shapeTexture.Create();
            }
            else if (shapeTexture.dimension != source.dimension)
            {
                shapeTexture.Release();
                shapeTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);
                shapeTexture.Create();
            }


            Graphics.SetRenderTarget(shapeTexture);

            OutlineDrawMaterial.SetPass(0);
            foreach (var outline in outlines)
            {
                Graphics.DrawMeshNow(outline.Mesh.mesh, outline.transform.localToWorldMatrix);
            }


            outlines.Clear();
            ppeMaterial.SetColor("_Color", outlineColor);
            ppeMaterial.SetTexture("_OutlineShape", shapeTexture);
            Graphics.Blit(source, destination, ppeMaterial);
            shapeTexture.Release();
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}