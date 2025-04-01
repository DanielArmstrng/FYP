using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks
{
    public string name;

    public bool isSolid;
    public bool isTransparent;

    public Vector2 texture;
    public Vector2 textureUp;
    public Vector2 textureDown;
    public Vector2 textureSide;

    public bool multiTexture;

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

    public Blocks(string name, bool isSolid, bool isTransparent, Vector2 textureUp, Vector2 textureDown, Vector2 textureSide)
    {
        this.name = name;
        this.isSolid = isSolid;
        this.isTransparent = isTransparent;
        this.textureUp = textureUp;
        this.textureDown = textureDown;
        this.textureSide = textureSide;

        multiTexture = true;
    }

    public Blocks()
    {
        name = "Air";
        isSolid = false;
        isTransparent = true;
    }
}
