using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Upgrade : MonoBehaviour {

	public Gun gun;
	public UnityEvent OnPickup;

	void OnTriggerEnter(Collider other) {
		gun.UpgradeGun();
		OnPickup.Invoke ();
		OnPickup.RemoveAllListeners();
		Destroy(gameObject);
		SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpPickup);
	}
}
