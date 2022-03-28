using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum ViewMode { Noise, Colors, Mesh };
    public ViewMode viewMode;
    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float scale;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public int octaves;
    public bool autoUpdate;

    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 Offset;

    public TerrainTypes[] region;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, scale, octaves, persistance, lacunarity, seed, Offset);
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < region.Length; i++)
                {
                    if (currentHeight <= region[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = region[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        if (viewMode == ViewMode.Colors) mapDisplay.DrawTexture(TextureGenerator.TexturefromColorMap(colorMap, mapChunkSize, mapChunkSize));
        else if (viewMode == ViewMode.Noise) mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (viewMode == ViewMode.Mesh)
        {
            mapDisplay.DrawMesh(MeshGenerator.TerrainMeshGenerator(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail),
            TextureGenerator.TexturefromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }

    }

    void OnValidate()
    {
        if (octaves < 0) octaves = 0;
    }




}

[System.Serializable]
public struct TerrainTypes
{
    public string Name;
    [Range(0f, 1f)]
    public float height;

    public Color color;

}