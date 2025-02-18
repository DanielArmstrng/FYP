using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> UVs = new List<Vector2>();

    Mesh CubeMesh;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCube();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateCube()
    {
        CubeMesh = new Mesh();

        CreateCubeSide("front");
        CreateCubeSide("back");
        CreateCubeSide("top"); 
        CreateCubeSide("bottom"); 
        CreateCubeSide("left"); 
        CreateCubeSide("right");

        GeneratePhysicalCube();
    }

    void GeneratePhysicalCube()
    {
        MeshFilter mf = GetComponent<MeshFilter>();

        CubeMesh.vertices = vertices.ToArray();
        CubeMesh.triangles = triangles.ToArray();
        CubeMesh.uv = UVs.ToArray();

        CubeMesh.RecalculateBounds();
        CubeMesh.RecalculateNormals();

        mf.mesh = CubeMesh;
    }

    void CreateCubeSide(string side)
    {
        triangles.Add(0 + vertices.Count);
        triangles.Add(1 + vertices.Count);
        triangles.Add(2 + vertices.Count);

        triangles.Add(0 + vertices.Count);
        triangles.Add(2 + vertices.Count);
        triangles.Add(3 + vertices.Count);

        //Front side
        Vector3 v0 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v2 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, 0.5f);

        //Back side
        Vector3 v4 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v5 = new Vector3(-0.5f, 0.5f, -0.5f);
        Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v7 = new Vector3(0.5f, -0.5f, -0.5f);

        UVs.Add(new Vector2(1, 1));
        UVs.Add(new Vector2(0, 1));
        UVs.Add(new Vector2(0, 0));
        UVs.Add(new Vector2(1, 0));

        switch (side)
        {
            case "front":
                vertices.Add(v0);
                vertices.Add(v1);
                vertices.Add(v2);
                vertices.Add(v3);
                break;
            case "back":
                vertices.Add(v4);
                vertices.Add(v5);
                vertices.Add(v6);
                vertices.Add(v7);
                break;
            case "top":
                vertices.Add(v1);
                vertices.Add(v6);
                vertices.Add(v5);
                vertices.Add(v2);
                break;
            case "bottom":
                vertices.Add(v3);
                vertices.Add(v4);
                vertices.Add(v7);
                vertices.Add(v0);
                break;
            case "left":
                vertices.Add(v7);
                vertices.Add(v6);
                vertices.Add(v1);
                vertices.Add(v0);
                break;
            case "right":
                vertices.Add(v3);
                vertices.Add(v2);
                vertices.Add(v5);
                vertices.Add(v4);
                break;
        }
    }
}
