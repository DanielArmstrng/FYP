using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Noise : MonoBehaviour
{
    static int maxHeight = 150;
    static float smoothness = 0.01f;
    static int octaves = 4;
    static float persistence = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static float Map(float value, float min, float max, float newMin, float newMax)
    {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(min, max, value));
    }

    public static int GenerateHeight(float x, float z)
    {
        float height = Map(FBM(x * smoothness, z * smoothness, octaves, persistence), 0, 1, 0, maxHeight);
        return (int)height;
    }

    static float FBM(float x, float z, int octaves, float persistence)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;

            maxValue += amplitude;

            amplitude *= persistence;
            frequency *= 2;
        }

        return total / maxValue;
    }
}
