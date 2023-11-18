using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;

public class RayMeshDeformer : MonoBehaviour
{
    public MeshFilter meshF;
    public float radius;
    List<Vector3> vertices = new List<Vector3>();
    private LayerMask mask;
    public Transform circleTransform;

    private void Start()
    {
        meshF.mesh.GetVertices(vertices);
        string[] layers = { "Walls", "Doors" };
        mask = LayerMask.GetMask(layers);
    }

    Vector3 PolarToUnitCartesian(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 30; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, PolarToUnitCartesian(360 - (12 * i)), out hit, radius, mask))
            {
                vertices[i + 1] = circleTransform.InverseTransformPoint(circleTransform.position + PolarToUnitCartesian(360 - (12 * i)) * hit.distance);
            }
            else {
                vertices[i + 1] = circleTransform.InverseTransformPoint(circleTransform.position + PolarToUnitCartesian(360 - (12 * i)) * radius);
            }
        }
        meshF.mesh.SetVertices(vertices);
        meshF.mesh.RecalculateBounds();
    }
}