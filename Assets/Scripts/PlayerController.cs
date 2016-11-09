using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public LayerMask layerMask;
	public float moveSpeed = 50.0f;
	public Rigidbody head;
	public Rigidbody marineBody;
	public Animator bodyAnimator;
	public float[] hitForce;
	public float timeBetweenHits = 2.5f;
	public GameObject hitOvelay;

	private DeathParticles deathParticles;
	private Vector3 currentLookTarget = Vector3.zero;
	private CharacterController characterController;
	private bool isHit = false;
	private bool isDead = false;
	private float timeSinceHit = 0;
	private int hitNumber = -1;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		deathParticles = GetComponentInChildren<DeathParticles> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		characterController.SimpleMove(moveDirection * moveSpeed);

		if (isHit) {
			timeSinceHit += Time.deltaTime;
			if (timeSinceHit > timeBetweenHits) {
				isHit = false;
				timeSinceHit = 0;
				hitOvelay.SetActive (false);
			}
		}
	}

	void FixedUpdate() {
		
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if (moveDirection == Vector3.zero) {
			bodyAnimator.SetBool("IsMoving", false);
		} else {
			head.AddForce(transform.right * 150, ForceMode.Acceleration);
			bodyAnimator.SetBool("IsMoving", true);
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		// Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

		if (Physics.Raycast(ray, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore)) {
			
			if (hit.point != currentLookTarget) {
				currentLookTarget = hit.point;
			}

			// 1
			Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
			// 2
			Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
			// 3
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
		}
	}

	void OnTriggerEnter(Collider other) {
		Alien alien = other.gameObject.GetComponent<Alien>();
		if (alien != null) {
			if (!isHit) {
				hitNumber += 1;
				if (hitNumber < hitForce.Length) {
					hitOvelay.SetActive (true);
				} else {
					Die ();
				}
				isHit = true;
				SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
			}
			alien.Die ();
		}
	}

	public void Die() {

		bodyAnimator.SetBool("IsMoving", false);
		marineBody.transform.parent = null;
		marineBody.isKinematic = false;
		marineBody.useGravity = true;
		marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
		marineBody.gameObject.GetComponent<Gun>().enabled = false;

		deathParticles.transform.parent = null;
		deathParticles.Activate();

		Destroy(head.gameObject.GetComponent<HingeJoint>());
		head.transform.parent = null;
		head.useGravity = true;
		SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);
		Destroy(gameObject);
	}
}
