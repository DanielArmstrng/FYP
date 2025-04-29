using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDB : MonoBehaviour
{
    public static List<Blocks> blocksList = new List<Blocks>();

    void Awake()
    {
        InitialiseBlocks();
    }

    public static void InitialiseBlocks()
    {
        blocksList.Add(new Blocks());
        blocksList.Add(new Blocks("Dirt", true, false, true,  new Vector2(2, 1)));
        blocksList.Add(new Blocks("Grass", true, false, true,  new Vector2(1, 0), new Vector2(3, 1), new Vector2(0, 0)));
        blocksList.Add(new Blocks("Stone", true, false, true,  new Vector2(0, 1)));
        blocksList.Add(new Blocks("Bedrock", true, false, false, new Vector2(1, 1)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Blocks GetBlockName(string name)
    {
        if(blocksList.Count == 0)
        {
            InitialiseBlocks();
        }

        for (int i = 0; i < blocksList.Count; i++)
        {
            if (blocksList[i].name == name)
            {
                return blocksList[i];
            }
        }
        return null;
    }

    public void GenerateDB()
    {
        blocksList.Add(new Blocks());
    }
}
