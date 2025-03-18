using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    Blocks[,,] chunkMap;

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
        chunkMap = new Blocks[World.chunkSize, World.chunkSize, World.chunkSize];

        GenerateVirtualMap();
    }

    void GeneratePhysicalChunk()
    {
        chunkMesh = new Mesh();

        MeshFilter mf = chunkObject.AddComponent<MeshFilter>();
        MeshRenderer mr = chunkObject.AddComponent<MeshRenderer>();
        MeshCollider mc = chunkObject.AddComponent<MeshCollider>();

        chunkMesh.vertices = vertices.ToArray();
        chunkMesh.triangles = triangles.ToArray();
        chunkMesh.uv = UVs.ToArray();

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        mf.mesh = chunkMesh;
        mr.material = chunkMaterial;
        mc.sharedMesh = chunkMesh;

        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;
    }

    void GenerateVirtualMap()
    {
        for (int x = 0; x < World.chunkSize; x++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int z = 0; z < World.chunkSize; z++)
                {
                    if (y == World.chunkSize - 1)
                    {
                        Blocks blockToCreate = BlockDB.GetBlockName("Dirt");
                        chunkMap[x, y, z] = blockToCreate;
                    }
                    else
                    {
                        Blocks blockToCreate = BlockDB.GetBlockName("Air");
                        chunkMap[x, y, z] = blockToCreate;
                    }


                    int offset = 5 * World.chunkSize;

                    int worldX = (int)(chunkObject.transform.position.x + x);
                    int worldY = (int)(chunkObject.transform.position.y + (y + offset));
                    int worldZ = (int)(chunkObject.transform.position.z + z);

                    if (worldY < Noise.GenerateHeight(worldX, worldZ))
                    {
                        Blocks dirtBlock = BlockDB.GetBlockName("Dirt");
                        chunkMap[x, y, z] = dirtBlock;
                    }
                    else
                    {
                        Blocks airBlock = BlockDB.GetBlockName("Air");
                        chunkMap[x, y, z] = airBlock;
                    }

                }

        //GenerateBlocksMap();
    }

    public void GenerateBlocksMap()
    {
        for (int x = 0; x < World.chunkSize; x++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int z = 0; z < World.chunkSize; z++)
                {
                    if (chunkMap[x, y, z] != BlockDB.GetBlockName("Air"))
                    {
                        if (!IsFullySurrounded(new Vector3(x, y, z)))
                        {
                            ChunkCube newCube = new ChunkCube(this, new Vector3(x, y, z), chunkMap[x, y, z]);
                        }
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
        {
            Chunk neighbourChunk = World.GetChunk(chunkObject.transform.position + position);

            x = ConvertIndexToLocal(x);
            y = ConvertIndexToLocal(y);
            z = ConvertIndexToLocal(z);

            if (neighbourChunk != null)
                return neighbourChunk.CubeCheck(new Vector3(x, y, z));
            else
                return false;
        }

        else if (chunkMap[x, y, z].isTransparent)
            return false;

        else             
            return true;
    }

    private bool IsFullySurrounded(Vector3 pos)
    {
        return CubeCheck(new Vector3(pos.x, pos.y, pos.z + 1)) &&
               CubeCheck(new Vector3(pos.x, pos.y, pos.z - 1)) &&
               CubeCheck(new Vector3(pos.x, pos.y + 1, pos.z)) &&
               CubeCheck(new Vector3(pos.x, pos.y - 1, pos.z)) &&
               CubeCheck(new Vector3(pos.x + 1, pos.y, pos.z)) &&
               CubeCheck(new Vector3(pos.x - 1, pos.y, pos.z));
    }

    int ConvertIndexToLocal(int i)
    {
        if (i == -1)
            i = World.chunkSize - 1;
        else if (i == World.chunkSize)
            i = 0;
        return i;
    }
}
