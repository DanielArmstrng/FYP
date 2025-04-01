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

        // Order: front, back, top, bottom, right, left
        for (int i = 0; i < visibleFaces.Length; i++)
        {
            if (visibleFaces[i])
            {
                CreateCubeSide(owner, position, blockType, (CubeSide)i);
            }
        }
    }

    private enum CubeSide
    {
        Front,
        Back,
        Top,
        Bottom,
        Right,
        Left
    }

    void CreateCubeSide(Chunk owner, Vector3 position, Blocks blockType, CubeSide side)
    {
        // Add triangles based on side
        if (side == CubeSide.Back || side == CubeSide.Bottom || side == CubeSide.Left)
        {
            owner.triangles.Add(0 + owner.vertices.Count);
            owner.triangles.Add(2 + owner.vertices.Count);
            owner.triangles.Add(1 + owner.vertices.Count);
            owner.triangles.Add(0 + owner.vertices.Count);
            owner.triangles.Add(3 + owner.vertices.Count);
            owner.triangles.Add(2 + owner.vertices.Count);
        }
        else
        {
            owner.triangles.Add(0 + owner.vertices.Count);
            owner.triangles.Add(1 + owner.vertices.Count);
            owner.triangles.Add(2 + owner.vertices.Count);
            owner.triangles.Add(0 + owner.vertices.Count);
            owner.triangles.Add(2 + owner.vertices.Count);
            owner.triangles.Add(3 + owner.vertices.Count);
        }

        Vector3 v0, v1, v2, v3;
        //Vector2 uv0, uv1, uv2, uv3;

        switch (side)
        {
            case CubeSide.Front:
                v0 = new Vector3(0.5f, -0.5f, 0.5f);
                v1 = new Vector3(0.5f, 0.5f, 0.5f);
                v2 = new Vector3(-0.5f, 0.5f, 0.5f);
                v3 = new Vector3(-0.5f, -0.5f, 0.5f);
                //uv0 = new Vector2(1, 0);
                //uv1 = new Vector2(1, 1);
                //uv2 = new Vector2(0, 1);
                //uv3 = new Vector2(0, 0);
                break;

            case CubeSide.Back:
                v0 = new Vector3(0.5f, -0.5f, -0.5f);
                v1 = new Vector3(0.5f, 0.5f, -0.5f);
                v2 = new Vector3(-0.5f, 0.5f, -0.5f);
                v3 = new Vector3(-0.5f, -0.5f, -0.5f);
                //uv0 = new Vector2(0, 0);
                //uv1 = new Vector2(0, 1);
                //uv2 = new Vector2(1, 1);
                //uv3 = new Vector2(1, 0);
                break;

            case CubeSide.Top:
                v0 = new Vector3(0.5f, 0.5f, 0.5f);
                v1 = new Vector3(0.5f, 0.5f, -0.5f);
                v2 = new Vector3(-0.5f, 0.5f, -0.5f);
                v3 = new Vector3(-0.5f, 0.5f, 0.5f);
                //uv0 = new Vector2(1, 1);
                //uv1 = new Vector2(1, 0);
                //uv2 = new Vector2(0, 0);
                //uv3 = new Vector2(0, 1);
                break;

            case CubeSide.Bottom:
                v0 = new Vector3(0.5f, -0.5f, 0.5f);
                v1 = new Vector3(0.5f, -0.5f, -0.5f);
                v2 = new Vector3(-0.5f, -0.5f, -0.5f);
                v3 = new Vector3(-0.5f, -0.5f, 0.5f);
                //uv0 = new Vector2(1, 1);
                //uv1 = new Vector2(1, 0);
                //uv2 = new Vector2(0, 0);
                //uv3 = new Vector2(0, 1);
                break;

            case CubeSide.Right:
                v0 = new Vector3(0.5f, -0.5f, -0.5f);
                v1 = new Vector3(0.5f, 0.5f, -0.5f);
                v2 = new Vector3(0.5f, 0.5f, 0.5f);
                v3 = new Vector3(0.5f, -0.5f, 0.5f);
                //uv0 = new Vector2(1, 0);
                //uv1 = new Vector2(1, 1);
                //uv2 = new Vector2(0, 1);
                //uv3 = new Vector2(0, 0);
                break;

            case CubeSide.Left:
                v0 = new Vector3(-0.5f, -0.5f, -0.5f);
                v1 = new Vector3(-0.5f, 0.5f, -0.5f);
                v2 = new Vector3(-0.5f, 0.5f, 0.5f);
                v3 = new Vector3(-0.5f, -0.5f, 0.5f);
                //uv0 = new Vector2(0, 0);
                //uv1 = new Vector2(0, 1);
                //uv2 = new Vector2(1, 1);
                //uv3 = new Vector2(1, 0);
                break;

            default:
                throw new System.ArgumentException("Invalid cube side");
        }

        owner.vertices.Add(v0 + position);
        owner.vertices.Add(v1 + position);
        owner.vertices.Add(v2 + position);
        owner.vertices.Add(v3 + position);

        float textureOffset = 1f / 2f;
        Vector2 texturePos;

        if(block.multiTexture)
        {
            if (side == CubeSide.Top)
                texturePos = block.textureUp;
            else if (side == CubeSide.Bottom)
                texturePos = block.textureDown;
            else
                texturePos = block.textureSide;
        }
        else
        {
            texturePos = block.texture;
        }

        owner.UVs.Add(new Vector2((textureOffset * texturePos.x) + textureOffset, textureOffset * texturePos.y));
        owner.UVs.Add(new Vector2((textureOffset * texturePos.x) + textureOffset, (textureOffset * texturePos.y) + textureOffset));
        owner.UVs.Add(new Vector2(textureOffset * texturePos.x, (textureOffset * texturePos.y) + textureOffset));
        owner.UVs.Add(new Vector2(textureOffset * texturePos.x, textureOffset * texturePos.y));
    }
}
