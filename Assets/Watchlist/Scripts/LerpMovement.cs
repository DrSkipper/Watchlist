﻿using UnityEngine;
using System.Collections.Generic;

public class LerpMovement : VoBehavior
{
    public Vector2 TargetPosition;
    public float MovementSpeed = 100.0f;
    public bool IsMoving = false;

    public delegate void LerpFinishedCallback(GameObject gameObject);

    void Awake()
    {
        _callbacks = new List<LerpFinishedCallback>();
    }

    public void BeginMovement(Vector2 targetPosition)
    {
        this.TargetPosition = targetPosition;
        this.IsMoving = true;
    }

    public void BeginMovement(Vector2 targetPosition, float timeToReachPoint)
    {
        this.BeginMovement(targetPosition);
        this.MovementSpeed = Vector2.Distance(targetPosition, this.transform.position) / timeToReachPoint;
    }

    public void HaltMovement()
    {
        this.MovementSpeed = 0.0f;
        this.IsMoving = false;
        this.ClearCallbacks();
    }

    public void AddCallback(LerpFinishedCallback callback)
    {
        _callbacks.Add(callback);
    }

    public void ClearCallbacks()
    {
        _callbacks.Clear();
    }

    void Update()
    {
        if (!PauseController.IsPaused() && this.IsMoving)
        {
            Vector2 lerpPos;
            if (Vector2.Distance(this.transform.position, this.TargetPosition) < 0.1f)
            {
                this.IsMoving = false;
                lerpPos = this.TargetPosition;
            }
            else
            {
                lerpPos = Vector2.Lerp(this.transform.position, this.TargetPosition, this.MovementSpeed * Time.deltaTime / (Vector2.Distance(this.transform.position, this.TargetPosition)));
            }

            this.transform.position = new Vector3(lerpPos.x, lerpPos.y, this.transform.position.z);

            if (!this.IsMoving)
            {
                for (int i = 0; i < _callbacks.Count; ++i)
                {
                    _callbacks[i](this.gameObject);
                }
            }
        }
    }

    /**
     * Private
     */
    private List<LerpFinishedCallback> _callbacks;
}
