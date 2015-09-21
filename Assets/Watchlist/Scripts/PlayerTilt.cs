using UnityEngine;

public class PlayerTilt : VoBehavior
{
    public float TiltAngle = 30.0f;

    void Update()
    {
        Vector2 moveAxis = GameplayInput.GetMovementAxis();

        if (Mathf.Abs(moveAxis.x) > 0.001f || Mathf.Abs(moveAxis.y) > 0.001f)
        {
            moveAxis.Normalize();
            Vector3 perpendicular = new Vector3(moveAxis.y, 0, -moveAxis.x);
            this.transform.localRotation = Quaternion.AngleAxis(this.TiltAngle, perpendicular);
        }
        else
        {
            this.transform.localRotation = Quaternion.identity;
        }
    }
}
