using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private Rigidbody projectileRigidbody;
	private bool paused = false;
	private Vector3 speed;

	void Start() {
		projectileRigidbody = GetComponent<Rigidbody> ();
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		Destroy(gameObject);
	}

	void OnPauseGame ()
	{
		if (!paused) {
			paused = true;
			speed = projectileRigidbody.velocity;
			projectileRigidbody.velocity = Vector3.zero;
		} else {
			paused = false;
			projectileRigidbody.velocity = speed;
		}

	}
}
