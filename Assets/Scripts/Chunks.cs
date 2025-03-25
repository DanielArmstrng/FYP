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
                        Vector3 pos = new Vector3(x, y, z);
                        bool[] visibleFaces = GetVisibleFaces(pos);
                        if (HasAnyVisibleFace(visibleFaces))
                        {
                            ChunkCube newCube = new ChunkCube(this, pos, chunkMap[x, y, z], visibleFaces);
                        }
                    }
                }

        GeneratePhysicalChunk();
    }

    private bool[] GetVisibleFaces(Vector3 pos)
    {
        // Order: front, back, top, bottom, right, left
        bool[] visibleFaces = new bool[6];
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        // Check each face
        visibleFaces[0] = !CubeCheck(new Vector3(x, y, z + 1)); // front
        visibleFaces[1] = !CubeCheck(new Vector3(x, y, z - 1)); // back
        visibleFaces[2] = !CubeCheck(new Vector3(x, y + 1, z)); // top
        visibleFaces[3] = !CubeCheck(new Vector3(x, y - 1, z)); // bottom
        visibleFaces[4] = !CubeCheck(new Vector3(x + 1, y, z)); // right
        visibleFaces[5] = !CubeCheck(new Vector3(x - 1, y, z)); // left

        return visibleFaces;
    }

    private bool HasAnyVisibleFace(bool[] visibleFaces)
    {
        for (int i = 0; i < visibleFaces.Length; i++)
        {
            if (visibleFaces[i]) return true;
        }
        return false;
    }

    public bool CubeCheck(Vector3 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        if (x < 0 || x >= World.chunkSize || y < 0 || y >= World.chunkSize || z < 0 || z >= World.chunkSize)
        {
            Vector3 neighborPos = chunkObject.transform.position;
            Vector3 checkPos = position;

            // Adjust the position to check the correct neighboring chunk
            if (x < 0)
            {
                neighborPos.x -= World.chunkSize;
                checkPos.x = World.chunkSize - 1;
            }
            else if (x >= World.chunkSize)
            {
                neighborPos.x += World.chunkSize;
                checkPos.x = 0;
            }

            if (y < 0)
            {
                neighborPos.y -= World.chunkSize;
                checkPos.y = World.chunkSize - 1;
            }
            else if (y >= World.chunkSize)
            {
                neighborPos.y += World.chunkSize;
                checkPos.y = 0;
            }

            if (z < 0)
            {
                neighborPos.z -= World.chunkSize;
                checkPos.z = World.chunkSize - 1;
            }
            else if (z >= World.chunkSize)
            {
                neighborPos.z += World.chunkSize;
                checkPos.z = 0;
            }

            Chunk neighbourChunk = World.GetChunk(neighborPos);

            if (neighbourChunk != null)
            {
                return neighbourChunk.chunkMap[(int)checkPos.x, (int)checkPos.y, (int)checkPos.z] == BlockDB.GetBlockName("Dirt");
            }
            else
            {
                // If we're checking above the chunk, treat it as air
                if (y >= World.chunkSize)
                {
                    return false;
                }
                // For other missing neighbors, treat as solid
                return true;
            }
        }
        else
        {
            return chunkMap[x, y, z] == BlockDB.GetBlockName("Dirt");
        }
    }

    //private bool IsFullySurrounded(Vector3 pos)
    //{
    //    // Only check for surrounding blocks if we're not at a chunk boundary
    //    bool isEdgeBlock = pos.x == 0 || pos.x == World.chunkSize - 1 ||
    //                      pos.y == 0 || pos.y == World.chunkSize - 1 ||
    //                      pos.z == 0 || pos.z == World.chunkSize - 1;

    //    if (isEdgeBlock)
    //    {
    //        // For edge blocks, we need to check if there's actually a neighboring chunk
    //        return CubeCheck(new Vector3(pos.x, pos.y, pos.z + 1)) &&
    //               CubeCheck(new Vector3(pos.x, pos.y, pos.z - 1)) &&
    //               CubeCheck(new Vector3(pos.x, pos.y + 1, pos.z)) &&
    //               CubeCheck(new Vector3(pos.x, pos.y - 1, pos.z)) &&
    //               CubeCheck(new Vector3(pos.x + 1, pos.y, pos.z)) &&
    //               CubeCheck(new Vector3(pos.x - 1, pos.y, pos.z));
    //    }
    //    else
    //    {
    //        // For internal blocks, we can use direct array access which is faster
    //        int x = (int)pos.x;
    //        int y = (int)pos.y;
    //        int z = (int)pos.z;
            
    //        return !chunkMap[x, y, z + 1].isTransparent &&
    //               !chunkMap[x, y, z - 1].isTransparent &&
    //               !chunkMap[x, y + 1, z].isTransparent &&
    //               !chunkMap[x, y - 1, z].isTransparent &&
    //               !chunkMap[x + 1, y, z].isTransparent &&
    //               !chunkMap[x - 1, y, z].isTransparent;
    //    }
    //}

    //int ConvertIndexToLocal(int i)
    //{
    //    if (i == -1)
    //        i = World.chunkSize - 1;
    //    else if (i == World.chunkSize)
    //        i = 0;
    //    return i;
    //}
}
