using UnityEngine;
using System.IO;

[RequireComponent(typeof(TileMapOutlineRenderer))]
[ExecuteInEditMode]
public class TestLevelLayout : VoBehavior
{
    public string LevelFileName;

    public const string LEVEL_DATA_ROOT = "Levels/";
    public const string LEVEL_DATA_SUFFIX = ".xml";

    void Start()
    {
        this.CreateGeometry();
    }

    public void CreateGeometry()
    {
        TileMapOutlineRenderer outlineRenderer = this.GetComponent<TileMapOutlineRenderer>();
        outlineRenderer.Clear();

        LevelData level = LevelData.Load(Path.Combine(Application.streamingAssetsPath, LEVEL_DATA_ROOT + LevelFileName + LEVEL_DATA_SUFFIX));
        _levelLayout = level.Grid;

        outlineRenderer.CreateMapWithGrid(_levelLayout);

        TileGeometryCreator geometryCreator = this.GetComponent<TileGeometryCreator>();
        if (geometryCreator != null)
            geometryCreator.CreateMapWithGrid(_levelLayout);
    }

    /**
     * Private
     */
    private int[,] _levelLayout;
}
