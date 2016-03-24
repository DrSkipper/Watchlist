using UnityEngine;

public class ReticlePositioner : VoBehavior
{
    public float Distance = 0.25f;
    public bool YIsUp = false;
    public float InitialAngle = 0.0f;
    public bool VisibleAtRest = false;
    public int PlayerIndex = 0;
    public Transform PlayerTransform;

    void Start()
    {
        _rotationAxis = this.YIsUp ? new Vector3(0, 0, 1) : new Vector3(0, 1, 0);
    }

    void Update()
    {
        if (PauseController.IsPaused())
            return;

        Vector2 aimAxis = GameplayInput.GetAimingAxis(this.PlayerIndex, this.PlayerTransform.position);

        if (Mathf.Abs(aimAxis.x) >= 0.001f || Mathf.Abs(aimAxis.y) >= 0.001f)
        {
            if (!this.spriteRenderer.enabled)
                this.spriteRenderer.enabled = true;

            aimAxis.Normalize();
            
            this.transform.localPosition = this.YIsUp ? new Vector3(aimAxis.x * this.Distance, aimAxis.y * this.Distance, this.transform.localPosition.z) : new Vector3(aimAxis.x * this.Distance, this.transform.localPosition.y, aimAxis.y * this.Distance);

            float angle = Vector2.Angle(new Vector2(0, 1), aimAxis);
            if ((this.YIsUp && aimAxis.x > 0.0f) || (!this.YIsUp && aimAxis.x < 0.0f))
                angle = -angle;

            this.transform.localRotation = Quaternion.AngleAxis(angle + this.InitialAngle, _rotationAxis);
        }
        else
        {
            this.transform.localPosition = this.YIsUp ? new Vector3(0, 0, this.transform.localPosition.z) : new Vector3(0, this.transform.localPosition.y, 0);

            if (!this.VisibleAtRest)
                this.spriteRenderer.enabled = false;
        }
    }

    /**
     * Private
     */
    private Vector3 _rotationAxis;
}
