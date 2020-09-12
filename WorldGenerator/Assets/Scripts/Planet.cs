using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(1, 20)]
    public int precision = 40;

    [Range(0.1f, 100.0f)]
    public float m_radius = 1.0f;

    public GameObject m_MeshGameObject;

    private void OnValidate()
    {
        GeneratePlanet();
    }

    private void GeneratePlanet()
    {
        GenerateSphereMesh(precision, m_radius);
    }

    private void GenerateSphereMesh(int precision, float radius)
    {
        m_MeshGameObject.GetComponent<MeshFilter>().sharedMesh = NormalizedSphereMeshGenerator.GenerateMesh(precision, radius);
    }
}
