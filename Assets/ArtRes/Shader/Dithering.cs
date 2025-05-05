using UnityEngine;
public class Dithering : PostEffectBase
{
    public Shader DitherScreenShader;
    private Material _DitherScreenMat = null;
    public Material material
    {
        get
        {
            _DitherScreenMat = CheckShaderAndCreateMaterial(DitherScreenShader, _DitherScreenMat);
            return _DitherScreenMat;
        }
    }
    //定义目标传参
    [Range(1, 20)] public int pixelScale = 1;
    public int ditherSize = 1;
    public int cameraX = 1;
    public int cameraY = 1;
    [Range(0.0f, 1.0f)] public float pixelPower = 0.5f;
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_PixelScale", pixelScale);
            material.SetFloat("_DitherSize", ditherSize);
            material.SetFloat("_XOffset", cameraX);
            material.SetFloat("_YOffset", cameraY);
            material.SetFloat("_PixelPower", pixelPower);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
