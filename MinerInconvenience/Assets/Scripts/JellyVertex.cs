using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyVertex
{
    //fields
    public int vertexIndex;
    public Vector3 intialVertexPosition;
    public Vector3 currentVertexPosition;

    public Vector3 currentVelocity;

    public JellyVertex(int vertexIndex, Vector3 initialVertexPos, Vector3 currentVertexPos, Vector3 currentVel)
    {
        this.vertexIndex = vertexIndex;
        this.intialVertexPosition = initialVertexPos;
        this.currentVertexPosition = currentVertexPos;
        this.currentVelocity = currentVel;
    }

    public Vector3 GetCurrentDisplacement()
    {
        return currentVertexPosition - intialVertexPosition;
    }

    public void UpdateVelocity(float bounceSpeed)
    {
        currentVelocity = currentVelocity - GetCurrentDisplacement() * bounceSpeed * Time.deltaTime;
    }

    public void Settle(float stiffness)
    {
        currentVelocity *= 1.0f - stiffness * Time.deltaTime;
    }

    public void ApplyPressureToVertex(Transform transform,Vector3 _position, float pressure)
    {
        Vector3 distanceVertexPoint = currentVertexPosition - transform.InverseTransformPoint(_position);
        float adaptedPressure = pressure / (1f + distanceVertexPoint.sqrMagnitude*distanceVertexPoint.sqrMagnitude);
        float velocity = adaptedPressure * Time.deltaTime;
        currentVelocity += distanceVertexPoint.normalized * velocity;
    }

}
