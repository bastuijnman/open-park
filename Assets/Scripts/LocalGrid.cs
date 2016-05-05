using UnityEngine;
using System.Collections;

public class LocalGrid : MonoBehaviour {

	private float tileSize = 1.0f;

	private float width;
	private float height;

	private float x;
	private float z;

	private bool active = true;

	private GameObject tile;

	// Use this for initialization
	void Start () {
		Vector3 size = GetComponent<Collider> ().bounds.size;

		width = size.x;
		height = size.z;

		x = transform.position.x;
		z = transform.position.z;

		tile = GameObject.CreatePrimitive (PrimitiveType.Quad);
		tile.transform.parent = transform;

		// Quads are upright, so rotate to align with grid
		tile.transform.rotation = Quaternion.Euler (new Vector3 (90.0f, 0.0f, 0.0f));
		tile.transform.localScale = new Vector3 (tileSize, tileSize, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {

		if (active) {
			Vector2 mouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Ray ray = Camera.main.ScreenPointToRay (mouse);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 50)) {

				// This does not seem performant :S
				if (hit.collider == GetComponent<Collider> ()) {
					int row = Mathf.FloorToInt ((height + z + hit.point.z) / tileSize);
					int col = Mathf.FloorToInt ((width + x + hit.point.x) / tileSize);

					tile.transform.localPosition = new Vector3 (
						col * tileSize,
						0.1f, // Ever so slightly higher than the grid object
						row * tileSize
					);
				}
			}
		}
	
	}
}
