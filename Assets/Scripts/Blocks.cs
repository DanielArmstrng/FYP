using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks
{
    public string name;

    public bool isSolid;
    public bool isTransparent;

    public Vector2 texture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Blocks(string name, bool isSolid, bool isTransparent, Vector2 texture)
    {
        this.name = name;
        this.isSolid = isSolid;
        this.isTransparent = isTransparent;
        this.texture = texture;
    }

    public Blocks()
    {
        name = "Air";
        isSolid = false;
        isTransparent = true;
    }
}
