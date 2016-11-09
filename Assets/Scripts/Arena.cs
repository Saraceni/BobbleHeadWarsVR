using UnityEngine;
using System.Collections;

public class Arena : MonoBehaviour {

	public GameObject player;
	public Transform elevator;

	private Animator arenaAnimator;
	private SphereCollider sphereCollider;


	// Use this for initialization
	void Start () {
		arenaAnimator = GetComponent<Animator> ();
		sphereCollider = GetComponent<SphereCollider> ();
	}
	
	void OnTriggerEnter(Collider other) {
		
		Camera.main.transform.parent.gameObject.GetComponent<CameraMovement> ().enabled = false;
		player.transform.parent = elevator.transform;

		player.GetComponent<PlayerController>().enabled = false;
		SoundManager.Instance.PlayOneShot (SoundManager.Instance.elevatorArrived);
		arenaAnimator.SetBool ("OnElevator", true);
	}

	public void ActivatePlatform() {
		sphereCollider.enabled = true;
	}
}
