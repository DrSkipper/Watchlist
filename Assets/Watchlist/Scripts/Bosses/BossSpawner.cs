using UnityEngine;

public class BossSpawner : VoBehavior
{
    public delegate void SpawnedObjectCallback(GameObject spawnedObject);

    public float SpawnDelay = 0.5f;
    public SpawnedObjectCallback SpawnCallback;

    public void InitiateSpawn(GameObject prefabToSpawn, GameObject spawnVisualPrefab, SpawnedObjectCallback callback)
    {
        _prefabToSpawn = prefabToSpawn;
        _callback = callback;
        Instantiate(spawnVisualPrefab, this.transform.position, Quaternion.identity);
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.Spawn, this.SpawnDelay);
    }

    public void Spawn()
    {
        GameObject spawn = Instantiate(_prefabToSpawn, this.transform.position, Quaternion.identity) as GameObject;
        _callback(spawn);
        _prefabToSpawn = null;
        _callback = null;
    }

    /**
     * Private
     */
    private GameObject _prefabToSpawn;
    private SpawnedObjectCallback _callback;
}
