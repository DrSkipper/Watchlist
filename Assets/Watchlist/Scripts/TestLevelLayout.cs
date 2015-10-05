using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapOutlineRenderer))]
[ExecuteInEditMode]
public class TestLevelLayout : VoBehavior
{
    public Vector2 Size;
    public Vector2[] Walls;

    void Start()
    {
        this.CreateGeometry();
    }

    public void CreateGeometry()
    {
        TileMapOutlineRenderer outlineRenderer = this.GetComponent<TileMapOutlineRenderer>();
        outlineRenderer.Clear();

        _levelLayout = new int[Size.IntX(), Size.IntY()];

        for (int x = 0; x < Size.IntX(); ++x)
            for (int y = 0; y < Size.IntY(); ++y)
                _levelLayout[x, y] = 0;

        foreach (Vector2 wall in this.Walls)
        {
            _levelLayout[wall.IntX(), wall.IntY()] = 1;
        }

        outlineRenderer.CreateMapWithGrid(_levelLayout);
        this.GetComponent<TileGeometryCreator>().CreateMapWithGrid(_levelLayout);
    }

    /**
     * Private
     */
    private int[,] _levelLayout;
}
