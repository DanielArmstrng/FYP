using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDB : MonoBehaviour
{
    public static List<Blocks> blocksList = new List<Blocks>();

    void Awake()  // Changed from Start to Awake
    {
        InitialiseBlocks();
    }

    // Start is called before the first frame update
    public static void InitialiseBlocks()
    {
        blocksList.Add(new Blocks());
        blocksList.Add(new Blocks("Dirt", true, false, new Vector2(0, 0)));
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
