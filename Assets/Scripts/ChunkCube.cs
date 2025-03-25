using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChunkCube
{
    public Chunk owner;
    public Vector3 position;
    Blocks block;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ChunkCube(Chunk owner, Vector3 position, Blocks blockType, bool[] visibleFaces)
    {
        this.owner = owner;
        this.position = position;
        this.block = blockType;

        // Only create the faces that are marked as visible
        if (visibleFaces[0]) CreateFront(owner, position, blockType);
        if (visibleFaces[1]) CreateBack(owner, position, blockType);
        if (visibleFaces[2]) CreateTop(owner, position, blockType);
        if (visibleFaces[3]) CreateBottom(owner, position, blockType);
        if (visibleFaces[4]) CreateRight(owner, position, blockType);
        if (visibleFaces[5]) CreateLeft(owner, position, blockType);
    }

    void CreateFront(Chunk owner, Vector3 position, Blocks blockType)
    {
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(1 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(3 + owner.vertices.Count);

        Vector3 v0 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v2 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, 0.5f);

        owner.vertices.Add(v0 + position);
        owner.vertices.Add(v1 + position);
        owner.vertices.Add(v2 + position);
        owner.vertices.Add(v3 + position);

        owner.UVs.Add(new Vector2(1, 0));
        owner.UVs.Add(new Vector2(1, 1));
        owner.UVs.Add(new Vector2(0, 1));
        owner.UVs.Add(new Vector2(0, 0));
    }

    void CreateBack(Chunk owner, Vector3 position, Blocks blockType)
    {
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(1 + owner.vertices.Count);
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(3 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);

        Vector3 v0 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 v1 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v2 = new Vector3(-0.5f, 0.5f, -0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);

        owner.vertices.Add(v0 + position);
        owner.vertices.Add(v1 + position);
        owner.vertices.Add(v2 + position);
        owner.vertices.Add(v3 + position);

        owner.UVs.Add(new Vector2(0, 0));
        owner.UVs.Add(new Vector2(0, 1));
        owner.UVs.Add(new Vector2(1, 1));
        owner.UVs.Add(new Vector2(1, 0));
    }

    void CreateTop(Chunk owner, Vector3 position, Blocks blockType)
    {
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(1 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(3 + owner.vertices.Count);

        Vector3 v0 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v2 = new Vector3(-0.5f, 0.5f, -0.5f);
        Vector3 v3 = new Vector3(-0.5f, 0.5f, 0.5f);

        owner.vertices.Add(v0 + position);
        owner.vertices.Add(v1 + position);
        owner.vertices.Add(v2 + position);
        owner.vertices.Add(v3 + position);

        owner.UVs.Add(new Vector2(1, 1));
        owner.UVs.Add(new Vector2(1, 0));
        owner.UVs.Add(new Vector2(0, 0));
        owner.UVs.Add(new Vector2(0, 1));
    }

    void CreateBottom(Chunk owner, Vector3 position, Blocks blockType)
    {
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(1 + owner.vertices.Count);
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(3 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);

        Vector3 v0 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 v2 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, 0.5f);

        owner.vertices.Add(v0 + position);
        owner.vertices.Add(v1 + position);
        owner.vertices.Add(v2 + position);
        owner.vertices.Add(v3 + position);

        owner.UVs.Add(new Vector2(1, 1));
        owner.UVs.Add(new Vector2(1, 0));
        owner.UVs.Add(new Vector2(0, 0));
        owner.UVs.Add(new Vector2(0, 1));
    }

    void CreateRight(Chunk owner, Vector3 position, Blocks blockType)
    {
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(1 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(3 + owner.vertices.Count);

        Vector3 v0 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 v1 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v2 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v3 = new Vector3(0.5f, -0.5f, 0.5f);

        owner.vertices.Add(v0 + position);
        owner.vertices.Add(v1 + position);
        owner.vertices.Add(v2 + position);
        owner.vertices.Add(v3 + position);

        owner.UVs.Add(new Vector2(1, 0));
        owner.UVs.Add(new Vector2(1, 1));
        owner.UVs.Add(new Vector2(0, 1));
        owner.UVs.Add(new Vector2(0, 0));
    }

    void CreateLeft(Chunk owner, Vector3 position, Blocks blockType)
    {
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(1 + owner.vertices.Count);
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(3 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);

        Vector3 v0 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v1 = new Vector3(-0.5f, 0.5f, -0.5f);
        Vector3 v2 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, 0.5f);

        owner.vertices.Add(v0 + position);
        owner.vertices.Add(v1 + position);
        owner.vertices.Add(v2 + position);
        owner.vertices.Add(v3 + position);

        owner.UVs.Add(new Vector2(0, 0));
        owner.UVs.Add(new Vector2(0, 1));
        owner.UVs.Add(new Vector2(1, 1));
        owner.UVs.Add(new Vector2(1, 0));
    }
}
