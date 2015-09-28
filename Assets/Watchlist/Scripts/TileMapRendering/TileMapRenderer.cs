﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileMapRenderer : VoBehavior
{
    public enum TilingMethod
    {
        CPU,
        GPU
    }

    public int Width = 100;
    public int Height = 50;
    public int TileRenderSize = 64;
    public int TileTextureSize = 64;
    public Texture2D Atlas = null;
    public TilingMethod Method = TilingMethod.CPU;
    public bool ClearAtlasCacheAfterCreate = true;

	void Start()
    {
        //this.CreateMap();
        Sprite[] sprites = Resources.LoadAll<Sprite>(this.Atlas.name);
        Debug.Log("sprites.Length = " + sprites.Length);
    }

    public void CreateEmptyMap()
    {
        this.CreateMapWithGrid(new int[this.Width, this.Height]);
    }

    public void CreateMapWithGrid(int [,] grid)
    {
        this.Clear();
        _tileAtlas = new TileTextureAtlas(this.Atlas, this.TileTextureSize, this.TileTextureSize);
        this.Width = grid.GetLength(0);
        this.Height = grid.GetLength(1);

        switch (this.Method)
        {
            default:
            case TilingMethod.CPU:
                createMapUsingMesh(grid);
                break;
            case TilingMethod.GPU:
                createMapUsingTexture(grid);
                break;
        }

        if (this.ClearAtlasCacheAfterCreate)
            _tileAtlas.ClearCachedTextureData();
    }

    public void Clear()
    {
        _tileAtlas = null;
        this.GetComponent<MeshFilter>().mesh = null;
        this.renderer.material.mainTexture = null;
    }

    public void SetSpriteIndicesForTiles(int[] x, int[] y, int[] spriteIndices)
    {
        switch (this.Method)
        {
            default:
            case TilingMethod.CPU:
                setTileSpriteIndicesInMesh(x, y, spriteIndices);
                break;
            case TilingMethod.GPU:
                setTileSpriteIndicesInTexture(x, y, spriteIndices);
                break;
        }
    }

    public void SetSpriteIndexForTile(int tileX, int tileY, int spriteIndex)
    {
        switch (this.Method)
        {
            default:
            case TilingMethod.CPU:
                setTileSpriteIndexInMesh(tileX, tileY, spriteIndex);
                break;
            case TilingMethod.GPU:
                setTileSpriteIndexInTexture(tileX, tileY, spriteIndex);
                break;
        }
    }


    /**
     * Private
     */
    private TileTextureAtlas _tileAtlas;

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
                int spriteIndex = grid[x, y];
                Vector2[] spriteUVs = _tileAtlas.GetUVsForSprite(spriteIndex);
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

    private void createMapUsingTexture(int[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        // Populate our own texture the size of the whole map
        Texture2D texture = new Texture2D(width * this.TileTextureSize, height * this.TileTextureSize);
        texture.filterMode = FilterMode.Point;
        
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                int spriteIndex = grid[x, y];
                texture.SetPixels(x * this.TileTextureSize, y * this.TileTextureSize, this.TileTextureSize, this.TileTextureSize, _tileAtlas.GetPixelsForSprite(spriteIndex));
            }
        }

        texture.Apply();
        
        // Create very simple (2 triangle) mesh to render texture in
        float originX = this.transform.position.x;
        float originY = this.transform.position.y;
        float originZ = this.transform.position.z;
        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[2 * 3];

        vertices[0] = new Vector3(originX, originY, originZ); // bottom left
        vertices[1] = new Vector3(originX + width * this.TileRenderSize, originY, originZ); // bottom right
        vertices[2] = new Vector3(originX, originY + height * this.TileRenderSize, originZ); // top left
        vertices[3] = new Vector3(vertices[1].x, vertices[2].y, originZ); // top right
        normals[0] = Vector3.up;
        normals[1] = Vector3.up;
        normals[2] = Vector3.up;
        normals[3] = Vector3.up;
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(0, 1);
        uvs[3] = new Vector2(1, 1);

        triangles[0] = 2;
        triangles[1] = 1;
        triangles[2] = 0;

        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        // Populate a mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        // Assign mesh to behaviors
        this.GetComponent<MeshFilter>().mesh = mesh;
        this.renderer.material.mainTexture = texture;
    }

    private void setTileSpriteIndexInMesh(int tileX, int tileY, int spriteIndex)
    {
        int tileIndex = tileY * this.Width + tileX;
        int startingUVIndex = tileIndex * 4;
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        Vector2[] spriteUVs = _tileAtlas.GetUVsForSprite(spriteIndex);
        Vector2[] uvs = meshFilter.mesh.uv;
        uvs[startingUVIndex] = spriteUVs[0]; // bottom left
        uvs[startingUVIndex + 1] = spriteUVs[1]; // bottom right
        uvs[startingUVIndex + 2] = spriteUVs[2]; // top left
        uvs[startingUVIndex + 3] = spriteUVs[3]; // top right
        
        meshFilter.mesh.uv = uvs;
    }

    private void setTileSpriteIndexInTexture(int tileX, int tileY, int spriteIndex)
    {
        Texture2D texture = (Texture2D)this.renderer.material.mainTexture;
        texture.SetPixels(tileX * this.TileTextureSize, tileY * this.TileTextureSize, this.TileTextureSize, this.TileTextureSize, _tileAtlas.GetPixelsForSprite(spriteIndex));
        texture.Apply();
    }


    private void setTileSpriteIndicesInMesh(int[] x, int[] y, int[] spriteIndices)
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        Vector2[] uvs = meshFilter.mesh.uv;

        for (int i = 0; i < x.Length; ++i)
        {
            int tileIndex = y[i] * this.Width + x[i];
            int startingUVIndex = tileIndex * 4;

            Vector2[] spriteUVs = _tileAtlas.GetUVsForSprite(spriteIndices[i]);
            uvs[startingUVIndex] = spriteUVs[0]; // bottom left
            uvs[startingUVIndex + 1] = spriteUVs[1]; // bottom right
            uvs[startingUVIndex + 2] = spriteUVs[2]; // top left
            uvs[startingUVIndex + 3] = spriteUVs[3]; // top right
        }
        meshFilter.mesh.uv = uvs;
    }

    private void setTileSpriteIndicesInTexture(int[] x, int[] y, int[] spriteIndices)
    {
        Texture2D texture = (Texture2D)this.renderer.material.mainTexture;

        for (int i = 0; i < x.Length; ++i)
        {
            texture.SetPixels(x[i] * this.TileTextureSize, y[i] * this.TileTextureSize, this.TileTextureSize, this.TileTextureSize, _tileAtlas.GetPixelsForSprite(spriteIndices[i]));
        }

        texture.Apply();
    }
}
