using System.Collections;
using System.Collections.Generic;
//using TreeEditor;
using UnityEngine;

public class Noise : MonoBehaviour
{
    static int maxHeight = 150;
    static float smoothness = 0.007f; //How stretched out the noise is, higher = more stretched
    static int octaves = 4; // The number of layers of noise
    static float persistence = 0.3f; //How much each octave contributes to final shape of the noise
    static int seed = Random.Range(0, 1000) * Random.Range(0, 1000);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Maps out a value from one range to another
    static float Map(float value, float min, float max, float newMin, float newMax)
    {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(min, max, value));
    }

    //Generates the height of the surface at the given x and z coordinates
    public static int GenerateHeight(float x, float z)
    {
        float height = Map(FBM((x + seed) * smoothness, (z + seed) * smoothness, octaves, persistence), 0, 1, 0, maxHeight);
        return (int)height;
    }

    //Geneerates the height of the stone layer at the given x and z coordinates
    public static int GenerateStoneHeight(float x, float z)
    {
        float height = Map(FBM((x + seed) * smoothness, (z + seed) * smoothness, octaves, persistence), 0, 1, 0, maxHeight);
        return (int)height - 10;
    }

    //Uses the Fractal Brownian Motion algorithm
    //Combines mutiples layers of Perlin noise to create a more natural looking surface
    static float FBM(float x, float z, int octaves, float persistence)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        //Loop through the number of octaves to combine layers with different frequencies and amplitudes
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;

            //Keeps track of the maximum possible value
            maxValue += amplitude;

            //Reduces the amplitude and increase the frequency for the next octave
            amplitude *= persistence;
            frequency *= 2;
        }

        //Normalise the value to be between 0 and 1
        return total / maxValue;
    }
}
