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

    public ChunkCube(Chunk own, Vector3 pos, Blocks blocks)
    {
        owner = own;
        position = pos;
        block = blocks;

        GenerateCube();
    }

    void GenerateCube()
    {
        if(owner.CubeCheck(new Vector3(position.x, position.y, position.z + 1)) == false)  
            CreateCubeSide("front");
        if (owner.CubeCheck(new Vector3(position.x, position.y, position.z - 1)) == false)
            CreateCubeSide("back");
        if (owner.CubeCheck(new Vector3(position.x, position.y + 1, position.z)) == false)
            CreateCubeSide("top");
        if (owner.CubeCheck(new Vector3(position.x, position.y - 1, position.z)) == false)
            CreateCubeSide("bottom");
        if (owner.CubeCheck(new Vector3(position.x + 1, position.y, position.z)) == false)
            CreateCubeSide("left");
        if (owner.CubeCheck(new Vector3(position.x - 1, position.y, position.z)) == false)
            CreateCubeSide("right");
    }

    void CreateCubeSide(string side)
    {
        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(1 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);

        owner.triangles.Add(0 + owner.vertices.Count);
        owner.triangles.Add(2 + owner.vertices.Count);
        owner.triangles.Add(3 + owner.vertices.Count);

        //Front side
        Vector3 v0 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v2 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, 0.5f);

        //Back side
        Vector3 v4 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v5 = new Vector3(-0.5f, 0.5f, -0.5f);
        Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v7 = new Vector3(0.5f, -0.5f, -0.5f);

        owner.UVs.Add(new Vector2(1, 1));
        owner.UVs.Add(new Vector2(0, 1));
        owner.UVs.Add(new Vector2(0, 0));
        owner.UVs.Add(new Vector2(1, 0));

        switch (side)
        {
            case "front":
                owner.vertices.Add(v0 + position);
                owner.vertices.Add(v1 + position);
                owner.vertices.Add(v2 + position);
                owner.vertices.Add(v3 + position);
                break;
            case "back":
                owner.vertices.Add(v4 + position);
                owner.vertices.Add(v5 + position);
                owner.vertices.Add(v6 + position);
                owner.vertices.Add(v7 + position);
                break;
            case "top":
                owner.vertices.Add(v1 + position);
                owner.vertices.Add(v6 + position);
                owner.vertices.Add(v5 + position);
                owner.vertices.Add(v2 + position);
                break;
            case "bottom":
                owner.vertices.Add(v3 + position);
                owner.vertices.Add(v4 + position);
                owner.vertices.Add(v7 + position);
                owner.vertices.Add(v0 + position);
                break;
            case "left":
                owner.vertices.Add(v7 + position);
                owner.vertices.Add(v6 + position);
                owner.vertices.Add(v1 + position);
                owner.vertices.Add(v0 + position);
                break;
            case "right":
                owner.vertices.Add(v3 + position);
                owner.vertices.Add(v2 + position);
                owner.vertices.Add(v5 + position);
                owner.vertices.Add(v4 + position);
                break;
        }
    }

}
