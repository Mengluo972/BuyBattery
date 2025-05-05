using UnityEngine;
/// <summary>
/// 后处理摄像机脚本基类
/// 编辑器状态下可以执行脚本查看效果
/// 一些屏幕特效可能需要更多的设置， 例如设置一些默认值等， 可以重载Start、 CheckResources或CheckSupport 函数。
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectBase : MonoBehaviour
{
    //调用：开始
    protected void CheckResources()
    {
        bool isSupported = CheckSupport();
        if (isSupported == false)
        {
            NotSupported();
        }
    }
    //调用：检查资源是否支持平台
    protected bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects == false || SystemInfo.supportsRenderTextures == false)
        {
            Debug.LogWarning("此平台不支持图像特效或者渲染贴图");
            return false;
        }
        return true;
    }
    //调用：平台不支持此特效
    protected void NotSupported()
    {
        enabled = false;
    }
    //
    protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null)
        {
            return null;
        }
        if(shader.isSupported && material && material.shader == shader)
            return material;
        if (!shader.isSupported)
        {
            return material;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if(material)
                return material;
            else
            {
                return null;
            }
        }
    }
    protected void Start()
    {
        CheckResources();
    }
}