using UnityEngine;

public class CameraController : VoBehavior
{
    public Transform CenterTarget;
    public float AimingImpact = 50.0f;
    public float NormalApproachSpeed = 8.0f;
    //public float BoostedApproachSpeed = 60.0f;
    //public float ApproachBoostDistance = 250.0f;
    public float MaxDistanceForSnap = 0.01f;
    public Vector2 TargetPosition; // Exposed for debugging

    void Start()
    {
        _lockPosition = (Vector2)this.transform.position;
    }

    void Update()
    {
        Vector2 aimAxis = GameplayInput.GetAimingAxis(false);
        this.TargetPosition = ((Vector2)CenterTarget.position);

        float distance = Vector2.Distance(_lockPosition, this.TargetPosition);
        float d = distance <= this.MaxDistanceForSnap ? distance : this.NormalApproachSpeed * Time.deltaTime * distance;
        _lockPosition = Vector2.MoveTowards(_lockPosition, this.TargetPosition, d);

        Vector2 finalPosition = _lockPosition + (aimAxis * this.AimingImpact);
        this.transform.position = new Vector3(finalPosition.x, finalPosition.y, this.transform.position.z);
    }

    /**
     * Private
     */
    private Vector2 _lockPosition;
}
