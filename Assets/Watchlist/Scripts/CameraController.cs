using UnityEngine;

public class CameraController : VoBehavior
{
    public PlayerController[] PlayerControllers;
    public ZoomLevel[] ZoomLevels;
    public float AimingImpact = 50.0f;
    public float NormalApproachSpeed = 8.0f;
    //public float BoostedApproachSpeed = 60.0f;
    //public float ApproachBoostDistance = 250.0f;
    public float MaxDistanceForSnap = 0.01f;
    public Vector2 TargetPosition; // Exposed for debugging
    public Vector2 OffsetPosition;
    public float ZoomSpeed = 20.0f;
    public float RotationSpeed = 5.0f;
    public float MaxAngle = 30.0f;
    
    [System.Serializable]
    public struct ZoomLevel
    {
        public int OrthographicSize;
        public float MaxTargetDistance;
        public float MinTargetDistance;
    }

    void Awake()
    {
        _camera = this.GetComponent<Camera>();
        if (this.PlayerControllers.Length == 0)
            this.PlayerControllers = new PlayerController[DynamicData.MAX_PLAYERS];
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
            Vector2 oldTarget = _lockPosition;
            Vector2 centerTarget = calculateCenterTarget();
            this.transform.position = new Vector3(centerTarget.x + this.OffsetPosition.x, centerTarget.y + this.OffsetPosition.y, this.transform.position.z);

            //if (Vector2.Distance(oldTarget, centerTarget) > 0.01f)
                rotate();
            
            _currentZoomLevelIndex = findZoomLevelIndex();
            ZoomLevel zoomLevel = this.ZoomLevels[_currentZoomLevelIndex];
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
    private int _currentZoomLevelIndex;
    private float _currentAngle;
    private int _angleDirection = 1;

    private void rotate()
    {
        _currentAngle += this.RotationSpeed * Time.deltaTime * _angleDirection;
        if (Mathf.Abs(_currentAngle) > this.MaxAngle)
            _angleDirection = -_angleDirection;
        this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
    }

    private void playerSpawned(LocalEventNotifier.Event playerSpawnedEvent)
    {
        PlayerSpawnedEvent spawnEvent = playerSpawnedEvent as PlayerSpawnedEvent;
        this.PlayerControllers[spawnEvent.PlayerIndex] = spawnEvent.PlayerObject.GetComponent<PlayerController>();
    }

    private Vector2 calculateCenterTarget()
    {
        Vector2 avgCenter = Vector2.zero;
        Vector2 avgAiming = Vector2.zero;
        int count = 0;
        for (int i = 0; i < this.PlayerControllers.Length; ++i)
        {
            if (this.PlayerControllers[i] != null)
            {
                Transform target = this.PlayerControllers[i].ActualPosition;
                if (target != null)
                {
                    avgCenter += (Vector2)target.position;
                    avgAiming += this.PlayerControllers[i].AimAxis;
                    ++count;
                }
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

    private int findZoomLevelIndex()
    {
        float farthestDistance = 0.0f;
        for (int i = 0; i < this.PlayerControllers.Length; ++i)
        {
            if (this.PlayerControllers[i] != null)
            {
                float d = Vector2.Distance(_lockPosition, this.PlayerControllers[i].ActualPosition.position);
                if (d > farthestDistance)
                    farthestDistance = d;
            }
        }

        ZoomLevel currentZoomLevel = this.ZoomLevels[_currentZoomLevelIndex];

        if (_currentZoomLevelIndex < this.ZoomLevels.Length - 1 && farthestDistance > currentZoomLevel.MaxTargetDistance)
        {
            return _currentZoomLevelIndex + 1;
        }
        else if (_currentZoomLevelIndex > 0 && farthestDistance < currentZoomLevel.MinTargetDistance)
        {
            return _currentZoomLevelIndex - 1;
        }

        return _currentZoomLevelIndex;
    }
}
