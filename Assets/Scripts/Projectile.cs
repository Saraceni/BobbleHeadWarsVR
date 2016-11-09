using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		Destroy(gameObject);
	}
}
