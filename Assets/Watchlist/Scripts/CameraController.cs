﻿using UnityEngine;
using System.Collections.Generic;

public class CameraController : VoBehavior
{
    public Transform[] PlayerTransforms;
    public ZoomLevel[] ZoomLevels;
    public float AimingImpact = 50.0f;
    public float NormalApproachSpeed = 8.0f;
    //public float BoostedApproachSpeed = 60.0f;
    //public float ApproachBoostDistance = 250.0f;
    public float MaxDistanceForSnap = 0.01f;
    public Vector2 TargetPosition; // Exposed for debugging
    public Vector2 OffsetPosition;
    public float ZoomSpeed = 20.0f;
    
    [System.Serializable]
    public struct ZoomLevel
    {
        public int OrthographicSize;
        public float MaxTargetDistance;
    }

    void Awake()
    {
        _camera = this.GetComponent<Camera>();
        if (this.PlayerTransforms.Length == 0)
            this.PlayerTransforms = new Transform[DynamicData.MAX_PLAYERS];
    }

    void Start()
    {
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        _lockPosition = (Vector2)this.transform.position;
    }

    void Update()
    {
        if (!PauseController.IsPaused())
        {
            Vector2 centerTarget = calculateCenterTarget();
            this.transform.position = new Vector3(centerTarget.x + this.OffsetPosition.x, centerTarget.y + this.OffsetPosition.y, this.transform.position.z);

            ZoomLevel zoomLevel = findZoomLevel();
            int size = Mathf.RoundToInt(Mathf.MoveTowards(_camera.orthographicSize, zoomLevel.OrthographicSize, this.ZoomSpeed));
            if (Mathf.Abs(size - zoomLevel.OrthographicSize) < 2)
                size = zoomLevel.OrthographicSize;
            _camera.orthographicSize = size;
        }
    }

    public override void OnDestroy()
    {
        if (GlobalEvents.Notifier != null)
            GlobalEvents.Notifier.RemoveAllListenersForOwner(this);
        base.OnDestroy();
    }

    /**
     * Private
     */
    private Vector2 _lockPosition;
    private Camera _camera;

    private void playerSpawned(LocalEventNotifier.Event playerSpawnedEvent)
    {
        PlayerSpawnedEvent spawnEvent = playerSpawnedEvent as PlayerSpawnedEvent;
        this.PlayerTransforms[spawnEvent.PlayerIndex] = spawnEvent.PlayerObject.GetComponent<PlayerController>().ActualPosition;
    }

    private Vector2 calculateCenterTarget()
    {
        Vector2 avgCenter = Vector2.zero;
        Vector2 avgAiming = Vector2.zero;
        int count = 0;
        for (int i = 0; i < this.PlayerTransforms.Length; ++i)
        {
            Transform target = this.PlayerTransforms[i];
            if (target != null)
            {
                avgCenter += (Vector2)target.position;
                avgAiming += GameplayInput.GetAimingAxis(i, target.position, false);
                ++count;
            }
        }

        if (count > 0)
        {
            avgCenter /= count;
            avgAiming /= count;

            this.TargetPosition = avgCenter;

            float distance = Vector2.Distance(_lockPosition, this.TargetPosition);
            if (distance <= this.MaxDistanceForSnap)
            {
                _lockPosition = this.TargetPosition;
            }
            else
            {
                _lockPosition = Vector2.MoveTowards(_lockPosition, this.TargetPosition, this.NormalApproachSpeed * Time.deltaTime * distance);
            }

            return _lockPosition + (avgAiming * this.AimingImpact);
        }

        return _lockPosition;
    }

    private ZoomLevel findZoomLevel()
    {
        float farthestDistance = 0.0f;
        foreach (Transform target in this.PlayerTransforms)
        {
            if (target != null)
            {
                float d = Vector2.Distance(_lockPosition, target.position);
                if (d > farthestDistance)
                    farthestDistance = d;
            }
        }
        foreach (ZoomLevel zoomLevel in this.ZoomLevels)
        {
            if (zoomLevel.MaxTargetDistance >= farthestDistance)
                return zoomLevel;
        }
        return this.ZoomLevels[this.ZoomLevels.Length - 1];
    }
}
