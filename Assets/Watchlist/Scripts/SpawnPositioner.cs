using UnityEngine;

public class SpawnPositioner : VoBehavior
{
    public GameObject LevelGenerator;
    public GameObject Tiles;
    public GameObject EnemySpawnPrefab;
    public int NumEnemies = 10;

    void Start()
    {
        _levelGenManager = this.LevelGenerator.GetComponent<LevelGenManager>();
        _tileRenderer = this.Tiles.GetComponent<TileMapOutlineRenderer>();
        _levelGenManager.UpdateDelegate = this.LevelGenUpdate;
    }

    void LevelGenUpdate()
    {
        if (_levelGenManager.Finished)
        {
            LevelGenOutput output = _levelGenManager.GetOutput();
            
        }
    }

    /**
     * Private
     */
    private LevelGenManager _levelGenManager;
    private TileMapOutlineRenderer _tileRenderer;
}
