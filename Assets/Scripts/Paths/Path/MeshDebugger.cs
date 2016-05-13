using UnityEngine;
using System.Collections;

public class MeshDebugger : MonoBehaviour
{

	private Mesh mesh;

	// Use this for initialization
	void Start ()
	{
		mesh = GetComponent<MeshFilter> ().sharedMesh;	
	}
	
	// Update is called once per frame
	void Update ()
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i < triangles.Length; i += 3) {
			Debug.DrawLine (vertices [triangles [i]], vertices [triangles [i + 1]], Color.green);
			Debug.DrawLine (vertices [triangles [i + 1]], vertices [triangles [i + 2]], Color.green);
			Debug.DrawLine (vertices [triangles [i + 2]], vertices [triangles [i]], Color.green);
		}
	}
}

