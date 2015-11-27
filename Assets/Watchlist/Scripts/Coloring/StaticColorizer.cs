using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StaticColorizer : MonoBehaviour
{
    public string ColorTag = "background";

    void Start()
    {
        Color color = GameplayPalette.GetColorFromTag(this.ColorTag);

        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        else
        {
            LineRenderer lineRenderer = this.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.SetColors(color, color);
            }
            else
            {
                Camera camera = this.GetComponent<Camera>();
                if (camera != null)
                {
                    camera.backgroundColor = color;
                }
                else
                {
                    MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        //meshRenderer.material.SetColor("_TintColor", color);
                        meshRenderer.material.color = color;
                    }
                    else
                    {
                        Image image = this.GetComponent<Image>();
                        if (image != null)
                            image.color = color;
                    }
                }
            }
        }
    }
}
