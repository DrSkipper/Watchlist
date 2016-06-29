using UnityEngine;
using System.IO;

[RequireComponent(typeof(TileMapOutlineRenderer))]
[ExecuteInEditMode]
public class TestLevelLayout : VoBehavior
{
    public string LevelFileName;

    public const string LEVEL_DATA_ROOT = "Levels/";

    void Start()
    {
        this.CreateGeometry();
    }

    public void CreateGeometry()
    {
        TileMapOutlineRenderer outlineRenderer = this.GetComponent<TileMapOutlineRenderer>();
        outlineRenderer.Clear();

        LevelData level = LevelData.Load(Resources.Load<TextAsset>(LEVEL_DATA_ROOT + LevelFileName));
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
