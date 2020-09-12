using UnityEngine;

public class NormalizedSphereMeshGenerator
{
    class Face
    {
        Mesh m_Mesh;
        Vector3 m_Up;
        Vector3 m_AxisA;
        Vector3 m_AxisB;

        public Face(int precision, float radius, Vector3 up)
        {
            m_Up = up;
            m_AxisA = new Vector3(m_Up.y, m_Up.z, m_Up.x);
            m_AxisB = Vector3.Cross(m_Up, m_AxisB);

            Vector3[] vertices = GenerateVertices(precision, radius);
            int[] triangles = GenerateTriangles(precision);
            GenerateMesh(vertices, triangles, precision);
            Normalize();
        }

        public Mesh GetMesh()
        {
            return m_Mesh;
        }

        public void Normalize()
        {

        }

        private Mesh GenerateMesh(Vector3[] vertices, int[] triangles, int precision)
        {
            m_Mesh = new Mesh();
            m_Mesh.Clear();
            m_Mesh.vertices = vertices;
            m_Mesh.triangles = triangles;
            m_Mesh.RecalculateNormals();
            return m_Mesh;
        }

        private Vector3[] GenerateVertices(int precision, float radius)
        {
            Vector3[] vertices = new Vector3[(precision + 1) * (precision + 1)];
            for (int i = 0; i < precision + 1; i++)
            {
                for (int j = 0; j < precision + 1; j++)
                {
                    Vector2 percent = new Vector2(j, i) / precision;
                    Vector3 point = m_Up + (percent.x - 0.5f) * 2 * m_AxisA + (percent.y - 0.5f) * 2 * m_AxisB;
                    vertices[i * (precision + 1) + j] = new Vector3(j, 0, i);
                    vertices[i * (precision + 1) + j] = point;
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
    public static Mesh GenerateMesh(int precision, float radius)
    {
        Face[] faces = new Face[1];
        faces[0] = new Face(precision, radius, Vector3.up);
        return RegroupFaces(faces);
    }

    private static Mesh RegroupFaces(Face[] faces)
    {
        return faces[0].GetMesh();
    }
}
