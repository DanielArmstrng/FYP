using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldInteraction : MonoBehaviour
{
    public GameObject target;

    Vector3 playerHeadPos;
    Vector3 playerBodyPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5.0f))
        {
            Vector3 playerPos = gameObject.transform.position;
            playerHeadPos = new Vector3(Mathf.Round(playerPos.x), Mathf.Round(playerPos.y + 1), Mathf.Round(playerPos.z));
            playerBodyPos = new Vector3(Mathf.Round(playerPos.x), Mathf.Round(playerPos.y), Mathf.Round(playerPos.z));

            Vector3 hitBlock;
            hitBlock = hit.point - hit.normal / 2;

            Vector3 placeBlock;
            placeBlock = hit.point + hit.normal / 2;

            hitBlock.x = (int)Mathf.Round(hitBlock.x);
            hitBlock.y = (int)Mathf.Round(hitBlock.y);
            hitBlock.z = (int)Mathf.Round(hitBlock.z);

            placeBlock.x = (int)Mathf.Round(placeBlock.x);
            placeBlock.y = (int)Mathf.Round(placeBlock.y);
            placeBlock.z = (int)Mathf.Round(placeBlock.z);

            target.transform.position = hitBlock;

            Debug.DrawLine(Camera.main.transform.position, hitBlock, Color.red, Mathf.Infinity);
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green, Mathf.Infinity);

            Interaction(hit, hitBlock, placeBlock);
        }
        else
        {
            target.transform.position = new Vector3(0, -5000, 0);
        }
    }

    void Interaction(RaycastHit hit, Vector3 hitPosition, Vector3 placePosition)
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 chunkBlockPosition = hitPosition - hit.collider.gameObject.transform.position;
            
            Chunk chunkAtPosition = World.GetChunk(hit.collider.transform.position);

            ClearChunk(chunkAtPosition);

            chunkAtPosition.chunkMap[(int)chunkBlockPosition.x, (int)chunkBlockPosition.y, (int)chunkBlockPosition.z] = BlockDB.GetBlockName("Air");
        
            chunkAtPosition.GenerateBlocksMap();

            UpdateNeighbours(chunkBlockPosition, chunkAtPosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (placePosition != playerHeadPos && placePosition != playerBodyPos)
            {
                Chunk chunkAtPos = World.GetChunk(placePosition);

                ClearChunk(chunkAtPos);

                Vector3 pos = World.GetBlockPosition(placePosition);
                chunkAtPos.chunkMap[(int)pos.x, (int)pos.y, (int)pos.z] = BlockDB.GetBlockName("Stone");
                chunkAtPos.GenerateBlocksMap();

                UpdateNeighbours(pos, chunkAtPos);
            }
        }
    }

    void ClearChunk(Chunk chunk)
    {
        DestroyImmediate(chunk.chunkObject.GetComponent<MeshFilter>());
        DestroyImmediate(chunk.chunkObject.GetComponent<MeshRenderer>());
        DestroyImmediate(chunk.chunkObject.GetComponent<MeshCollider>());
        chunk.ClearChunk();
    }

    void UpdateNeighbours(Vector3 position, Chunk actualChunk)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        if (x == 0)
        {
            RegenerateNeighbour(World.FindNeighbour(actualChunk, "left"));
        }
        else if (x == World.chunkSize - 1)
        {
            RegenerateNeighbour(World.FindNeighbour(actualChunk, "right"));
        }

        if (y == 0)
        {
            RegenerateNeighbour(World.FindNeighbour(actualChunk, "bottom"));
        }
        else if (y == World.chunkSize - 1)
        {
            RegenerateNeighbour(World.FindNeighbour(actualChunk, "top"));
        }

        if (z == 0)
        {
            RegenerateNeighbour(World.FindNeighbour(actualChunk, "back"));
        }
        else if (z == World.chunkSize - 1)
        {
            RegenerateNeighbour(World.FindNeighbour(actualChunk, "front"));
        }
    }

    void RegenerateNeighbour(Chunk neighbour)
    {
        if(neighbour != null)
        {
            ClearChunk(neighbour);
            neighbour.GenerateBlocksMap();
        }
    }


}
