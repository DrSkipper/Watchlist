using UnityEngine;

public class SimpleCameraShaderEffect : MonoBehaviour
{
    public Material Material;
    public bool SetTexture = false;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (this.SetTexture)
            this.Material.SetTexture("_Texture", source);
        Graphics.Blit(source, destination, this.Material);
    }
}
