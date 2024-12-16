using UnityEngine;

public class DataHandler : MonoBehaviour {
	public bool isPlaying = false;
	public bool isSettingsOpen = false;
	public bool isSfxMuted = false;
	public bool isMusicMuted = false;

	private void Awake() {
		if (GameObject.Find(gameObject.name) != gameObject) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}
}
