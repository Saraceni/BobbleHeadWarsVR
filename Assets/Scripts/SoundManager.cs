using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager Instance = null;

	public AudioClip gunFire;
	public AudioClip upgradedGunFire;
	public AudioClip hurt;
	public AudioClip alienDeath;
	public AudioClip marineDeath;
	public AudioClip victory;
	public AudioClip elevatorArrived;
	public AudioClip powerUpPickup;
	public AudioClip powerUpAppear;

	private AudioSource soundEffectAudio;

	// Use this for initialization
	void Start () {
		
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}

		AudioSource[] sources = GetComponents<AudioSource>();
		foreach (AudioSource source in sources) {
			if (source.clip == null) {
				soundEffectAudio = source;
			}
		}
	}
	
	public void PlayOneShot(AudioClip clip) {
		soundEffectAudio.PlayOneShot(clip);
	}
}
