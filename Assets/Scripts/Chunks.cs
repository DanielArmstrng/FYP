using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;


public class Chunk
{
    //3D array stores all blocks in the chunk
    public Blocks[,,] chunkMap;

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> UVs = new List<Vector2>();

    Mesh chunkMesh;
    public GameObject chunkObject;
    public Material chunkMaterial;

    // Start is called before the first frame update
    void Start()
    {
     
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

    //Initializes the block layout of the hcunk
    void MakeChunk()
    {
        chunkMap = new Blocks[World.chunkSize, World.chunkSize, World.chunkSize];

        GenerateVirtualMap();
    }

    //Creates the physical mesh for the chunk
    public void GeneratePhysicalChunk()
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

        //mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //mr.receiveShadows = false;
    }

    //Generates the virtual layout of the chunk
    void GenerateVirtualMap()
    {
        for (int x = 0; x < World.chunkSize; x++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int z = 0; z < World.chunkSize; z++)
                {
                    int offset = 2 * World.chunkSize;

                    //Calculate the world position of the block
                    int worldX = (int)(chunkObject.transform.position.x + x);
                    int worldY = (int)(chunkObject.transform.position.y + (y + offset));
                    //int worldY = (int)(chunkObject.transform.position.y + y);
                    int worldZ = (int)(chunkObject.transform.position.z + z);

                    //Generates the different block layers depending on the height
                    if (worldY <= Noise.GenerateStoneHeight(worldX, worldZ))
                    {
                        Blocks stoneBlock = BlockDB.GetBlockName("Stone");
                        chunkMap[x, y, z] = stoneBlock;
                    }

                    else if (worldY < Noise.GenerateHeight(worldX, worldZ))
                    {
                        Blocks dirtBlock = BlockDB.GetBlockName("Dirt");
                        chunkMap[x, y, z] = dirtBlock;
                    }

                    else if (worldY == Noise.GenerateHeight(worldX, worldZ))
                    {
                        Blocks grassBlock = BlockDB.GetBlockName("Grass");
                        chunkMap[x, y, z] = grassBlock;
                    }

                    else
                    {
                        Blocks airBlock = BlockDB.GetBlockName("Air");
                        chunkMap[x, y, z] = airBlock;
                    }

                    if (worldY - offset == 0)
                    {
                        Blocks bedrockBlock = BlockDB.GetBlockName("Bedrock");
                        chunkMap[x, y, z] = bedrockBlock;
                    }
                }

        //GenerateBlocksMap();
    }

    //Generates the mesh for all block that are visible
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

    //Checks which faces of the block should be visible
    private bool[] GetVisibleFaces(Vector3 pos)
    {
        bool[] visibleFaces = new bool[6];
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;

        int offset = 2 * World.chunkSize;
        int worldX = (int)(chunkObject.transform.position.x + x);
        int worldY = (int)(chunkObject.transform.position.y + (y + offset));
        //int worldY = (int)(chunkObject.transform.position.y + y);
        int worldZ = (int)(chunkObject.transform.position.z + z);

        // If the block is at the top of the chunk, make all faces visible
        if (worldY == Noise.GenerateHeight(worldX, worldZ) || worldY == Noise.GenerateHeight(worldX, worldZ) - 1)
        {
            for (int i = 0; i < visibleFaces.Length; i++)
            {
                visibleFaces[i] = true;
            }
            return visibleFaces;
        }

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

    //Checks if there is a block at the given position
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
                Blocks block = neighbourChunk.chunkMap[(int)checkPos.x, (int)checkPos.y, (int)checkPos.z];
                return block == BlockDB.GetBlockName("Dirt") || block == BlockDB.GetBlockName("Stone") || block == BlockDB.GetBlockName("Bedrock");
            }
            else
            {
                // If checking above the chunk treat it as air
                if (y >= World.chunkSize)
                {
                    return false;
                }
                // For other missing neighbors treat as solid
                return true;
            }
        }
        else
        {
            Blocks block = chunkMap[x, y, z];
            return block == BlockDB.GetBlockName("Dirt") || block == BlockDB.GetBlockName("Stone") || block == BlockDB.GetBlockName("Bedrock");
        }
    }

    public void ClearChunk()
    {
        vertices.Clear();
        triangles.Clear();
        UVs.Clear();
    }
}
