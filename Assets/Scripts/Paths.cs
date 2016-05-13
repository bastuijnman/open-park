using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paths : MonoBehaviour {

	private float width = 2.0f;

	private Path paths;

	private bool editing = false;

	void Start () {
		paths = new Path (transform);
	}

	void Update () {

		// In path edit mode
		if (editing) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				editing = false;
				paths.StopGhosting ();
				paths.StopSnapping ();
				paths.AddSection ();
				return;
			}
			
			Vector2 mouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Ray ray = Camera.main.ScreenPointToRay (mouse);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 50)) {

				paths.Ghosting (hit.point);
				paths.GetClosestEdge (hit.point);
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			paths.AddPoint(GetMousePosition(), 2.0f);
			editing = true;
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
}
