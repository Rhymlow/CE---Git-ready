using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ColoredCube : GameSystem
{
    public float size = 1f;

    void Start()
    {
        GeneratePlane();
    }

    void GeneratePlane()
    {
        Mesh mesh = new Mesh();
        mesh.name = "FlatPlane";

        int quadsPerSide = 4;   // 4 cuadritos por lado
        int vertsPerSide = quadsPerSide * 2; // 8 vértices por lado para no compartir vértices

        // Pero si quieres 4x4 cuadritos con 64 vertices sin compartir, es:
        // cada cuadrito = 4 vertices únicos
        // 4x4 cuadritos = 16 cuadritos * 4 vertices = 64 vertices

        // Para organizarlo vamos a iterar cuadrito por cuadrito creando sus 4 vertices

        int quadCount = quadsPerSide * quadsPerSide;
        Vector3[] vertices = new Vector3[quadCount * 4];
        int[] triangles = new int[quadCount * 6];
        Color[] colors = new Color[vertices.Length];

        float quadSize = 1f / quadsPerSide;  // tamaño de cada cuadrito (en plano de 1x1)

        int v = 0;
        int t = 0;

        for (int y = 0; y < quadsPerSide; y++)
        {
            for (int x = 0; x < quadsPerSide; x++)
            {
                // posición base del cuadrito
                float xPos = -0.5f + x * quadSize;
                float yPos = -0.5f + y * quadSize;

                // Crear 4 vertices (cada cuadrito no comparte vertices)
                vertices[v + 0] = new Vector3(xPos, 0, yPos);
                vertices[v + 1] = new Vector3(xPos, 0, yPos + quadSize);
                vertices[v + 2] = new Vector3(xPos + quadSize, 0, yPos);
                vertices[v + 3] = new Vector3(xPos + quadSize, 0, yPos + quadSize);

                // Triangulos para el cuadrito (2 triángulos por cuadrito)
                triangles[t++] = v + 0;
                triangles[t++] = v + 1;
                triangles[t++] = v + 2;

                triangles[t++] = v + 2;
                triangles[t++] = v + 1;
                triangles[t++] = v + 3;

                // Color aleatorio para los 4 vertices (mismo color por cuadrito)
                Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
                colors[v + 0] = c;
                colors[v + 1] = c;
                colors[v + 2] = c;
                colors[v + 3] = c;

                v += 4;  // siguiente grupo de vertices
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        // Usa un shader que permita vertex colors
        Material mat = new Material(Shader.Find("GUI/Text Shader"));
        mat.SetColor("_BaseColor", Color.white);
        meshRenderer.material = mat;
    }
}
