using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : VoBehavior
{
    public int NumPoints = 16;
    public float InitialRadius = 0.0f;
    public float Radius { get { return _radius; } set { _radius = value; _dirty = true; } }

    void Start()
    {
        _lines = this.GetComponent<LineRenderer>();
        this.Radius = this.InitialRadius;
    }

    void Update()
    {
        if (_dirty)
        {
            _dirty = false;

            float thetaScale = (2.0f * Mathf.PI) / ((float)this.NumPoints);
            float theta = 0.0f;
            _lines.SetVertexCount(this.NumPoints + 2);

            for (int i = 0; i < this.NumPoints + 2; ++i)
            {
                theta += thetaScale;
                _lines.SetPosition(i, new Vector3(_radius * Mathf.Cos(theta), _radius * Mathf.Sin(theta), 0));
            }
        }
    }

    /**
     * Private
     */
    private LineRenderer _lines;
    private bool _dirty;
    private float _radius;
}
