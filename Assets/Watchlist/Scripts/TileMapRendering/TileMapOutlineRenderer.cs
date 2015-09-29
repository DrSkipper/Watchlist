﻿using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class TileMapOutlineRenderer : VoBehavior
{
    public enum TilingMethod
    {
        CPU,
        GPU
    }

    public int Width = 100;
    public int Height = 50;
    public int TileRenderSize = 64;
    public bool OffMapIsFilled = true;
    public bool OffsetTilesToCenter = true;
    public Texture2D Atlas = null;

	void Start()
    {
        //this.CreateMap();
    }

    public void CreateEmptyMap()
    {
        this.CreateMapWithGrid(new int[this.Width, this.Height]);
    }

    public void CreateMapWithGrid(int [,] grid)
    {
        this.Clear();
        
        _sprites = this.Atlas.GetSprites();
        this.Width = grid.GetLength(0);
        this.Height = grid.GetLength(1);

        createMapUsingMesh(grid);

        if (this.OffsetTilesToCenter)
        {
            this.transform.position = new Vector3(this.transform.position.x - this.TileRenderSize * this.Width / 2, this.transform.position.y - this.TileRenderSize * this.Height / 2, this.transform.position.z);
        }
    }

    public void Clear()
    {
        _sprites = null;
        this.GetComponent<MeshFilter>().mesh = null;
        
        if (this.OffsetTilesToCenter)
            this.transform.position = new Vector3(0, 0, this.transform.position.z);
    }


    /**
     * Private
     */
    private Dictionary<string, Sprite> _sprites;

    private void createMapUsingMesh(int[,] grid)
    {
        float originX = this.transform.position.x;
        float originY = this.transform.position.y;
        float originZ = this.transform.position.z;

        int numTiles = this.Width * this.Height;
        int numTriangles = numTiles * 2;
        
        // Generate mesh data
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        int[] triangles = new int[numTriangles * 3]; // Clockwise order of vertices within triangles (for correct render direction)

        for (int y = 0; y < this.Height; ++y)
        {
            for (int x = 0; x < this.Width; ++x)
            {
                int tileIndex = this.Width * y + x;
                int triangleIndex = tileIndex * 2 * 3;

                // Create 4 verts
                Vector3 bottomLeft = new Vector3(originX + x * this.TileRenderSize, originY + y * this.TileRenderSize, originZ);
                Vector3 bottomRight = new Vector3(bottomLeft.x + this.TileRenderSize, bottomLeft.y, originZ);
                Vector3 topLeft = new Vector3(bottomLeft.x, bottomLeft.y + this.TileRenderSize, originZ);
                Vector3 topRight = new Vector3(bottomRight.x, topLeft.y, originZ);

                // Indices of verts
                int bottomLeftVert = vertices.Count;
                int bottomRightVert = bottomLeftVert + 1;
                int topLeftVert = bottomRightVert + 1;
                int topRightVert = topLeftVert + 1;

                // Assign vert indices to triangles
                triangles[triangleIndex] = topLeftVert;
                triangles[triangleIndex + 1] = bottomRightVert;
                triangles[triangleIndex + 2] = bottomLeftVert;

                triangles[triangleIndex + 3] = topLeftVert;
                triangles[triangleIndex + 4] = topRightVert;
                triangles[triangleIndex + 5] = bottomRightVert;

                // Handle UVs
                int[,] neighbors = getNeighbors(grid, x, y);
                Vector2[] spriteUVs = getUVsForSprite(neighbors);

                Vector2 bottomLeftUV = spriteUVs[0];
                Vector2 bottomRightUV = spriteUVs[1];
                Vector2 topLeftUV = spriteUVs[2];
                Vector2 topRightUV = spriteUVs[3];

                // Add vertices and vertex data to mesh data
                vertices.Add(bottomLeft);
                vertices.Add(bottomRight);
                vertices.Add(topLeft);
                vertices.Add(topRight);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                uvs.Add(bottomLeftUV);
                uvs.Add(bottomRightUV);
                uvs.Add(topLeftUV);
                uvs.Add(topRightUV);
            }
        }

        // Populate a mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles;

        // Assign mesh to behaviors
        this.GetComponent<MeshFilter>().mesh = mesh;
        this.renderer.material.mainTexture = this.Atlas;
    }

    private int[,] getNeighbors(int[,] grid, int x, int y)
    {
        int[,] neighbors = new int[3, 3];
        int offscreenValue = this.OffMapIsFilled ? 1 : 0;

        int maxX = grid.GetLength(0) - 1;
        int maxY = grid.GetLength(1) - 1;

        neighbors[0, 0] = x > 0 && y > 0 ? grid[x - 1, y - 1] : offscreenValue; // bottom left
        neighbors[1, 0] = y > 0 ? grid[x, y - 1] : offscreenValue; // bottom
        neighbors[2, 0] = x < maxX && y > 0 ? grid[x + 1, y - 1] : offscreenValue; // bottom right
        neighbors[0, 1] = x > 0 ? grid[x - 1, y] : offscreenValue; // left
        neighbors[1, 1] = grid[x, y]; // center
        neighbors[2, 1] = x < maxX ? grid[x + 1, y] : offscreenValue; // right
        neighbors[0, 2] = x > 0 && y < maxY ? grid[x - 1, y + 1] : offscreenValue; // top left
        neighbors[1, 2] = y < maxY ? grid[x, y + 1] : offscreenValue; // top
        neighbors[2, 2] = x < maxX && y < maxY ? grid[x + 1, y + 1] : offscreenValue; // top right

        return neighbors;
    }

    private Vector2[] getUVsForSprite(int[,] neighbors)
    {
        // Empty tile
        if (neighbors[1, 1] == 0)
            return _sprites["empty"].uv;

        bool bottom = neighbors[1, 0] != 0;
        bool left = neighbors[0, 1] != 0;
        bool top = neighbors[1, 2] != 0;
        bool right = neighbors[2, 1] != 0;
        bool anyCornerEmpty = neighbors[0, 0] == 0 || neighbors[0, 2] == 0 || neighbors[2, 0] == 0 || neighbors[2, 2] == 0;

        // Completely surrounded
        if (!anyCornerEmpty && bottom && left && top && right)
            return _sprites["filled"].uv;
        
        // Cross intersection
        if (anyCornerEmpty && bottom && top && left && right)
        {
            if ((neighbors[0, 0] == 0 && neighbors[2, 2] == 0) || (neighbors[2, 0] == 0 && neighbors[0, 2] == 0))
                return _sprites["cross"].uv;
        }

        // Corners and T-intersections
        if (bottom && left)
        {
            if (!top && !right && neighbors[0, 0] != 0)
            {
                return _sprites["corner_bottom_right"].uv;
            }
            else if (!top && right && (neighbors[0, 0] == 0 || neighbors[2, 0] == 0))
            {
                return _sprites["t_up"].uv;
            }
            else if (!right && top && (neighbors[0, 0] == 0 || neighbors[0, 2] == 0))
            {
                return _sprites["t_left"].uv;
            }
            else
            {
                if (right && neighbors[0, 0] == 0 && neighbors[2, 0] == 0)
                    return _sprites["t_up"].uv;
                if (top && neighbors[0, 0] == 0 && neighbors[0, 2] == 0)
                    return _sprites["t_left"].uv;
                if (neighbors[0, 0] == 0)
                    return _sprites["corner_bottom_right"].uv;
            }
        }

        if (bottom && right)
        {
            if (!top && !left && neighbors[2, 0] != 0)
            {
                return _sprites["corner_bottom_left"].uv;
            }
            else if (!top && left && (neighbors[0, 0] == 0 || neighbors[2, 0] == 0))
            {
                return _sprites["t_up"].uv;
            }
            else if (!left && top && (neighbors[2, 0] == 0 || neighbors[2, 2] == 0))
            {
                return _sprites["t_right"].uv;
            }
            else
            {
                if (left && neighbors[0, 0] == 0 && neighbors[2, 0] == 0)
                    return _sprites["t_up"].uv;
                if (top && neighbors[2, 0] == 0 && neighbors[2, 2] == 0)
                    return _sprites["t_right"].uv;
                if (neighbors[2, 0] == 0)
                    return _sprites["corner_bottom_left"].uv;
            }
        }
        if (top && left)
        {
            if (!bottom && !right && neighbors[0, 2] != 0)
            {
                return _sprites["corner_top_right"].uv;
            }
            else if (!bottom && right && (neighbors[0, 2] == 0 || neighbors[2, 2] == 0))
            {
                return _sprites["t_down"].uv;
            }
            else if (!right && bottom && (neighbors[0, 0] == 0 || neighbors[0, 2] == 0))
            {
                return _sprites["t_left"].uv;
            }
            else
            {
                if (right && neighbors[0, 2] == 0 && neighbors[2, 2] == 0)
                    return _sprites["t_down"].uv;
                if (bottom && neighbors[0, 0] == 0 && neighbors[0, 2] == 0)
                    return _sprites["t_left"].uv;
                if (neighbors[0, 2] == 0)
                    return _sprites["corner_top_right"].uv;
            }
        }
        if (top && right)
        {
            if (!bottom && !left && neighbors[2, 2] != 0)
            {
                return _sprites["corner_top_left"].uv;
            }
            else if (!bottom && left && (neighbors[0, 2] == 0 || neighbors[2, 2] == 0))
            {
                return _sprites["t_down"].uv;
            }
            else if (!left && bottom && (neighbors[2, 0] == 0 || neighbors[2, 2] == 0))
            {
                return _sprites["t_right"].uv;
            }
            else
            {
                if (left && neighbors[0, 2] == 0 && neighbors[2, 2] == 0)
                    return _sprites["t_down"].uv;
                if (bottom && neighbors[2, 0] == 0 && neighbors[2, 2] == 0)
                    return _sprites["t_right"].uv;
                if (neighbors[2, 2] == 0)
                    return _sprites["corner_top_left"].uv;
            }
        }
        
        // Sides
        if (bottom && top)
            return _sprites["side_vertical"].uv;
        if (left && right)
            return _sprites["side_horizontal"].uv;

        // Tips
        if (bottom && !top && !left && !right)
            return _sprites["tip_bottom"].uv;
        if (!bottom && top && !left && !right)
            return _sprites["tip_top"].uv;
        if (!bottom && !top && left && !right)
            return _sprites["tip_right"].uv;
        if (!bottom && !top && !left && right)
            return _sprites["tip_left"].uv;

        // Lone tile
        //if (!bottom && !left && !top && !right)
        return _sprites["lone"].uv;
    }
}
