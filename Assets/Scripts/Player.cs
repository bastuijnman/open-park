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

		// This is probably stupid to do
		gameObject.tag = "MainCamera";
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

		// Handle camera zooming and make sure scrolling is inverted
		playerHeight -= Input.GetAxis("Mouse ScrollWheel") * speed;
		playerHeight = Mathf.Clamp (playerHeight, 5.0f, 35.0f);

		// Calculate movement velocity
		Vector3 velocityVertical = transform.forward * speed * verticalMovement;
		Vector3 velocityHorizontal = transform.right * speed * horizontalMovement;

		Vector3 calculatedVelocity = velocityHorizontal + velocityVertical;
		calculatedVelocity.y = 0.0f;

		// Apply calculated velocity
		GetComponent<Rigidbody>().velocity = calculatedVelocity;

		// Keep position with a variable height
		transform.position = new Vector3 (transform.position.x, playerHeight, transform.position.z);

		// Set player rotation
		transform.rotation = Quaternion.Euler (playerRotation);
	
	}
}
