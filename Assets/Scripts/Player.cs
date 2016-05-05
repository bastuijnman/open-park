using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Movement speed, can be set in the editor
	public float speed = 1.0f;

	// Player (camera) rotation
	private Vector3 playerRotation = new Vector3 (35, 0, 0);

	// Player (camera) height
	private float playerHeight = 5.0f;

	void Start () {

		// Setup the player camera
		Camera camera = gameObject.AddComponent<Camera> ();
	}
		
	void Update () {

		float horizontalMovement = Input.GetAxis ("Horizontal");
		float verticalMovement = Input.GetAxis ("Vertical");
		float rotationMovement = 0.0f;

		// Handle camera rotation
		if (Input.GetKey (KeyCode.Q)) {
			rotationMovement = -1.0f;
		} else if (Input.GetKey (KeyCode.E)) {
			rotationMovement = 1.0f;
		}
		playerRotation.y += rotationMovement * (speed / 2);

		// Handle movement on the terrain
		Vector3 movement = new Vector3 (horizontalMovement, 0.0f, verticalMovement);
		GetComponent<Rigidbody>().velocity = movement * speed;

		// Keep position with a variable height
		transform.position = new Vector3 (transform.position.x, playerHeight, transform.position.z);

		// Set player rotation
		transform.rotation = Quaternion.Euler (playerRotation);
	
	}
}
