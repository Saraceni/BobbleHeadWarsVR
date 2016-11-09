using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject player;
	public GameObject[] spawnPoints;
	public GameObject alien;
	public GameObject upgradePrefab;
	public GameObject deathFloor;
	public Animator arenaAnimator;
	public Gun gun;
	public float upgradeMaxTimeSpawn = 7.5f;

	public int maxAliensOnScreen;
	public int totalAliens;
	public float minSpawnTime;
	public float maxSpawnTime;
	public int aliensPerSpawn;

	private int aliensOnScreen = 0;
	private float generatedSpawnTime = 0;
	private float currentSpawnTime = 0;

	private bool spawnedUpgrade = false;
	private float actualUpgradeTime = 0;
	private float currentUpgradeTime = 0;

	// Use this for initialization
	void Start () {
		actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
		actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
	}
	
	// Update is called once per frame
	void Update () {

		if (player == null) { return; }
		
		currentSpawnTime += Time.deltaTime;
		currentUpgradeTime += Time.deltaTime;

		spawnAlienIfNeeded ();
		spawnUpgradeIfNeeded ();
	}

	void spawnUpgradeIfNeeded() {
		
		if (currentUpgradeTime > actualUpgradeTime) {
			if (!spawnedUpgrade) { // 1
				// 2
				int randomNumber = Random.Range(0, spawnPoints.Length - 1);
				GameObject spawnLocation = spawnPoints[randomNumber];
				// 3
				GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
				Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
				upgradeScript.gun = gun;
				upgrade.transform.position = spawnLocation.transform.position;
				// 4
				spawnedUpgrade = true;
			}
		}
	}

	void spawnAlienIfNeeded() {
		
		if (currentSpawnTime > generatedSpawnTime) {

			currentSpawnTime = 0;
			generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

			if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens) {

				List<int> previousSpawnLocations = new List<int>();

				if (aliensPerSpawn > spawnPoints.Length) {
					aliensPerSpawn = spawnPoints.Length - 1;
				}

				aliensPerSpawn = (aliensPerSpawn > totalAliens) ? aliensPerSpawn - totalAliens : aliensPerSpawn;

				for (int i = 0; i < aliensPerSpawn; i++) {
					if (aliensOnScreen < maxAliensOnScreen) {
						aliensOnScreen += 1;
						int spawnPoint = getRandomSpawnPoint (previousSpawnLocations);
						createNewAlien (spawnPoint);
					}
				}

			}
		}
	}

	int getRandomSpawnPoint(List<int> previousSpawnLocations) {
		
		int spawnPoint = -1;
		while (spawnPoint == -1) {
			// 3
			int randomNumber = Random.Range(0, spawnPoints.Length - 1);
			// 4
			if (!previousSpawnLocations.Contains(randomNumber)) {
				previousSpawnLocations.Add(randomNumber);
				spawnPoint = randomNumber;
				if (!previousSpawnLocations.Contains(randomNumber)) {
					previousSpawnLocations.Add(randomNumber);
					spawnPoint = randomNumber;
				}
			}
		}

		return spawnPoint;
	}

	void createNewAlien(int spawnPoint) {
		
		GameObject spawnLocation = spawnPoints[spawnPoint];
		GameObject newAlien = Instantiate(alien) as GameObject;
		newAlien.transform.position = spawnLocation.transform.position;
		Alien alienScript = newAlien.GetComponent<Alien>();
		alienScript.target = player.transform;
		Vector3 targetRotation = new Vector3(player.transform.position.x, newAlien.transform.position.y, player.transform.position.z);
		newAlien.transform.LookAt(targetRotation);
		alienScript.OnDestroy.AddListener(AlienDestroyed);
		alienScript.GetDeathParticles().SetDeathFloor(deathFloor);
	}

	private void endGame() {
		SoundManager.Instance.PlayOneShot (SoundManager.Instance.elevatorArrived);
		arenaAnimator.SetTrigger ("PlayerWon");
	}

	public void AlienDestroyed() {
		aliensOnScreen -= 1;
		totalAliens -= 1;

		if (totalAliens == 0) {
			Invoke("endGame", 2.0f);
		}
	}
}
