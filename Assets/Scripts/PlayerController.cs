using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public LayerMask layerMask;
	public float moveSpeed = 50.0f;
	public GameObject head;
	public GameObject marineBody;
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

	private float f_front = 0;
	private float f_back = 0;
	private float f_right = 0;
	private float f_left = 0;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		deathParticles = GetComponentInChildren<DeathParticles> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (GameManager.Pause) {
			characterController.SimpleMove(Vector3.zero);
			return;
		}

		if (GameManager.DesktopMode) {
			setDirectionFromKeyBoard ();
		} else {
			setDirectionFromJoystick ();
		}

		Vector3 moveDirection = (marineBody.transform.forward * (f_front - f_back)) +
		                        (marineBody.transform.right * (f_right - f_left));

		characterController.SimpleMove(moveDirection.normalized * moveSpeed);
		

		if (isHit) {
			timeSinceHit += Time.deltaTime;
			if (timeSinceHit > timeBetweenHits) {
				isHit = false;
				timeSinceHit = 0;
				hitOvelay.SetActive (false);
			}
		}
	}

	private float getDirectionState(KeyCode onState, KeyCode offState, float current) {
		if (Input.GetKeyDown (onState)) {
			return 1;
		} else if (Input.GetKeyDown (offState)) {
			return  0;
		} else {
			return current;
		}
	}

	private void setDirectionFromKeyBoard() {

		f_front = getDirectionState(KeyCode.W, KeyCode.Space, f_front);
		f_back = getDirectionState(KeyCode.S, KeyCode.Space, f_back);
		f_right = getDirectionState(KeyCode.D, KeyCode.Space, f_right);
		f_left = getDirectionState(KeyCode.A, KeyCode.Space, f_left);
	}

	private void setDirectionFromJoystick() {

		f_front = getDirectionState(KeyCode.JoystickButton0, KeyCode.JoystickButton1, f_front);
		f_back = getDirectionState(KeyCode.JoystickButton6, KeyCode.JoystickButton7, f_back);
		f_right = getDirectionState(KeyCode.JoystickButton4, KeyCode.JoystickButton5, f_right);
		f_left = getDirectionState(KeyCode.JoystickButton2, KeyCode.JoystickButton3, f_left);
	}

	void FixedUpdate() {

		Vector3 eulerAngles = new Vector3 (marineBody.transform.rotation.eulerAngles.x,
			head.transform.rotation.eulerAngles.y, marineBody.transform.rotation.eulerAngles.z);

		marineBody.transform.rotation = Quaternion.Euler (eulerAngles);

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

		marineBody.transform.parent = null;
		marineBody.GetComponent<Rigidbody>().isKinematic = false;
		marineBody.GetComponent<Rigidbody>().useGravity = true;
		marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
		marineBody.gameObject.GetComponent<Gun>().enabled = false;

		deathParticles.transform.parent = null;
		deathParticles.Activate();

		Destroy(head.gameObject.GetComponent<HingeJoint>());
		head.transform.parent = null;
		head.GetComponent<Rigidbody>().useGravity = true;
		head.GetComponent<Rigidbody>().isKinematic = false;
		SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);
		Destroy(gameObject);
	}
}
