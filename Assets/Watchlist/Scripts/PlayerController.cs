using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : VoBehavior
{
    public float AccelerationDuration = 0.5f;
    public float MaxSpeed = 1.0f;
    public bool DirectionalAcceleration = true; //TODO - Implement "false" approach for this
    public float ShotCooldown = 0.2f;
    public float ShotSpeed = 1.5f;
    public Vector3 Up = Vector3.up;

    public GameObject BulletPrefab;

    // Made public for display/debugging/editor purposes only
    public Vector2 Velocity = new Vector2();

    void Start()
    {
        _acceleration = this.AccelerationDuration > 0 ? this.MaxSpeed / this.AccelerationDuration : this.MaxSpeed * 1000;
    }

    void Update()
    {
        Vector2 movementAxis = this.GetMovementAxis();

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
                                              this.transform.position.z) + this.Up * (this.Velocity.y * Time.deltaTime);

        // Shooting
        //TODO - fcole - Weapon behaviors should be handled in own class(es)
        if (_shotCooldown <= 0.0f)
        {
            _shotCooldown = this.ShotCooldown;
            Vector2 aimAxis = getAimingAxis();

            if (aimAxis.x != 0 || aimAxis.y != 0)
            {
                // Create instance of bullet prefab and set velocity on it's BulletController component
                GameObject bullet = Instantiate(BulletPrefab, this.transform.position, Quaternion.identity) as GameObject;
                bullet.GetComponent<BulletController>().Velocity = new Vector2(aimAxis.x * this.ShotSpeed, aimAxis.y * this.ShotSpeed);
            }
        }
        else
        {
            _shotCooldown -= Time.deltaTime;
        }
    }
    
    // Move to player input handling file which calculates things like axes in pre update?
    public Vector2 GetMovementAxis()
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

    /**
     * Private
     */
    private float _acceleration;
    private float _shotCooldown;
    private bool _usingMovingSprite;

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
