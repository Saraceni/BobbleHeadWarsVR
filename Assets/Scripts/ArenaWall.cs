using UnityEngine;
using System.Collections;

public class ArenaWall : MonoBehaviour {

	private Animator arenaAnimator;

	// Use this for initialization
	void Start () {
		arenaAnimator = transform.parent.gameObject.GetComponent<Animator> ();
	}
	
	void OnTriggerEnter(Collider other) {
		arenaAnimator.SetBool("IsLowered", true);
	}

	void OnTriggerExit(Collider other) {
		arenaAnimator.SetBool("IsLowered", false);
	}
}
