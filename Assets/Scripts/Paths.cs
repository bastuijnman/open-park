using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paths : MonoBehaviour {

	private List<Vector3> points = new List<Vector3>();

	private float width = 2.0f;

	private Vector3 activePoint;

	private Path paths;

	private bool editing = false;

	void Start () {
		paths = new Path (transform);
	}

	void Update () {
		if (editing) {

			if (Input.GetKeyDown (KeyCode.Escape)) {
				editing = false;

				points.RemoveAt (points.Count - 1);

				return;
			}
			
			Vector2 mouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Ray ray = Camera.main.ScreenPointToRay (mouse);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 50)) {
				points [points.Count - 1] = hit.point;
			}
		}
	}

	void OnDrawGizmos() {
		
		if (points.Count > 1) {
			for (int i = 0; i < points.Count; i++) {
				Vector3 direction;

				if (i != points.Count - 1) {
					direction = new Vector3 (-(points [i + 1].z - points [i].z), points [i].y, points [i + 1].x - points [i].x).normalized;
					Debug.DrawLine (points [i], points [i + 1], Color.red);
				} else {
					direction = new Vector3 (-(points [i].z - points [i - 1].z), points [i].y, points [i].x - points [i - 1].x).normalized;
				}

				Gizmos.color = Color.red;
				Gizmos.DrawCube (points [i] + direction * width, new Vector3 (0.10f, 0.10f, 0.10f));
				Gizmos.color = Color.gray;
				Gizmos.DrawCube (points [i], new Vector3 (0.25f, 0.25f, 0.25f));
				Gizmos.color = Color.red;
				Gizmos.DrawCube (points [i] + direction * -width, new Vector3 (0.10f, 0.10f, 0.10f));
				Gizmos.color = Color.gray;

			}
		}
	}

	Vector3 GetMousePosition () {
		Vector2 mouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Ray ray = Camera.main.ScreenPointToRay (mouse);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 50)) {
			return hit.point;
		}

		return Vector3.zero;
	}

	void OnMouseDown () {
		// Actual pathing
		paths.AddPoint(GetMousePosition(), 2.0f);

		if (activePoint != null) {
			points.Add (GetMousePosition ());
		}

		activePoint = GetMousePosition ();
		points.Add (activePoint);

		editing = true;
	}
}
