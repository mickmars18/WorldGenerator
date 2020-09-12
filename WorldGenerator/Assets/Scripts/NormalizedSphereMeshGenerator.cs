using System;
using UnityEngine;

public class NormalizedSphereMeshGenerator
{
    class Face
    {
        public Mesh m_Mesh;
        Vector3 m_up;
        Vector3 m_right;
        Vector3 m_forward;

        int m_precision;
        float m_radius;

        public Face(int precision, float radius, Vector3 up)
        {
            m_precision = precision;
            m_radius = radius;

            Vector3[] vertices = GenerateVertices(up);
            int[] triangles = GenerateTriangles(up);
            GenerateMesh(vertices, triangles);
            Normalize();
        }

        public Mesh GetMesh()
        {
            return m_Mesh;
        }

        public void Normalize()
        {
            
        }

        private Mesh GenerateMesh(Vector3[] vertices, int[] triangles)
        {
            m_Mesh = new Mesh();
            m_Mesh.Clear();
            m_Mesh.vertices = vertices;
            m_Mesh.triangles = triangles;
            m_Mesh.RecalculateNormals();
            return m_Mesh;
        }

        private Vector3[] GenerateVertices(Vector3 up)
        {
            Vector3[] vertices = new Vector3[(m_precision + 1) * (m_precision + 1)];
            for (int i = 0; i < m_precision + 1; i++)
            {
                for (int j = 0; j < m_precision + 1; j++)
                {
                    Vector3 point = new Vector3();
                    if (up.x != 0)
                    {
                        point = new Vector3(up.x * m_precision / 2, i - m_precision / 2, j - m_precision / 2);
                    }
                    else if(up.y != 0)
                    {
                        point = new Vector3(j - m_precision / 2, up.y * m_precision / 2, i - m_precision / 2);
                    }
                    else if(up.z != 0)
                    {
                        point = new Vector3(i - m_precision / 2, j - m_precision / 2, up.z * m_precision / 2);
                    }
                    vertices[i * (m_precision + 1) + j] = Vector3.Normalize(point) * m_radius;
                }
            }
            return vertices;
        }

        private int[] GenerateTriangles(Vector3 up)
        {
            int[] triangles = new int[m_precision * m_precision * 6];

            // Reverse order of triangles if necessary
            int index1 = 1;
            int index2 = 2;
            int index4 = 4;
            int index5 = 5;
            if(up.x + up.y + up.z != 1)
            {
                index1 = 2;
                index2 = 1;
                index4 = 5;
                index5 = 4;
            }

            int triangleIndex = 0;
            for (int i = 0; i < m_precision; i++)
            {
                for (int j = 0; j < m_precision; j++)
                {
                    triangles[triangleIndex] = j + i * (m_precision + 1);
                    triangles[triangleIndex + index1] = m_precision + j + 1 + i * (m_precision + 1);
                    triangles[triangleIndex + index2] = j + 1 + i * (m_precision + 1);

                    triangles[triangleIndex + 3] = m_precision + j + 1 + i * (m_precision + 1);
                    triangles[triangleIndex + index4] = m_precision + j + 2 + i * (m_precision + 1);
                    triangles[triangleIndex + index5] = j + 1 + i * (m_precision + 1);

                    triangleIndex += 6;
                }
            }
            return triangles;
        }
    }
    public static Mesh GenerateMesh(int precision, float radius)
    {
        precision = precision * 2;
        Face[] faces = new Face[6];
        faces[0] = new Face(precision, radius, Vector3.up);
        faces[1] = new Face(precision, radius, Vector3.right);
        faces[2] = new Face(precision, radius, Vector3.forward);
        faces[3] = new Face(precision, radius, Vector3.left);
        faces[4] = new Face(precision, radius, Vector3.back);
        faces[5] = new Face(precision, radius, Vector3.down);
        return RegroupFaces(faces, precision);
    }

    private static Mesh RegroupFaces(Face[] faces, int precision)
    {
        int nbVerticesPerFace = (precision + 1) * (precision + 1);
        int nbTrianglesPerFace = precision * precision * 6;
        int nbFaces = 6;

        Vector3[] vertices = new Vector3[nbVerticesPerFace * nbFaces];
        int[] triangles = new int[nbTrianglesPerFace * nbFaces];

        for (int i = 0; i < faces.Length; i++)
        {
            for (int j = 0; j < nbVerticesPerFace; j++)
            {
                vertices[i * nbVerticesPerFace + j] = faces[i].m_Mesh.vertices[j];
            }

            for (int j = 0; j < nbTrianglesPerFace; j++)
            {
                triangles[i * nbTrianglesPerFace + j] = faces[i].m_Mesh.triangles[j] + i * nbVerticesPerFace;
            }
        }

        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
