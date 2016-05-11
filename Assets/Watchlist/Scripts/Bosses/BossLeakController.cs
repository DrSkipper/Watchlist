using UnityEngine;
using System.Collections.Generic;

public class BossLeakController : VoBehavior
{
    public GameObject EndFlowObject;

    void Awake()
    {
        _health = this.GetComponent<BossHealth>();
        _targets = new List<Transform>();
        this.GetComponent<BossSpawner>().SpawnCallback = spawned;
    }

    void Start()
    {
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        GlobalEvents.Notifier.Listen(PlayerDiedEvent.NAME, this, playerDied);
    }

    void Update()
    {
        if (_health.CurrentHealth <= 0)
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
    }

    private BossHealth _health;
    private List<Transform> _targets;

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        _targets.Add((e as PlayerSpawnedEvent).PlayerObject.transform);
    }

    private void playerDied(LocalEventNotifier.Event e)
    {
        _targets.Remove((e as PlayerDiedEvent).PlayerObject.transform);
    }

    private void spawned(GameObject go)
    {
        BossLeakMainBehavior mainBehavior = go.GetComponent<BossLeakMainBehavior>();
        mainBehavior.Targets = _targets;
        for (int i = 0; i < mainBehavior.SubBosses.Count; ++i)
        {
            _health.AddDamagable(mainBehavior.SubBosses[i].GetComponent<Damagable>());
        }
    }
}
