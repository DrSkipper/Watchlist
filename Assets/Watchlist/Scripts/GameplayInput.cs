﻿using UnityEngine;

public static class GameplayInput
{
    //TODO - Calculates things like axes in pre update?
    public static Vector2 GetMovementAxis()
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
            movementAxis.x = Input.GetAxis("Horizontal");
            movementAxis.y = Input.GetAxis("Vertical");
        }

        return movementAxis;
    }

    public static Vector2 GetAimingAxis()
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