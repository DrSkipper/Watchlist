using UnityEngine;

public class CouncilChair : VoBehavior
{
    public int PlayerIndex = -1;
    public float DistanceToSnapPlayer = 50.0f;
    public EnemySpawner Spawner;

    public bool ChairFilled { get { return this.PlayerIndex == -1 || _filledByPlayer; } }

    void Start()
    {
        if (this.PlayerIndex >= 0)
        {
            if (!DynamicData.GetSessionPlayer(this.PlayerIndex).HasJoined)
                this.PlayerIndex = -1;
            else
                GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        }

        if (this.PlayerIndex == -1)
        {
            this.Spawner.Targets = PlayerTargetController.Targets;
            Spawner.BeginSpawn();
        }
    }

    void Update()
    {
        if (_targetPlayerTransform != null && !_filledByPlayer && 
            Vector2.Distance(this.transform.position, _targetPlayerTransform.position) <= this.DistanceToSnapPlayer)
        {
            _targetPlayerTransform.position = this.transform.position;
            _targetPlayerTransform.GetComponent<PlayerController>().NoMove = true;
            _filledByPlayer = true;
        }
    }

    /**
     * Private
     */
    private Transform _targetPlayerTransform;
    private bool _filledByPlayer;

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        PlayerSpawnedEvent playerSpawnedEvent = e as PlayerSpawnedEvent;
        if (playerSpawnedEvent.PlayerIndex == this.PlayerIndex)
        {
            _targetPlayerTransform = playerSpawnedEvent.PlayerObject.transform;
        }
    }
}
