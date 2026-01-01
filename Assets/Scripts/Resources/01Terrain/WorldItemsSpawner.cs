using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WorldItemsSpawner : MonoBehaviour
{
    [Header("Configuración de la malla")]
    public int width = 50;
    public int length = 50;
    public float spacing = 1f;

    [Header("Ruido Perlin")]
    public float noiseScale = 10f;
    public float noiseThreshold = 0.8f;

    [Header("Altura máxima del ruido")]
    public float maxHeight = 5f;

    [Header("Actualización")]
    public float updateInterval = 1f; // Tiempo en segundos entre actualizaciones

    private List<GameObject> spawnedCubes = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        GenerateCubes();
    }

    void Update()
    {
        /*timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            ClearCubes();
            GenerateCubes();
        }*/
    }

    void GenerateCubes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                float worldX = transform.position.x + x * spacing;
                float worldZ = transform.position.z + z * spacing;
                float noiseValue = Mathf.PerlinNoise(worldX / noiseScale, worldZ / noiseScale);
                if (noiseValue > noiseThreshold)
                {
                    float height = noiseValue * maxHeight;
                    Vector3 position = new Vector3(worldX, height / 2f, worldZ); // Altura centrada
                    GameObject arbol = Instantiate(Resources.Load("arbol chico") as GameObject, position + new Vector3(0, height+20, 0), new quaternion(0,0,0,0));
                    arbol.transform.SetParent(transform);
                    spawnedCubes.Add(arbol);
                }
            }
        }
    }

    void ClearCubes()
    {
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }
        spawnedCubes.Clear();
    }
}
