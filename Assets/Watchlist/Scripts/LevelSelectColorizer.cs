using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LevelSelectColorizer : VoBehavior
{
    public Color Color = Color.white;
    
    void Start()
    {
        this.UpdateColor();
    }

    public void UpdateColor()
    {
        foreach (LineRenderer line in this.gameObject.GetComponentsInChildren<LineRenderer>())
        {
            line.SetColors(this.Color, this.Color);
        }
    }
}
