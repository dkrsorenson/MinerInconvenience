using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFier : MonoBehaviour
{
    public float bounceSpeed;
    public float fallForce;
    public float stiffness;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Renderer renderer;

    JellyVertex[] jellyVertices;
    Vector3[] currentMeshVertices;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        renderer = GetComponent<Renderer>();

        GetVertices();
    }

    void GetVertices()
    {
        jellyVertices = new JellyVertex[mesh.vertices.Length];
        currentMeshVertices = new Vector3[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            jellyVertices[i] = new JellyVertex(i, mesh.vertices[i], mesh.vertices[i], Vector3.zero);
            currentMeshVertices[i] = mesh.vertices[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (renderer.isVisible)
        {
            UpdateVertices();
        }
    }

    void UpdateVertices()
    {
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].UpdateVelocity(bounceSpeed);
            jellyVertices[i].Settle(stiffness);

            jellyVertices[i].currentVertexPosition += jellyVertices[i].currentVelocity * Time.deltaTime;
            currentMeshVertices[i] = jellyVertices[i].currentVertexPosition;
        }

        mesh.vertices = currentMeshVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.transform.position.y > transform.position.y)
        {
            ContactPoint[] collisionPoints = collision.contacts;
            Vector2 offset = GetComponentInChildren<CapsuleCollider2D>().offset;
            offset.y = -0.3f;
            GetComponentInChildren<CapsuleCollider2D>().offset = offset;

            Vector3 curOffset = GetComponent<BoxCollider>().center;
            curOffset.y = -0.1f;

            for (int i = 0; i < collisionPoints.Length; i++)
            {
                Vector3 inputPoint = collisionPoints[i].point;
                ApplyPressureToPoint(inputPoint, fallForce);
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        Vector2 offset = GetComponentInChildren<CapsuleCollider2D>().offset;
        offset.y = 0f;
        GetComponentInChildren<CapsuleCollider2D>().offset = offset;

        Vector3 curOffset = GetComponent<BoxCollider>().center;
        curOffset.y = 0f;
        //GetComponent<BoxCollider>().center = curOffset;
    }

    public void ApplyPressureToPoint(Vector3 point, float pressure)
    {
        for (int i = 0; i < jellyVertices.Length; i++)
        {
            jellyVertices[i].ApplyPressureToVertex(transform, point, pressure);
        }
    }
}
