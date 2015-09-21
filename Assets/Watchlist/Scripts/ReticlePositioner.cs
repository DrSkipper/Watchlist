﻿using UnityEngine;

public class ReticlePositioner : VoBehavior
{
    public float Distance = 0.25f;

    void Update()
    {
        Vector2 aimAxis = GameplayInput.GetAimingAxis();
        this.transform.localPosition = new Vector3(aimAxis.x * this.Distance, this.transform.localPosition.y, aimAxis.y * this.Distance);
        float angle = Vector2.Angle(new Vector2(0, 1), aimAxis);
        if (aimAxis.x < 0.0f)
            angle = -angle;
        this.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
    }
}