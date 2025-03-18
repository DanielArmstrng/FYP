using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class World : MonoBehaviour
//{
//    public Material terrainMaterial;
//    public Transform player;
//    public int viewDistance = 3;

//    public static int worldSize = 3;
//    public static int chunkSize = 10;
//    public static int columnHeight = 10;

//    public static Dictionary<Vector3, Chunk> chunkList;
//    private Vector3 lastCheckedPosition;

//    // Start is called before the first frame update
//    void Start()
//    {
//        chunkList = new Dictionary<Vector3, Chunk>();

//        GetComponent<BlockDB>().GenerateDB();

//        for (int x = 0; x < worldSize * chunkSize; x += chunkSize)
//            for (int z = 0; z < worldSize * chunkSize; z += chunkSize)
//            {
//                GenerateColumn(x, z);
//            }

//        foreach (KeyValuePair<Vector3, Chunk> chunk in chunkList)
//        {
//            chunk.Value.GenerateBlocksMap();
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    void GenerateChunk(Vector3 pos)
//    {
//        Chunk newChunk = new Chunk(pos, terrainMaterial);

//        chunkList.Add(pos, newChunk);
//    }

//    void GenerateColumn(int x, int z)
//    {
//        for (int y = 0; y < columnHeight * chunkSize; y += chunkSize)
//        {
//            GenerateChunk(new Vector3(x, y, z));
//        }
//    }

//    public static Chunk GetChunk(Vector3 pos)
//    {
//        float x = pos.x;
//        float y = pos.y;
//        float z = pos.z;

//        x = Mathf.FloorToInt(x / chunkSize) * chunkSize;
//        y = Mathf.FloorToInt(y / chunkSize) * chunkSize;
//        z = Mathf.FloorToInt(z / chunkSize) * chunkSize;

//        Vector3 chunkPos = new Vector3(x, y, z);

//        Chunk foundChunk;

//        if (chunkList.TryGetValue(chunkPos, out foundChunk))
//        {
//            return foundChunk;
//        }
//        else
//        {
//            return null;
//        }
//    }
//}

public class World : MonoBehaviour
{
    public Material terrainMaterial;
    public Transform player; 
    public int viewDistance = 3; 

    public static int worldSize = 10;
    public static int chunkSize = 10;
    public static int columnHeight = 5;

    public static Dictionary<Vector3, Chunk> chunkList;
    private Vector3 lastCheckedPosition;

    public float generationThrottleSeconds = 0.1f;
    private Queue<Vector3> chunksToGenerate = new Queue<Vector3>();
    public bool isGenerating = false;

    void Start()
    {
        chunkList = new Dictionary<Vector3, Chunk>();
        //GetComponent<BlockDB>().GenerateDB();
        BlockDB.InitialiseBlocks();

        lastCheckedPosition = GetChunkPosition(player.position);

        UpdateChunks();
    }

    void Update()
    {
        Vector3 playerChunkPosition = GetChunkPosition(player.position);

        if (playerChunkPosition != lastCheckedPosition)
        {
            UpdateChunks();
            lastCheckedPosition = playerChunkPosition;
        }
    }

    Vector3 GetChunkPosition(Vector3 position)
    {
        float x = Mathf.FloorToInt(position.x / chunkSize) * chunkSize;
        float z = Mathf.FloorToInt(position.z / chunkSize) * chunkSize;
        return new Vector3(x, 0, z);
    }

    //void UpdateChunks()
    //{
    //    Vector3 playerChunkPos = GetChunkPosition(player.position);

    //    for (int x = -viewDistance; x <= viewDistance; x++)
    //        for (int z = -viewDistance; z <= viewDistance; z++)
    //        {
    //            Vector3 chunkPos = new Vector3(
    //                playerChunkPos.x + (x * chunkSize),
    //                0,
    //                playerChunkPos.z + (z * chunkSize)
    //            );

    //            if (!chunkList.ContainsKey(chunkPos))
    //            {
    //                GenerateColumn((int)chunkPos.x, (int)chunkPos.z);

    //                for (int y = 0; y < columnHeight * chunkSize; y += chunkSize)
    //                {
    //                    Vector3 columnChunkPos = new Vector3(chunkPos.x, y, chunkPos.z);
    //                    if (chunkList.ContainsKey(columnChunkPos))
    //                    {
    //                        chunkList[columnChunkPos].GenerateBlocksMap();
    //                    }
    //                }
    //            }
    //        }
    //}

    void UpdateChunks()
    {
        Vector3 playerChunkPos = GetChunkPosition(player.position);

        for (int x = -viewDistance; x <= viewDistance; x++)
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector3 chunkPos = new Vector3(
                    playerChunkPos.x + (x * chunkSize),
                    0,
                    playerChunkPos.z + (z * chunkSize)
                );

                if (!chunkList.ContainsKey(chunkPos) && !chunksToGenerate.Contains(chunkPos))
                {
                    chunksToGenerate.Enqueue(chunkPos);
                }
            }

        if (!isGenerating && chunksToGenerate.Count > 0)
        {
            StartCoroutine(GenerateChunksRoutine());
        }
    }

    IEnumerator GenerateChunksRoutine()
    {
        isGenerating = true;

        while (chunksToGenerate.Count > 0)
        {
            Vector3 chunkPos = chunksToGenerate.Dequeue();

            GenerateColumn((int)chunkPos.x, (int)chunkPos.z);
       
            for (int y = 0; y < columnHeight * chunkSize; y += chunkSize)
            {
                Vector3 columnChunkPos = new Vector3(chunkPos.x, y, chunkPos.z);
                if (chunkList.ContainsKey(columnChunkPos))
                {
                    chunkList[columnChunkPos].GenerateBlocksMap();
                }
            }

            yield return new WaitForSeconds(generationThrottleSeconds);
        }

        isGenerating = false;
    }

    void GenerateChunk(Vector3 pos)
    {
        Chunk newChunk = new Chunk(pos, terrainMaterial);

        chunkList.Add(pos, newChunk);
    }

    void GenerateColumn(int x, int z)
    {
        for (int y = 0; y < columnHeight * chunkSize; y += chunkSize)
        {
            GenerateChunk(new Vector3(x, y, z));
        }
    }

    public static Chunk GetChunk(Vector3 pos)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;

        x = Mathf.FloorToInt(x / chunkSize) * chunkSize;
        y = Mathf.FloorToInt(y / chunkSize) * chunkSize;
        z = Mathf.FloorToInt(z / chunkSize) * chunkSize;

        Vector3 chunkPos = new Vector3(x, y, z);

        Chunk foundChunk;

        if (chunkList.TryGetValue(chunkPos, out foundChunk))
        {
            return foundChunk;
        }
        else
        {
            return null;
        }
    }
}
