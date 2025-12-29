using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainGenerator : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float scale = 1f;
    public float elevation = 5f;
    public float mountainThreshold = 3.5f;
    public float snowThreshold = 4.5f;
    public float edgeBeachWidth = 8f;
    public float radialFalloffStrength = 2f;  // Nuevo: controla forma de isla

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

        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                float u = (float)x / width;
                float v = (float)z / height;

                // Distancia al borde (arena)
                float edgeDistance = Mathf.Min(x, z, width - x, height - z);
                float edgeFactor = Mathf.Clamp01(edgeDistance / edgeBeachWidth);

                // Falloff radial para forma de isla
                float distanceToCenter = Vector2.Distance(new Vector2(x, z), center);
                float radialFalloff = Mathf.Pow(distanceToCenter / maxDistance, radialFalloffStrength);
                radialFalloff = Mathf.Clamp01(1 - radialFalloff); // Invertir: centro = 1, bordes = 0

                // Perlin noise
                float noise = Mathf.PerlinNoise(x * scale * 0.1f, z * scale * 0.1f);
                float heightValue = noise * elevation * edgeFactor * radialFalloff;

                vertices[i] = new Vector3(x, heightValue, z);
                uv[i] = new Vector2(u, v);

                // Colores por elevación
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
        if (collider == null)
            collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }
}
