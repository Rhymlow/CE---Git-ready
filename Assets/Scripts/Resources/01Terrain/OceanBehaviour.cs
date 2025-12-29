using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OceanBehaviour : GameSystem
{
    public bool followPlayer = true;
    public int width = 50;
    public int height = 50;
    public float scale = 2.5f;
    public float waveHeight = 0.1f;
    public float waveSpeed = 3f;

    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        GenerateMesh();
        transform.localScale = new Vector3(20f, 20f, 20f);
        FollowPlayer();
    }

    void Update()
    {
        AnimateWaves();
        FollowPlayer();
    }

    float timer = 15f;
    void FollowPlayer()
    {
        if(followPlayer == true)
        {
            timer += Time.deltaTime;
            if (timer > 10f)
            {
                transform.position = new Vector3(player.transform.position.x - 500f, -1.0f, player.transform.position.z - 500f);
                timer = 0f;
            }
        }    
    }

    void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(width + 1) * (height + 1)];
        int[] triangles = new int[width * height * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        // Generar vértices
        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
                uv[i] = new Vector2((float)x / width, (float)z / height);
            }
        }

        // Generar triángulos
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

    void AnimateWaves()
    {
        Vector3[] updatedVertices = mesh.vertices;

        for (int i = 0, z = 0; z <= height; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                float y = Mathf.PerlinNoise((x + Time.time * waveSpeed) * scale * 0.1f, (z + Time.time * waveSpeed) * scale * 0.1f) * waveHeight;
                updatedVertices[i] = new Vector3(vertices[i].x, y, vertices[i].z);
            }
        }

        mesh.vertices = updatedVertices;
        mesh.RecalculateNormals();
    }
}
