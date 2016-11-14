using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform launchPosition;
	public bool isUpgraded;
	public float upgradeTime = 10.0f;

	private float currentTime;
	private AudioSource audioSource;

	private KeyCode startFiringKey;
	private KeyCode stopFiringKey;

	void Awake() {
		startFiringKey = GameManager.DesktopMode ? KeyCode.O : KeyCode.JoystickButton10;
		stopFiringKey = GameManager.DesktopMode ? KeyCode.L : KeyCode.JoystickButton11;
	}

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown (startFiringKey)) {
			fireBullet ();
		}

		currentTime += Time.deltaTime;
		if (currentTime > upgradeTime && isUpgraded == true) {
			isUpgraded = false;
		}
	}

	void fireBullet() {

		if (GameManager.Pause) { return; }
		
		Rigidbody bullet = createBullet ();
		bullet.velocity =  transform.forward * 100;

		if (isUpgraded) {
			Rigidbody bullet2 = createBullet();
			bullet2.velocity = (transform.right + transform.forward / 0.5f) * 100;
			Rigidbody bullet3 = createBullet();
			bullet3.velocity = ((transform.right * -1) + transform.forward / 0.5f) * 100;
		}

		if (isUpgraded) {
			audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire);
		} else {
			audioSource.PlayOneShot(SoundManager.Instance.gunFire);
		}
	}

	private Rigidbody createBullet() {
		GameObject bullet = Instantiate(bulletPrefab) as GameObject;
		bullet.transform.position = launchPosition.position;
		return bullet.GetComponent<Rigidbody>();
	}

	public void UpgradeGun() {
		isUpgraded = true;
		currentTime = 0;
	}


}
