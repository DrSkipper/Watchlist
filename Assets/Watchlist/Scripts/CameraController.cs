using UnityEngine;
using System.Collections.Generic;

public class CameraController : VoBehavior
{
    public Transform CenterTarget;
    public float AimingImpact = 50.0f;
    //public float Speed;
    public float ApproachSpeed = 240f;
    public float MaxDistanceForSnap = 0.01f;
    public Vector2 TargetPosition; // Exposed for debugging

    void Update()
    {
        Vector2 aimAxis = GameplayInput.GetAimingAxis(false);
        this.TargetPosition = ((Vector2)CenterTarget.position) + (aimAxis * this.AimingImpact);
        //Vector2 lerpedPosition = Vector2.MoveTowards(this.transform.position, this.TargetPosition, this.Speed * Time.deltaTime);
        float distance = Vector2.Distance(this.transform.position, this.TargetPosition);
        float d = distance <= this.MaxDistanceForSnap ? distance : this.ApproachSpeed * Time.deltaTime * distance;
        Vector2 lerpedPosition = Vector2.MoveTowards(this.transform.position, this.TargetPosition, d);
        this.transform.position = new Vector3(lerpedPosition.x, lerpedPosition.y, this.transform.position.z);
    }
}
