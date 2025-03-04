using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material terrainMaterial;

    public static int worldSize = 4;
    public static int chunkSize = 10;
    public static int columnHeight = 10;

    public static Dictionary<Vector3, Chunk> chunkList;

    // Start is called before the first frame update
    void Start()
    {
        chunkList = new Dictionary<Vector3, Chunk>();

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
}
