using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RockGenerator : MonoBehaviour
{
    [Header("Configuración de la roca")]
    public float width = 1f;
    public float height = 1f;
    public float noiseScale = 1f;
    public float noiseStrength = 0.5f;
    public float noiseOffset = 0f;
    public float updateInterval = 1f;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] baseVertices; // para recordar la forma original
    private float timer;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.name = "RockCube";
        meshFilter.mesh = mesh;

        CreateMesh();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            ApplyNoise();
        }
    }

    void CreateMesh()
    {
        Vector3[] vertices = new Vector3[16];

        // Niveles de altura
        float h0 = 0f;
        float h1 = height * 0.25f;
        float h2 = height * 0.5f;
        float h3 = height;

        // Base cuadrada en XZ
        float halfW = width / 2f;

        // Creamos los 16 vértices (4 por nivel)
        // Orden: nivel inferior -> superior, cada nivel en sentido horario

        vertices[0] = new Vector3(-halfW, h0, -halfW);
        vertices[1] = new Vector3(halfW, h0, -halfW);
        vertices[2] = new Vector3(halfW, h0, halfW);
        vertices[3] = new Vector3(-halfW, h0, halfW);

        vertices[4] = new Vector3(-halfW, h1, -halfW);
        vertices[5] = new Vector3(halfW, h1, -halfW);
        vertices[6] = new Vector3(halfW, h1, halfW);
        vertices[7] = new Vector3(-halfW, h1, halfW);

        vertices[8] = new Vector3(-halfW, h2, -halfW);
        vertices[9] = new Vector3(halfW, h2, -halfW);
        vertices[10] = new Vector3(halfW, h2, halfW);
        vertices[11] = new Vector3(-halfW, h2, halfW);

        vertices[12] = new Vector3(-halfW, h3, -halfW);
        vertices[13] = new Vector3(halfW, h3, -halfW);
        vertices[14] = new Vector3(halfW, h3, halfW);
        vertices[15] = new Vector3(-halfW, h3, halfW);

        baseVertices = vertices;

        // Triángulos (caras conectando niveles)
        int[] triangles = new int[]
        {
            // Caras verticales entre niveles (ej: entre 0-4, 1-5, etc.)
            0, 4, 5, 0, 5, 1,
            1, 5, 6, 1, 6, 2,
            2, 6, 7, 2, 7, 3,
            3, 7, 4, 3, 4, 0,

            4, 8, 9, 4, 9, 5,
            5, 9,10, 5,10, 6,
            6,10,11, 6,11, 7,
            7,11, 8, 7, 8, 4,

            8,12,13, 8,13, 9,
            9,13,14, 9,14,10,
            10,14,15,10,15,11,
            11,15,12,11,12, 8,

            // Tapa inferior
            0, 1, 2, 0, 2, 3,

            // Tapa superior
            12,14,13,12,15,14
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void ApplyNoise()
    {
        Vector3[] newVertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 baseV = baseVertices[i];
            Vector2 noisePos = new Vector2(baseV.x + noiseOffset, baseV.z + noiseOffset);
            float noise = Mathf.PerlinNoise(noisePos.x * noiseScale, noisePos.y * noiseScale);
            float heightVariation = noise * noiseStrength;

            newVertices[i] = new Vector3(baseV.x, baseV.y + heightVariation, baseV.z);
        }

        mesh.vertices = newVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
