using UnityEngine;
using System.Collections.Generic;

public class Path
{

	// Dummy gameobject used for the path
	// TODO: Seems like a stupid way to do this
	private GameObject obj;

	// The mesh filter used by unity for mesh rendering
	private MeshRenderer renderer;

	// The mesh filter used for adding the mesh
	private MeshFilter filter;

	// The mesh collider used for snapping etc
	private MeshCollider collider;

	// Our path mesh
	private Mesh mesh;

	// The currently managed section
	private Section activeSection;

	// Our section list
	private List<Section> sections = new List<Section>();

	// Whether the path is currently snapping
	private bool snapping = false;

	/*
	 * Constructor of the path class.
	 * <param name="parent">This is the parent object the path should be attached to</param>
	 */
	public Path (Transform parent)
	{
		// Initiate our dummy game object
		obj = new GameObject ();

		// Initiate the path mesh
		mesh = new Mesh ();

		// Set object name
		obj.name = "Path";

		// Needed components
		renderer = obj.AddComponent<MeshRenderer> ();
		filter = obj.AddComponent<MeshFilter> ();
		collider = obj.AddComponent<MeshCollider> ();
		obj.AddComponent<MeshDebugger> ();

		// Make sure mesh is set for both filter and collider
		filter.sharedMesh = mesh;
		collider.sharedMesh = mesh;

		// Make sure we attach it to the parent given by the constructor
		obj.transform.parent = parent;
	}

	/*
	 * Handles ghosting effect of a new point
	 */
	public void Ghosting (Vector3 point) {
		if (!snapping) {
			activeSection [activeSection.Count - 1] = point;
			FillVertices ();
		}
	}

	public void StopGhosting () {
		activeSection.Remove (activeSection [activeSection.Count - 1]);
		FillVertices ();
	}

	public void StopSnapping () {
		snapping = false;
	}

	/*
	 * This method adds a new active section.
	 */
	public void AddSection() {
		activeSection = new Section ();
		sections.Add (activeSection);
	}

	/*
	 * This method adds a point to the active section. If no section is currently
	 * active a new one will be added.
	 */
	public void AddPoint(Vector3 point, float width) {
		if (activeSection == null) {
			AddSection ();
		}
			
		if (activeSection.Count == 0) {
			activeSection.Add (point);
		} else {
			activeSection [activeSection.Count - 1] = point;
		}

		// Ghosting point?
		activeSection.Add (point);

		if (activeSection.Count > 1) {
			FillVertices ();
		}
	}

	/**
	 * Fills the vertices and normals for the mesh. After that it will triangulate
	 * the mesh to make it ready for display
	 */
	public void FillVertices() {

		// Path width
		float width = 2.0f;

		// The last vertex index (used for triangulating sections)
		int lastVertex = 0;

		// Lists for the mesh internals
		List<Vector3> vertices = new List<Vector3> ();
		List<Vector3> normals = new List<Vector3> ();
		List<int> triangles = new List<int> ();

		// Clear the mesh before filling it again
		// TODO: see if this is needed
		mesh.Clear ();

		// Loop through every separate path section
		for (int s = 0; s < sections.Count; s++) {

			// Get the list of points for the current section
			Section section = sections [s];

			// Triangulation start at the last known added vertex (of the previous section)
			int triangulateStart = lastVertex;

			// Loop through every section point
			for (int i = 0; i < section.Count; i++) {

				/*
				 * Get a directional value for the current point based on the next point in the
				 * list (if there is any). This will determine vertex placement for the path width.
				 */
				Vector3 direction;
				if (i != section.Count - 1) {
					direction = new Vector3 (-(section [i + 1].z - section [i].z), section [i].y, section [i + 1].x - section [i].x).normalized;
				} else {
					direction = new Vector3 (-(section [i].z - section [i - 1].z), section [i].y, section [i].x - section [i - 1].x).normalized;
				}

				// Add two vertexes for this point
				vertices.Add (section [i] + direction * width);
				vertices.Add (section [i] + direction * -width);

				// Normals should be facing up
				normals.Add (Vector3.up);
				normals.Add (Vector3.up);

				// We've added two vertexes so add 2 to the lastVertex index
				lastVertex += 2;
			}

			/*
			 * Generate the triangles for this section and add them to our list we've created
			 * to store the mesh triangles in.
			 */
			triangles.AddRange (Triangulate (vertices, triangulateStart, lastVertex));
		}

		// Add Borders

		// Set vertices, triangles and normals of the mesh
		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.normals = normals.ToArray ();

		// Recalculate normals of the mesh
		mesh.RecalculateNormals ();

		// Update the collider mesh
		// TODO: is this even performant?
		collider.sharedMesh = mesh;
	}

	/*
	 * This method triangulates a list of verticese (as quads) based on the start and end
	 * vertex given.
	 */
	public List<int> Triangulate (List<Vector3> vertices, int start, int end) {

		/*
		 * We want to return a list of triangles for the vertices we're given. These triangles
		 * will be stored one-dimensional like: {2, 1, 0, 1, 2, 3} but will be interpreted by
		 * the mesh like: {2, 1, 0} {1, 2, 3} ...etc
		 */
		List<int> triangles = new List<int> ();

		/*
		 * Start calculating from the starting point given for a section and make sure we stop 
		 * looping before reaching the last two vertices (as these don't have any other vertices
		 * to connect to)
		 */
		for (int i = start; i < end - 2; i++) {
			// Create opposite facing triangles
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

		return triangles;

	}

	public void GetClosestEdge (Vector3 point) {
		Vector3 previous = activeSection [activeSection.Count - 2];
		Vector3 direction = (previous - point).normalized;

		Vector3 end = point - direction * 2.0f;
		Vector3 start = point - direction * 0.1f;

		Debug.DrawLine (start, end, Color.yellow);
	}

}
