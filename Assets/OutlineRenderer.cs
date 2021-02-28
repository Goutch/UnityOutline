using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class OutlineRenderer : MonoBehaviour
{
    [SerializeField] Material OutlineDrawMaterial;
    [SerializeField] Shader OutlinePostProcessing;
    private LinkedList<Outline> outlines=new LinkedList<Outline>();
    private Material ppeMaterial;

    private void Start()
    {
        ppeMaterial = new Material(OutlinePostProcessing);

    }

    public void DrawOutline(Outline outline)
    {
        outlines.AddLast(outline);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (outlines.Count != 0)
        {
            RenderTexture shapeTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);
            shapeTexture.Create();
            Graphics.SetRenderTarget(shapeTexture);
            OutlineDrawMaterial.SetPass(0);
            foreach (var outline in outlines)
            {
                Graphics.DrawMeshNow(outline.Mesh.mesh, outline.transform.localToWorldMatrix);
            }


            outlines.Clear();
            
            
            ppeMaterial.SetTexture("_OutlineShape",shapeTexture);

            Graphics.Blit(source,destination,ppeMaterial);
            shapeTexture.Release();
        }
        else
        {
            Graphics.Blit(source,destination);
        }
        
    }
}