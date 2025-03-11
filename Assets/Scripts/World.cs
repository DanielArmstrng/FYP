using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material terrainMaterial;

    public static int worldSize = 10;
    public static int chunkSize = 10;
    public static int columnHeight = 20;

    public static Dictionary<Vector3, Chunk> chunkList;

    // Start is called before the first frame update
    void Start()
    {
        chunkList = new Dictionary<Vector3, Chunk>();

        GetComponent<BlockDB>().GenerateDB();

        for(int x = 0; x < worldSize * chunkSize; x += chunkSize)
            for(int z = 0; z < worldSize * chunkSize; z += chunkSize)
            {
                GenerateColumn(x, z);
            }

        foreach(KeyValuePair<Vector3, Chunk> chunk in chunkList)
        {
            chunk.Value.GenerateBlocksMap();
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if(chunkList.TryGetValue(chunkPos, out foundChunk))
        {
            return foundChunk;
        }
        else
        {
            return null;
        }
    }
}
