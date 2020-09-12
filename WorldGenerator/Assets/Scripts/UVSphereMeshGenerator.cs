using UnityEngine;

public class UVSphereMeshGenerator
{
    public static Mesh GenerateMesh(int precision, float radius)
    {
        Vector3[] vertices = GenerateVertices(precision, radius);
        int[] triangles = GenerateTriangles(precision);
        return GenerateMesh(vertices, triangles, precision);
    }

    private static Mesh GenerateMesh(Vector3[] vertices, int[] triangles, int precision)
    {
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private static Vector3[] GenerateVertices(int precision, float radius)
    {
        Vector3[] vertices = new Vector3[(precision + 1) * (precision + 1)];
        for (int i = 0; i < precision + 1; i++)
        {
            float parallel = Mathf.PI * i / (float)precision;
            for (int j = 0; j < precision + 1; j++)
            {
                float meridian = 2.0f * (float)Mathf.PI * j / (float)precision;
                float xCoord = radius * Mathf.Sin(parallel) * Mathf.Cos(meridian);
                float yCoord = radius * Mathf.Sin(parallel) * Mathf.Sin(meridian);
                float zCoord = radius * Mathf.Cos(parallel);
                vertices[i * (precision + 1) + j] = new Vector3(xCoord, yCoord, zCoord);
            }
        }
        return vertices;
    }

    private static int[] GenerateTriangles(int precision)
    {
        int[] triangles = new int[precision * precision * 6];

        int triangleIndex = 0;
        for (int i = 0; i < precision; i++)
        {
            for (int j = 0; j < precision; j++)
            {
                triangles[triangleIndex] = j + i * (precision + 1);
                triangles[triangleIndex + 1] = precision + j + 1 + i * (precision + 1);
                triangles[triangleIndex + 2] = j + 1 + i * (precision + 1);

                triangles[triangleIndex + 3] = precision + j + 1 + i * (precision + 1);
                triangles[triangleIndex + 4] = precision + j + 2 + i * (precision + 1);
                triangles[triangleIndex + 5] = j + 1 + i * (precision + 1);

                triangleIndex += 6;
            }
        }
        return triangles;
    }
}
