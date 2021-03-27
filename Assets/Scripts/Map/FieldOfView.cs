using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    [SerializeField]
    private Vision vision;

    void FixedUpdate() {
        vision = GameObject.Find("Human(Clone)").GetComponent<Vision>();
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        List<Vector3> hits = vision.HitsVector3;
        int hitsCount = hits.Count;

        Vector3 origin = new Vector3(vision.Origin.position.x, 0.1f, vision.Origin.position.z);

        Vector3[] vertices = new Vector3[hits.Count + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[hits.Count * 3];

        vertices[0] = origin;
        hits.Add(hits[0]);

        for(int i = 0; i < hits.Count; i++) {
            hits[i] = new Vector3(hits[i].x, 0.1f, hits[i].z);
        }

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= hitsCount; i++) {
            vertices[vertexIndex] = hits[i];

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }
}
