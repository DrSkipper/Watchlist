using UnityEngine;

public class BossSpawner : VoBehavior
{
    public delegate void SpawnedObjectCallback(GameObject spawnedObject);

    public float SpawnDelay = 0.5f;
    public SpawnedObjectCallback SpawnCallback;
    public GameObject PrefabToSpawn;
    public GameObject SpawnVisualPrefab;
    public bool SpawnOnStart = false;
    public float InitialDelay = 0.0f;
    
    void Start()
    {
        if (this.SpawnOnStart)
            this.GetComponent<TimedCallbacks>().AddCallback(this, initialSpawn, this.InitialDelay);
    }

    public void InitiateSpawn(GameObject prefabToSpawn, GameObject spawnVisualPrefab, SpawnedObjectCallback callback)
    {
        this.PrefabToSpawn = prefabToSpawn;
        this.SpawnCallback = callback;
        Instantiate(spawnVisualPrefab, this.transform.position, Quaternion.identity);
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.Spawn, this.SpawnDelay);
    }

    public void Spawn()
    {
        GameObject spawn = Instantiate(this.PrefabToSpawn, this.transform.position, Quaternion.identity) as GameObject;
        if (this.SpawnCallback != null)
            this.SpawnCallback(spawn);
    }

    /**
     * Private
     */
    private void initialSpawn()
    {
        this.InitiateSpawn(this.PrefabToSpawn, this.SpawnVisualPrefab, this.SpawnCallback);
    }
}
