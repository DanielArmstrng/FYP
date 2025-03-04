using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    int[,,] chunkMap;

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> UVs = new List<Vector2>();

    //public static int chunkSize = 10;

    Mesh chunkMesh;
    GameObject chunkObject;
    public Material chunkMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //chunkMap = new int[chunkSize, chunkSize, chunkSize];

        //GenerateVirtualMap();

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

    public Chunk(Vector3 position, Material material)
    {
        chunkObject = new GameObject("Chunk");
        chunkObject.transform.position = position;
        chunkMaterial = material;

        MakeChunk();
    }

    void MakeChunk()
    {
        chunkMap = new int[World.chunkSize, World.chunkSize, World.chunkSize];

        GenerateVirtualMap();
    }

    void GeneratePhysicalChunk()
    {
        chunkMesh = new Mesh();

        MeshFilter mf = chunkObject.AddComponent<MeshFilter>();
        MeshRenderer mr = chunkObject.AddComponent<MeshRenderer>();

        chunkMesh.vertices = vertices.ToArray();
        chunkMesh.triangles = triangles.ToArray();
        chunkMesh.uv = UVs.ToArray();

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        mf.mesh = chunkMesh;
        mr.material = chunkMaterial;
    }

    void GenerateVirtualMap()
    {
        for (int x = 0; x < World.chunkSize; x++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int z = 0; z < World.chunkSize; z++)
                {
                    chunkMap[x, y, z] = 1;

                    //int offset = 5 * World.chunkSize;

                    //int worldX = (int)(chunkObject.transform.position.x + x);
                    //int worldY = (int)(chunkObject.transform.position.y + (y + offset));
                    //int worldZ = (int)(chunkObject.transform.position.z + z);

                    //if (worldY < Noise.GenerateHeight(worldX, worldZ))
                    //    chunkMap[x, y, z] = 1;
                }

        //GenerateBlocksMap();
    }

    public void GenerateBlocksMap()
    {
        for (int x = 0; x < World.chunkSize; x++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int z = 0; z < World.chunkSize; z++)
                {
                    if (chunkMap[x, y, z] == 1)
                    {
                        ChunkCube newCube = new ChunkCube(this, new Vector3(x, y, z));
                    }
                }

        GeneratePhysicalChunk();
    }

    public bool CubeCheck(Vector3 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        if (x < 0 || x >= World.chunkSize || y < 0 || y >= World.chunkSize || z < 0 || z >= World.chunkSize)
            return false;

        else if (chunkMap[x, y, z] == 0)
            return false;

        else             
            return true;
    }
}
