using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	public float destructTime = 3.0f;

	public void Initiate() {
		Invoke("selfDestruct", destructTime);
	}

	private void selfDestruct() {
		Destroy(gameObject);
	}
}
