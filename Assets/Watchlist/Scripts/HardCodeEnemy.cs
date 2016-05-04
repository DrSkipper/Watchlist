using UnityEngine;

[RequireComponent(typeof(GenericEnemy))]
public class HardCodeEnemy : MonoBehaviour
{
    public int EnemyId;

    void Awake()
    {
        this.GetComponent<GenericEnemy>().EnemyType = StaticData.EnemyData.EnemyTypes[this.EnemyId];
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
    }

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        this.GetComponent<GenericEnemy>().Targets.Add((e as PlayerSpawnedEvent).PlayerObject.transform);
    }
}
