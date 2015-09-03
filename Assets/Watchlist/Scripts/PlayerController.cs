using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : VoBehavior
{
    public float AccelerationDuration = 0.5f;
    public float MaxSpeed = 1.0f;
    public bool DirectionalAcceleration = true; //TODO - Implement "false" approach for this

    // Made public for display/debugging/editor purposes only
    public Vector2 Velocity = new Vector2();

    void Start()
    {
        _acceleration = this.AccelerationDuration > 0 ? this.MaxSpeed / this.AccelerationDuration : this.MaxSpeed * 1000;
    }

    void Update()
    {
        Vector2 movementAxis = getMovementAxis();

        float targetX = movementAxis.x * this.MaxSpeed;
        float targetY = movementAxis.y * this.MaxSpeed;

        float changeX = targetX - this.Velocity.x;
        float changeY = targetY - this.Velocity.y;

        if (changeX != 0 || changeY != 0)
        {
            float changeTotal = Mathf.Sqrt(Mathf.Pow(changeX, 2) + Mathf.Pow(changeY, 2));

            if (changeX != 0)
            {
                float ax = Mathf.Abs(_acceleration * changeX / changeTotal);
                this.Velocity.x = Mathf.Lerp(this.Velocity.x, targetX, ax * Time.deltaTime / Math.Abs(changeX));
            }

            if (changeY != 0)
            {
                float ay = Mathf.Abs(_acceleration * changeY / changeTotal);
                this.Velocity.y = Mathf.Lerp(this.Velocity.y, targetY, ay * Time.deltaTime / Math.Abs(changeY));
            }
        }

        this.transform.position = new Vector3(this.transform.position.x + this.Velocity.x * Time.deltaTime, 
                                              this.transform.position.y,
                                              this.transform.position.z + this.Velocity.y * Time.deltaTime);
    }

    /**
     * Private
     */
    private float _acceleration;

    private Vector2 getMovementAxis()
    {
        Vector2 movementAxis = new Vector2();

        if (Input.anyKey)
        {
            // Construct movment axis from 4-directional keyboard input
            float x = 0;
            float y = 0;

            if (Input.GetKey(KeyCode.W)) y += 1;
            if (Input.GetKey(KeyCode.A)) x -= 1;
            if (Input.GetKey(KeyCode.S)) y -= 1;
            if (Input.GetKey(KeyCode.D)) x += 1;

            if (y != 0 && x != 0)
            {
                x = Mathf.Sign(x) * 0.70710678f;
                y = Mathf.Sign(y) * 0.70710678f;
            }

            movementAxis.x = x;
            movementAxis.y = y;
        }

        else
        {
            // Use controller input

        }

        return movementAxis;
    }
}
