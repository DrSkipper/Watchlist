using UnityEngine;

public class CameraController : VoBehavior
{
    public Transform CenterTarget;
    public float AimingImpact = 50.0f;
    public float NormalApproachSpeed = 8.0f;
    //public float BoostedApproachSpeed = 60.0f;
    //public float ApproachBoostDistance = 250.0f;
    public float MaxDistanceForSnap = 0.01f;
    public Vector2 TargetPosition; // Exposed for debugging
    public Vector2 OffsetPosition;

    void Start()
    {
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        _lockPosition = (Vector2)this.transform.position;
    }

    void Update()
    {
        if (CenterTarget != null)
        {
            //TODO - Need to aggregate aiming axes for all players
            Vector2 aimAxis = GameplayInput.GetAimingAxis(1, false);
            this.TargetPosition = ((Vector2)CenterTarget.position);

            float distance = Vector2.Distance(_lockPosition, this.TargetPosition);
            float d = distance <= this.MaxDistanceForSnap ? distance : this.NormalApproachSpeed * Time.deltaTime * distance;
            _lockPosition = Vector2.MoveTowards(_lockPosition, this.TargetPosition, d);

            Vector2 finalPosition = _lockPosition + (aimAxis * this.AimingImpact);
            this.transform.position = new Vector3(finalPosition.x + this.OffsetPosition.x, finalPosition.y + this.OffsetPosition.y, this.transform.position.z);
        }
        else
        {
            this.transform.position = new Vector3(_lockPosition.x + this.OffsetPosition.x, _lockPosition.y + this.OffsetPosition.y, this.transform.position.z);
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

    private void playerSpawned(LocalEventNotifier.Event playerSpawnedEvent)
    {
        //TODO - Account for multiple camera targets
        if (this.CenterTarget == null)
        {
            this.CenterTarget = (playerSpawnedEvent as PlayerSpawnedEvent).PlayerObject.transform;
        }
    }
}
