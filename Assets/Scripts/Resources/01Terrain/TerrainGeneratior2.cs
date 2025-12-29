using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainGeneratior2 : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float scale = 1f;
    public float elevation = 5f;
    public float mountainThreshold = 3.5f;
    public float snowThreshold = 4.5f;
    public float edgeBeachWidth = 8f;
    public float radialFalloffStrength = 2f;
    public float uvScale = 0.1f; // <-- NUEVA línea pública para controlar tileado desde el Inspector

    public Color grassColor = Color.green;
    public Color rockColor = Color.gray;
    public Color snowColor = Color.white;
    public Color sandColor = new Color(1f, 0.92f, 0.5f);
    public Color borderColor = Color.white;

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    private float lastUpdateTime = 0f;
    private float updateInterval = 2f;

    void Start()
    {
        GenerateMesh();
    }

    void Update()
    {
        //GenerateUpdatedMesh();
    }

    void GenerateUpdatedMesh()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            GenerateMesh();
            lastUpdateTime = Time.time;
        }
    }

    void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(width + 1) * (height + 1)];
        int[] triangles = new int[width * height * 6];
        Vector2[] uv = new Vector2[vertices.Length];
        colors = new Color[vertices.Length];

        Vector2 center = new Vector2(width / 2f, height / 2f);
        float maxDistance = Vector2.Distance(Vector2.zero, center);

        float[,] noiseMap = new float[width + 1, height + 1];

        // Calcular noise
        for (int z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float noise = Mathf.PerlinNoise(x * scale * 0.1f, z * scale * 0.1f);
                noiseMap[x, z] = noise;
            }
        }

        // Generar vértices, colores y UVs
        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                // Edge factor para arena
                float edgeDistance = Mathf.Min(x, z, width - x, height - z);
                float edgeFactor = Mathf.Clamp01(edgeDistance / edgeBeachWidth);

                // Distancia radial (forma de isla)
                float distanceToCenter = Vector2.Distance(new Vector2(x, z), center);
                float radialFalloff = Mathf.Pow(distanceToCenter / maxDistance, radialFalloffStrength);
                radialFalloff = Mathf.Clamp01(1 - radialFalloff);

                // Suavizado con vecinos (promedio 3x3)
                float averageNoise = 0f;
                int count = 0;
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        int nx = Mathf.Clamp(x + dx, 0, width);
                        int nz = Mathf.Clamp(z + dz, 0, height);
                        averageNoise += noiseMap[nx, nz];
                        count++;
                    }
                }
                averageNoise /= count;

                float heightValue = averageNoise * elevation * (edgeFactor / 10.0f) * radialFalloff;

                vertices[i] = new Vector3(x, heightValue, z);

                // UVs corregidas para evitar estiramiento
                uv[i] = new Vector2(x * uvScale, z * uvScale);

                // Colores
                if ((x == 0 || x == width) && (z == 0 || z == height))
                {
                    colors[i] = borderColor;
                }
                else if (edgeDistance <= edgeBeachWidth)
                {
                    colors[i] = sandColor;
                }
                else
                {
                    if (heightValue > snowThreshold)
                        colors[i] = snowColor;
                    else if (heightValue > mountainThreshold)
                        colors[i] = rockColor;
                    else
                        colors[i] = grassColor;
                }
            }
        }

        // Triángulos
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // Aplicar a Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.colors = colors;
        mesh.RecalculateNormals();

        // MeshCollider
        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }
}
