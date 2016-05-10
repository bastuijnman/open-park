using UnityEngine;
using System.Collections.Generic;

public class Path
{

	private GameObject obj;

	private MeshRenderer renderer;

	private MeshFilter filter;

	private Mesh mesh;

	private List<Vector3> points = new List<Vector3> ();

	public Path (Transform parent)
	{
		obj = new GameObject ();

		obj.name = "Path";
		renderer = obj.AddComponent<MeshRenderer> ();
		filter = obj.AddComponent<MeshFilter> ();
		mesh = new Mesh ();
		filter.sharedMesh = mesh;

		obj.transform.parent = parent;
	}

	public void AddPoint(Vector3 point, float width) {
		points.Add (point);

		if (points.Count > 1) {
			FillVertices ();
		}
	}

	public void FillVertices() {
		
		float width = 2.0f;
		List<Vector3> vertices = new List<Vector3> ();
		List<Vector3> normals = new List<Vector3> ();

		mesh.Clear ();

		for (int i = 0; i < points.Count; i++) {
			Vector3 direction;

			if (i != points.Count - 1) {
				direction = new Vector3 (-(points [i + 1].z - points [i].z), points [i].y, points [i + 1].x - points [i].x).normalized;
			} else {
				direction = new Vector3 (-(points [i].z - points [i - 1].z), points [i].y, points [i].x - points [i - 1].x).normalized;
			}

			vertices.Add (points [i] + direction * width);
			normals.Add (Vector3.up);
			vertices.Add (points [i] + direction * -width);
			normals.Add (Vector3.up);
		}

		Triangulate (vertices, normals);
	}

	public void Triangulate (List<Vector3> vertices, List<Vector3> normals) {
		List<int> triangles = new List<int> ();

		for (int i = 0; i < vertices.Count - 2; i++) {
			if (i % 2 == 0) {
				triangles.Add (i + 2);
				triangles.Add (i + 1);
				triangles.Add (i);
			} else {
				triangles.Add (i);
				triangles.Add (i + 1);
				triangles.Add (i + 2);
			}
		}

		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.normals = normals.ToArray ();

		mesh.RecalculateNormals ();
	}

}
