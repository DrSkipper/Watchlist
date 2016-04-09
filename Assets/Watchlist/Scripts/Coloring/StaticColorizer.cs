using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StaticColorizer : MonoBehaviour
{
    public string ColorTag = "background";
    public int ColorParameter = 0;
    public StaticColorizer[] DependentColorizers;
    public bool UpdateOnStart = true;

    void Start()
    {
        if (this.UpdateOnStart)
            this.UpdateVisual(this.ColorTag, this.ColorParameter);
    }

    public void UpdateVisual(string colorTag, int colorParameter)
    {
        this.ColorTag = colorTag;
        this.ColorParameter = colorParameter;

        foreach (StaticColorizer colorizer in this.DependentColorizers)
        {
            colorizer.UpdateVisual(colorTag, colorParameter);
        }

        updateVisual(GameplayPalette.GetColorFromTag(colorTag, colorParameter));
    }

    /**
     * Private
     */
    private void updateVisual(Color color)
    {
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
                        {
                            image.color = color;
                        }
                        else
                        {
                            Text text = this.GetComponent<Text>();
                            if (text != null)
                                text.color = color;
                        }
                    }
                }
            }
        }
    }
}
