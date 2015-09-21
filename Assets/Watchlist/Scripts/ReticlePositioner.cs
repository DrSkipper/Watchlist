using UnityEngine;

public class ReticlePositioner : VoBehavior
{
    public float Distance = 0.25f;

    void Update()
    {
        Vector2 aimAxis = getAimingAxis();
        this.transform.localPosition = new Vector3(aimAxis.x * this.Distance, this.transform.localPosition.y, aimAxis.y * this.Distance);
        float angle = Vector2.Angle(new Vector2(0, 1), aimAxis);
        if (aimAxis.x < 0.0f)
            angle = -angle;
        this.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
    }

    /**
     * Private
     */
    private Vector2 getAimingAxis()
    {
        Vector2 aimAxis = new Vector2();

        if (Input.anyKey)
        {
            // Construct movment axis from 4-directional keyboard input
            float x = 0;
            float y = 0;

            if (Input.GetKey(KeyCode.UpArrow)) y += 1;
            if (Input.GetKey(KeyCode.LeftArrow)) x -= 1;
            if (Input.GetKey(KeyCode.DownArrow)) y -= 1;
            if (Input.GetKey(KeyCode.RightArrow)) x += 1;

            if (y != 0 && x != 0)
            {
                x = Mathf.Sign(x) * 0.70710678f;
                y = Mathf.Sign(y) * 0.70710678f;
            }

            aimAxis.x = x;
            aimAxis.y = y;
        }

        else
        {
            // Use controller input
            aimAxis.x = Input.GetAxis("Horizontal 2");
            aimAxis.y = Input.GetAxis("Vertical 2");
            aimAxis.Normalize();
        }

        return aimAxis;
    }
}
