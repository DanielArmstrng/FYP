using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    int[,,] chunkMap;

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> UVs = new List<Vector2>();

    public static int chunkSize = 10;

    Mesh chunkMesh;

    public Material chunkMaterial;

    // Start is called before the first frame update
    void Start()
    {
        chunkMap = new int[chunkSize, chunkSize, chunkSize];

        GenerateVirtualMap();

        //for (int x = 0; x < 5; x++)
        //{
        //    ChunkCube newCube = new ChunkCube(this, new Vector3(x, 0 , 0));
        //}

        //GeneratePhysicalChunk();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GeneratePhysicalChunk()
    {
        chunkMesh = new Mesh();

        MeshFilter mf = GetComponent<MeshFilter>();

        chunkMesh.vertices = vertices.ToArray();
        chunkMesh.triangles = triangles.ToArray();
        chunkMesh.uv = UVs.ToArray();

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        mf.mesh = chunkMesh;
    }

    void GenerateVirtualMap()
    {
        for (int x = 0; x < chunkSize; x++)
            for (int y = 0; y < chunkSize; y++)
                for (int z = 0; z < chunkSize; z++)
                {
                    chunkMap[x, y, z] = 1;
                }

        GenerateBlocksMap();
    }

    void GenerateBlocksMap()
    {
        for (int x = 0; x < chunkSize; x++)
            for (int y = 0; y < chunkSize; y++)
                for (int z = 0; z < chunkSize; z++)
                {
                    if (chunkMap[x, y, z] == 1)
                    {
                        ChunkCube newCube = new ChunkCube(this, new Vector3(x, y, z));
                    }
                }

        GeneratePhysicalChunk();
    }
}
