using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Alien : MonoBehaviour {

	public Rigidbody head;
	public Transform target;
	public float navigationUpdate;
	public UnityEvent OnDestroy;
	public bool isAlive = true;

	private NavMeshAgent agent;
	private float navigationTime = 0;
	private DeathParticles deathParticles;


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

		if (isAlive) {
			navigationTime += Time.deltaTime;
			if (navigationTime > navigationUpdate) {
				agent.destination = target.position;
				navigationTime = 0;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (isAlive) {
			Die ();
		}
	}

	void DetachHead() {
		
		head.GetComponent<Animator>().enabled = false;
		head.isKinematic = false;
		head.useGravity = true;
		head.GetComponent<SphereCollider>().enabled = true;
		head.gameObject.transform.parent = null;
		head.velocity = new Vector3(0, 26.0f, 3.0f);
		head.GetComponent<SelfDestruct>().Initiate();
	}

	public void Die() {
		
		isAlive = false;

		DetachHead ();

		if (deathParticles) {
			deathParticles.transform.parent = null;
			deathParticles.Activate();
		}

		OnDestroy.Invoke();
		OnDestroy.RemoveAllListeners();
		SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
		Destroy(gameObject);
	}

	public DeathParticles GetDeathParticles() {
		if (deathParticles == null) {
			deathParticles = GetComponentInChildren<DeathParticles>();
		}
		return deathParticles;
	}
}
