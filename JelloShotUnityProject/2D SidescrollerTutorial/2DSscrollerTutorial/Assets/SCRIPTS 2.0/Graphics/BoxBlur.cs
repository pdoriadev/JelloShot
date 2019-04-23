using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoxBlur : MonoBehaviour
{
    public Material blurMaterial;
    [Range(0, 10)]
    public int _Iterations;
    [Range(0, 4)]
    public int _ResolutionDown;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width >> _ResolutionDown;
        int height = source.height >> _ResolutionDown;

        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(source, rt);

        for (int i = 0; i < _Iterations; i++)
        {
            RenderTexture rt2 = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(rt, rt2, blurMaterial);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        Graphics.Blit(rt, destination);
        RenderTexture.ReleaseTemporary(rt);
    }


}
